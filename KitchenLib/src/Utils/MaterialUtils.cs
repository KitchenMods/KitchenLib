using KitchenLib.Customs;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class MaterialUtils
	{
		private static Dictionary<string, Material> materialIndex = new Dictionary<string, Material>();

		public static void ApplyMaterial(GameObject prefab, string path, Material[] materials)
		{
			var currentRef = prefab.transform;
			var splitPath = path.Split('/');
			foreach (var part in splitPath)
				currentRef = currentRef?.Find(part);

			var component = currentRef?.GetComponent<MeshRenderer>();
			if (component == null)
				return; // or throw?

			component.materials = materials;
		}

		public static void ApplyMaterial<T>(GameObject prefab, string path, Material[] materials) where T : Renderer
		{
			var currentRef = prefab.transform;
			var splitPath = path.Split('/');
			foreach (var part in splitPath)
				currentRef = currentRef?.Find(part);

			var component = currentRef?.GetComponent<T>();
			if (component == null)
				return; // or throw?

			component.materials = materials;
		}

		private static List<GameObject> ListOfChildren = new List<GameObject>();
		private static void getChildRecursive(GameObject obj)
		{
			if (null == obj)
				return;

			if (obj.name.ToLower().Contains("wallpaper") || obj.name.ToLower().Contains("flooring"))
				return;

			foreach (Transform child in obj.transform)
			{
				if (null == child)
					continue;
				ListOfChildren.Add(child.gameObject);
				getChildRecursive(child.gameObject);
			}
		}

		public static void SetupMaterialIndex()
		{
			if (materialIndex.Count > 0)
				return;

			foreach (Material material in Resources.FindObjectsOfTypeAll(typeof(Material)))
			{
				if (!materialIndex.ContainsKey(material.name))
				{
					materialIndex.Add(material.name, material);
				}
			}

		}

		public static List<Material> GetAllMaterials(bool includeCustom)
		{
			List<Material> materials = new List<Material>();
			foreach (Material material in materialIndex.Values)
			{
				materials.Add(material);
			}

			return materials;
		}

		public static List<Material> GetAllMaterials(bool includeCustom, List<string> shaders)
		{
			List<Material> materials = new List<Material>();
			foreach (Material material in materialIndex.Values)
			{
				if (shaders.Contains(material.shader.name))
					materials.Add(material);
			}

			foreach (Material material in CustomMaterials.GetCustomMaterials())
			{
				Main.instance.Log("1");
				Main.instance.Log(material.shader.name);
				if (shaders.Contains(material.shader.name))
				{
					Main.instance.Log("2");
					materials.Add(material);
				}
			}

			return materials;
		}

		public static Material GetExistingMaterial(string materialName)
		{
			if (materialIndex.ContainsKey(materialName))
				return materialIndex[materialName];
			else
				return null;
		}

		public static Material GetCustomMaterial(string materialName)
		{
			return CustomMaterials.CustomMaterialsIndex[materialName];
		}
	}
}
