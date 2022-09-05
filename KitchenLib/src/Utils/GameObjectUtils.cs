using UnityEngine;

namespace KitchenLib.Utils
{
	public class GameObjectUtils
	{
		public static GameObject GetChildObject(GameObject prefab, string path)
		{
			var currentRef = prefab.transform;
			var splitPath = path.Split('/');
			foreach (var part in splitPath)
				currentRef = currentRef?.Find(part);

			if (currentRef != null)
				return currentRef.gameObject;
			else
				return null;
		}
	}
}
