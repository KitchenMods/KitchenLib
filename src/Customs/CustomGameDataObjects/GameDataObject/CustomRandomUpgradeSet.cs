using System.Collections.Generic;
using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomRandomUpgradeSet : CustomGameDataObject<RandomUpgradeSet>
	{
		#region Base Game Variables
		
		public virtual UpgradeRewardTier Tier { get; protected set; }
		public virtual List<IUpgrade> Rewards { get; protected set; } = new();
		
		#endregion
		
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is not RandomUpgradeSet randomUpgradeSet)
				return;

			#region Apply Properties

			OverrideVariable(randomUpgradeSet, "Tier", Tier);
			OverrideVariable(randomUpgradeSet, "Rewards", Rewards);
				
			#endregion
			
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (gameDataObject is RandomUpgradeSet randomUpgradeSet)
			{
			}
		}
	}
}