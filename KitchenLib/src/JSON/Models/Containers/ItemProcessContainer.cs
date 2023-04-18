using KitchenData;
using KitchenLib.JSON;

namespace KitchenLib.src.JSON.Models.Containers
{
	public class ItemProcessContainer
	{
		public string Process;
		public string Result;
		public float Duration;
		public bool IsBad;
		public bool RequiresWrapper;

		public Item.ItemProcess Convert()
		{
			Item.ItemProcess itemProcess = new Item.ItemProcess();

			itemProcess.Process = ContentPackUtils.Find<Process>(Process);
			itemProcess.Result = ContentPackUtils.Find<Item>(Result);
			itemProcess.Duration = Duration;
			itemProcess.IsBad = IsBad;
			itemProcess.RequiresWrapper = RequiresWrapper;

			return itemProcess;
		}
	}
}
