using System;
using KitchenData;
using KitchenLib.Utils;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomThemeUnlock : CustomUnlock<ThemeUnlock>
    {
	    // Base-Game Variables
        public virtual bool IsPrimary { get; protected set; } = true;
        public virtual DecorationType Type { get; protected set; }
        public virtual ThemeUnlock ParentTheme1 { get; protected set; }
        public virtual ThemeUnlock ParentTheme2 { get; protected set; }
		
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            ThemeUnlock result = ScriptableObject.CreateInstance<ThemeUnlock>();
			
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "IsPrimary", IsPrimary);
            OverrideVariable(result, "Type", Type);
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
            ThemeUnlock result = (ThemeUnlock)gameDataObject;
            
            OverrideVariable(result, "ParentTheme1", ParentTheme1);
            OverrideVariable(result, "ParentTheme2", ParentTheme2);
            OverrideVariable(result, "AllowedFoods", AllowedFoods);
            OverrideVariable(result, "ForceFranchiseSetting", ForceFranchiseSetting);
            OverrideVariable(result, "HardcodedRequirements", HardcodedRequirements);
            OverrideVariable(result, "HardcodedBlockers", HardcodedBlockers);
        }
    }
}