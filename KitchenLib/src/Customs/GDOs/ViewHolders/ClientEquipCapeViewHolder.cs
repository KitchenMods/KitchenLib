using KitchenLib.Systems;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	internal class ClientEquipCapeViewHolder : BaseViewHolder<ClientEquipCapes>
	{
		public override string UniqueNameID => "ClientEquipCapeViewHolder";
		public override GameObject Prefab => new GameObject("Client Equip Cape View Holder");
		public override string Name => "Client Equip Cape View Holder";
	}
}
