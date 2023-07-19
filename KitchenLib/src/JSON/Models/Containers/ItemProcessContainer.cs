using KitchenData;

namespace KitchenLib.JSON.Models.Containers
{

	/// <summary>
	/// Represents a storage for item process information.
	/// </summary>
	public struct ItemProcessContainer
	{
		/// <summary>
		/// The process associated with the item.
		/// </summary>
		public string Process;

		/// <summary>
		/// The result of the item process.
		/// </summary>
		public string Result;

		/// <summary>
		/// The duration of the item process.
		/// </summary>
		public float Duration;

		/// <summary>
		/// Indicates whether the item process is considered bad.
		/// </summary>
		public bool IsBad;

		/// <summary>
		/// Indicates whether the item process requires a wrapper.
		/// </summary>
		public bool RequiresWrapper;

		/// <summary>
		/// Converts the ItemProcessStorage to an Item.ItemProcess object.
		/// </summary>
		/// <returns>The converted Item.ItemProcess object.</returns>
		public Item.ItemProcess Convert()
		{
			return new Item.ItemProcess()
			{
				Process = JSONPackUtils.GDOConverter<Process>(Process),
				Result = JSONPackUtils.GDOConverter<Item>(Result),
				Duration = Duration,
				IsBad = IsBad,
				RequiresWrapper = RequiresWrapper
			};
		}
	}
}
