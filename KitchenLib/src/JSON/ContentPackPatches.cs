using HarmonyLib;
using KitchenData;
using KitchenLib.JSON.Models.Containers;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.JSON
{
	public class ContentPackPatches
	{
		public static void PostfixPatch(Type type, string methodname)
		{
			Main.harmonyInstance.Patch(type.GetMethod(methodname), null, new HarmonyMethod(type.GetMethod($"{methodname}_Postfix")));
		}

		public static GameObject PrefabConverter(string key, string str)
		{
			if (int.TryParse(str, out int id))
				return ((Item)GDOUtils.GetExistingGDO(id) ?? (Item)GDOUtils.GetCustomGameDataObject(id)?.GameDataObject).Prefab;
			else
				return ContentPackManager.AssetBundleTable[key].FirstOrDefault(x => x.LoadAsset<GameObject>(str) != null)?.LoadAsset<GameObject>(str);
		}

		public static List<Item.ItemProcess> ItemProcessesConverter(List<ItemProcessContainer> container)
		{
			if (container == null)
				return null;
			List<Item.ItemProcess> Processes = new List<Item.ItemProcess>();
			for (int i = 0; i < container.Count; i++)
			{
				Item.ItemProcess process = container[i].Convert();
				Processes.Add(process);
			}
			return Processes;
		}

		public static List<ItemGroup.ItemSet> ItemSetsConverter(List<ItemSetContainer> container)
		{
			if (container == null)
				return null;
			List<ItemGroup.ItemSet> Sets = new List<ItemGroup.ItemSet>();
			for (int i = 0; i < container.Count; i++)
			{
				ItemGroup.ItemSet Set = container[i].Convert();
				Sets.Add(Set);
			}
			return Sets;
		}

		public static HashSet<Dish.IngredientUnlock> IngredientUnlocksConverter(List<IngredientUnlockContainer> container)
		{
			if (container == null)
				return null;
			List<Dish.IngredientUnlock> unlocks = new List<Dish.IngredientUnlock>();
			for (int i = 0; i < container.Count; i++)
			{
				Dish.IngredientUnlock unlock = container[i].Convert();
				unlocks.Add(unlock);
			}
			return new HashSet<Dish.IngredientUnlock>(unlocks);
		}

		public static List<Appliance.ApplianceProcesses> ApplianceProcessesConverter(List<ApplianceProcessesContainer> container)
		{
			if (container == null)
				return null;
			List<Appliance.ApplianceProcesses> processes = new List<Appliance.ApplianceProcesses>();
			for (int i=0; i<container.Count; i++)
			{
				processes.Add(container[i].Convert());
			}
			return processes;
		}

		public static List<Dish.MenuItem> MenuItemsConverter(List<MenuItemContainer> container)
		{
			if (container == null)
				return null;
			List<Dish.MenuItem> items = new List<Dish.MenuItem>();
			for (int i=0; i<container.Count; i++)
			{
				items.Add(container[i].Convert());
			}
			return items;
		}

		public static T GDOConverter<T>(string str) where T : GameDataObject
		{
			if (str == null)
				return null;
			Main.LogInfo($"ItemID: {str}");
			if (int.TryParse(str, out int id))
				return (T)GDOUtils.GetExistingGDO(id) ?? (T)GDOUtils.GetCustomGameDataObject(id)?.GameDataObject;
			else
			{
				string[] split = str.Split(':');
				string mod_id = split[0];
				string name = split[1];
				return (T)GDOUtils.GetCustomGameDataObject(mod_id, name).GameDataObject;
			}
		}

		public static List<T> GDOsConverter<T>(List<string> liststr) where T : GameDataObject
		{
			if (liststr == null)
				return null;
			List<T> list = new List<T>();
			for (int i = 0; i < liststr.Count; i++)
			{
				string str = liststr[i];
				list.Add(GDOConverter<T>(str));
			}
			return list;
		}

		public static HashSet<T> HashGDOsConverter<T>(List<string> liststr) where T : GameDataObject
		{
			if (liststr == null)
				return null;
			return new HashSet<T>(GDOsConverter<T>(liststr));
		}
	}
}
