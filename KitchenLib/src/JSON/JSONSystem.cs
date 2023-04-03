using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenMods;
using UnityEngine;

namespace KitchenLib.src.JSON
{
	public class JSONSystem : GenericSystemBase, IModSystem
	{
		protected override void Initialise()
		{
			Main.instance.Log("Loading packs...");
			
		}

		protected override void OnUpdate()
		{
		}
	}
}
