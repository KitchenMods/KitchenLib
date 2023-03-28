using KitchenData;
using KitchenLib.Views;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class SendToClientViewHolder : CustomAppliance
	{
		public override string UniqueNameID => "SendToClientViewHolder";
		public override GameObject Prefab => new GameObject("Send To Client View Holder");
		public override bool IsPurchasable => false;
		public override bool IsNonInteractive => true;
		public override string Name => "Send To Client View Holder";

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<SendToClientView>();
		}
	}
}
