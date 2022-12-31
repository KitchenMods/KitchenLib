using KitchenData;
using KitchenLib.Utils;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
            ThemeUnlock result = ScriptableObject.CreateInstance<ThemeUnlock>();
			ThemeUnlock empty = ScriptableObject.CreateInstance<ThemeUnlock>();

			if (BaseGameDataObjectID != -1)
				result = UnityEngine.Object.Instantiate(gameData.Get<ThemeUnlock>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

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

			FieldInfo hardcodedRequirements = ReflectionUtils.GetField<UnlockEffect>("HardcodedRequirements");
			FieldInfo hardcodedBlockers = ReflectionUtils.GetField<UnlockEffect>("HardcodedBlockers");

			if (hardcodedRequirements.GetValue(empty) != HardcodedRequirements) hardcodedRequirements.SetValue(result, HardcodedRequirements);
			if (hardcodedBlockers.GetValue(empty) != HardcodedBlockers) hardcodedBlockers.SetValue(result, HardcodedBlockers);
			if (empty.Info != Info) result.Info = Info;

			gameDataObject = result;
        }
    }
}