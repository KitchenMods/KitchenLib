using KitchenData;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomUnlock<T> : CustomLocalisedGameDataObject<T, UnlockInfo> where T : Unlock
    {
	    // Base-Game Variables
        public virtual Unlock.RewardLevel ExpReward { get; protected set; } = Unlock.RewardLevel.Medium;
        public virtual bool IsUnlockable { get; protected set; } = true;
        public virtual UnlockGroup UnlockGroup { get; protected set; }
        public virtual CardType CardType { get; protected set; }
        public virtual int MinimumFranchiseTier { get; protected set; } = 0;
        public virtual bool IsSpecificFranchiseTier { get; protected set; }
        public virtual DishCustomerChange CustomerMultiplier { get; protected set; }
        public virtual float SelectionBias { get; protected set; } = 0f;
        public virtual List<Unlock> HardcodedRequirements { get; protected set; } = new List<Unlock>();
        public virtual List<Unlock> HardcodedBlockers { get; protected set; } = new List<Unlock>();
		public virtual bool BlocksAllOtherFood { get; protected set; }
		public virtual List<Unlock> AllowedFoods { get; protected set; } = new List<Unlock>();
		public virtual RestaurantSetting ForceFranchiseSetting { get; protected set; }
        
		// KitchenLib Variables
        public virtual string IconOverride { get; protected set; }
        public virtual Color ColourOverride { get; protected set; }
	}

	public abstract class CustomUnlock : CustomUnlock<Unlock>
	{
	}
}