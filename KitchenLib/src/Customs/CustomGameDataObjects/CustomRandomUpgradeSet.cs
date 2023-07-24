using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomRandomUpgradeSet : CustomGameDataObject<RandomUpgradeSet>
    {
        public virtual UpgradeRewardTier Tier { get; protected set; }
        public virtual List<IUpgrade> Rewards { get; protected set; } = new List<IUpgrade>();

        //private static readonly RandomUpgradeSet empty = ScriptableObject.CreateInstance<RandomUpgradeSet>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            RandomUpgradeSet result = ScriptableObject.CreateInstance<RandomUpgradeSet>();

			Main.LogDebug($"[CustomRandomUpgradeSet.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<RandomUpgradeSet>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Tier != Tier) result.Tier = Tier;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            RandomUpgradeSet result = (RandomUpgradeSet)gameDataObject;

			Main.LogDebug($"[CustomRandomUpgradeSet.AttachDependentProperties] [1.1] Converting Base");

			if (result.Rewards != Rewards) result.Rewards = Rewards;
        }
    }
}