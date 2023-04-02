using KitchenLib.ShhhDontTellAnyone;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal class TileHighlighterViewHolder : BaseViewHolder<TileHighlighter>
	{
		public override string UniqueNameID => "TileHighlighterViewController";
		public override GameObject Prefab => new GameObject("Tile Highlighter View Controller");
		public override string Name => "Tile Highlighter View Controller";
	}
}
