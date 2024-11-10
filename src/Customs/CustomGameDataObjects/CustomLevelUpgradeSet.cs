using KitchenData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomLevelUpgradeSet : CustomGameDataObject<LevelUpgradeSet>
    {
	    // Base-Game Variables
        public virtual List<LevelUpgrade> Upgrades { get; protected set; } = new List<LevelUpgrade>();
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            LevelUpgradeSet result = ScriptableObject.CreateInstance<LevelUpgradeSet>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Upgrades", Upgrades);

            gameDataObject = result;
        }
    }
}