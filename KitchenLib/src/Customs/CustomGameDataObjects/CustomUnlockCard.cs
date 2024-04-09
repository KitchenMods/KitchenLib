using System;
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
	    // Base-Game Variables
        public virtual List<UnlockEffect> Effects { get; protected set; } = new List<UnlockEffect>();

        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            UnlockCard result = ScriptableObject.CreateInstance<UnlockCard>();

            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Effects", Effects);
            OverrideVariable(result, "ExpReward", ExpReward);
            OverrideVariable(result, "IsUnlockable", IsUnlockable);
            OverrideVariable(result, "UnlockGroup", UnlockGroup);
            OverrideVariable(result, "CardType", CardType);
            OverrideVariable(result, "MinimumFranchiseTier", MinimumFranchiseTier);
            OverrideVariable(result, "IsSpecificFranchiseTier", IsSpecificFranchiseTier);
            OverrideVariable(result, "CustomerMultiplier", CustomerMultiplier);
            OverrideVariable(result, "SelectionBias", SelectionBias);
            OverrideVariable(result, "BlocksAllOtherFood", BlocksAllOtherFood);
            OverrideVariable(result, "Info", Info);

            if (InfoList.Count > 0)
            {
	            SetupLocalisation<UnlockInfo>(InfoList, ref result.Info);
            }

            if (!string.IsNullOrEmpty(IconOverride))
            {
	            Main.LogDebug($"Assigning : {IconOverride} >> IconOverride");
	            UnlockOverrides.AddIconOverride(result.ID, IconOverride);
            }

            if (ColourOverride != new Color())
            {
	            Main.LogDebug($"Assigning : {ColourOverride} >> ColourOverride");
	            UnlockOverrides.AddColourOverride(result.ID, ColourOverride);
            }
            
            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            UnlockCard result = (UnlockCard)gameDataObject;
            
			OverrideVariable(result, "HardcodedRequirements", HardcodedRequirements);
			OverrideVariable(result, "HardcodedBlockers", HardcodedBlockers);
            OverrideVariable(result, "AllowedFoods", AllowedFoods);
            OverrideVariable(result, "ForceFranchiseSetting", ForceFranchiseSetting);
		}
    }
}