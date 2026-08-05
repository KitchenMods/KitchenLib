using KitchenLib.UI.IMGUI;

namespace KitchenLib.Commands
{
	/// <summary>
	/// Empties the console output.
	/// </summary>
	public class ClearCommand : ConsoleCommand
	{
		/// <inheritdoc/>
		public override string Name => "Clear";

		/// <inheritdoc/>
		public override string Description => "Clears the console output.";

		/// <inheritdoc/>
		public override void Execute(string[] args)
		{
			DeveloperConsole.Clear();
		}
	}
}
