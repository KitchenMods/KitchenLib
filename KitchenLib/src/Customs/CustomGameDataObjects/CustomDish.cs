using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KitchenLib.Customs
{
    public abstract class CustomDish : CustomUnlock
    {
		public virtual DishType Type { get; internal set; }
		public virtual string AchievementName { get; internal set; }

		[Obsolete("Use the List<Dish.MenuItem>ResultingMenuItems instead")]
		public virtual List<Dish.MenuItem> UnlocksMenuItems { get { return new List<Dish.MenuItem>(); } }

		[Obsolete("Use the List<Dish.IngredientUnlock>IngredientsUnlocks instead")]
		public virtual HashSet<Dish.IngredientUnlock> UnlocksIngredients { get { return new HashSet<Dish.IngredientUnlock>(); } }
		public virtual HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks { get { return new HashSet<Dish.IngredientUnlock>(); } }
		public virtual List<string> StartingNameSet { get { return new List<string>(); } }
		public virtual HashSet<Item> MinimumIngredients { get { return new HashSet<Item>(); } }
		public virtual HashSet<Process> RequiredProcesses { get { return new HashSet<Process>(); } }
		public virtual HashSet<Item> BlockProviders { get { return new HashSet<Item>(); } }

		[Obsolete("Use the Hashset<Dish>PrerequisiteDishesEditor instead")]
		public virtual HashSet<Dish> PrerequisiteDishes { get { return new HashSet<Dish>(); } }
		public virtual GameObject IconPrefab { get; internal set; }
		public virtual GameObject DisplayPrefab { get; internal set; }


		public virtual List<Dish.MenuItem> ResultingMenuItems { get { return new List<Dish.MenuItem>(); } }
		public virtual HashSet<Dish.IngredientUnlock> IngredientsUnlocks { get { return new HashSet<Dish.IngredientUnlock>(); } }
		public virtual HashSet<Dish> PrerequisiteDishesEditor { get { return new HashSet<Dish>(); } }
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
        {
            Dish result = new Dish();
            Dish empty = new Dish();
            
            if (empty.ID != ID) result.ID = ID;
			if (empty.Type != Type) result.Type = Type;
			if (empty.AchievementName != AchievementName) result.AchievementName = AchievementName;
			//if (empty.UnlocksMenuItems != UnlocksMenuItems) result.UnlocksMenuItems = UnlocksMenuItems;
			//if (empty.UnlocksIngredients != UnlocksIngredients) result.UnlocksIngredients = UnlocksIngredients;
			if (empty.ExtraOrderUnlocks != ExtraOrderUnlocks) result.ExtraOrderUnlocks = ExtraOrderUnlocks;
			if (empty.StartingNameSet != StartingNameSet) result.StartingNameSet = StartingNameSet;
			if (empty.MinimumIngredients != MinimumIngredients) result.MinimumIngredients = MinimumIngredients;
			if (empty.RequiredProcesses != RequiredProcesses) result.RequiredProcesses = RequiredProcesses;
			if (empty.BlockProviders != BlockProviders) result.BlockProviders = BlockProviders;
			//if (empty.PrerequisiteDishes != PrerequisiteDishes) result.PrerequisiteDishes = PrerequisiteDishes;
			if (empty.IconPrefab != IconPrefab) result.IconPrefab = IconPrefab;
			if (empty.DisplayPrefab != DisplayPrefab) result.DisplayPrefab = DisplayPrefab;
			
			FieldInfo resultingMenuItems = ReflectionUtils.GetField<Dish>("ResultingMenuItems");
			FieldInfo ingredientsUnlocks = ReflectionUtils.GetField<Dish>("IngredientsUnlocks");
			FieldInfo prerequisiteDishesEditor = ReflectionUtils.GetField<Dish>("PrerequisiteDishesEditor");

			if (resultingMenuItems.GetValue(empty) != ResultingMenuItems) resultingMenuItems.SetValue(result, ResultingMenuItems);
			if (ingredientsUnlocks.GetValue(empty) != IngredientsUnlocks) ingredientsUnlocks.SetValue(result, IngredientsUnlocks);
			if (prerequisiteDishesEditor.GetValue(empty) != PrerequisiteDishesEditor) prerequisiteDishesEditor.SetValue(result, PrerequisiteDishesEditor);

			if (empty.ExpReward != ExpReward) result.ExpReward = ExpReward;
			if (empty.IsUnlockable != IsUnlockable) result.IsUnlockable = IsUnlockable;
			if (empty.UnlockGroup != UnlockGroup) result.UnlockGroup = UnlockGroup;
			if (empty.CardType != CardType) result.CardType = CardType;
			if (empty.MinimumFranchiseTier != MinimumFranchiseTier) result.MinimumFranchiseTier = MinimumFranchiseTier;
			if (empty.IsSpecificFranchiseTier != IsSpecificFranchiseTier) result.IsSpecificFranchiseTier = IsSpecificFranchiseTier;
			if (empty.CustomerMultiplier != CustomerMultiplier) result.CustomerMultiplier = CustomerMultiplier;
			if (empty.SelectionBias != SelectionBias) result.SelectionBias = SelectionBias;
			if (empty.Requires != Requires) result.Requires = Requires;
			if (empty.BlockedBy != BlockedBy) result.BlockedBy = BlockedBy;

            gameDataObject = result;
        }
    }
}