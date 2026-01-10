using KitchenData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KitchenLib.Customs
{
    public class CustomGDO
	{
		public static Dictionary<int, CustomGameDataObject> GDOs = new Dictionary<int, CustomGameDataObject>();
		[Obsolete("This collection creates a singleton design class pattern. Use GDOTypeInstances collection. Only purpose now is the public access to the collection might be used by mods")]
		public static Dictionary<Type, CustomGameDataObject> GDOsByType = new Dictionary<Type, CustomGameDataObject>(); //TODO: Move towards removing this by creating a getter for a virtual collection
		public static Dictionary<Type, List<CustomGameDataObject>> GDOTypeInstances = new Dictionary<Type, List<CustomGameDataObject>>();
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
			if (!GDOsByType.ContainsKey(gdo.GetType())) GDOsByType.Add(gdo.GetType(), gdo); //support legacy collection
			SafeAddTypeInstance(GDOTypeInstances, gdo);
			GDOsByGUID.Add(new KeyValuePair<string, string>(gdo.ModID, gdo.UniqueNameID), gdo);
			GDOsByModName.Add(new KeyValuePair<string, string>(gdo.ModName, gdo.UniqueNameID), gdo);

			return gdo;
		}

		static void SafeAddTypeInstance(Dictionary<Type,List<CustomGameDataObject>> dictionary, CustomGameDataObject gdo)
		{
			if (!dictionary.TryGetValue(gdo.GetType(), out List<CustomGameDataObject> twins))
			{
				twins = new List<CustomGameDataObject>();				
				dictionary.Add(gdo.GetType(), twins);
			}
			twins.Add(gdo);
		}
	}
}
