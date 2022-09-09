using UnityEngine;
using Unity.Entities;
using KitchenLib.Appliances;
using System.Collections.Generic;
using System.Reflection;
using Kitchen;

namespace KitchenLib.Utils
{
	public class SpawnUtils
	{
		public static Entity SpawnApplianceBlueprintAtPlayer(int id, float priceModifier = 0f, int forcePrice = -1) {
			var position = GameObject.Find("Player(Clone)").transform.position;
			return PostHelpers.CreateBlueprintLetter(EntityUtils.GetEntityManager(), position, id, priceModifier, forcePrice);
		}

		public static Entity SpawnApplianceBlueprintAtPlayer<T>(float priceModifier = 0f, int forcePrice = -1) where T : CustomAppliance {
			var appliance = CustomAppliances.Get<T>();
			if(appliance == null)
				return default(Entity);
			return SpawnApplianceBlueprintAtPlayer(appliance.ID, priceModifier, forcePrice);
		}

		public static Entity SpawnApplianceBlueprint(int id, float priceModifier = 0f, int forcePrice = -1)
		{
			ProvideStartingEnvelopes provideStartingEnvelopesInstance = SystemUtils.GetSystem<ProvideStartingEnvelopes>();
			MethodInfo getPostTile = ReflectionUtils.GetMethod<ProvideStartingEnvelopes>("GetPostTiles");
			List<Vector3> postTiles = (List<Vector3>)getPostTile.Invoke(provideStartingEnvelopesInstance, null);
			int num = 0;
			Vector3 position;
			if (provideStartingEnvelopesInstance.FindTile(ref num, postTiles, out position))
				return PostHelpers.CreateBlueprintLetter(EntityUtils.GetEntityManager(), position, id, priceModifier, forcePrice);
			else
				return default(Entity);
		}

		public static Entity SpawnApplianceBlueprint<T>(int id, float priceModifier = 0f, int forcePrice = -1)
		{
			var appliance = CustomAppliances.Get<T>();
			if (appliance == null)
				return default(Entity);
			return SpawnApplianceBlueprint(appliance.ID, priceModifier, forcePrice);
		}
	}
}
