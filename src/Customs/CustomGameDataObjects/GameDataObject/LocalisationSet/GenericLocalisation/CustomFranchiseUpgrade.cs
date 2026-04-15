using System.Collections.Generic;
using KitchenData;
using TMPro;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomFranchiseUpgrade : CustomGenericLocalisation<FranchiseUpgrade>
	{
		#region Base Game Variables
		
		public virtual int MaximumUpgradeCount { get; protected set; }
		public virtual List<IFranchiseUpgrade> Upgrades { get; protected set; }
		public virtual GameObject Prefab { get; protected set; }
		
		#endregion
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (!(gameDataObject is FranchiseUpgrade franchiseUpgrade))
				return;
			
			#region Apply Properties

			OverrideVariable(franchiseUpgrade, "MaximumUpgradeCount", MaximumUpgradeCount);
			OverrideVariable(franchiseUpgrade, "Upgrades", Upgrades);
			OverrideVariable(franchiseUpgrade, "Prefab", Prefab);
				
			#endregion
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);
			
			if (!(gameDataObject is FranchiseUpgrade franchiseUpgrade))
				return;
		}
	}
}