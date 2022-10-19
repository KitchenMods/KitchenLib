using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomGameDifficultySettings
	{
		public virtual string Name { get; internal set; }
		public virtual bool IsActive { get {return false;} }
        public virtual float CustomersPerHourBase { get {return 1f;}}
        public virtual float CustomersPerHourIncreasePerDay { get {return 0.2f;}}
        public virtual float CustomerSideChance { get {return 1f;}}
        public virtual float QueuePatienceTime { get {return 100f;}}
        public virtual float QueuePatienceBoost { get {return 10f;}}
        public virtual float CustomerStarterChance { get {return 1f;}}
        public virtual float GroupDessertChance { get {return 1f;}}

        public virtual int ID { get; internal set; }

		public GameDifficultySettings GameDifficultySettings{ get; internal set; }
		public string ModName { get; internal set; }
		public virtual int BaseGameDifficultySettingsId { get { return -1; } }

		public virtual void OnRegister(GameDifficultySettings gameDifficultySettings) {}
		
		public int GetHash()
		{
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}
