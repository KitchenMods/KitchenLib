using System;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
    public class CustomGDO
	{
		public static Dictionary<int, CustomGameDataObject> GDOs = new Dictionary<int, CustomGameDataObject>();
		public static Dictionary<Type, CustomGameDataObject> GDOsByType = new Dictionary<Type, CustomGameDataObject>();
		public static Dictionary<KeyValuePair<string, string>, CustomGameDataObject> GDOsByGUID = new Dictionary<KeyValuePair<string, string>, CustomGameDataObject>();
		public static Dictionary<KeyValuePair<string, string>, CustomGameDataObject> GDOsByModName = new Dictionary<KeyValuePair<string, string>, CustomGameDataObject>();
		public static Dictionary<int, CustomGameDataObject> GDOsByLegacyID = new Dictionary<int, CustomGameDataObject>();
		public static Dictionary<int, int> LegacyGDOIDs = new Dictionary<int, int>();

		public static T RegisterGameDataObject<T>(T gdo) where T : CustomGameDataObject
		{
			if (gdo.ID == 0)
				gdo.ID = gdo.GetHash();

			gdo.LegacyID = gdo.GetLegacyHash();

			if (GDOs.ContainsKey(gdo.ID))
			{
				Main.LogInfo($"Error while registering custom GDO of type {gdo.GetType().FullName} with ID={gdo.ID} and Name=\"{gdo.ModName}:{gdo.UniqueNameID}\". Double-check to ensure that the UniqueNameID is actually unique. (Clashing with : {GDOs[gdo.ID]})");
				return null;
			}

			GDOs.Add(gdo.ID, gdo);
			GDOsByLegacyID.Add(gdo.LegacyID, gdo);
			LegacyGDOIDs.Add(gdo.LegacyID, gdo.ID);
			GDOsByType.Add(gdo.GetType(), gdo);
			GDOsByGUID.Add(new KeyValuePair<string, string>(gdo.ModID, gdo.UniqueNameID), gdo);
			GDOsByModName.Add(new KeyValuePair<string, string>(gdo.ModName, gdo.UniqueNameID), gdo);

			return gdo;
		}
	}
}
