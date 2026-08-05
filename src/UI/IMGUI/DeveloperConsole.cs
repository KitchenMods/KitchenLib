using Controllers;
using Platforms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using KitchenLib.Commands;
using UnityEngine;

namespace KitchenLib.UI.IMGUI
{
	/// <summary>
	/// An in game developer console, toggled with the backquote key. Commands are typed into a bar
	/// along the bottom of the screen and anything they log is listed above it.
	/// </summary>
	public class DeveloperConsole : MonoBehaviour
	{
		private const ControllerType TypedControllers = ControllerType.Keyboard | ControllerType.Mouse;
		private const string InputControlName = "kitchenlib.console.input";
		private const int MaxOutputLines = 512;
		private const int MaxHistoryEntries = 64;
		private const float MaxOutputScreenFraction = 0.4f;
		private const float ScrollbarPadding = 20f;
		private const float DisabledMessageDuration = 3f;

		private static readonly Color CommandColor = new Color(0.55f, 0.8f, 1f);
		private static readonly Color InfoColor = new Color(0.88f, 0.88f, 0.88f);
		private static readonly Color WarningColor = new Color(1f, 0.82f, 0.35f);
		private static readonly Color ErrorColor = new Color(1f, 0.45f, 0.4f);
		private static readonly Color HintColor = new Color(0.55f, 0.55f, 0.55f);

		private static readonly List<ConsoleCommand> RegisteredCommands = new List<ConsoleCommand>();

		private static readonly ReadOnlyCollection<ConsoleCommand> ExposedCommands = RegisteredCommands.AsReadOnly();
		private static readonly List<ConsoleLine> Output = new List<ConsoleLine>();
		private static readonly List<string> History = new List<string>();

		private static int outputVersion;

		private readonly List<ConsoleCommand> suggestions = new List<ConsoleCommand>();

		private string input = "";
		private int selectedSuggestion;
		private int historyIndex = -1;
		private bool focusInput;
		private bool syncTextEditor;
		private int drawnOutputVersion = -1;
		private float disabledMessageTime = float.NegativeInfinity;
		private Vector2 scrollPosition;

		private Texture2D inputBackground;
		private Texture2D panelBackground;
		private Texture2D highlightBackground;
		private GUIStyle textStyle;
		private GUIStyle rightAlignedStyle;
		private GUIStyle inputStyle;
		private int styledScreenHeight;
		private float lineHeight;
		private int promptWidth;

		private InputLock.Lock _lock;

		/// <summary>
		/// Whether the console is currently open and capturing what the player types.
		/// </summary>
		public static bool IsOpen { get; private set; }

		/// <summary>
		/// Every registered command, in registration order.
		/// </summary>
		public static IList<ConsoleCommand> Commands => ExposedCommands;

		/// <summary>
		/// Whether this game belongs to the player running it, which covers both hosting a multiplayer
		/// game and playing on your own. Commands marked <see cref="ConsoleCommand.IsHostOnly"/> are
		/// refused while this is false.
		/// </summary>
		public static bool IsHost => Kitchen.Session.NetworkedPlayState.IsOwnGame();

		/// <summary>
		/// Makes a command available to the console. A command registered under a name that is already
		/// taken replaces the existing one.
		/// </summary>
		public static void RegisterCommand(ConsoleCommand command)
		{
			if (command == null)
				throw new ArgumentNullException(nameof(command));

			if (string.IsNullOrEmpty(command.Name))
				throw new ArgumentException("Console commands must have a name.", nameof(command));

			if (!IsValidName(command.Name))
				throw new ArgumentException($"Console command names may only contain letters and underscores, so '{command.Name}' cannot be registered. Anything else would be mistaken for an argument, such as the page number the help command takes.", nameof(command));

			ConsoleCommand existing = FindCommand(command.Name);
			if (existing != null)
			{
				Debug.LogWarning($"[KitchenLib] Console command '{command.Name}' was already registered by {existing.GetType().FullName} and has been replaced.");
				// RegisteredCommands.Remove(existing);
			}

			RegisteredCommands.Add(command);
		}

		private static bool IsValidName(string name)
		{
			foreach (char character in name)
			{
				if (!char.IsLetter(character) && character != '_')
					return false;
			}

			return true;
		}

		/// <summary>
		/// Whether a command can be seen and run at the moment. Commands marked
		/// <see cref="ConsoleCommand.IsCheat"/> stay hidden until cheats have been enabled.
		/// </summary>
		public static bool IsAvailable(ConsoleCommand command) => command != null && (!command.IsCheat || Main.cheatsEnabled);

		/// <summary>
		/// Finds a registered command by name, ignoring case.
		/// </summary>
		/// <returns>The command, or null when no command uses that name.</returns>
		public static ConsoleCommand FindCommand(string name)
		{
			foreach (ConsoleCommand command in RegisteredCommands)
			{
				if (string.Equals(command.Name, name, StringComparison.OrdinalIgnoreCase))
					return command;
			}

			return null;
		}

		/// <summary>Writes a line to the console output.</summary>
		public static void Log(string message) => Append(message, InfoColor);

		/// <summary>Writes a highlighted warning line to the console output.</summary>
		public static void LogWarning(string message) => Append(message, WarningColor);

		/// <summary>Writes a highlighted error line to the console output.</summary>
		public static void LogError(string message) => Append(message, ErrorColor);

		/// <summary>Removes every line from the console output.</summary>
		public static void Clear()
		{
			Output.Clear();
			outputVersion++;
		}

		private static void Append(string message, Color color)
		{
			foreach (string line in (message ?? "").Split('\n'))
			{
				Output.Add(new ConsoleLine(line.TrimEnd('\r'), color));
			}

			if (Output.Count > MaxOutputLines)
				Output.RemoveRange(0, Output.Count - MaxOutputLines);

			outputVersion++;
		}

		private void Awake()
		{
			inputBackground = CreateTexture(Color.black);
			panelBackground = CreateTexture(new Color(0f, 0f, 0f, 0.78f));
			highlightBackground = CreateTexture(new Color(0.16f, 0.28f, 0.42f, 0.95f));
		}

		private void OnDestroy()
		{
			Destroy(inputBackground);
			Destroy(panelBackground);
			Destroy(highlightBackground);
		}

		private void OnGUI()
		{
			// Fully qualified because KitchenLib.Event is a namespace of its own.
			UnityEngine.Event current = UnityEngine.Event.current;
			bool pressedToggle = current.type == EventType.KeyDown && current.keyCode == KeyCode.BackQuote;

			if (!Main._enableDeveloperConsole)
			{
				// Turning the console off mid session closes whatever is on screen and hands input back.
				if (IsOpen)
					SetOpen(false);

				if (pressedToggle)
				{
					disabledMessageTime = Time.realtimeSinceStartup;
					current.Use();
				}

				DrawDisabledMessage();

				return;
			}

			if (pressedToggle)
			{
				SetOpen(!IsOpen);
				current.Use();
				return;
			}

			if (!IsOpen)
				return;

			GUI.depth = -1000;
			RefreshStyles();
			HandleShortcuts(current);

			float inputHeight = lineHeight + 8f;
			Rect inputArea = new Rect(0f, Screen.height - inputHeight, Screen.width, inputHeight);

			// The input field is drawn first so that the suggestions above it reflect what was just typed.
			DrawInput(inputArea);
			float hintHeight = DrawHint(inputArea.y);
			DrawOutput(inputArea.y - hintHeight);
		}

		/// <summary>
		/// Draws the notice that the console is off, in the bar the console would have opened into, for a
		/// few seconds after the key was last pressed.
		/// </summary>
		private void DrawDisabledMessage()
		{
			if (Time.realtimeSinceStartup - disabledMessageTime > DisabledMessageDuration)
				return;

			GUI.depth = -1000;
			RefreshStyles();

			float height = lineHeight + 8f;
			Rect area = new Rect(0f, Screen.height - height, Screen.width, height);

			GUI.DrawTexture(area, panelBackground);
			DrawText(area, "The developer console is disabled.", WarningColor, textStyle);
		}

		private void HandleShortcuts(UnityEngine.Event current)
		{
			if (current.type != EventType.KeyDown)
				return;

			switch (current.keyCode)
			{
				case KeyCode.Escape:
					SetOpen(false);
					current.Use();
					break;
				case KeyCode.Return:
				case KeyCode.KeypadEnter:
					Submit();
					current.Use();
					break;
				case KeyCode.Tab:
					ApplySuggestion();
					current.Use();
					break;
				case KeyCode.UpArrow:
					StepSelection(-1);
					current.Use();
					break;
				case KeyCode.DownArrow:
					StepSelection(1);
					current.Use();
					break;
				case KeyCode.None:
					// Printable characters arrive as their own event; swallow the ones our shortcuts produce
					// so they are never typed into the field.
					if (current.character == '\t' || current.character == '\n' || current.character == '\r' || current.character == '`')
						current.Use();
					break;
			}
		}

		private void Submit()
		{
			string line = input.Trim();

			input = "";
			historyIndex = -1;
			syncTextEditor = true;
			RefreshSuggestions();

			if (line.Length == 0)
				return;

			Remember(line);
			Append("> " + line, CommandColor);

			string[] tokens = Tokenize(line);
			ConsoleCommand command = FindCommand(tokens[0]);
			if (command == null)
			{
				LogError($"Unknown Command: {tokens[0]}");
				return;
			}

			if (!IsAvailable(command))
			{
				LogError($"{command.Name} is a cheat, and cheats are not enabled.");
				return;
			}

			if (command.IsHostOnly && !IsHost)
			{
				LogError(Kitchen.Session.NetworkedPlayState == NetworkedPlayState.NotInGame
					? $"Only the host can run {command.Name}, and you are not in a game."
					: $"Only the host can run {command.Name}.");
				return;
			}

			string[] args = new string[tokens.Length - 1];
			Array.Copy(tokens, 1, args, 0, args.Length);

			if (args.Length < command.MinimumArguments)
			{
				string plural = command.MinimumArguments == 1 ? "argument" : "arguments";
				LogError($"{command.Name} needs at least {command.MinimumArguments} {plural}.");
				command.LogUsage();
				return;
			}

			try
			{
				command.Execute(args);
			}
			catch (Exception exception)
			{
				LogError($"{command.Name} threw {exception.GetType().Name}: {exception.Message}");
				Debug.LogException(exception);
			}
		}

		private void Remember(string line)
		{
			History.Remove(line);
			History.Insert(0, line);

			if (History.Count > MaxHistoryEntries)
				History.RemoveRange(MaxHistoryEntries, History.Count - MaxHistoryEntries);
		}

		/// <summary>
		/// Moves through the suggestion list while it is open, and through previously run commands otherwise.
		/// </summary>
		private void StepSelection(int direction)
		{
			if (suggestions.Count > 0)
			{
				selectedSuggestion = (selectedSuggestion + direction + suggestions.Count) % suggestions.Count;
				return;
			}

			if (History.Count == 0)
				return;

			// History is newest first, so pressing up walks further back into it.
			int recalled = Mathf.Clamp(historyIndex - direction, -1, History.Count - 1);
			if (recalled == historyIndex)
				return;

			historyIndex = recalled;
			input = historyIndex < 0 ? "" : History[historyIndex];
			syncTextEditor = true;
			RefreshSuggestions();
		}

		private void ApplySuggestion()
		{
			if (suggestions.Count == 0)
				return;

			input = suggestions[selectedSuggestion].Name + " ";
			syncTextEditor = true;
			RefreshSuggestions();
		}

		private void RefreshSuggestions()
		{
			suggestions.Clear();
			selectedSuggestion = 0;

			string typed = input.TrimStart();
			if (typed.Length == 0 || typed.IndexOf(' ') >= 0)
				return;

			foreach (ConsoleCommand command in RegisteredCommands)
			{
				if (command.DoNotAutocomplete || !IsAvailable(command))
					continue;

				if (command.Name.StartsWith(typed, StringComparison.OrdinalIgnoreCase))
					suggestions.Add(command);
			}

			suggestions.Sort((first, second) => string.Compare(first.Name, second.Name, StringComparison.OrdinalIgnoreCase));

			// Once the only match has been typed out in full there is nothing left to complete, and the
			// usage line takes over the strip instead.
			if (suggestions.Count == 1 && suggestions[0].Name.Equals(typed, StringComparison.OrdinalIgnoreCase))
				suggestions.Clear();
		}

		private void DrawInput(Rect area)
		{
			UnityEngine.Event current = UnityEngine.Event.current;

			GUI.DrawTexture(area, inputBackground);
			GUI.Label(new Rect(area.x, area.y, promptWidth, area.height), ">", textStyle);

			Color cursorColor = GUI.skin.settings.cursorColor;
			Color selectionColor = GUI.skin.settings.selectionColor;
			GUI.skin.settings.cursorColor = Color.white;
			GUI.skin.settings.selectionColor = new Color(0.25f, 0.45f, 0.7f, 0.8f);

			GUI.SetNextControlName(InputControlName);
			string edited = GUI.TextField(area, input, inputStyle);

			GUI.skin.settings.cursorColor = cursorColor;
			GUI.skin.settings.selectionColor = selectionColor;

			// Taking focus clears the keyboard control for a pass, and the field selects all of its text
			// when it gets that focus back. Only chase focus once a repaint has shown it really is lost,
			// which is the one pass where the field is drawn normally rather than on a used event.
			if (focusInput || (current.type == EventType.Repaint && GUI.GetNameOfFocusedControl() != InputControlName))
			{
				GUI.FocusControl(InputControlName);
				focusInput = false;
			}

			if (syncTextEditor)
			{
				SyncTextEditor(current);
			}
			else if (edited != input)
			{
				input = edited;
				historyIndex = -1;
				RefreshSuggestions();
			}
		}

		/// <summary>
		/// Pushes text that was set in code into the field and drops the caret at the end of it with
		/// nothing selected, so a completed command can be typed onto rather than overtyped.
		/// </summary>
		private void SyncTextEditor(UnityEngine.Event current)
		{
			if (GUIUtility.keyboardControl != 0)
			{
				TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				editor.text = input;
				editor.cursorIndex = input.Length;
				editor.selectIndex = input.Length;
			}

			// The field selects its whole contents whenever it takes focus, so hold the caret in place
			// until a repaint has drawn it where it belongs.
			if (current.type == EventType.Repaint)
				syncTextEditor = false;
		}

		/// <summary>
		/// Draws the completion list, or the usage line for a command that has already been named.
		/// </summary>
		/// <returns>The height the strip took up above <paramref name="bottom"/>.</returns>
		private float DrawHint(float bottom)
		{
			if (suggestions.Count > 0)
			{
				float height = suggestions.Count * lineHeight;
				Rect area = new Rect(0f, bottom - height, Screen.width, height);
				GUI.DrawTexture(area, panelBackground);

				for (int i = 0; i < suggestions.Count; i++)
				{
					ConsoleCommand suggestion = suggestions[i];
					Rect row = new Rect(area.x, area.y + i * lineHeight, area.width, lineHeight);
					bool isSelected = i == selectedSuggestion;

					if (isSelected)
						GUI.DrawTexture(row, highlightBackground);

					DrawText(row, $"{suggestion.Name} {suggestion.Arguments}", isSelected ? Color.white : InfoColor, textStyle);
					DrawText(row, suggestion.Description, HintColor, rightAlignedStyle);
				}

				return height;
			}

			ConsoleCommand named = FindNamedCommand();
			if (named == null)
				return 0f;

			Rect usageArea = new Rect(0f, bottom - lineHeight, Screen.width, lineHeight);
			GUI.DrawTexture(usageArea, panelBackground);
			DrawText(usageArea, $"Usage: {named.Name} {named.Arguments}", HintColor, textStyle);

			return lineHeight;
		}

		private ConsoleCommand FindNamedCommand()
		{
			string typed = input.TrimStart();
			int space = typed.IndexOf(' ');
			string name = space < 0 ? typed : typed.Substring(0, space);
			if (name.Length == 0)
				return null;

			// A cheat command gives nothing away until cheats are on, not even its usage line.
			ConsoleCommand command = FindCommand(name);

			return IsAvailable(command) ? command : null;
		}

		private void DrawOutput(float bottom)
		{
			if (Output.Count == 0)
				return;

			float contentHeight = Output.Count * lineHeight;
			float height = Mathf.Min(contentHeight, Screen.height * MaxOutputScreenFraction);
			Rect area = new Rect(0f, bottom - height, Screen.width, height);
			Rect content = new Rect(0f, 0f, area.width - ScrollbarPadding, contentHeight);

			GUI.DrawTexture(area, panelBackground);

			// Follow the newest line, but leave the view alone while the player scrolls back through it.
			if (drawnOutputVersion != outputVersion)
			{
				drawnOutputVersion = outputVersion;
				scrollPosition.y = Mathf.Max(0f, contentHeight - height);
			}

			scrollPosition = GUI.BeginScrollView(area, scrollPosition, content, false, false);

			int first = Mathf.Max(0, Mathf.FloorToInt(scrollPosition.y / lineHeight));
			int last = Mathf.Min(Output.Count - 1, Mathf.CeilToInt((scrollPosition.y + height) / lineHeight));
			for (int i = first; i <= last; i++)
			{
				DrawText(new Rect(0f, i * lineHeight, content.width, lineHeight), Output[i].Text, Output[i].Color, textStyle);
			}

			GUI.EndScrollView();
		}

		private void DrawText(Rect area, string text, Color color, GUIStyle style)
		{
			Color previous = GUI.color;
			GUI.color = color;
			GUI.Label(area, text, style);
			GUI.color = previous;
		}

		private void SetOpen(bool open)
		{
			IsOpen = open;

			if (open)
			{
				focusInput = true;
				_lock = InputSourceIdentifier.Default.SetLock(PlayerLockState.NonPause);
				RefreshSuggestions();
			}
			else
			{
				suggestions.Clear();
				InputSourceIdentifier.Default.ReleaseLock(_lock);
				GUIUtility.keyboardControl = 0;
			}
		}

		/// <summary>
		/// Builds the styles the console draws with, sized against the current resolution.
		/// </summary>
		private void RefreshStyles()
		{
			if (textStyle != null && styledScreenHeight == Screen.height)
				return;

			styledScreenHeight = Screen.height;

			int fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height * 0.016f), 12, 30);
			lineHeight = fontSize + 8f;
			promptWidth = Mathf.RoundToInt(fontSize * 0.8f) + 8;

			textStyle = new GUIStyle(GUI.skin.label)
			{
				fontSize = fontSize,
				alignment = TextAnchor.MiddleLeft,
				wordWrap = false,
				clipping = TextClipping.Clip,
				padding = new RectOffset(8, 8, 0, 0)
			};
			textStyle.normal.textColor = Color.white;

			rightAlignedStyle = new GUIStyle(textStyle) { alignment = TextAnchor.MiddleRight };

			inputStyle = new GUIStyle(textStyle) { padding = new RectOffset(promptWidth, 8, 0, 0) };
			inputStyle.focused.textColor = Color.white;
			inputStyle.hover.textColor = Color.white;
			inputStyle.active.textColor = Color.white;
		}

		private static Texture2D CreateTexture(Color color)
		{
			Texture2D texture = new Texture2D(1, 1) { hideFlags = HideFlags.HideAndDontSave };
			texture.SetPixel(0, 0, color);
			texture.Apply();

			return texture;
		}

		/// <summary>
		/// Splits a line into a command name and its arguments, keeping quoted sections together.
		/// </summary>
		private static string[] Tokenize(string line)
		{
			List<string> tokens = new List<string>();
			StringBuilder token = new StringBuilder();
			bool quoted = false;

			foreach (char character in line)
			{
				if (character == '"')
				{
					quoted = !quoted;
					continue;
				}

				if (character == ' ' && !quoted)
				{
					if (token.Length > 0)
					{
						tokens.Add(token.ToString());
						token.Length = 0;
					}

					continue;
				}

				token.Append(character);
			}

			if (token.Length > 0)
				tokens.Add(token.ToString());

			return tokens.ToArray();
		}

		private struct ConsoleLine
		{
			public readonly string Text;
			public readonly Color Color;

			public ConsoleLine(string text, Color color)
			{
				Text = text;
				Color = color;
			}
		}
	}
}
