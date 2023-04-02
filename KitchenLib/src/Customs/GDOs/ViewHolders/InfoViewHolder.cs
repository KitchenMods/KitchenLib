using KitchenLib.ShhhDontTellAnyone;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal class InfoViewHolder : BaseViewHolder<InfoView>
	{
		public override string UniqueNameID => "InfoViewHolder";
		public override GameObject Prefab => new GameObject("Info View Holder");
		public override string Name => "Info View Holder";
	}
}
