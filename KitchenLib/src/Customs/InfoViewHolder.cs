using KitchenData;
using KitchenLib.Views;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class InfoViewHolder : CustomAppliance
	{
		public override string UniqueNameID => "InfoViewHolder";
		public override GameObject Prefab => new GameObject("Info View Holder");
		public override bool IsPurchasable => false;
		public override bool IsNonInteractive => true;
		public override string Name => "Info View Holder";

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<InfoView>();
		}
	}
}
