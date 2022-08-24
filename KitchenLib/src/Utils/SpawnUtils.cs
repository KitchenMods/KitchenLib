using System;
using System.Text;
using UnityEngine;
using Unity.Entities;
using KitchenLib.Appliances;

namespace KitchenLib.Utils
{
	public class SpawnUtils
	{
		public static Entity SpawnApplianceBlueprint(int id, float priceModifier = 0f, int forcePrice = -1) {
			var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<Kitchen.PlayerManager>().EntityManager;
			var position = GameObject.Find("Player(Clone)").transform.position;
			return Kitchen.PostHelpers.CreateBlueprintLetter(em, position, id, priceModifier, forcePrice);
		}

		public static Entity SpawnApplianceBlueprint<T>(float priceModifier = 0f, int forcePrice = -1) where T : CustomAppliance {
			var appliance = CustomAppliances.Get<T>();
			if(appliance == null)
				return default(Entity);
			return SpawnApplianceBlueprint(appliance.ID, priceModifier, forcePrice);
		}
	}
}
