using UnityEngine;
using System.Collections.Generic;
using KitchenData;

namespace KitchenLib.Utils
{
	public class MaterialUtils
	{
        private static Dictionary<string, Material> materialIndex = new Dictionary<string, Material>();

        public static void ApplyMaterial(GameObject prefab, string path, Material[] materials) {
			var currentRef = prefab.transform;
			var splitPath = path.Split('/');
			foreach(var part in splitPath)
				currentRef = currentRef?.Find(part);
			
			var component = currentRef?.GetComponent<MeshRenderer>();
			if(component == null)
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

        public static void SetupMaterialIndex(GameData gameData)
        {
			materialIndex.Clear();

			foreach (Appliance app in gameData.Get<Appliance>())
                getChildRecursive(app.Prefab);
            foreach (Item item in gameData.Get<Item>())
                getChildRecursive(item.Prefab);
            foreach (GameObject o in MaterialUtils.ListOfChildren)
                if (o.GetComponent<MeshRenderer>() != null)
                    foreach (Material material in o.GetComponent<MeshRenderer>().materials)
                        if (!materialIndex.ContainsKey(material.name))
                            materialIndex.Add(material.name, material);
        }

        public static Material GetExistingMaterial(string materialName)
        {
            materialName = materialName + " (Instance)";
            if (materialIndex.ContainsKey(materialName))
                return materialIndex[materialName];
            else
                return null;
        }
    }
}
