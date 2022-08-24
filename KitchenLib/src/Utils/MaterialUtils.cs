using UnityEngine;

namespace KitchenLib.Utils
{
	public class MaterialUtils
	{
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
	}
}
