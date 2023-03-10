using System.Collections.Generic;
namespace KitchenLib.Utils
{
	public class VariousUtils
	{
		private static Dictionary<string, int> ids = new Dictionary<string, int>();
		public static int GetID(string name)
		{
			int ID = 0;
			if (ids.ContainsKey(name))
				return ids[name];
			while (ID == 0)
			{
				int x = UnityEngine.Random.Range(-999999, 999999);
				if (!ids.ContainsValue(x))
				{
					ids.Add(name, x);
					ID = x;
				}
			}
			return ID;
		}
	}
}