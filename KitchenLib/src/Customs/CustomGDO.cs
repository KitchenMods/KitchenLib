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

		public static Dictionary<int, CustomProcess> Processes = new Dictionary<int, CustomProcess>();
		public static Dictionary<Type, CustomProcess> processesByType = new Dictionary<Type, CustomProcess>();

		public static Dictionary<int, CustomContract> Contracts = new Dictionary<int, CustomContract>();
		public static Dictionary<Type, CustomContract> contractsByType = new Dictionary<Type, CustomContract>();

		public static Dictionary<int, CustomDecor> Decors = new Dictionary<int, CustomDecor>();
		public static Dictionary<Type, CustomDecor> decorsByType = new Dictionary<Type, CustomDecor>();

		public static Dictionary<int, CustomDish> Dishes = new Dictionary<int, CustomDish>();
		public static Dictionary<Type, CustomDish> dishesByType = new Dictionary<Type, CustomDish>();

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
			Processes.Add(process.ID, process);
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

		public static T RegisterContract<T>(T contract) where T : CustomContract
		{
			if (contract.ID == 0)
				contract.ID = contract.GetHash();
			
			if (Contracts.ContainsKey(contract.ID))
			{
				Mod.Error("Contract: " + contract.Name + " failed to register - key:" + contract.ID + " already in use. Generating custom key. " + contract.GetHash());
				return null;
			}

			Contracts.Add(contract.ID, contract);
			contractsByType.Add(contract.GetType(), contract);
			Mod.Log($"Registered contract '{contract.ModName}:{contract.Name}' as {contract.ID}");
			return contract;
		}

		public static T RegisterDecor<T>(T decor) where T : CustomDecor
		{
			if (decor.ID == 0)
				decor.ID = decor.GetHash();
			
			if (Decors.ContainsKey(decor.ID))
			{
				Mod.Error("Decor: " + decor.Name + " failed to register - key:" + decor.ID + " already in use. Generating custom key. " + decor.GetHash());
				return null;
			}

			Decors.Add(decor.ID, decor);
			decorsByType.Add(decor.GetType(), decor);
			Mod.Log($"Registered contract '{decor.ModName}:{decor.Name}' as {decor.ID}");
			return decor;
		}

		public static T RegisterDish<T>(T dish) where T : CustomDish
		{
			if (dish.ID == 0)
				dish.ID = dish.GetHash();
			
			if (Dishes.ContainsKey(dish.ID))
			{
				Mod.Error("Dish: " + dish.Name + " failed to register - key:" + dish.ID + " already in use. Generating custom key. " + dish.GetHash());
				return null;
			}

			Dishes.Add(dish.ID, dish);
			dishesByType.Add(dish.GetType(), dish);
			Mod.Log($"Registered dish '{dish.ModName}:{dish.Name}' as {dish.ID}");
			return dish;
		}

		//Get Custom Appliance
		public static CustomAppliance GetCustomAppliance(int id)
		{
			Appliances.TryGetValue(id, out var result);
			return result;
		}

		public static CustomAppliance GetCustomAppliance<T>()
		{
			appliancesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Item
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

		//Get Custom Process
		public static CustomProcess GetCustomProcess(int id)
		{
			Processes.TryGetValue(id, out var result);
			return result;
		}
		public static CustomProcess GetCustomProcess<T>()
		{
			processesByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Item Process

		public static CustomItemProcess GetCustomItemProcess(string name)
		{
			ItemProcesses.TryGetValue(name, out var result);
			return result;
		}

		public static CustomItemProcess GetCustomItemProcess<T>()
		{
			ItemProcesses.TryGetValue(typeof(T).Name, out var result);
			return result;
		}

		//Get Custom Appliance Process

		public static CustomApplianceProcess GetCustomApplianceProcess(string name)
		{
			ApplianceProcesses.TryGetValue(name, out var result);
			return result;
		}

		public static CustomApplianceProcess GetCustomApplianceProcess<T>()
		{
			ApplianceProcesses.TryGetValue(typeof(T).Name, out var result);
			return result;
		}

		//Get Custom Contract
		public static CustomContract GetCustomContract(int id)
		{
			Contracts.TryGetValue(id, out var result);
			return result;
		}
		public static CustomContract GetCustomContract<T>()
		{
			contractsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Decor
		public static CustomDecor GetCustomDecor(int id)
		{
			Decors.TryGetValue(id, out var result);
			return result;
		}
		public static CustomDecor GetCustomDecor<T>()
		{
			decorsByType.TryGetValue(typeof(T), out var result);
			return result;
		}

		//Get Custom Dish
		public static CustomDish GetCustomDish(int id)
		{
			Dishes.TryGetValue(id, out var result);
			return result;
		}
		public static CustomDish GetCustomDish<T>()
		{
			dishesByType.TryGetValue(typeof(T), out var result);
			return result;
		}
	}
}
