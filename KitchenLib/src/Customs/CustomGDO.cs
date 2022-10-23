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

			GDOs.Add(gdo.ID, gdo);
			GDOsByType.Add(gdo.GetType(), gdo);

			return gdo;
		}

		public static CustomGameDataObject GetGameDataObject(int id)
		{
			GDOs.TryGetValue(id, out var result);
			return result;
		}

		public static CustomGameDataObject GetGameDataObject<T>()
		{
			GDOsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		/*
		 * Obsolete Methods
		 */

		[Obsolete("Use the GetGameDataObject method instead")]
		public static CustomAppliance GetCustomAppliance(int id)
		{
			return (CustomAppliance)GetGameDataObject(id);
		} 

		[Obsolete("Use the GetGameDataObject method instead")]
		public static CustomAppliance GetCustomAppliance<T>()
		{
			return (CustomAppliance)GetGameDataObject<T>();
		}

		[Obsolete("Use the GetGameDataObject method instead")]
		public static CustomItem GetCustomItem(int id)
		{
			return (CustomItem)GetGameDataObject(id);
		} 

		[Obsolete("Use the GetGameDataObject method instead")]
		public static CustomItem GetCustomItem<T>()
		{
			return (CustomItem)GetGameDataObject<T>();
		}

		[Obsolete("Use the GetGameDataObject method instead")]
		public static CustomProcess GetCustomProcess(int id)
		{
			return (CustomProcess)GetGameDataObject(id);
		} 

		[Obsolete("Use the GetGameDataObject method instead")]
		public static CustomProcess GetCustomProcessByType<T>()
		{
			return (CustomProcess)GetGameDataObject<T>();
		}
	}
}
