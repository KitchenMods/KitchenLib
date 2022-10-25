using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomRandomUpgradeSet : CustomGameDataObject
    {
		public virtual UpgradeRewardTier Tier { get; internal set; }
		public virtual List<IUpgrade> Rewards { get { return new List<IUpgrade>(); } }

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            RandomUpgradeSet result = new RandomUpgradeSet();
            RandomUpgradeSet empty = new RandomUpgradeSet();

            if (empty.ID != ID) result.ID = ID;
            if (empty.Tier != Tier) result.Tier = Tier;
            if (empty.Rewards != Rewards) result.Rewards = Rewards;

            gameDataObject = result;
        }
    }
}