using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomRandomUpgradeSet : CustomGameDataObject
    {
        public virtual UpgradeRewardTier Tier { get; internal set; }
        public virtual List<IUpgrade> Rewards { get { return new List<IUpgrade>(); } }

        private static readonly RandomUpgradeSet empty = ScriptableObject.CreateInstance<RandomUpgradeSet>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            RandomUpgradeSet result = ScriptableObject.CreateInstance<RandomUpgradeSet>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<RandomUpgradeSet>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.Tier != Tier) result.Tier = Tier;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameDataObject gameDataObject)
        {
            RandomUpgradeSet result = (RandomUpgradeSet)gameDataObject;

            if (empty.Rewards != Rewards) result.Rewards = Rewards;
        }
    }
}