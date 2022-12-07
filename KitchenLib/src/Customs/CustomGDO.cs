using System;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public class CustomGDO
	{
		public static Dictionary<int, CustomGameDataObject> GDOs = new Dictionary<int, CustomGameDataObject>();
		public static Dictionary<Type, CustomGameDataObject> GDOsByType = new Dictionary<Type, CustomGameDataObject>();

		public static T RegisterGameDataObject<T>(T gdo) where T : CustomGameDataObject
		{
			if (gdo.ID == 0)
				gdo.ID = gdo.GetHash();

			if (GDOs.ContainsKey(gdo.ID))
			{
				return null;
			}

			if (GDOs.ContainsKey(gdo.ID))
				return null;

			GDOs.Add(gdo.ID, gdo);
			GDOsByType.Add(gdo.GetType(), gdo);

			return gdo;
		}
	}
}
