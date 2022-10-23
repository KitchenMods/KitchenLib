using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomLevelUpgradeSet : CustomGameDataObject
    {
		public virtual List<LevelUpgrade> Upgrades { get { return new List<LevelUpgrade>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            LevelUpgradeSet result = new LevelUpgradeSet();
            LevelUpgradeSet empty = new LevelUpgradeSet();

            if (empty.ID != ID) result.ID = ID;
            if (empty.Upgrades != Upgrades) result.Upgrades = Upgrades;

            gameDataObject = result ;
        }
    }
}