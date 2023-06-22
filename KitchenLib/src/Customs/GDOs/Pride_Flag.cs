using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenLib.Customs.GDOs
{
	public class Pride_Flag : CustomItem
	{
		public override string UniqueNameID => "Pride_Flag";
		public override GameObject Prefab => Main.bundle.LoadAsset<GameObject>("Pride_Flag");
	}
}
