using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomLevelUpgradeSet : CustomGameDataObject
	{
		public virtual List<LevelUpgrade> Upgrades { get; protected set; } = new List<LevelUpgrade>();

		//private static readonly LevelUpgradeSet empty = ScriptableObject.CreateInstance<LevelUpgradeSet>();
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			LevelUpgradeSet result = ScriptableObject.CreateInstance<LevelUpgradeSet>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<LevelUpgradeSet>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (result.ID != ID) result.ID = ID;
			if (result.Upgrades != Upgrades) result.Upgrades = Upgrades;

			gameDataObject = result;
		}
	}
}