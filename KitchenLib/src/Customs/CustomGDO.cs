using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CustomGDO
	{
		public static Dictionary<int, CustomGameDataObject> GDOs = new Dictionary<int, CustomGameDataObject>();
		public static Dictionary<Type, CustomGameDataObject> GDOsByType = new Dictionary<Type, CustomGameDataObject>();
		public static Dictionary<KeyValuePair<string, string>, CustomGameDataObject> GDOsByGUID = new Dictionary<KeyValuePair<string, string>, CustomGameDataObject>();

		public static T RegisterGameDataObject<T>(T gdo) where T : CustomGameDataObject
		{
			if (gdo.ID == 0)
				gdo.ID = gdo.GetHash();

			if (GDOs.ContainsKey(gdo.ID))
			{
				Debug.LogWarning($"[KitchenLib] Error while registering custom GDO of type {gdo.GetType().FullName} with ID={gdo.ID} and Name=\"{gdo.ModName}:{gdo.UniqueNameID}\". Double-check to ensure that the UniqueNameID is actually unique.");
				return null;
			}

			GDOs.Add(gdo.ID, gdo);
			GDOsByType.Add(gdo.GetType(), gdo);
			GDOsByGUID.Add(new KeyValuePair<string, string>(gdo.ModName, gdo.UniqueNameID), gdo);

			return gdo;
		}
	}
}
