using KitchenData;
using KitchenLib.Customs;
using KitchenLib.JSON.Interfaces;
using KitchenLib.JSON.Models.Containers;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.JSON.Models.Jsons
{
	public class JsonDish : CustomDish, IHasDisplayPrefab, IHasIconPrefab
	{
		[field: JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }
		[JsonProperty("Author", Required = Required.Always)]
		public string Author { get; set; }
		[JsonProperty("GDOName")]
		public string GDOName { get; set; } = "";

		[JsonProperty("DishType")]
		public override DishType Type { get; protected set; }

		[JsonProperty("ExtraOrderUnlocks")]
		public List<IngredientUnlockContainer> TempExtraOrderUnlocks { get; set; }
		[JsonProperty("MinimumIngredients")]
		public List<string> TempMinimumIngredients { get; set; }
		[JsonProperty("RequiredProcesses")]
		public List<string> TempRequiredProcesses { get; set; }
		[JsonProperty("BlockProviders")]
		public List<string> TempBlockProviders { get; set; }
		[JsonProperty("IconPrefab")]
		public string TempIconPrefab { get; set; }
		[JsonProperty("DisplayPrefab")]
		public string TempDisplayPrefab { get; set; }
		[JsonProperty("ResultingMenuItems")]
		public List<MenuItemContainer> TempResultingMenuItems { get; set; }
		[JsonProperty("IngredientsUnlocks")]
		public List<IngredientUnlockContainer> TempIngredientsUnlocks { get; set; }
		[JsonProperty("RequiredDishItem")]
		public string TempRequiredDishItem { get; set; }

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			ModName = context.Context.ToString();
			ModID = $"{Author}.{ModName}";
		}

		public override void OnRegister(Dish gameDataObject)
		{
			gameDataObject.name = GDOName;
		}

		public static void get_ExtraOrderUnlocks_Postfix(JsonDish __instance, ref HashSet<Dish.IngredientUnlock> __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.IngredientUnlocksConverter(__instance.TempExtraOrderUnlocks);
			}
		}

		public static void get_MinimumIngredients_Postfix(JsonDish __instance, ref HashSet<Item> __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.HashGDOsConverter<Item>(__instance.TempMinimumIngredients);
			}
		}

		public static void get_RequiredProcesses_Postfix(JsonDish __instance, ref HashSet<Process> __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.HashGDOsConverter<Process>(__instance.TempRequiredProcesses);
			}
		}

		public static void get_BlockProviders_Postfix(JsonDish __instance, ref HashSet<Item> __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.HashGDOsConverter<Item>(__instance.TempBlockProviders);
			}
		}

		public static void get_IconPrefab_Postfix(JsonDish __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.IconPrefabConverter<Dish>(__instance.ModName, __instance.TempIconPrefab);
			}
		}

		public static void get_DisplayPrefab_Postfix(JsonDish __instance, ref GameObject __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.DisplayPrefabConverter<Dish>(__instance.ModName, __instance.TempDisplayPrefab);
			}
		}

		public static void get_ResultingMenuItems_Postfix(JsonDish __instance, ref List<Dish.MenuItem> __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.MenuItemsConverter(__instance.TempResultingMenuItems);
			}
		}

		public static void get_IngredientsUnlocks_Postfix(JsonDish __instance, ref HashSet<Dish.IngredientUnlock> __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.IngredientUnlocksConverter(__instance.TempIngredientsUnlocks);
			}
		}

		public static void get_RequiredDishItem_Postfix(JsonDish __instance, ref Item __result)
		{
			if (__instance.GetType() == typeof(JsonDish))
			{
				__result = ContentPackPatches.GDOConverter<Item>(__instance.TempRequiredDishItem);
			}
		}
	}
}
