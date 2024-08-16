using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomFranchiseUpgrade : CustomGenericLocalisation<RestaurantSetting>
	{
		// Base-Game Variables
		public virtual int MaximumUpgradeCount { get; protected set; } = 1;
		public virtual List<IFranchiseUpgrade> Upgrades { get; protected set; } = new List<IFranchiseUpgrade>();
		public virtual GameObject Prefab { get; protected set; }

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			FranchiseUpgrade result = ScriptableObject.CreateInstance<FranchiseUpgrade>();
			
			OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "MaximumUpgradeCount", MaximumUpgradeCount);
            OverrideVariable(result, "Upgrades", Upgrades);
            OverrideVariable(result, "Info", Info);
            OverrideVariable(result, "Prefab", Prefab);

			if (InfoList.Count > 0)
			{
				SetupLocalisation<BasicInfo>(InfoList, ref result.Info);
			}
			gameDataObject = result;
		}
	}
}
