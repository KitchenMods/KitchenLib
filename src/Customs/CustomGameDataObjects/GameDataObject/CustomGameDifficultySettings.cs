using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomGameDifficultySettings : CustomGameDataObject<GameDifficultySettings>
	{
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is GameDifficultySettings gameDifficultySettings)
			{
				#region Apply Properties

				OverrideVariable(gameDifficultySettings, "IsActive", IsActive);
				OverrideVariable(gameDifficultySettings, "CustomersPerHourBase", CustomersPerHourBase);
				OverrideVariable(gameDifficultySettings, "CustomersPerHourIncreasePerDay", CustomersPerHourIncreasePerDay);
				OverrideVariable(gameDifficultySettings, "CustomerSideChance", CustomerSideChance);
				OverrideVariable(gameDifficultySettings, "QueuePatienceTime", QueuePatienceTime);
				OverrideVariable(gameDifficultySettings, "QueuePatienceBoost", QueuePatienceBoost);
				OverrideVariable(gameDifficultySettings, "CustomerStarterChance", CustomerStarterChance);
				OverrideVariable(gameDifficultySettings, "GroupDessertChance", GroupDessertChance);

				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is GameDifficultySettings gameDifficultySettings)
			{
			}
		}

		#region Base Game Variables

		public virtual bool IsActive { get; protected set; }
		public virtual float CustomersPerHourBase { get; protected set; } = 1f;
		public virtual float CustomersPerHourIncreasePerDay { get; protected set; } = 0.2f;
		public virtual float CustomerSideChance { get; protected set; } = 1f;
		public virtual float QueuePatienceTime { get; protected set; } = 100f;
		public virtual float QueuePatienceBoost { get; protected set; } = 10f;
		public virtual float CustomerStarterChance { get; protected set; } = 1f;
		public virtual float GroupDessertChance { get; protected set; } = 1f;

		#endregion
	}
}