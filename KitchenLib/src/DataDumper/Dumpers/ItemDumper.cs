using KitchenData;
using System.Diagnostics;
using System.Text;

namespace KitchenLib.DataDumper.Dumpers
{
	public class ItemDumper : BaseDataDumper
	{
		public override void Dump()
		{
			StringBuilder itemDump = new StringBuilder();
			StringBuilder itemProcesses = new StringBuilder();
			StringBuilder itemProperties = new StringBuilder();
			StringBuilder itemMayRequestExtraItems = new StringBuilder();
			StringBuilder itemSplitDepletedItems = new StringBuilder();
			StringBuilder itemAutomaticItemProcess = new StringBuilder();

			itemDump.AppendLine("ID,Type,Prefab,ExtraTimeGranted,ItemValue," +
				"Reward,DirtiesTo,IsConsumedByCustomer,MaxOrderSharers,AlwaysOrderAdditionalItem,AutoSatisfied,SplitSubItem,SplitCount," +
				"SplitSpeed,AllowSplitMerging,PreventExplicitSplit,SplitByComponents,SplitByComponentsHolder," +
				"SplitByCopying,RefuseSplitWith,DisposesTo,IsIndisposable,ItemCategory," +
				"ItemStorageFlags,DedicatedProvider,HoldPose,IsMergeableSide,ExtendedDirtItem");
			itemProcesses.AppendLine("ID,Type,Process,Result,Duration,IsBad,RequiresWrapper");
			itemProperties.AppendLine("ID,Type,IItemProperty");
			itemMayRequestExtraItems.AppendLine("ID,Type,Item");
			itemSplitDepletedItems.AppendLine("ID,Type,Item");
			itemAutomaticItemProcess.AppendLine("ID,Type,Process,Result,Duration,IsBad,RequiresWrapper");

			foreach (Item item in GameData.Main.Get<Item>())
			{
				itemDump.AppendLine($"{item.ID},{item.name},{item.Prefab},{item.ExtraTimeGranted},{item.ItemValue}," +
					$"{item.Reward},{item.DirtiesTo},{item.IsConsumedByCustomer},{item.MaxOrderSharers},{item.AlwaysOrderAdditionalItem},{item.AutoSatisfied},{item.SplitSubItem},{item.SplitCount}," +
					$"{item.SplitSpeed},{item.AllowSplitMerging},{item.PreventExplicitSplit},{item.SplitByComponents},{item.SplitByComponentsHolder}," +
					$"{item.SplitByCopying},{item.RefuseSplitWith},{item.DisposesTo},{item.IsIndisposable},{item.ItemCategory.ToString().Replace(",", ".")}," +
					$"{item.ItemStorageFlags},{item.DedicatedProvider},{item.HoldPose},{item.IsMergeableSide},{item.ExtendedDirtItem}");

				if (item.AutomaticItemProcess.Process != null)
					itemAutomaticItemProcess.AppendLine($"{item.ID},{item.name},{item.AutomaticItemProcess.Process},{item.AutomaticItemProcess.Result}," +
					$"{item.AutomaticItemProcess.Duration},{item.AutomaticItemProcess.IsBad},{item.AutomaticItemProcess.RequiresWrapper}");

				foreach (Item.ItemProcess process in item.DerivedProcesses)
				{
					itemProcesses.AppendLine($"{item.ID},{item.name},{process.Process},{process.Result}," +
						$"{process.Duration},{process.IsBad},{process.RequiresWrapper}");
				}

				foreach (IItemProperty property in item.Properties)
					itemProperties.AppendLine($"{item.ID},{item.name},{property.GetType().Name}");

				foreach (Item mayRequestExtraItem in item.MayRequestExtraItems)
					itemMayRequestExtraItems.AppendLine($"{item.ID},{item.name},{mayRequestExtraItem}:");

				foreach (Item splitDepletedItem in item.SplitDepletedItems)
					itemSplitDepletedItems.AppendLine($"{item.ID},{item.name},{splitDepletedItem}");
			}

			SaveCSV("Item", "Items", itemDump);
			SaveCSV("Item", "ItemProcesses", itemProcesses);
			SaveCSV("Item", "ItemProperties", itemProperties);
			SaveCSV("Item", "ItemMayRequestExtraItems", itemMayRequestExtraItems);
			SaveCSV("Item", "ItemSplitDepletedItems", itemSplitDepletedItems);
			SaveCSV("Item", "ItemAutomaticItemProcess", itemAutomaticItemProcess);
		}
	}
}
