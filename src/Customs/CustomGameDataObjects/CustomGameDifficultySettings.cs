using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomGameDifficultySettings : CustomGameDataObject<GameDifficultySettings>
    {
	    // Base-Game Variables
        public virtual bool IsActive { get; protected set; } = false;
        public virtual float CustomersPerHourBase { get; protected set; } = 1f;
        public virtual float CustomersPerHourIncreasePerDay { get; protected set; } = 0.2f;
        public virtual float CustomerSideChance { get; protected set; } = 1f;
        public virtual float QueuePatienceTime { get; protected set; } = 100f;
        public virtual float QueuePatienceBoost { get; protected set; } = 10f;
        public virtual float CustomerStarterChance { get; protected set; } = 1f;
        public virtual float GroupDessertChance { get; protected set; } = 1f;
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            GameDifficultySettings result = ScriptableObject.CreateInstance<GameDifficultySettings>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "IsActive", IsActive);
            OverrideVariable(result, "CustomersPerHourBase", CustomersPerHourBase);
            OverrideVariable(result, "CustomersPerHourIncreasePerDay", CustomersPerHourIncreasePerDay);
            OverrideVariable(result, "CustomerSideChance", CustomerSideChance);
            OverrideVariable(result, "QueuePatienceTime", QueuePatienceTime);
            OverrideVariable(result, "QueuePatienceBoost", QueuePatienceBoost);
            OverrideVariable(result, "CustomerStarterChance", CustomerStarterChance);
            OverrideVariable(result, "GroupDessertChance", GroupDessertChance);

            gameDataObject = result;
        }
    }
}