using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
	public struct PrefabContainer
	{
		public float ScaleX;
		public float ScaleY;
		public float ScaleZ;
		public string MaterialName;
		public bool Collider;

		public GameObject Convert(string guid)
		{
			return (GameObject)JSONPackUtils.GetSoftDependency(
				"TestCubes.TestCubeManager",
				"GetPrefab",
				false,
				new[]
				{
					typeof(string),
					typeof(float),
					typeof(float),
					typeof(float),
					typeof(Material),
					typeof(bool)
				}).Invoke(null, new object[] { guid, ScaleX, ScaleY, ScaleZ, JSONPackUtils.GetMaterialByName(MaterialName), Collider });
		}
	}
}