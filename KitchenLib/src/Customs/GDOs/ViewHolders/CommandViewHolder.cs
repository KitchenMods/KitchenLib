using KitchenLib.ShhhDontTellAnyone;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal class CommandViewHolder : BaseViewHolder<CommandView>
	{
		public override string UniqueNameID => "ViewHolder";
		public override GameObject Prefab => new GameObject("Command View Holder");
		public override string Name => "Command View Holder";
	}
}
