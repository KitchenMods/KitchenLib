using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomRandomUpgradeSet : CustomGameDataObject<RandomUpgradeSet>
    {
	    // Base-Game Variables
        public virtual UpgradeRewardTier Tier { get; protected set; }
        public virtual List<IUpgrade> Rewards { get; protected set; } = new List<IUpgrade>();

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            RandomUpgradeSet result = ScriptableObject.CreateInstance<RandomUpgradeSet>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Tier", Tier);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            RandomUpgradeSet result = (RandomUpgradeSet)gameDataObject;

			OverrideVariable(result, "Rewards", Rewards);
        }
    }
}