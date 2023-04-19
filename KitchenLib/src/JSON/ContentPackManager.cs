using KitchenLib.Customs;
using KitchenLib.JSON.Models.Jsons;
using KitchenMods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.JSON
{
	public class ContentPackManager
	{
		public static Dictionary<string, List<JObject>> JSONTable = new Dictionary<string, List<JObject>>();
		public static Dictionary<string, List<AssetBundle>> AssetBundleTable = new Dictionary<string, List<AssetBundle>>();

		public static void Initialise()
		{
			Main.LogInfo("Loading packs...");
			FindMods(FolderModSource.ModsFolder);
			FindMods(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..", "..", "workshop", "content", "1599600")));
		}

		public static void InjectGDOs()
		{
			foreach (string modname in JSONTable.Keys)
			{
				foreach (JObject jObject in JSONTable[modname])
				{
					if(jObject.TryGetValue("Type", out JToken jToken1))
						if(Enum.TryParse(jToken1.ToString(), true, out GDOType GDOType))
							if(jObject.TryGetValue("ModificationType", out JToken jToken2))
								if(Enum.TryParse(jToken2.ToString(), true, out ModificationType ModType))
								{
									InitialiseSerializer(modname);

									switch (ModType)
									{
										case ModificationType.NewGDO:
											CustomGameDataObject gdo = ContentPackUtils.DeserializeJson(jObject, ContentPackUtils.keyValuePairs[GDOType]);
											Main.LogInfo(ContentPackUtils.SerializeJson(gdo));

											switch (GDOType)
											{
												case GDOType.Item:
													CustomGDO.RegisterGameDataObject((JsonItem)gdo);
													break;
												case GDOType.ItemGroup:
													CustomGDO.RegisterGameDataObject((JsonItemGroup)gdo);
													break;
												case GDOType.Dish:
													CustomGDO.RegisterGameDataObject((JsonDish)gdo);
													break;
												case GDOType.Appliance:
													CustomGDO.RegisterGameDataObject((JsonAppliance)gdo);
													break;
											}
											break;
									}
								}
				}
			}
		}

		public static void ApplyPatches()
		{
			//JsonItem
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_Prefab");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_SidePrefab");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_Processes");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_DirtiesTo");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_MayRequestExtraItems");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_SplitSubItem");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_SplitDepletedItems");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_SplitByComponentsHolder");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_RefuseSplitWith");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_DisposesTo");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_DedicatedProvider");
			ContentPackPatches.PostfixPatch(typeof(JsonItem), "get_ExtendedDirtItem");

			//JsonItemGroup
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_Prefab");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_SidePrefab");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_Processes");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_DirtiesTo");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_MayRequestExtraItems");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_SplitSubItem");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_SplitDepletedItems");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_SplitByComponentsHolder");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_RefuseSplitWith");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_DisposesTo");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_DedicatedProvider");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_ExtendedDirtItem");
			ContentPackPatches.PostfixPatch(typeof(JsonItemGroup), "get_Sets");

			//JsonDish
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_ExtraOrderUnlocks");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_MinimumIngredients");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_RequiredProcesses");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_BlockProviders");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_IconPrefab");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_DisplayPrefab");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_ResultingMenuItems");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_IngredientsUnlocks");
			ContentPackPatches.PostfixPatch(typeof(JsonDish), "get_RequiredDishItem");

			//JsonAppliance
			ContentPackPatches.PostfixPatch(typeof(JsonAppliance), "get_Prefab");
			ContentPackPatches.PostfixPatch(typeof(JsonAppliance), "get_HeldAppliancePrefab");
			ContentPackPatches.PostfixPatch(typeof(JsonAppliance), "get_Processes");
			ContentPackPatches.PostfixPatch(typeof(JsonAppliance), "get_RequiresForShop");
			ContentPackPatches.PostfixPatch(typeof(JsonAppliance), "get_RequiresProcessForShop");
			ContentPackPatches.PostfixPatch(typeof(JsonAppliance), "get_Upgrades");
			ContentPackPatches.PostfixPatch(typeof(JsonAppliance), "get_CrateItem");
		}

		public static void InitialiseSerializer(string modname)
		{
			ContentPackUtils.settings.Context = new StreamingContext(StreamingContextStates.Other, modname);
			ContentPackUtils.serializer = JsonSerializer.Create(ContentPackUtils.settings);
		}

		public static void RegisterJSONGDO(JObject jObject)
		{
			string key = Main.instance.ModName;
			if(JSONTable.ContainsKey(key))
				JSONTable[key].Add(jObject);
			else
				JSONTable.Add(key, new List<JObject>() { jObject });

			Main.LogInfo($"JSON registered {key}: {jObject}");
		}

		public static void RegisterAssetBundles(List<AssetBundle> bundle)
		{
			string key = Main.instance.ModName;
			AssetBundleTable[key] = bundle;
			Main.LogInfo($"Assetbundle Registered for {key}");
		}

		public static void FindMods(string dir)
		{
			BaseMod.instance.Log("Searching for mods in " + dir);
			if (Directory.Exists(dir))
			{
				foreach (string subdirectory in Directory.GetDirectories(dir))
				{
					LoadModFromFolder(subdirectory);
				}
			}
		}

		public static void LoadModFromFolder(string dir)
		{
			List<string> files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories).ToList();
			foreach (string file in files)
			{
				JObject jObject = JObject.Parse(File.ReadAllText(file));
				RegisterJSONGDO(jObject);
			}
		}
	}
}
