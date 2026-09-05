using System;

namespace KitchenLib.Colorblind
{
	[Obsolete("Please use CustomItem.ColourBlindTag or CustomItemGroup.Labels instead.")]
	public class ItemLabel
	{
		public int itemId { get; set; }
		public string label { get; set; }
	}
}
