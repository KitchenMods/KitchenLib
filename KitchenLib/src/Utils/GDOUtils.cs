using System.Collections.Generic;
using KitchenData;
using System;

namespace KitchenLib.Utils
{
	public class GDOUtils
	{
		private static Dictionary<int, GameDataObject> GDOs = new Dictionary<int, GameDataObject>();

		private static Dictionary<string, Item.ItemProcess> customItemProcesses = new Dictionary<string, Item.ItemProcess>();
		private static Dictionary<string, Appliance.ApplianceProcesses> customApplianceProcesses = new Dictionary<string, Appliance.ApplianceProcesses>();

		public static void SetupGDOIndex(GameData gameData)
		{
			GDOs.Clear();
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

		/*
		 * Obsolete Methods
		 */

		[Obsolete("Use the GetExistingGDO method instead")]
		public static Appliance GetExistingAppliance(int id)
		{
			GDOs.TryGetValue(id, out GameDataObject appliance);
			return (Appliance)appliance;
		}

		[Obsolete("Use the GetExistingGDO method instead")]
		public static Item GetExistingItem(int id)
		{
			GDOs.TryGetValue(id, out GameDataObject item);
			return (Item)item;
		}

		[Obsolete("Use the GetExistingGDO method instead")]
		public static Process GetExistingProcess(int id)
		{
			GDOs.TryGetValue(id, out GameDataObject process);
			return (Process)process;
		}

/*
		public static Item.ItemProcess GetCustomItemProcess(string name)
		{
			customItemProcesses.TryGetValue(name, out Item.ItemProcess process);
			return process;	
		}

		public static Appliance.ApplianceProcesses GetCustomApplianceProcess(string name)
		{
			customApplianceProcesses.TryGetValue(name, out Appliance.ApplianceProcesses process);
			return process;	
		}

		public static void AddCustomItemProcess(string name, Item.ItemProcess process)
		{
			customItemProcesses.Add(name, process);
		}

		public static void AddCustomApplianceProcess(string name, Appliance.ApplianceProcesses process)
		{
			customApplianceProcesses.Add(name, process);
		}
		*/
	}
}
