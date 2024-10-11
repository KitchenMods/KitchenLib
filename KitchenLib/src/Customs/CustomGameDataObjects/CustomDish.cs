using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomDish : CustomUnlock<Dish>
    {
	    // Base-Game Variables
        public virtual DishType Type { get; protected set; }
		public virtual int Difficulty { get; protected set; }
		public virtual Item UnlockItemOverride { get; protected set; }
		public virtual bool HideInfoPanel { get; protected set; }
        public virtual string AchievementName { get; protected set; }
        public virtual List<Dish.MenuItem> ResultingMenuItems { get; protected set; } = new List<Dish.MenuItem>();
        public virtual HashSet<Dish.IngredientUnlock> IngredientsUnlocks { get; protected set; } = new HashSet<Dish.IngredientUnlock>();
		public virtual List<Dish> AlsoAddRecipes { get; protected set; } = new List<Dish>();
        public virtual HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks { get; protected set; } = new HashSet<Dish.IngredientUnlock>();
        public virtual List<string> StartingNameSet { get; protected set; } = new List<string>();
        public virtual HashSet<Item> MinimumIngredients { get; protected set; } = new HashSet<Item>();
        public virtual HashSet<Process> RequiredProcesses { get; protected set; } = new HashSet<Process>();
        public virtual HashSet<Item> BlockProviders { get; protected set; } = new HashSet<Item>();

        [Obsolete("Please use HardcodedRequirements")]
        public virtual HashSet<Dish> PrerequisiteDishesEditor { get; protected set; } = new HashSet<Dish>();
        public virtual GameObject IconPrefab { get; protected set; }
        public virtual GameObject DisplayPrefab { get; protected set; }
        
        public virtual bool IsMainThatDoesNotNeedPlates { get; protected set; }
        
        // KitchenLib Variables
		public virtual Item RequiredDishItem { get; protected set; }
		public virtual bool RequiredNoDishItem { get; protected set; } = false;

        public virtual bool IsAvailableAsLobbyOption { get; protected set; } = false;
        public virtual bool DestroyAfterModUninstall { get; protected set; } = true;
        public virtual Dictionary<Locale, string> Recipe { get; protected set; } = new Dictionary<Locale, string>();
        public virtual bool BypassMainRequirementsCheck { get; protected set; } = false;

        
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Dish result = ScriptableObject.CreateInstance<Dish>();
            
            OverrideVariable(result, "ID", ID);
            OverrideVariable(result, "Type", Type);
            OverrideVariable(result, "Difficulty", Difficulty);
            OverrideVariable(result, "HideInfoPanel", HideInfoPanel);
            OverrideVariable(result, "AchievementName", AchievementName);
            OverrideVariable(result, "StartingNameSet", StartingNameSet);
            OverrideVariable(result, "IconPrefab", IconPrefab);
            OverrideVariable(result, "DisplayPrefab", DisplayPrefab);
            OverrideVariable(result, "ExpReward", ExpReward);
            OverrideVariable(result, "IsUnlockable", IsUnlockable);
            OverrideVariable(result, "UnlockGroup", UnlockGroup);
            OverrideVariable(result, "CardType", CardType);
            OverrideVariable(result, "MinimumFranchiseTier", MinimumFranchiseTier);
            OverrideVariable(result, "IsSpecificFranchiseTier", IsSpecificFranchiseTier);
            OverrideVariable(result, "CustomerMultiplier", CustomerMultiplier);
            OverrideVariable(result, "SelectionBias", SelectionBias);
            OverrideVariable(result, "BlocksAllOtherFood", BlocksAllOtherFood);
            OverrideVariable(result, "IsMainThatDoesNotNeedPlates", IsMainThatDoesNotNeedPlates);
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

            if (RequiredNoDishItem)
            {
	            result.IsMainThatDoesNotNeedPlates = RequiredNoDishItem;
            }

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Dish result = (Dish)gameDataObject;
			
			OverrideVariable(result, "UnlockItemOverride", UnlockItemOverride);
			OverrideVariable(result, "AlsoAddRecipes", AlsoAddRecipes);
			OverrideVariable(result, "ExtraOrderUnlocks", ExtraOrderUnlocks);
			OverrideVariable(result, "MinimumIngredients", MinimumIngredients);
			OverrideVariable(result, "RequiredProcesses", RequiredProcesses);
			OverrideVariable(result, "BlockProviders", BlockProviders);
			OverrideVariable(result, "AllowedFoods", AllowedFoods);
			OverrideVariable(result, "ForceFranchiseSetting", ForceFranchiseSetting);
			OverrideVariable(result, "ResultingMenuItems", ResultingMenuItems);
			OverrideVariable(result, "IngredientsUnlocks", IngredientsUnlocks);
			OverrideVariable(result, "PrerequisiteDishesEditor", PrerequisiteDishesEditor);
			OverrideVariable(result, "HardcodedRequirements", HardcodedRequirements);
			OverrideVariable(result, "HardcodedBlockers", HardcodedBlockers);

			if (RequiredDishItem != null)
			{
				Main.LogDebug($"Adding : {RequiredDishItem} >> MinimumIngredients");
				result.MinimumIngredients.Add(RequiredDishItem);
			}

			if (result.Type == DishType.Main && HardcodedRequirements.Count == 0 && !BypassMainRequirementsCheck)
			{
				Main.LogDebug($"Assigning : {DishType.Base} >> Type");
				result.Type = DishType.Base;
			}
			
			
			CustomDish customDish = (CustomDish)GDOUtils.GetCustomGameDataObject(result.ID);
			string fallback = "";
			foreach (var recipe in customDish.Recipe)
			{
				if (recipe.Key == Locale.English)
				{
					fallback = recipe.Value;
				}
				RecipeInfo info = gameData.GlobalLocalisation.Recipes.Info.Get(recipe.Key);
				if (info != null)
				{
					if (!info.Text.ContainsKey(result))
					{
						info.Text.Add(result, recipe.Value);
					}
				}
			}
						
			if (!string.IsNullOrEmpty(fallback))
			{
				foreach (Locale locale in Enum.GetValues(typeof(Locale)))
				{
					RecipeInfo info = gameData.GlobalLocalisation.Recipes.Info.Get(locale);
					if (!info.Text.ContainsKey(result))
					{
						info.Text.Add(result, fallback);
					}
				}
			}
        }
        public override void OnRegister(GameDataObject gameDataObject)
        {
            Dish dish = gameDataObject as Dish;
            
            if (dish?.DisplayPrefab != null)
            {
                SetupDisplayPrefab(dish.DisplayPrefab);
            }
            if (dish?.IconPrefab != null)
            {
                SetupDisplayPrefab(dish.IconPrefab);
            }
            

            base.OnRegister(gameDataObject);
        }

        [Obsolete("Please use OnRegister")]
        public virtual void SetupDisplayPrefab(GameObject prefab) { }
        [Obsolete("Please use OnRegister")]
        public virtual void SetupIconPrefab(GameObject prefab) { }
    }
}