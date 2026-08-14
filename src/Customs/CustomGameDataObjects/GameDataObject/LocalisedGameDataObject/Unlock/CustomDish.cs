using System;
using System.Collections.Generic;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomDish : CustomUnlock<Dish>
	{

		#region Base Game Variables

		public virtual DishType Type { get; protected set; }
		public virtual int Difficulty { get; protected set; }
		public virtual int RewardOverride { get; protected set; }
		public virtual Item UnlockItemOverride { get; protected set; }
		public virtual bool HideInfoPanel { get; protected set; }
		public virtual bool SkipOwnRecipe { get; protected set; }
		public virtual List<Dish> AlsoAddRecipes { get; protected set; } = new();
		public virtual GameObject IconPrefab { get; protected set; }
		public virtual GameObject DisplayPrefab { get; protected set; }
		public virtual string ImageKey { get; protected set; }
		public virtual List<Dish.MenuItem> ResultingMenuItems { get; protected set; } = new();
		public virtual HashSet<Dish.IngredientUnlock> IngredientsUnlocks { get; protected set; } = new();
		public virtual HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks { get; protected set; } = new();
		public virtual bool IsMainThatDoesNotNeedPlates { get; protected set; }
		public virtual List<RestaurantStatus> AddsStatuses { get; protected set; } = new();
		public virtual List<string> StartingNameSet { get; protected set; } = new();
		public virtual HeatLevels  HeatOverride { get; protected set; } = new();
		public virtual HashSet<Item> MinimumIngredients { get; protected set; } = new();
		public virtual HashSet<Process> RequiredProcesses { get; protected set; } = new();
		public virtual HashSet<Item> BlockProviders { get; protected set; } = new();
		public virtual HashSet<Item> BeneficialIngredients { get; protected set; } = new();

		#endregion

		#region KitchenLib Variables

		public virtual bool IsAvailableAsLobbyOption { get; protected set; } = false;
		public virtual bool DestroyAfterModUninstall { get; protected set; } = true;
		public virtual Dictionary<Locale, string> Recipe { get; protected set; } = new();
		public virtual Item RequiredDishItem { get; protected set; }
		public virtual bool RequiredNoDishItem { get; protected set; } = false;
		public virtual bool BypassMainRequirementsCheck { get; protected set; } = false;

		#endregion
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is Dish dish)
			{
				#region Apply Properties

				OverrideVariable(dish, "Type", Type);
				OverrideVariable(dish, "Difficulty", Difficulty);
				OverrideVariable(dish, "RewardOverride", RewardOverride);
				OverrideVariable(dish, "HideInfoPanel", HideInfoPanel);
				OverrideVariable(dish, "SkipOwnRecipe", SkipOwnRecipe);
				OverrideVariable(dish, "IconPrefab", IconPrefab);
				OverrideVariable(dish, "DisplayPrefab", DisplayPrefab);
				OverrideVariable(dish, "ImageKey", ImageKey);
				OverrideVariable(dish, "IsMainThatDoesNotNeedPlates", IsMainThatDoesNotNeedPlates);
				OverrideVariable(dish, "AddsStatuses", AddsStatuses);
				OverrideVariable(dish, "StartingNameSet", StartingNameSet);

				#endregion

				if (RequiredNoDishItem)
				{
					dish.IsMainThatDoesNotNeedPlates = RequiredNoDishItem;
				}
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is Dish dish)
			{
				#region Apply Properties

				OverrideVariable(dish, "UnlockItemOverride", UnlockItemOverride);
				OverrideVariable(dish, "AlsoAddRecipes", AlsoAddRecipes);
				OverrideVariable(dish, "ResultingMenuItems", ResultingMenuItems);
				OverrideVariable(dish, "IngredientsUnlocks", IngredientsUnlocks);
				OverrideVariable(dish, "ExtraOrderUnlocks", ExtraOrderUnlocks);
				OverrideVariable(dish, "HeatOverride", HeatOverride);
				OverrideVariable(dish, "MinimumIngredients", MinimumIngredients);
				OverrideVariable(dish, "RequiredProcesses", RequiredProcesses);
				OverrideVariable(dish, "BlockProviders", BlockProviders);
				OverrideVariable(dish, "BeneficialIngredients", BeneficialIngredients);

				#endregion

				#region Apply Recipe

				var fallback = "";
				var customDish = (CustomDish)GDOUtils.GetCustomGameDataObject(dish.ID);
				foreach (var recipe in customDish.Recipe)
				{
					if (recipe.Key == Locale.English)
					{
						fallback = recipe.Value;
					}

					var info = gameData.GlobalLocalisation.Recipes.Info.Get(recipe.Key);
					if (info != null)
					{
						if (!info.Text.ContainsKey(dish))
						{
							info.Text.Add(dish, recipe.Value);
						}
					}
				}

				if (!string.IsNullOrEmpty(fallback))
				{
					foreach (Locale locale in Enum.GetValues(typeof(Locale)))
					{
						var info = gameData.GlobalLocalisation.Recipes.Info.Get(locale);
						if (!info.Text.ContainsKey(dish))
						{
							info.Text.Add(dish, fallback);
						}
					}
				}

				#endregion

				if (RequiredDishItem != null)
				{
					Main.LogDebug($"Adding : {RequiredDishItem} >> MinimumIngredients");
					dish.MinimumIngredients.Add(RequiredDishItem);
				}

				if (dish.Type == DishType.Main && HardcodedRequirements.Count == 0 && !BypassMainRequirementsCheck)
				{
					Main.LogDebug($"Assigning : {DishType.Base} >> Type");
					dish.Type = DishType.Base;
				}
			}
		}
		
		public override void OnRegister(GameDataObject gameDataObject)
		{
			if (gameDataObject is Dish dish)
			{
				if (dish.DisplayPrefab != null)
				{
					SetupDisplayPrefab(dish.DisplayPrefab);
				}

				if (dish.IconPrefab != null)
				{
					SetupDisplayPrefab(dish.IconPrefab);
				}
			}

			base.OnRegister(gameDataObject);
		}		

		[Obsolete("Please use OnRegister")]
		public virtual void SetupDisplayPrefab(GameObject prefab) { }
		[Obsolete("Please use OnRegister")]
		public virtual void SetupIconPrefab(GameObject prefab) { }
	}
}