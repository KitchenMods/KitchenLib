using System.Collections.Generic;
using KitchenData;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomFranchiseUpgrade : CustomGenericLocalisation<FranchiseUpgrade>
	{

		#region Base Game Variables

		public virtual int MaximumUpgradeCount { get; protected set; } = 1;
		public virtual List<IFranchiseUpgrade> Upgrades { get; protected set; } = new();
		public virtual GameObject Prefab { get; protected set; }

		#endregion
		
		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is not FranchiseUpgrade franchiseUpgrade) return;

			#region Apply Properties

			OverrideVariable(franchiseUpgrade, "MaximumUpgradeCount", MaximumUpgradeCount);
			OverrideVariable(franchiseUpgrade, "Upgrades", Upgrades);
			OverrideVariable(franchiseUpgrade, "Prefab", Prefab);

			#endregion
		}
	}
}