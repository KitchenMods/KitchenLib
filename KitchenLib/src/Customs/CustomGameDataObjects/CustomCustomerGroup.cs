using KitchenData;
using KitchenLib.Utils;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomCustomerGroup : CustomUnlockCard
	{
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
	        CustomerGroup result = ScriptableObject.CreateInstance<CustomerGroup>();
	        
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
			CustomerGroup result = (CustomerGroup)gameDataObject;
			
			OverrideVariable(result, "HardcodedRequirements", HardcodedRequirements);
			OverrideVariable(result, "HardcodedBlockers", HardcodedBlockers);
			OverrideVariable(result, "AllowedFoods", AllowedFoods);
			OverrideVariable(result, "ForceFranchiseSetting", ForceFranchiseSetting);
		}
	}
}