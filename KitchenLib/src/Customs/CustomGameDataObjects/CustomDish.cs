using KitchenData;
using KitchenLib.References;
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

			if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Dish>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Type != Type) result.Type = Type;
            if (result.Difficulty != Difficulty) result.Difficulty = Difficulty;
            if (result.HideInfoPanel != HideInfoPanel) result.HideInfoPanel = HideInfoPanel;
            if (result.AchievementName != AchievementName) result.AchievementName = AchievementName;
            if (result.StartingNameSet != StartingNameSet) result.StartingNameSet = StartingNameSet;
            if (result.IconPrefab != IconPrefab) result.IconPrefab = IconPrefab;
            if (result.DisplayPrefab != DisplayPrefab) result.DisplayPrefab = DisplayPrefab;

            if (result.ExpReward != ExpReward) result.ExpReward = ExpReward;
            if (result.IsUnlockable != IsUnlockable) result.IsUnlockable = IsUnlockable;
            if (result.UnlockGroup != UnlockGroup) result.UnlockGroup = UnlockGroup;
            if (result.CardType != CardType) result.CardType = CardType;
            if (result.MinimumFranchiseTier != MinimumFranchiseTier) result.MinimumFranchiseTier = MinimumFranchiseTier;
            if (result.IsSpecificFranchiseTier != IsSpecificFranchiseTier) result.IsSpecificFranchiseTier = IsSpecificFranchiseTier;
            if (result.CustomerMultiplier != CustomerMultiplier) result.CustomerMultiplier = CustomerMultiplier;
            if (result.SelectionBias != SelectionBias) result.SelectionBias = SelectionBias;
            if (result.BlocksAllOtherFood != BlocksAllOtherFood) result.BlocksAllOtherFood = BlocksAllOtherFood;

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
            Dish result = (Dish)gameDataObject;
			
			if (result.UnlockItemOverride != UnlockItemOverride) result.UnlockItemOverride = UnlockItemOverride;
			if (result.AlsoAddRecipes != AlsoAddRecipes) result.AlsoAddRecipes = AlsoAddRecipes;
			if (result.ExtraOrderUnlocks != ExtraOrderUnlocks) result.ExtraOrderUnlocks = ExtraOrderUnlocks;
            if (result.MinimumIngredients != MinimumIngredients) result.MinimumIngredients = MinimumIngredients;
            if (result.RequiredProcesses != RequiredProcesses) result.RequiredProcesses = RequiredProcesses;
            if (result.BlockProviders != BlockProviders) result.BlockProviders = BlockProviders;
            if (result.AllowedFoods != AllowedFoods) result.AllowedFoods = AllowedFoods;
            if (result.ForceFranchiseSetting != ForceFranchiseSetting) result.ForceFranchiseSetting = ForceFranchiseSetting;

            FieldInfo resultingMenuItems = ReflectionUtils.GetField<Dish>("ResultingMenuItems");
            FieldInfo ingredientsUnlocks = ReflectionUtils.GetField<Dish>("IngredientsUnlocks");
            FieldInfo prerequisiteDishesEditor = ReflectionUtils.GetField<Dish>("PrerequisiteDishesEditor");

            if (resultingMenuItems.GetValue(result) != ResultingMenuItems) resultingMenuItems.SetValue(result, ResultingMenuItems);
            if (ingredientsUnlocks.GetValue(result) != IngredientsUnlocks) ingredientsUnlocks.SetValue(result, IngredientsUnlocks);
            if (prerequisiteDishesEditor.GetValue(result) != PrerequisiteDishesEditor) prerequisiteDishesEditor.SetValue(result, PrerequisiteDishesEditor);

            FieldInfo hardcodedRequirements = ReflectionUtils.GetField<Unlock>("HardcodedRequirements");
            FieldInfo hardcodedBlockers = ReflectionUtils.GetField<Unlock>("HardcodedBlockers");

            if (hardcodedRequirements.GetValue(result) != HardcodedRequirements) hardcodedRequirements.SetValue(result, HardcodedRequirements);
            if (hardcodedBlockers.GetValue(result) != HardcodedBlockers) hardcodedBlockers.SetValue(result, HardcodedBlockers);
			
			if (!RequiredNoDishItem)
			{
				if (RequiredDishItem != null)
					result.MinimumIngredients.Add(RequiredDishItem);
				else
					result.MinimumIngredients.Add((Item)GDOUtils.GetExistingGDO(ItemReferences.Plate));
			}

			if (result.Type == DishType.Main && HardcodedRequirements.Count == 0)
			{
				if (!BypassMainRequirementsCheck)
				{
					result.Type = DishType.Base;
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
            else
            {
                Main.LogWarning($"Dish with ID '{UniqueNameID}' does not have a display prefab set.");
            }
            if (dish?.IconPrefab != null)
            {
                SetupDisplayPrefab(dish.IconPrefab);
            }
            else
            {
                Main.LogWarning($"Dish with ID '{UniqueNameID}' does not have an icon prefab set.");
            }

            base.OnRegister(gameDataObject);
        }

        public virtual void SetupDisplayPrefab(GameObject prefab) { }
        public virtual void SetupIconPrefab(GameObject prefab) { }
    }
}