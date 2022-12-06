using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomLevelUpgradeSet : CustomGameDataObject
    {
		public virtual List<LevelUpgrade> Upgrades { get { return new List<LevelUpgrade>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            LevelUpgradeSet result = ScriptableObject.CreateInstance<LevelUpgradeSet>();
			LevelUpgradeSet empty = ScriptableObject.CreateInstance<LevelUpgradeSet>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<LevelUpgradeSet>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

			if (empty.ID != ID) result.ID = ID;
            if (empty.Upgrades != Upgrades) result.Upgrades = Upgrades;

            gameDataObject = result ;
        }
    }
}