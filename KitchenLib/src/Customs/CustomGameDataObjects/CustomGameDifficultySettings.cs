using KitchenData;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomGameDifficultySettings : CustomGameDataObject<GameDifficultySettings>
    {
        public virtual bool IsActive { get; protected set; } = false;
        public virtual float CustomersPerHourBase { get; protected set; } = 1f;
        public virtual float CustomersPerHourIncreasePerDay { get; protected set; } = 0.2f;
        public virtual float CustomerSideChance { get; protected set; } = 1f;
        public virtual float QueuePatienceTime { get; protected set; } = 100f;
        public virtual float QueuePatienceBoost { get; protected set; } = 10f;
        public virtual float CustomerStarterChance { get; protected set; } = 1f;
        public virtual float GroupDessertChance { get; protected set; } = 1f;

        //private static readonly GameDifficultySettings empty = ScriptableObject.CreateInstance<GameDifficultySettings>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            GameDifficultySettings result = ScriptableObject.CreateInstance<GameDifficultySettings>();

			Main.LogDebug($"[CustomGameDifficultySettings.Convert] [1.1] Converting Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<GameDifficultySettings>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.IsActive != IsActive) result.IsActive = IsActive;
            if (result.CustomersPerHourBase != CustomersPerHourBase) result.CustomersPerHourBase = CustomersPerHourBase;
            if (result.CustomersPerHourIncreasePerDay != CustomersPerHourIncreasePerDay) result.CustomersPerHourIncreasePerDay = CustomersPerHourIncreasePerDay;
            if (result.CustomerSideChance != CustomerSideChance) result.CustomerSideChance = CustomerSideChance;
            if (result.QueuePatienceTime != QueuePatienceTime) result.QueuePatienceTime = QueuePatienceTime;
            if (result.QueuePatienceBoost != QueuePatienceBoost) result.QueuePatienceBoost = QueuePatienceBoost;
            if (result.CustomerStarterChance != CustomerStarterChance) result.CustomerStarterChance = CustomerStarterChance;
            if (result.GroupDessertChance != GroupDessertChance) result.GroupDessertChance = GroupDessertChance;

            gameDataObject = result;
        }
    }
}