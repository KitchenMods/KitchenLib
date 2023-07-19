using KitchenData;

namespace KitchenLib.JSON.Models.Containers
{
    public struct IngredientUnlockContainer
    {
        public string MenuItem;
        public string Ingredient;

        public Dish.IngredientUnlock Convert()
        {
			return new Dish.IngredientUnlock()
			{
				MenuItem = JSONPackUtils.GDOConverter<ItemGroup>(MenuItem),
				Ingredient = JSONPackUtils.GDOConverter<Item>(Ingredient)
			};
        }
    }
}