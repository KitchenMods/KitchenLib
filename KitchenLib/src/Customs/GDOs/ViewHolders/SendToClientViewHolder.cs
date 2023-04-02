using KitchenLib.ShhhDontTellAnyone;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal class SendToClientViewHolder : BaseViewHolder<SendToClientView>
	{
		public override string UniqueNameID => "SendToClientViewHolder";
		public override GameObject Prefab => new GameObject("Send To Client View Holder");
		public override string Name => "Send To Client View Holder";
	}
}
