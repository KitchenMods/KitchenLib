using KitchenData;
using KitchenData.Workshop;
using System.Collections.Generic;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class DishDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder dishDump = new StringBuilder();
			StringBuilder dishUnlocksMenuItemsDump = new StringBuilder();
			StringBuilder dishUnlocksIngredientsDump = new StringBuilder();
			StringBuilder dishExtraOrderUnlocksDump = new StringBuilder();
			StringBuilder dishStartingNameSetDump = new StringBuilder();
			StringBuilder dishMinimumIngredientsDump = new StringBuilder();
			StringBuilder dishRequiredProcessesDump = new StringBuilder();
			StringBuilder dishBlockProvidersDump = new StringBuilder();
			StringBuilder dishPrerequisiteDishesDump = new StringBuilder();

			StringBuilder dishRequiresDump = new StringBuilder();
			StringBuilder dishBlockedByDump = new StringBuilder();

			dishDump.AppendLine("ID,Type,Type,AchievementName,IconPrefab," +
				"DisplayPrefab,ExpReward,IsUnlockable,UnlockGroup,CardType," +
				"MinimumFranchiseTier,IsSpecificFranchiseTier,CustomerMultiplier,SelectionBias");

			dishUnlocksMenuItemsDump.AppendLine("ID,Type,Item.Phase,Weight,DynamicMenuType,DynamicMenuIngredient");
			dishUnlocksIngredientsDump.AppendLine("ID,Type,MenuItem,Ingredient");
			dishExtraOrderUnlocksDump.AppendLine("ID,Type,MenuItem,Ingredient");
			dishStartingNameSetDump.AppendLine("ID,Type,Name");
			dishMinimumIngredientsDump.AppendLine("ID,Type,Item");
			dishRequiredProcessesDump.AppendLine("ID,Type,Process");
			dishBlockProvidersDump.AppendLine("ID,Type,Item");
			dishPrerequisiteDishesDump.AppendLine("ID,Type,Dish");

			dishRequiresDump.AppendLine("ID,Type,Unlock");
			dishBlockedByDump.AppendLine("ID,Type,Unlock");

			foreach (Dish dish in GameData.Main.Get<Dish>())
			{
				dishDump.AppendLine($"{dish.ID},{dish.name},{dish.Type},{dish.AchievementName},{dish.IconPrefab}," +
					$"{dish.DisplayPrefab},{dish.ExpReward},{dish.IsUnlockable},{dish.UnlockGroup},{dish.CardType}," +
					$"{dish.MinimumFranchiseTier},{dish.IsSpecificFranchiseTier},{dish.CustomerMultiplier},{dish.SelectionBias}");

				foreach (Dish.MenuItem dishUnlockMenuItem in dish.UnlocksMenuItems)
					dishUnlocksMenuItemsDump.AppendLine($"{dish.ID},{dish.name},{dishUnlockMenuItem.Phase},{dishUnlockMenuItem.Weight},{dishUnlockMenuItem.DynamicMenuType},{dishUnlockMenuItem.DynamicMenuIngredient}");

				foreach (Dish.IngredientUnlock dishUnlockIngredient in dish.UnlocksIngredients)
					dishUnlocksIngredientsDump.AppendLine($"{dish.ID},{dish.name},{dishUnlockIngredient.MenuItem},{dishUnlockIngredient.Ingredient}");

				foreach (Dish.IngredientUnlock dishExtraOrderUnlock in dish.ExtraOrderUnlocks)
					dishExtraOrderUnlocksDump.AppendLine($"{dish.ID},{dish.name},{dishExtraOrderUnlock.MenuItem},{dishExtraOrderUnlock.Ingredient}");

				foreach (string dishStartingName in dish.StartingNameSet)
					dishStartingNameSetDump.AppendLine($"{dish.ID},{dish.name},{dishStartingName}");

				foreach (Item dishMinimumIngredient in dish.MinimumIngredients)
					dishMinimumIngredientsDump.AppendLine($"{dish.ID},{dish.name},{dishMinimumIngredient}");

				foreach (Process dishRequiredProcess in dish.RequiredProcesses)
					dishRequiredProcessesDump.AppendLine($"{dish.ID},{dish.name},{dishRequiredProcess}");

				foreach (Item dishBlockProvider in dish.BlockProviders)
					dishBlockProvidersDump.AppendLine($"{dish.ID},{dish.name},{dishBlockProvider}");

				foreach (Dish dishPrerequisiteDish in dish.PrerequisiteDishes)
					dishPrerequisiteDishesDump.AppendLine($"{dish.ID},{dish.name},{dishPrerequisiteDish}");

				foreach (Unlock dishRequires in dish.Requires)
					dishRequiresDump.AppendLine($"{dish.ID},{dish.name},{dishRequires}");

				foreach (Unlock dishBlockedBy in dish.BlockedBy)
					dishBlockedByDump.AppendLine($"{dish.ID},{dish.name},{dishBlockedBy}");
			}

			SaveCSV("Dish", "Dishs", dishDump);
			SaveCSV("Dish", "DishUnlocksMenuItems", dishUnlocksMenuItemsDump);
			SaveCSV("Dish", "DishUnlocksIngredients", dishUnlocksIngredientsDump);
			SaveCSV("Dish", "DishExtraOrderUnlocks", dishExtraOrderUnlocksDump);
			SaveCSV("Dish", "DishStartingNameSet", dishStartingNameSetDump);
			SaveCSV("Dish", "DishMinimumIngredients", dishMinimumIngredientsDump);
			SaveCSV("Dish", "DishRequiredProcesses", dishRequiredProcessesDump);
			SaveCSV("Dish", "DishBlockProviders", dishBlockProvidersDump);
			SaveCSV("Dish", "DishPrerequisiteDishes", dishPrerequisiteDishesDump);
			SaveCSV("Dish", "DishRequires", dishRequiresDump);
			SaveCSV("Dish", "DishBlockedBy", dishBlockedByDump);
		}
	}
}
