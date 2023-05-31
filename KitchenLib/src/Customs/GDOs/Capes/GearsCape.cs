using KitchenData;
using KitchenLib.Customs.GDOs;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	internal class GearsCape : BaseCape
	{
		public override string CapeName => "Gears";

		public override void OnRegister(PlayerCosmetic gameDataObject)
		{
			MaterialUtils.ApplyMaterial(gameDataObject.Visual, "Cape", new Material[] { MaterialUtils.GetCustomMaterial("Gears Cape") });
		}
	}
}
