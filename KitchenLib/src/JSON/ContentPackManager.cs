using HarmonyLib;
using KitchenLib.Customs;
using KitchenLib.JSON.Models.Jsons;
using KitchenLib.src.JSON;
using KitchenMods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.JSON
{
	public class ContentPackManager
	{
		public static Dictionary<string, List<JObject>> JSONTable = new Dictionary<string, List<JObject>>();
		public static Dictionary<string, List<AssetBundle>> AssetBundleTable = new Dictionary<string, List<AssetBundle>>();
		public static List<CustomGameDataObject> NotInitialized = new List<CustomGameDataObject>();

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
												case GDOType.ItemGroup:
													CustomGDO.RegisterGameDataObject((JsonItemGroup)gdo);
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
			var harmony = Main.harmonyInstance;

			//Item
			harmony.Patch(typeof(JsonItem).GetMethod("get_Prefab"), null, new HarmonyMethod(typeof(JsonItem).GetMethod("get_Prefab_Postfix")));

			//ItemGroup
			harmony.Patch(typeof(JsonItemGroup).GetMethod("get_Prefab"), null, new HarmonyMethod(typeof(JsonItemGroup).GetMethod("get_Prefab_Postfix")));
			harmony.Patch(typeof(JsonItemGroup).GetMethod("get_Sets"), null, new HarmonyMethod(typeof(JsonItemGroup).GetMethod("get_Sets_Postfix")));
			
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
