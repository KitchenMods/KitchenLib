using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomDish : CustomUnlock
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
        public virtual HashSet<Dish> PrerequisiteDishesEditor { get; protected set; } = new HashSet<Dish>();

		public virtual bool IsAvailableAsLobbyOption { get; protected set; } = false;
		public virtual bool DestroyAfterModUninstall { get; protected set; } = true;
		public virtual Dictionary<Locale, string> Recipe { get; protected set; } = new Dictionary<Locale, string>();

        private static readonly Dish empty = ScriptableObject.CreateInstance<Dish>();
        public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Dish result = ScriptableObject.CreateInstance<Dish>();

            if (BaseGameDataObjectID != -1)
                result = UnityEngine.Object.Instantiate(gameData.Get<Dish>().FirstOrDefault(a => a.ID == BaseGameDataObjectID));

            if (empty.ID != ID) result.ID = ID;
            if (empty.Type != Type) result.Type = Type;
            if (empty.AchievementName != AchievementName) result.AchievementName = AchievementName;
            if (empty.StartingNameSet != StartingNameSet) result.StartingNameSet = StartingNameSet;
            if (empty.IconPrefab != IconPrefab) result.IconPrefab = IconPrefab;
            if (empty.DisplayPrefab != DisplayPrefab) result.DisplayPrefab = DisplayPrefab;

            if (empty.ExpReward != ExpReward) result.ExpReward = ExpReward;
            if (empty.IsUnlockable != IsUnlockable) result.IsUnlockable = IsUnlockable;
            if (empty.UnlockGroup != UnlockGroup) result.UnlockGroup = UnlockGroup;
            if (empty.CardType != CardType) result.CardType = CardType;
            if (empty.MinimumFranchiseTier != MinimumFranchiseTier) result.MinimumFranchiseTier = MinimumFranchiseTier;
            if (empty.IsSpecificFranchiseTier != IsSpecificFranchiseTier) result.IsSpecificFranchiseTier = IsSpecificFranchiseTier;
            if (empty.CustomerMultiplier != CustomerMultiplier) result.CustomerMultiplier = CustomerMultiplier;
            if (empty.SelectionBias != SelectionBias) result.SelectionBias = SelectionBias;

            if (empty.Info != Info) result.Info = Info;

            gameDataObject = result;
        }

        public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
        {
            Dish result = (Dish)gameDataObject;

            if (empty.ExtraOrderUnlocks != ExtraOrderUnlocks) result.ExtraOrderUnlocks = ExtraOrderUnlocks;
            if (empty.MinimumIngredients != MinimumIngredients) result.MinimumIngredients = MinimumIngredients;
            if (empty.RequiredProcesses != RequiredProcesses) result.RequiredProcesses = RequiredProcesses;
            if (empty.BlockProviders != BlockProviders) result.BlockProviders = BlockProviders;

            FieldInfo resultingMenuItems = ReflectionUtils.GetField<Dish>("ResultingMenuItems");
            FieldInfo ingredientsUnlocks = ReflectionUtils.GetField<Dish>("IngredientsUnlocks");
            FieldInfo prerequisiteDishesEditor = ReflectionUtils.GetField<Dish>("PrerequisiteDishesEditor");

            if (resultingMenuItems.GetValue(empty) != ResultingMenuItems) resultingMenuItems.SetValue(result, ResultingMenuItems);
            if (ingredientsUnlocks.GetValue(empty) != IngredientsUnlocks) ingredientsUnlocks.SetValue(result, IngredientsUnlocks);
            if (prerequisiteDishesEditor.GetValue(empty) != PrerequisiteDishesEditor) prerequisiteDishesEditor.SetValue(result, PrerequisiteDishesEditor);

            FieldInfo hardcodedRequirements = ReflectionUtils.GetField<Unlock>("HardcodedRequirements");
            FieldInfo hardcodedBlockers = ReflectionUtils.GetField<Unlock>("HardcodedBlockers");

            if (hardcodedRequirements.GetValue(empty) != HardcodedRequirements) hardcodedRequirements.SetValue(result, HardcodedRequirements);
            if (hardcodedBlockers.GetValue(empty) != HardcodedBlockers) hardcodedBlockers.SetValue(result, HardcodedBlockers);
        }
    }
}