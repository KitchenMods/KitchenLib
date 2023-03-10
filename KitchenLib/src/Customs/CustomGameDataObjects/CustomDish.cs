using KitchenData;
using KitchenLib.Patches;
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
        public virtual DishType Type { get; protected set; }
        public virtual string AchievementName { get; protected set; }
        public virtual HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks { get; protected set; } = new HashSet<Dish.IngredientUnlock>();
        public virtual List<string> StartingNameSet { get; protected set; } = new List<string>();
        public virtual HashSet<Item> MinimumIngredients { get; protected set; } = new HashSet<Item>();
        public virtual HashSet<Process> RequiredProcesses { get; protected set; } = new HashSet<Process>();
        public virtual HashSet<Item> BlockProviders { get; protected set; } = new HashSet<Item>();
        public virtual GameObject IconPrefab { get; protected set; }
        public virtual GameObject DisplayPrefab { get; protected set; }
        public virtual List<Dish.MenuItem> ResultingMenuItems { get; protected set; } = new List<Dish.MenuItem>();
        public virtual HashSet<Dish.IngredientUnlock> IngredientsUnlocks { get; protected set; } = new HashSet<Dish.IngredientUnlock>();
		
		[Obsolete("Please use HardcodedRequirements")]
		public virtual HashSet<Dish> PrerequisiteDishesEditor { get; protected set; } = new HashSet<Dish>();

		public virtual bool IsAvailableAsLobbyOption { get; protected set; } = false;
		public virtual bool DestroyAfterModUninstall { get; protected set; } = true;
		public virtual Dictionary<Locale, string> Recipe { get; protected set; } = new Dictionary<Locale, string>();

        //private static readonly Dish empty = ScriptableObject.CreateInstance<Dish>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Dish result = ScriptableObject.CreateInstance<Dish>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Dish>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (result.ID != ID) result.ID = ID;
            if (result.Type != Type) result.Type = Type;
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

            if (result.Info != Info) result.Info = Info;

			if (InfoList.Count > 0)
			{
				result.Info = new LocalisationObject<UnlockInfo>();
				foreach ((Locale, UnlockInfo) info in InfoList)
					result.Info.Add(info.Item1, info.Item2);
			}

			if (!string.IsNullOrEmpty(IconOverride))
				Unlock_Patch.AddIconOverride(result.ID, IconOverride);
			if (ColourOverride != new Color())
				Unlock_Patch.AddColourOverride(result.ID, ColourOverride);

			gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Dish result = (Dish)gameDataObject;

            if (result.ExtraOrderUnlocks != ExtraOrderUnlocks) result.ExtraOrderUnlocks = ExtraOrderUnlocks;
            if (result.MinimumIngredients != MinimumIngredients) result.MinimumIngredients = MinimumIngredients;
            if (result.RequiredProcesses != RequiredProcesses) result.RequiredProcesses = RequiredProcesses;
            if (result.BlockProviders != BlockProviders) result.BlockProviders = BlockProviders;

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