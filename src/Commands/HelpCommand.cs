using System;
using System.Collections.Generic;
using KitchenLib.UI.IMGUI;

namespace KitchenLib.Commands
{
	/// <summary>
	/// Lists the registered console commands a page at a time, or describes a single one.
	/// </summary>
	public class HelpCommand : ConsoleCommand
	{
		private const int CommandsPerPage = 5;

		/// <inheritdoc/>
		public override string Name => "Help";

		/// <inheritdoc/>
		public override string Description => "Lists every command a page at a time, or describes one of them.";

		/// <inheritdoc/>
		public override string Arguments => "[Page|Command]";

		/// <inheritdoc/>
		public override void Execute(string[] args)
		{
			if (args.Length == 0)
			{
				ListCommands(1);
				return;
			}

			// Command names are letters only, so anything that parses as a number is a page.
			int page;
			if (int.TryParse(args[0], out page))
			{
				ListCommands(page);
				return;
			}

			DescribeCommand(args[0]);
		}

		private void ListCommands(int page)
		{
			List<ConsoleCommand> commands = ListedCommands();
			if (commands.Count == 0)
			{
				Log("There are no commands to list.");
				return;
			}

			int pageCount = (commands.Count + CommandsPerPage - 1) / CommandsPerPage;
			if (page < 1 || page > pageCount)
			{
				LogError($"There is no page {page}, {Name} has {pageCount} of them.");
				return;
			}

			int first = (page - 1) * CommandsPerPage;
			int last = Math.Min(first + CommandsPerPage, commands.Count);

			Log($"{commands.Count} commands available, page {page} of {pageCount}:");
			for (int i = first; i < last; i++)
			{
				Log($"  {commands[i].Name} {commands[i].Arguments} - {commands[i].Description}");
			}

			if (page < pageCount)
				Log($"  Type '{Name} {page + 1}' for the next page.");
		}

		private void DescribeCommand(string name)
		{
			// Hidden cheat commands are reported as missing rather than described, so that help cannot be
			// used to go looking for them.
			ConsoleCommand command = DeveloperConsole.FindCommand(name);
			if (!DeveloperConsole.IsAvailable(command))
			{
				LogError($"Unknown Command: {name}");
				return;
			}

			Log($"{command.Name} {command.Arguments}");
			Log($"  {command.Description}");
		}

		private static List<ConsoleCommand> ListedCommands()
		{
			List<ConsoleCommand> commands = new List<ConsoleCommand>();
			foreach (ConsoleCommand command in DeveloperConsole.Commands)
			{
				if (!command.DoNotAutocomplete && DeveloperConsole.IsAvailable(command))
					commands.Add(command);
			}

			commands.Sort((first, second) => string.Compare(first.Name, second.Name, StringComparison.OrdinalIgnoreCase));

			return commands;
		}
	}
}
