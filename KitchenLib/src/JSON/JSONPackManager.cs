using KitchenLib.Customs;
using KitchenLib.JSON.Enums;
using KitchenLib.JSON.Models;
using KitchenLib.JSON.Models.Jsons;
using KitchenLib.Utils;
using KitchenMods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static KitchenLib.JSON.JSONPackSerializer;

namespace KitchenLib.JSON
{
	internal class JSONPackManager
	{
		internal static Dictionary<string, List<JObject>> JSONCache = new Dictionary<string, List<JObject>>();
		internal static Dictionary<string, List<AssetBundle>> AssetBundleCache = new Dictionary<string, List<AssetBundle>>();
		internal static Dictionary<string, Manifest> ManifestCache = new Dictionary<string, Manifest>();

		internal static void Start()
		{
			Initialise();
			InjectGDOs();
		}

		private static void Initialise()
		{
			Main.Logger.LogInfo("Loading JSON packs...");
			FindMods(FolderModSource.ModsFolder);
			FindMods(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "..", "..", "workshop", "content", "1599600")));
			CacheAssetBundles();
		}

		private static void InjectGDOs()
		{
			Main.Logger.LogInfo("Registering JSON gdos...");
			foreach (Manifest manifest in ManifestCache.Values)
			{
				string modname = manifest.ModName;
				InitialiseSerializer(manifest.Author, modname);

				foreach (JObject jObject in JSONCache[modname])
				{
					if (Enum.TryParse(jObject["Type"].ToString(), out GDOType gdoType) &&
						Enum.TryParse(jObject["Modification"].ToString(), out Modification modType))
					{
						CustomGameDataObject gdo = DeserializeJson(jObject, keyValuePairs[gdoType]);
						switch (modType)
						{
							case Modification.Test:
								Main.Logger.LogInfo($"GDO serialized {modname}:{gdo.UniqueNameID} with ID {gdo.ID}");
								Main.Logger.LogInfo(SerializeJson(gdo));
								break;
							case Modification.NewGDO:
								CustomGDO.RegisterJSONGameDataObject(gdo);
								Main.Logger.LogInfo($"GDO registered {modname}:{gdo.UniqueNameID} with ID {gdo.ID}");
								break;
						}
					}
				}
			}
		}

		private static void RegisterJSON(string key, JObject jObject, bool cache = false)
		{
			if (JSONCache.ContainsKey(key))
			{
				JSONCache[key].Add(jObject);
			}
			else
			{
				JSONCache.Add(key, new List<JObject> { jObject });
			}
			Main.Logger.LogInfo($"JSON cached for {key}");
		}

		private static void RegisterAssetBundles(string key, IEnumerable<AssetBundle> bundles)
		{
			if (AssetBundleCache.ContainsKey(key))
			{
				foreach (AssetBundle bundle in bundles)
				{
					AssetBundleCache[key].Add(bundle);
				}
			}
			else
			{
				AssetBundleCache.Add(key, bundles.ToList());
			}
			Main.Logger.LogInfo($"Assetbundles cached for {key}");
		}

		private static void RegisterManifest(string key, Manifest manifest)
		{
			if (ManifestCache.ContainsKey(key))
			{
				Main.Logger.LogError($"Failed to load {key}. Duplicate Manifest");
				return;
			}
			ManifestCache.Add(key, manifest);
			Main.Logger.LogInfo($"Manifest cached for {key}");
		}

		private static void FindMods(string dir)
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

		private static void LoadModFromFolder(string dir)
		{
			string modname = Path.GetFileName(dir);

			string[] files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories);
			string manifestFile = files
				.FirstOrDefault(_ => StringCompare(Path.GetFileName(_), "manifest.json"));
			if (manifestFile == null)
				return;

			string rawManifest = File.ReadAllText(manifestFile);
			IEnumerable<string> rawJsons = files
				.Select(_ => File.ReadAllText(_));

			RegisterJSONPack(rawManifest, rawJsons);
		}

		private static void CacheAssetBundles()
		{
			foreach (Mod mod in ModPreload.Mods)
			{
				IEnumerable<AssetBundle> assetBundles = mod
					.GetPacks<AssetBundleModPack>()
					.SelectMany(_ => _.AssetBundles);

				if (assetBundles != null)
					RegisterAssetBundles(mod.Name, assetBundles);
			}
		}

		private static Manifest RegisterJSONPack(string rawManifest, IEnumerable<string> rawJsons)
		{
			JObject manifestObject = JObject.Parse(rawManifest);
			if (!ValidateManifest(manifestObject))
				return null;

			Manifest manifest = null;
			try
			{
				manifest = DeserialiseManifest(manifestObject);
				RegisterManifest(manifest.ModName, manifest);
			}
			catch (JsonReaderException e)
			{
				Main.Logger.LogException(e);
			}

			if (manifest == null)
				return null;

			string modname = manifest.ModName;

			foreach (string rawJson in rawJsons)
			{
				JObject jObject = null;
				try
				{
					jObject = JObject.Parse(rawJson);
				}
				catch (JsonReaderException e)
				{
					Main.Logger.LogException(e);
				}

				if (ValidateJSONGDO(jObject))
				{
					RegisterJSON(modname, jObject);
				}
			}
			return manifest;
		}

		public static bool StringCompare(string str1, string str2)
		{
			return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
		}
	}
}
