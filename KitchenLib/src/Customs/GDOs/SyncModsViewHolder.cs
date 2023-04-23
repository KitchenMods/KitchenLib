using KitchenLib.Customs.GDOs;
using KitchenLib.Views;
using UnityEngine;

namespace KitchenLib.Customs
{
	internal class SyncModsViewHolder : BaseViewHolder<SyncMods>
	{
		public override string UniqueNameID => "SyncModsViewHolder";
		public override GameObject Prefab => new GameObject("SyncModsViewHolder");
		public override string Name => "SyncModsViewHolder";
	}
}
