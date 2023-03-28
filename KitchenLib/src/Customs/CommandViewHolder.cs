using KitchenData;
using KitchenLib.Views;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CommandViewHolder : CustomAppliance
	{
		public override string UniqueNameID => "ViewHolder";
		public override GameObject Prefab => new GameObject("Command View Holder");
		public override bool IsPurchasable => false;
		public override bool IsNonInteractive => true;
		public override string Name => "Command View Holder";

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<CommandView>();
		}
	}
}
