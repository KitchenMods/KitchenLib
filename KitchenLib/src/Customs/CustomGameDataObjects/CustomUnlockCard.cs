using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomUnlockCard : CustomUnlock<UnlockCard>
    {
        public virtual List<UnlockEffect> Effects { get; protected set; } = new List<UnlockEffect>();

        //private static readonly UnlockCard empty = ScriptableObject.CreateInstance<UnlockCard>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            UnlockCard result = ScriptableObject.CreateInstance<UnlockCard>();

			Main.LogDebug($"[CustomUnlockCard.Convert] [1.1] Convering Base");

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<UnlockCard>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Effects != Effects) result.Effects = Effects;

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

			Main.LogDebug($"[CustomUnlockCard.Convert] [1.2] Convering Overrides");

			if (!string.IsNullOrEmpty(IconOverride))
				UnlockOverrides.AddIconOverride(result.ID, IconOverride);
            if (ColourOverride != new Color())
				UnlockOverrides.AddColourOverride(result.ID, ColourOverride);

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            UnlockCard result = (UnlockCard)gameDataObject;

			Main.LogDebug($"[CustomUnlockCard.AttachDependentProperties] [1.1] Convering Base");

			FieldInfo hardcodedRequirements = ReflectionUtils.GetField<Unlock>("HardcodedRequirements");
            FieldInfo hardcodedBlockers = ReflectionUtils.GetField<Unlock>("HardcodedBlockers");

            if (hardcodedRequirements.GetValue(result) != HardcodedRequirements) hardcodedRequirements.SetValue(result, HardcodedRequirements);
            if (hardcodedBlockers.GetValue(result) != HardcodedBlockers) hardcodedBlockers.SetValue(result, HardcodedBlockers);
        }
    }
}