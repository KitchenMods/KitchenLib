using KitchenLib.UI.IMGUI;

namespace KitchenLib.Commands
{
	/// <summary>
	/// A command that can be typed into the <see cref="DeveloperConsole"/>.
	/// Register implementations with <see cref="DeveloperConsole.RegisterCommand"/>.
	/// </summary>
	public abstract class ConsoleCommand
	{
		/// <summary>
		/// The word that runs this command, which may only contain letters and underscores. Matched case
		/// insensitively, and used for auto completion. Digits are kept out of names so that a numeric
		/// argument, such as the page the help command takes, can never be mistaken for one.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// A single line describing what the command does, shown by the help command.
		/// </summary>
		public abstract string Description { get; }

		/// <summary>
		/// The argument template shown while typing, for example "&lt;Appliance ID&gt; &lt;Count&gt;".
		/// Angle brackets mark required arguments, square brackets optional ones.
		/// </summary>
		public virtual string Arguments => "";

		/// <summary>
		/// How many arguments the command has to be given. The console prints the usage line and does not
		/// run a command typed with fewer, so <see cref="Execute"/> can read that many without checking.
		/// </summary>
		public virtual int MinimumArguments => 0;

		/// <summary>
		/// When true the command is left out of the suggestion list, so it can only be run by typing its
		/// name out in full. It is still listed by the help command.
		/// </summary>
		public virtual bool DoNotAutocomplete => false;

		/// <summary>
		/// When true the command is refused unless the game belongs to the player running it, which covers
		/// both hosting a multiplayer game and playing on your own. Use it for commands that change the
		/// world, since a client's changes are overwritten by the host that owns the simulation.
		/// </summary>
		public virtual bool IsHostOnly => false;

		/// <summary>
		/// When true the command stays out of sight and refuses to run until cheats have been enabled,
		/// so that commands which trivialise the game cannot be stumbled into.
		/// </summary>
		public virtual bool IsCheat => false;

		/// <summary>
		/// Runs the command.
		/// </summary>
		/// <param name="args">The arguments typed after the command name, already split on spaces with quoted sections kept together.</param>
		public abstract void Execute(string[] args);

		/// <summary>Writes a line to the console output.</summary>
		protected void Log(string message) => DeveloperConsole.Log(message);

		/// <summary>Writes a highlighted warning line to the console output.</summary>
		protected void LogWarning(string message) => DeveloperConsole.LogWarning(message);

		/// <summary>Writes a highlighted error line to the console output.</summary>
		protected void LogError(string message) => DeveloperConsole.LogError(message);

		/// <summary>Writes this command's usage line to the console output.</summary>
		public void LogUsage() => DeveloperConsole.LogWarning($"Usage: {Name} {Arguments}");
	}
}
