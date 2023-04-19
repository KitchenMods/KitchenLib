using KitchenData;

namespace KitchenLib.JSON.Models.Containers
{
	public class MenuItemContainer
	{
		public string Item;
		public MenuPhase Phase;
		public float Weight;
		public DynamicMenuType DynamicMenuType;
		public string DynamicMenuIngredient;

		public Dish.MenuItem Convert()
		{
			Dish.MenuItem item = new Dish.MenuItem();
			item.Item = ContentPackPatches.GDOConverter<Item>(Item);
			item.Phase = Phase;
			item.Weight = Weight;
			item.DynamicMenuType = DynamicMenuType;
			item.DynamicMenuIngredient = ContentPackPatches.GDOConverter<Item>(DynamicMenuIngredient);
			return item;
		}
	}
}
