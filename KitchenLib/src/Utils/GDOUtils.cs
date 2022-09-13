using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitchen;
using KitchenData;

namespace KitchenLib.Utils
{
	public class GDOUtils
	{
		private static Dictionary<int, Appliance> appliances = new Dictionary<int, Appliance>();
		private static Dictionary<int, Item> items = new Dictionary<int, Item>();
		private static Dictionary<string, Process> processes = new Dictionary<string, Process>();
		private static Dictionary<string, Item.ItemProcess> customItemProcesses = new Dictionary<string, Item.ItemProcess>();
		private static Dictionary<string, Appliance.ApplianceProcesses> customApplianceProcesses = new Dictionary<string, Appliance.ApplianceProcesses>();

		public static void SetupGDOIndex(GameData gameData)
		{
			foreach (Item item in gameData.Get<Item>())
				GDOUtils.items.Add(item.ID, item);

			foreach (Appliance appliance in gameData.Get<Appliance>())
				GDOUtils.appliances.Add(appliance.ID, appliance);

			foreach (Process process in gameData.Get<Process>())
			{
				//Mod.Log(process.ID + " - " + process.name);
				GDOUtils.processes.Add(process.name, process);
			}
		}

		public static Appliance GetExistingAppliance(int id)
		{
			appliances.TryGetValue(id, out Appliance appliance);
			return appliance;
		}

		public static Item GetExistingItem(int id)
		{
			items.TryGetValue(id, out Item item);
			return item;
		}

		public static Process GetExistingProcess(string name)
		{
			processes.TryGetValue(name, out Process process);
			return process;
		}

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

	}
}
