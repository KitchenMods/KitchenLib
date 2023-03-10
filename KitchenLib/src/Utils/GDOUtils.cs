using KitchenData;
using KitchenLib.Customs;
using System.Collections.Generic;
using System.Linq;

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

		public static CustomGameDataObject GetCustomGameDataObject(string modID, string name)
		{
			CustomGDO.GDOsByGUID.TryGetValue(new KeyValuePair<string, string>(modID, name), out var result);
			return result;
		}

		public static List<CustomGameDataObject> GetCustomGameDataObjectsFromMod(string modID)
		{
			return CustomGDO.GDOsByGUID.Where(entry => entry.Key.Key == modID).Select(entry => entry.Value).ToList();
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

		public static T GetCastedGDO<T>(string modID, string name) where T : GameDataObject
		{
			return (T)GetCustomGameDataObject(modID, name)?.GameDataObject;
		}


		public static Dictionary<int, List<int>> BlacklistedDishSides = new Dictionary<int, List<int>>();
		public static void BlacklistSide(Item item, int side)
		{
			if (!BlacklistedDishSides.ContainsKey(item.ID))
			{
				BlacklistedDishSides.Add(item.ID, new List<int>());
			}
			if (!BlacklistedDishSides[item.ID].Contains(side))
			{
				BlacklistedDishSides[item.ID].Add(side);
			}
		}
		public static void WhitelistSide(Item item, int side)
		{
			if (GameData.Main == null)
			{
				Main.LogWarning("Please use WhitelistSide in OnInitialise");
				return;
			}
			foreach (Dish dish in GameData.Main.Get<Dish>())
			{
				foreach (Dish.MenuItem menuItem in dish.UnlocksMenuItems)
				{
					if (!BlacklistedDishSides.ContainsKey(menuItem.Item.ID))
						BlacklistedDishSides.Add(menuItem.Item.ID, new List<int>());
					if (!BlacklistedDishSides[menuItem.Item.ID].Contains(side))
						BlacklistedDishSides[menuItem.Item.ID].Add(side);
				}
			}

			if (BlacklistedDishSides.ContainsKey(item.ID))
			{
				if (BlacklistedDishSides[item.ID].Contains(side))
				{
					BlacklistedDishSides[item.ID].Remove(side);
				}
			}
		}
	}
}
