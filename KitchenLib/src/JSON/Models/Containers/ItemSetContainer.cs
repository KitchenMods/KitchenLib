using KitchenData;
using System.Collections.Generic;
using System.Linq;

namespace KitchenLib.JSON.Models.Containers
{
	public struct ItemSetContainer
	{
		public List<string> Items;
		public int Min;
		public int Max;
		public bool IsMandatory;
		public bool RequiresUnlock;
		public bool OrderingOnly;

		public ItemGroup.ItemSet Convert()
		{
			return new ItemGroup.ItemSet()
			{
				Items = Items
					.Select(_ => JSONPackUtils.GDOConverter<Item>(_))
					.ToList(),
				Min = Min,
				Max = Max,
				IsMandatory = IsMandatory,
				RequiresUnlock = RequiresUnlock,
				OrderingOnly = OrderingOnly
			};
		}
	}
}
