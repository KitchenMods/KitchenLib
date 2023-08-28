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

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			FranchiseUpgrade result = ScriptableObject.CreateInstance<FranchiseUpgrade>();

			Main.LogDebug($"[CustomFranchiseUpgrade.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<FranchiseUpgrade>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.MaximumUpgradeCount != MaximumUpgradeCount) result.MaximumUpgradeCount = MaximumUpgradeCount;
			if (result.Upgrades != Upgrades) result.Upgrades = Upgrades;
			if (result.Info != Info) result.Info = Info;

			if (InfoList.Count > 0)
			{
				result.Info = new LocalisationObject<BasicInfo>();
				foreach ((Locale, BasicInfo) info in InfoList)
					result.Info.Add(info.Item1, info.Item2);
			}
			gameDataObject = result;
		}
	}
}
