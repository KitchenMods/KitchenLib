using KitchenData;
using KitchenLib.Customs;
using KitchenLib.ShhhDontTellAnyone;
using KitchenLib.src.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
