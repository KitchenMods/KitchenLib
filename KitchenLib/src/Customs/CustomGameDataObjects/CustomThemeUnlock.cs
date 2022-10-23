using KitchenData;

namespace KitchenLib.Customs
{
    public abstract class CustomThemeUnlock : CustomUnlock
    {
		public virtual bool IsPrimary { get { return true; } }
		public virtual DecorationType Type { get; internal set; }
		public virtual ThemeUnlock ParentTheme1 { get; internal set; }
		public virtual ThemeUnlock ParentTheme2 { get; internal set; }
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ThemeUnlock result = new ThemeUnlock();
            ThemeUnlock empty = new ThemeUnlock();
            
            if (empty.ID != ID) result.ID = ID;
			if (empty.IsPrimary != IsPrimary) result.IsPrimary = IsPrimary;
			if (empty.Type != Type) result.Type = Type;
			if (empty.ParentTheme1 != ParentTheme1) result.ParentTheme1 = ParentTheme1;
			if (empty.ParentTheme2 != ParentTheme2) result.ParentTheme2 = ParentTheme2;

			if (empty.ExpReward != ExpReward) result.ExpReward = ExpReward;
			if (empty.IsUnlockable != IsUnlockable) result.IsUnlockable = IsUnlockable;
			if (empty.UnlockGroup != UnlockGroup) result.UnlockGroup = UnlockGroup;
			if (empty.CardType != CardType) result.CardType = CardType;
			if (empty.MinimumFranchiseTier != MinimumFranchiseTier) result.MinimumFranchiseTier = MinimumFranchiseTier;
			if (empty.IsSpecificFranchiseTier != IsSpecificFranchiseTier) result.IsSpecificFranchiseTier = IsSpecificFranchiseTier;
			if (empty.CustomerMultiplier != CustomerMultiplier) result.CustomerMultiplier = CustomerMultiplier;
			if (empty.SelectionBias != SelectionBias) result.SelectionBias = SelectionBias;
			if (empty.Requires != Requires) result.Requires = Requires;
			if (empty.BlockedBy != BlockedBy) result.BlockedBy = BlockedBy;

            gameDataObject = result;
        }
    }
}