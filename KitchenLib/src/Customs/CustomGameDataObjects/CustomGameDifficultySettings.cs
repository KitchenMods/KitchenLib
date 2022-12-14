using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomGameDifficultySettings : CustomGameDataObject
    {
        public virtual bool IsActive { get { return false; } }
        public virtual float CustomersPerHourBase { get { return 1f; } }
        public virtual float CustomersPerHourIncreasePerDay { get { return 0.2f; } }
        public virtual float CustomerSideChance { get { return 1f; } }
        public virtual float QueuePatienceTime { get { return 100f; } }
        public virtual float QueuePatienceBoost { get { return 10f; } }
        public virtual float CustomerStarterChance { get { return 1f; } }
        public virtual float GroupDessertChance { get { return 1f; } }

        private static readonly GameDifficultySettings empty = ScriptableObject.CreateInstance<GameDifficultySettings>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            GameDifficultySettings result = ScriptableObject.CreateInstance<GameDifficultySettings>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<GameDifficultySettings>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.IsActive != IsActive) result.IsActive = IsActive;
            if (empty.CustomersPerHourBase != CustomersPerHourBase) result.CustomersPerHourBase = CustomersPerHourBase;
            if (empty.CustomersPerHourIncreasePerDay != CustomersPerHourIncreasePerDay) result.CustomersPerHourIncreasePerDay = CustomersPerHourIncreasePerDay;
            if (empty.CustomerSideChance != CustomerSideChance) result.CustomerSideChance = CustomerSideChance;
            if (empty.QueuePatienceTime != QueuePatienceTime) result.QueuePatienceTime = QueuePatienceTime;
            if (empty.QueuePatienceBoost != QueuePatienceBoost) result.QueuePatienceBoost = QueuePatienceBoost;
            if (empty.CustomerStarterChance != CustomerStarterChance) result.CustomerStarterChance = CustomerStarterChance;
            if (empty.GroupDessertChance != GroupDessertChance) result.GroupDessertChance = GroupDessertChance;

            gameDataObject = result;
        }
    }
}