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
			StringBuilder itemSatisfiedBy = new StringBuilder();
			StringBuilder itemNeedsIngredients = new StringBuilder();

			itemDump.AppendLine("ID,Type,Prefab,ExtraTimeGranted,EatingTime,ItemValue," +
				"Reward,DirtiesTo,IsConsumedByCustomer,MaxOrderSharers,AlwaysOrderAdditionalItem,AutoSatisfied,SplitSubItem,SplitCount," +
				"SplitSpeed,AllowSplitMerging,PreventExplicitSplit,SplitByComponents,SplitByComponentsHolder,SplitByComponentsWrapper," +
				"SplitByCopying,RefuseSplitWith,DisposesTo,IsIndisposable,ItemCategory," +
				"ItemStorageFlags,DedicatedProvider,HoldPose,IsMergeableSide,CreditSourceDish,ExtendedDirtItem");
			itemProcesses.AppendLine("ID,Type,Process,Result,Duration,IsBad,RequiresWrapper");
			itemProperties.AppendLine("ID,Type,IItemProperty");
			itemMayRequestExtraItems.AppendLine("ID,Type,Item");
			itemSplitDepletedItems.AppendLine("ID,Type,Item");
			itemAutomaticItemProcess.AppendLine("ID,Type,Process,Result,Duration,IsBad,RequiresWrapper");
			itemSatisfiedBy.AppendLine("ID,Type,Item");
			itemNeedsIngredients.AppendLine("ID,Type,Item");

			foreach (Item item in GameData.Main.Get<Item>())
			{
				itemDump.AppendLine($"{item.ID},{item.name},{item.Prefab},{item.ExtraTimeGranted},{item.EatingTime},{item.ItemValue}," +
					$"{item.Reward},{item.DirtiesTo},{item.IsConsumedByCustomer},{item.MaxOrderSharers},{item.AlwaysOrderAdditionalItem},{item.AutoSatisfied},{item.SplitSubItem},{item.SplitCount}," +
					$"{item.SplitSpeed},{item.AllowSplitMerging},{item.PreventExplicitSplit},{item.SplitByComponents},{item.SplitByComponentsHolder},{item.SplitByComponentsWrapper}," +
					$"{item.SplitByCopying},{item.RefuseSplitWith},{item.DisposesTo},{item.IsIndisposable},{item.ItemCategory.ToString().Replace(",", ".")}," +
					$"{item.ItemStorageFlags},{item.DedicatedProvider},{item.HoldPose},{item.IsMergeableSide},{item.CreditSourceDish},{item.ExtendedDirtItem}");

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

				foreach (Item SatisfiedBy in item.SatisfiedBy)
					itemSatisfiedBy.AppendLine($"{item.ID},{item.name},{SatisfiedBy}");

				foreach (Item NeedsIngredients in item.NeedsIngredients)
					itemNeedsIngredients.AppendLine($"{item.ID},{item.name},{NeedsIngredients}");
			}

			SaveCSV("Item", "Items", itemDump);
			SaveCSV("Item", "ItemProcesses", itemProcesses);
			SaveCSV("Item", "ItemProperties", itemProperties);
			SaveCSV("Item", "ItemMayRequestExtraItems", itemMayRequestExtraItems);
			SaveCSV("Item", "ItemSplitDepletedItems", itemSplitDepletedItems);
			SaveCSV("Item", "ItemAutomaticItemProcess", itemAutomaticItemProcess);
			SaveCSV("Item", "ItemSatisfiedBy", itemSatisfiedBy);
			SaveCSV("Item", "ItemNeedsIngredients", itemNeedsIngredients);
		}
	}
}
