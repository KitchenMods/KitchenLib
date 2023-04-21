using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class Gears_Cape : Base_Cape
	{
		public override string CapeName => "Gears";

		public override void OnRegister(PlayerCosmetic gameDataObject)
		{
			base.OnRegister(gameDataObject);
			MaterialUtils.ApplyMaterial(gameDataObject.Visual, "Cape", new Material[] { MaterialUtils.GetCustomMaterial("Gears Cape") });
		}
	}
}
