using KitchenData;

namespace KitchenLib.JSON.Models.Containers
{
    public struct MenuItemContainer
    {
        public string Item;
        public MenuPhase Phase;
        public float Weight;
        public DynamicMenuType DynamicMenuType;
        public string DynamicMenuIngredient;

        public Dish.MenuItem Convert()
        {
			return new Dish.MenuItem()
			{
				Item = JSONPackUtils.GDOConverter<Item>(Item),
				Phase = Phase,
				Weight = Weight,
				DynamicMenuType = DynamicMenuType,
				DynamicMenuIngredient = JSONPackUtils.GDOConverter<Item>(DynamicMenuIngredient)
			};
        }
    }
}