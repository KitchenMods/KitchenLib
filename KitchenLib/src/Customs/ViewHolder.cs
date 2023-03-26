using KitchenData;
using KitchenLib.Views;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class ViewHolder : CustomAppliance
	{
		public override string UniqueNameID => "ViewHolder";
		public override GameObject Prefab => new GameObject("View Holder");

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<CommandView>();
		}
	}
}
