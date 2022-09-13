using System;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public class CustomGDO
	{
		public static Dictionary<int, CustomAppliance> Appliances = new Dictionary<int, CustomAppliance>();
		private static Dictionary<Type, CustomAppliance> appliancesByType = new Dictionary<Type, CustomAppliance>();

		public static Dictionary<int, CustomItem> Items = new Dictionary<int, CustomItem>();
		private static Dictionary<Type, CustomItem> itemsByType = new Dictionary<Type, CustomItem>();

		public static Dictionary<string, CustomItemProcess> ItemProcesses = new Dictionary<string, CustomItemProcess>();
		public static Dictionary<string, CustomApplianceProcess> ApplianceProcesses = new Dictionary<string, CustomApplianceProcess>();

		public static Dictionary<string, CustomProcess> Processes = new Dictionary<string, CustomProcess>();
		public static Dictionary<Type, CustomProcess> processesByType = new Dictionary<Type, CustomProcess>();

		public static T RegisterItemProcess<T>(T process) where T : CustomItemProcess
		{
			if (ItemProcesses.ContainsKey(process.ProcessName)) {
				return null;
			}

			ItemProcesses.Add(process.ProcessName, process);
			return process;
		}

		public static T RegisterApplianceProcess<T>(T process) where T : CustomApplianceProcess
		{
			if (ApplianceProcesses.ContainsKey(process.ProcessName)) {
				return null;
			}

			ApplianceProcesses.Add(process.ProcessName, process);
			return process;
		}

		public static T RegisterProcess<T>(T process) where T : CustomProcess
		{
			if (process.ID == 0)
				process.ID = process.GetHash();
			Processes.Add(process.ProcessName, process);
			processesByType.Add(process.GetType(), process);
			return process;
		}

		public static T RegisterAppliance<T>(T appliance) where T : CustomAppliance
		{
			if (appliance.ID == 0)
				appliance.ID = appliance.GetHash();

			if (Appliances.ContainsKey(appliance.ID))
			{
				Mod.Error("Appliance: " + appliance.Name + " failed to register - key:" + appliance.ID + " already in use. Generating custom key. " + appliance.GetHash());
				return null;
			}

			Appliances.Add(appliance.ID, appliance);
			appliancesByType.Add(appliance.GetType(), appliance);
			Mod.Log($"Registered appliance '{appliance.ModName}:{appliance.Name}' as {appliance.ID}");
			return appliance;
		}
		public static T RegisterItem<T>(T item) where T : CustomItem
		{
			if (item.ID == 0)
				item.ID = item.GetHash();

			if (Items.ContainsKey(item.ID))
			{
				Mod.Error("Item: " + item.Name + " failed to register - key:" + item.ID + " already in use. Generating custom key. " + item.GetHash());
				return null;
			}
			
			Items.Add(item.ID, item);
			itemsByType.Add(item.GetType(), item);
			Mod.Log($"Registered item '{item.ModName}:{item.Name}' as {item.ID}");
			return item;
		}

		public static CustomAppliance GetCustomAppliance(int id)
		{
			Appliances.TryGetValue(id, out var result);
			return result;
		}

		public static CustomItemProcess GetCustomItemProcess(string name)
		{
			ItemProcesses.TryGetValue(name, out var result);
			return result;
		}

		public static CustomProcess GetCustomProcess(string name)
		{
			Processes.TryGetValue(name, out var result);
			return result;
		}

		public static CustomProcess GetCustomProcessByType<T>()
		{
			processesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		public static CustomAppliance GetCustomAppliance<T>()
		{
			appliancesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		public static CustomItem GetCustomItem(int id)
		{
			Items.TryGetValue(id, out var result);
			return result;
		}

		public static CustomItem GetCustomItem<T>()
		{
			itemsByType.TryGetValue(typeof(T), out var result);
			return result;
		}
	}
}
