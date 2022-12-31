using KitchenData;
using System;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public abstract class CustomUnlock : CustomLocalisedGameDataObject<UnlockInfo>
    {
		public virtual Unlock.RewardLevel ExpReward { get { return Unlock.RewardLevel.Medium; } }
		public virtual bool IsUnlockable { get { return true; } }
		public virtual UnlockGroup UnlockGroup { get; internal set; }
		public virtual CardType CardType { get; internal set; }
		public virtual int MinimumFranchiseTier { get { return 0; } }
		public virtual bool IsSpecificFranchiseTier { get; internal set; }
		public virtual DishCustomerChange CustomerMultiplier { get; internal set; }
		public virtual float SelectionBias { get { return 0f; } }
		public virtual List<Unlock> HardcodedRequirements { get; internal set; }
		public virtual List<Unlock> HardcodedBlockers { get; internal set; }
    }
}