using KitchenData;
using KitchenLib.JSON;
using System.Collections.Generic;

namespace KitchenLib.JSON.Models.Containers
{
	public class ItemSetContainer
	{
		public List<string> Items;
		public int Min;
		public int Max;
		public bool IsMandatory;
		public bool RequiresUnlock;
		public bool OrderingOnly;

		public ItemGroup.ItemSet Convert()
		{
			ItemGroup.ItemSet Set = new ItemGroup.ItemSet();
			List<Item> Items = new List<Item>();

			for (int i=0; i<this.Items.Count; i++)
				Items.Add(ContentPackPatches.GDOConverter<Item>(this.Items[i]));

			Set.Items = Items;
			Set.Min = Min;
			Set.Max = Max;
			Set.IsMandatory = IsMandatory;
			Set.RequiresUnlock = RequiresUnlock;
			Set.OrderingOnly = OrderingOnly;

			return Set;
		}
	}
}
