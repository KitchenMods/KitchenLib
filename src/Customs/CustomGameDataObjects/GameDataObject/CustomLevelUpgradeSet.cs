using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Customs
{
	public abstract class CustomLevelUpgradeSet : CustomGameDataObject<LevelUpgradeSet>
	{
		#region Base Game Variables

		public virtual List<LevelUpgrade> Upgrades { get; protected set; } = new List<LevelUpgrade>();
		
		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is LevelUpgradeSet levelUpgradeSet)
			{
				#region Apply Properties

				OverrideVariable(levelUpgradeSet, "Upgrades", Upgrades);
				
				#endregion
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (gameDataObject is LevelUpgradeSet levelUpgradeSet)
			{
			}
		}
	}
}