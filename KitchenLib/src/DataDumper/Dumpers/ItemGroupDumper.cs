using KitchenData;
using System.Collections.Generic;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ItemGroupDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder itemGroupDump = new StringBuilder();
			StringBuilder itemGroupSets = new StringBuilder();

			itemGroupDump.AppendLine("ID,Type,CanContainSide,ApplyProcessesToComponents,AutoCollapsing");
			itemGroupSets.AppendLine("ID,Type,List<Item> Items,Min,Max,IsMandatory,RequiresUnlock,OrderingOnly");

			foreach (ItemGroup itemGroup in GameData.Main.Get<ItemGroup>())
			{
				itemGroupDump.AppendLine($"{itemGroup.ID},{itemGroup.name},{itemGroup.CanContainSide},{itemGroup.ApplyProcessesToComponents},{itemGroup.AutoCollapsing}");
				foreach (ItemGroup.ItemSet itemGroupSet in itemGroup.DerivedSets)
				{
					List<string> array = new List<string>();
					foreach (Item item in itemGroupSet.Items)
						array.Add(item.ToString());
					itemGroupSets.AppendLine($"{itemGroup.ID},{itemGroup.name},{string.Join(";", array)},{itemGroupSet.Min},{itemGroupSet.Max},{itemGroupSet.IsMandatory},{itemGroupSet.RequiresUnlock},{itemGroupSet.OrderingOnly}");
				}
			}

			SaveCSV("ItemGroup", "ItemGroups", itemGroupDump);
			SaveCSV("ItemGroup", "ItemGroupSets", itemGroupSets);
		}
	}
}
