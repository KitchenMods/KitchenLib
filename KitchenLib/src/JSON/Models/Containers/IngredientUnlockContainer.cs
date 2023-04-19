using KitchenData;

namespace KitchenLib.JSON.Models.Containers
{
	public class IngredientUnlockContainer
	{
		public string MenuItem;
		public string Ingredient;

		public Dish.IngredientUnlock Convert()
		{
			Dish.IngredientUnlock unlock = new Dish.IngredientUnlock();
			unlock.MenuItem = ContentPackPatches.GDOConverter<ItemGroup>(MenuItem);
			unlock.Ingredient = ContentPackPatches.GDOConverter<Item>(Ingredient);
			return unlock;
		}
	}
}
