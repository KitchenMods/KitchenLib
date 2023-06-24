using KitchenLib.Customs;
using KitchenLib.JSON.Models;
using KitchenLib.JSON.Models.Containers;
using KitchenLib.JSON.Models.Jsons;
using KitchenLib.Utils;
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
		public static Dictionary<string, JObject> ManifestTable = new Dictionary<string, JObject>();

		public static void Start()
		{
			Initialise();
			ApplyPatches();
			InjectGDOs();
		}

		public static void Initialise()
		{
			Main.LogInfo("Loading packs...");
			FindMods(FolderModSource.ModsFolder);
			FindMods(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..", "..", "workshop", "content", "1599600")));

			Main.LogInfo("Registering AssetBundles...");
			foreach (Mod mod in ModPreload.Mods)
				foreach (AssetBundleModPack pack in mod.GetPacks<AssetBundleModPack>())
				{
					RegisterAssetBundles(mod.Name, pack.AssetBundles);
					foreach (AssetBundle bundle in pack.AssetBundles)
					{
						foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
						{
							try
							{
								JObject jObject = JObject.Parse(asset.text);
								if (jObject.TryGetValue("Type", out JToken jToken))
								{
									if(!JSONManager.keyValuePairs.ContainsKey(jToken.ToObject<JsonType>()))
									{
										RegisterJSONGDO(mod.Name, jObject);
									}
								}
								else
								{
									if(asset.name == "Manifest.json")
									{
										RegisterManifest(mod.Name, jObject);
									}
								}
							}
							catch (Exception e)
							{
								Main.LogWarning(asset.name + " Could Not Be Loaded");
								Main.LogWarning(e.Message);
							}
						}
					}
				}
		}

		public static void InjectGDOs()
		{
			foreach (string modname in JSONTable.Keys)
			{
				Manifest manifest = ManifestTable[modname].ToObject<Manifest>();
				InitialiseSerializer(manifest.Author, manifest.ModName);

				foreach (JObject jObject in JSONTable[modname])
				{
					if(jObject.TryGetValue("Type", out JToken jToken1))
						if(Enum.TryParse(jToken1.ToString(), true, out GDOType GDOType))
							if(jObject.TryGetValue("ModificationType", out JToken jToken2))
								if(Enum.TryParse(jToken2.ToString(), true, out ModificationType ModType))
								{
									switch (ModType)
									{
										case ModificationType.NewGDO:
											CustomGameDataObject gdo = ContentPackUtils.DeserializeJson(jObject, ContentPackUtils.keyValuePairs[GDOType]);
											switch (GDOType)
											{
												case GDOType.Item:
													CustomGDO.RegisterJsonGameDataObject((JsonItem)gdo);
													break;
												case GDOType.ItemGroup:
													CustomGDO.RegisterJsonGameDataObject((JsonItemGroup)gdo);
													break;
												case GDOType.Dish:
													CustomGDO.RegisterJsonGameDataObject((JsonDish)gdo);
													break;
											}
											Main.LogInfo($"GDO registered {modname}:{gdo.UniqueNameID} with ID {gdo.ID}");
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

		public static void InitialiseSerializer(string author, string modname)
		{
			ContentPackUtils.settings.Context = new StreamingContext(StreamingContextStates.Other, new Tuple<string, string>(author, modname));
			ContentPackUtils.serializer = JsonSerializer.Create(ContentPackUtils.settings);
		}

		public static IEnumerable<Material> ConvertMaterialContainer(IEnumerable<MaterialContainer> container)
		{
			foreach (MaterialContainer material in container)
			{
				switch (material.Type)
				{
					case MaterialType.Existing:
						yield return MaterialUtils.GetExistingMaterial(material.Name);
						break;
					case MaterialType.Custom:
						yield return MaterialUtils.GetCustomMaterial(material.Name);
						break;
				}
			}
		}

		public static void RegisterJSONGDO(string key, JObject jObject)
		{
			if(JSONTable.ContainsKey(key))
				JSONTable[key].Add(jObject);
			else
				JSONTable.Add(key, new List<JObject>() { jObject });

			Main.LogInfo($"JSON cached {key}: {jObject["UniqueNameID"]}");
		}

		public static void RegisterAssetBundles(string key, List<AssetBundle> bundle)
		{
			AssetBundleTable[key] = bundle;
			Main.LogInfo($"Assetbundle cached for {key}");
		}

		public static void RegisterManifest(string key, JObject jObject)
		{
			if (ManifestTable.ContainsKey(key))
			{
				// Log Error duplicate manifest
				return;
			}
			ManifestTable.Add(key, jObject);
			Main.LogInfo($"Manifest cached for {key}");
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
			string modname = Path.GetFileName(dir);
			List<string> files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories).ToList();
			foreach (string file in files)
			{
				JObject jObject = JObject.Parse(File.ReadAllText(file));
				string filename = getFileName(file);
				if (filename == "Manifest.json")
					RegisterManifest(modname, jObject);
				else
					RegisterJSONGDO(modname, jObject);
			}
		}

		public static string getFileName(string path)
		{
			return Path.GetFileName(path);
		}
	}
}
