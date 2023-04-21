using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Views;
using UnityEngine;

namespace KitchenLib.src.Customs
{
	public class ClientEquipCapeViewHolder : CustomAppliance
	{
		public override string UniqueNameID => "ClientEquipCapeViewHolder";
		public override GameObject Prefab => new GameObject("Client Equip Cape View Holder");
		public override bool IsPurchasable => false;
		public override bool IsNonInteractive => true;
		public override string Name => "Client Equip Cape View Holder";

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<ClientEquipCapes>();
		}
	}
}
