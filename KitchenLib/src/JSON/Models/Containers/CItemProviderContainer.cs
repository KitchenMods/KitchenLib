using Kitchen;
using KitchenData;
using KitchenLib.Utils;

namespace KitchenLib.JSON.Models.Containers
{
	public struct CItemProviderContainer : IApplianceProperty
	{
		public int Item;
		public int Available;
		public int Maximum;
		public bool DirectInsertionOnly;
		public bool EmptyAtNight;
		public bool PreventReturns;
		public bool DestroyOnEmpty;
		public bool AutoGrabFromHolder;
		public bool AutoPlaceOnHolder;
		public bool AllowRefreshes;
		public int ProvidedItem;

		public CItemProvider Convert()
		{
			return KitchenPropertiesUtils.GetCItemProvider(
				Item,
				Available,
				Maximum,
				DirectInsertionOnly,
				EmptyAtNight,
				PreventReturns,
				DestroyOnEmpty,
				AutoGrabFromHolder,
				AutoPlaceOnHolder,
				AllowRefreshes);
		}
	}
}
