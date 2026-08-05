namespace KitchenLib.Commands
{
	/// <summary>
	/// Unlocks the commands marked as cheats for the rest of the session.
	/// </summary>
	public class EnableCheatsCommand : ConsoleCommand
	{
		/// <inheritdoc/>
		public override string Name => "Enable_Cheats";

		/// <inheritdoc/>
		public override string Description => "Enables Cheats";

		/// <inheritdoc/>
		public override void Execute(string[] args)
		{
			Main.cheatsEnabled = true;
			Log("Cheats enabled.");
		}
	}
}
