using System;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Appliances
{
	public class CustomApplianceInfo
	{
		public int ID;
		public string ModName;
		public string Name;
		public string Description;
		public int BaseApplianceId = -1248669347;
		public int BasePrefabId = -1248669347;
		public GameObject Prefab;

		public Func<InteractionData, bool> OnCheckInteractPossible;
		public Action<InteractionData> OnInteract;

		public Appliance Appliance;

		public int GetHash() {
			return StringUtils.GetInt32HashCode($"{ModName}:{Name}");
		}
	}
}
