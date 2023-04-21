using KitchenData;
using KitchenLib.Utils;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomThemeUnlock : CustomUnlock<ThemeUnlock>
    {
        public virtual bool IsPrimary { get; protected set; } = true;
        public virtual DecorationType Type { get; protected set; }
        public virtual ThemeUnlock ParentTheme1 { get; protected set; }
        public virtual ThemeUnlock ParentTheme2 { get; protected set; }

        //private static readonly ThemeUnlock empty = ScriptableObject.CreateInstance<ThemeUnlock>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ThemeUnlock result = ScriptableObject.CreateInstance<ThemeUnlock>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<ThemeUnlock>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.IsPrimary != IsPrimary) result.IsPrimary = IsPrimary;
            if (result.Type != Type) result.Type = Type;

            if (result.ExpReward != ExpReward) result.ExpReward = ExpReward;
            if (result.IsUnlockable != IsUnlockable) result.IsUnlockable = IsUnlockable;
            if (result.UnlockGroup != UnlockGroup) result.UnlockGroup = UnlockGroup;
            if (result.CardType != CardType) result.CardType = CardType;
            if (result.MinimumFranchiseTier != MinimumFranchiseTier) result.MinimumFranchiseTier = MinimumFranchiseTier;
            if (result.IsSpecificFranchiseTier != IsSpecificFranchiseTier) result.IsSpecificFranchiseTier = IsSpecificFranchiseTier;
            if (result.CustomerMultiplier != CustomerMultiplier) result.CustomerMultiplier = CustomerMultiplier;
            if (result.SelectionBias != SelectionBias) result.SelectionBias = SelectionBias;

            if (result.Info != Info) result.Info = Info;

            if (InfoList.Count > 0)
            {
                result.Info = new LocalisationObject<UnlockInfo>();
                foreach ((Locale, UnlockInfo) info in InfoList)
                    result.Info.Add(info.Item1, info.Item2);
            }

            if (!string.IsNullOrEmpty(IconOverride))
                UnlockOverrides.AddIconOverride(result.ID, IconOverride);
            if (ColourOverride != new Color())
				UnlockOverrides.AddColourOverride(result.ID, ColourOverride);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            ThemeUnlock result = (ThemeUnlock)gameDataObject;

            if (result.ParentTheme1 != ParentTheme1) result.ParentTheme1 = ParentTheme1;
            if (result.ParentTheme2 != ParentTheme2) result.ParentTheme2 = ParentTheme2;

            FieldInfo hardcodedRequirements = ReflectionUtils.GetField<Unlock>("HardcodedRequirements");
            FieldInfo hardcodedBlockers = ReflectionUtils.GetField<Unlock>("HardcodedBlockers");

            if (hardcodedRequirements.GetValue(result) != HardcodedRequirements) hardcodedRequirements.SetValue(result, HardcodedRequirements);
            if (hardcodedBlockers.GetValue(result) != HardcodedBlockers) hardcodedBlockers.SetValue(result, HardcodedBlockers);
        }
    }
}