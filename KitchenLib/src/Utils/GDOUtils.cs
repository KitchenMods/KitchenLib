using System.Collections.Generic;
using KitchenData;
using System;
using KitchenLib.Customs;

namespace KitchenLib.Utils
{
	public class GDOUtils
	{
		private static Dictionary<int, GameDataObject> GDOs = new Dictionary<int, GameDataObject>();

		private static Dictionary<string, Item.ItemProcess> customItemProcesses = new Dictionary<string, Item.ItemProcess>();
		private static Dictionary<string, Appliance.ApplianceProcesses> customApplianceProcesses = new Dictionary<string, Appliance.ApplianceProcesses>();

		public static void SetupGDOIndex(GameData gameData)
		{
			if (GDOs.Count > 0)
				return;
			foreach (GameDataObject gdo in gameData.Get<GameDataObject>())
			{
				GDOs.Add(gdo.ID, gdo);
			}
		}

		public static GameDataObject GetExistingGDO(int id)
		{
			GDOs.TryGetValue(id, out GameDataObject gdo);
			return gdo;
		}

		public static CustomGameDataObject GetCustomGameDataObject(int id)
		{
			CustomGDO.GDOs.TryGetValue(id, out var result);
			return result;
		}

		public static CustomGameDataObject GetCustomGameDataObject<T>()
		{
			CustomGDO.GDOsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		public static T GetCastedGDO<T, C>() where T : GameDataObject where C : CustomGameDataObject
		{
			return (T)GetCustomGameDataObject<C>()?.GameDataObject;
		}
	}
}
