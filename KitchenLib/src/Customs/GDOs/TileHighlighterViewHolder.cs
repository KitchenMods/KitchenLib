using KitchenData;
using KitchenLib.ShhhDontTellAnyone;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	public class TileHighlighterViewHolder : CustomAppliance
	{
		public override string UniqueNameID => "TileHighlighterViewController";
		public override GameObject Prefab => new GameObject("Tile Highlighter View Controller");
		public override bool IsPurchasable => false;
		public override bool IsNonInteractive => true;
		public override string Name => "Tile Highlighter View Controller";

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<TileHighlighter>();
		}
	}
}
