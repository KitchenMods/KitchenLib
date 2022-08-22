using System;
using System.Text;
using UnityEngine;
using Unity.Entities;

namespace KitchenLib.Utils
{
	public class SpawnUtils
	{
		public static Entity SpawnApplianceBlueprint(int id, float priceModifier = 0f, int forcePrice = -1) {
			var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<Kitchen.PlayerManager>().EntityManager;
			var position = GameObject.Find("Player(Clone)").transform.position;
			return Kitchen.PostHelpers.CreateBlueprintLetter(em, position, id, priceModifier, forcePrice);
		}
	}
}
