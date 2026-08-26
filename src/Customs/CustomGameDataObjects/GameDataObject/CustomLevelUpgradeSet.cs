using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomLevelUpgradeSet : CustomGameDataObject<LevelUpgradeSet>
	{
		#region Base Game Variables

		public virtual List<LevelUpgrade> Upgrades { get; protected set; } = new();

		#endregion
		
		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not LevelUpgradeSet levelUpgradeSet) return;

			#region Apply Properties

			OverrideVariable(levelUpgradeSet, "Upgrades", Upgrades);

			#endregion
		}
	}
}