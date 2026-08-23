using System;

namespace KitchenLib.Colorblind
{
	[Obsolete("Please use CustomItem.ColourBlindTag or CustomItemGroup.Labels instead.")]
	public class ItemLabelGroup
	{
		public int itemId { get; set; }
		public ItemLabel[] itemLabels { get; set; }
	}
}
