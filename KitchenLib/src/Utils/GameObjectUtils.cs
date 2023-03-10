using UnityEngine;

namespace KitchenLib.Utils
{
	public static class GameObjectUtils
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

        // Color
        public static Color ColorFromHex(int hex)
        {
            return new Color(((hex & 0xFF0000) >> 16) / 255.0f, ((hex & 0xFF00) >> 8) / 255.0f, (hex & 0xFF) / 255.0f);
        }

        // Prefab / GameObject
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() != null;
        }

        public static T TryAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T comp = gameObject.GetComponent<T>();
            if (comp == null)
                return gameObject.AddComponent<T>();
            return comp;
        }

        public static GameObject GetChild(this GameObject gameObject, string childPath)
        {
            return GetChildObject(gameObject, childPath);
        }

        public static int GetChildCount(this GameObject gameObject)
        {
            return gameObject.transform.childCount;
        }
    }
}
