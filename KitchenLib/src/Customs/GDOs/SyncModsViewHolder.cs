using KitchenData;
using KitchenLib.Customs;
using KitchenLib.ShhhDontTellAnyone;
using KitchenLib.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenLib.src.Customs
{
	public class SyncModsViewHolder : CustomAppliance
	{
		public override string UniqueNameID => "SyncModsViewHolder";
		public override GameObject Prefab => new GameObject("SyncModsViewHolder");
		public override bool IsPurchasable => false;
		public override bool IsNonInteractive => true;
		public override string Name => "SyncModsViewHolder";

		public override void OnRegister(Appliance gameDataObject)
		{
			gameDataObject.Prefab.AddComponent<SyncMods>();
		}
	}
}
