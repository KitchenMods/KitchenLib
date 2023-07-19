using KitchenLib.Customs;
using KitchenLib.JSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib
{
    internal static class JSONManager
	{
		internal static Dictionary<JsonType, Type> keyValuePairs = new Dictionary<JsonType, Type>
		{
			{ JsonType.Base, typeof(BaseJson) },
			{ JsonType.FlatColorMaterial, typeof(CustomSimpleFlat) },
			{ JsonType.TransparentMaterial, typeof(CustomSimpleTransparent) },
			{ JsonType.ImageMaterial, typeof(CustomFlatImage) },
			{ JsonType.CustomMaterial, typeof(CustomMaterial) },
			{ JsonType.CSimpleFlat, typeof(CSimpleFlat) },
			{ JsonType.CSimpleTransparent, typeof(CSimpleTransparent) },
			{ JsonType.CFlatImage, typeof(CFlatImage) },
			{ JsonType.CFlat, typeof(CFlat) },
			{ JsonType.CIndicatorLight, typeof(CIndicatorLight) },
			{ JsonType.CGhost, typeof(CGhost) },
			{ JsonType.CFairyLight, typeof(CFairyLight) },
			{ JsonType.CFoliage, typeof(CFoliage) },
			{ JsonType.CWalls, typeof(CWalls) },
			{ JsonType.CBlueprintLight, typeof(CBlueprintLight) },
		};

		internal static List<BaseJson> LoadedJsons = new List<BaseJson>();

		internal static T LoadJsonFromString<T>(string json) where T : BaseJson
		{
			var newJson = JsonConvert.DeserializeObject<T>(json);
			return newJson;
		}

		internal static Material LoadJsonMaterial<T>(string json) where T : CustomMaterial
		{
			var newJson = JsonConvert.DeserializeObject<T>(json);
			newJson.Deserialise();
			newJson.ConvertMaterial(out Material material);
			return material;
		}

		internal static void LoadAllJsons(string MOD_NAME, AssetBundle bundle)
		{
			if(Main.Logger != null)
			{
				Main.Logger.LogInfo($"Loading JSONs from {MOD_NAME}");
				foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
				{
					Main.Logger.LogInfo($"Loading JSON-based material asset '{asset.name}'");
					try
					{
						JObject jObject = JObject.Parse(asset.text);
						if (jObject.TryGetValue("Type", out JToken jToken1))
						{
							JsonType type = jToken1.ToObject<JsonType>();
							var json = jObject.ToObject(keyValuePairs[type]);
							LoadedJsons.Add(json as BaseJson);
						}
					}
					catch (Exception e)
					{
						Main.Logger.LogException(e);
					}
				}
			} else
			{
				Main.LogInfo($"Please recompile {bundle.name} with latest KitchenLib for more verbose logging.");
				foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
				{
					Main.LogInfo($"Loading JSON-based material asset '{asset.name}'");
					BaseJson baseJson = null;
					try
					{
						try
						{
							JObject jObject = JObject.Parse(asset.text);
							if (jObject.TryGetValue("Type", out JToken jToken))
							{
								JsonType type = jToken.ToObject<JsonType>();
								var json = jObject.ToObject(keyValuePairs[type]);
								LoadedJsons.Add(json as BaseJson);
							}
						}
						catch (Exception e)
						{
							Main.LogWarning(asset.name + " Could Not Be Loaded");
							Main.LogWarning(e.Message);
						}
					}
					catch (Exception e)
					{
						Main.LogWarning($"Material asset '{asset.name}' could not be loaded");
					}
				}

				foreach (BaseJson json in LoadedJsons)
				{
					if (json is CustomMaterial)
					{
						var material = json as CustomMaterial;
						material.Deserialise();
						material.ConvertMaterial(out Material newMaterial);
						CustomMaterials.AddMaterial(material.Name, newMaterial);
					}
					if (json is CustomBaseMaterial)
					{
						var material = json as CustomBaseMaterial;
						Material mat;
						material.ConvertMaterial(out mat);
						CustomMaterials.AddMaterial(mat.name, mat);
					}
				}
			}
		}

		[Obsolete]
		public static Material LoadMaterialFromJson(string json)
		{
			BaseJson baseJson = null;
			try
			{
				baseJson = JsonConvert.DeserializeObject<BaseJson>(json);
			}
			catch { }

			if (baseJson != null)
			{
				var newJson = JsonConvert.DeserializeObject(json, keyValuePairs[baseJson.Type]);
				CustomMaterial customMaterial = newJson as CustomMaterial;
				Material material;
				customMaterial.Deserialise();
				customMaterial.ConvertMaterial(out material);
				return material;
			}
			Main.LogWarning("Unable to load JSON");
			return new Material(Shader.Find("Simple Flat"));
		}
	}

	public class BaseJson
	{
		public virtual JsonType Type { get; set; }
	}

	public enum JsonType
	{
		Base,
		FlatColorMaterial,
		TransparentMaterial,
		ImageMaterial,
		CustomMaterial,
		CSimpleFlat,
		CSimpleTransparent,
		CFlatImage,
		CFlat,
		CIndicatorLight,
		CGhost,
		CFairyLight,
		CFoliage,
		CWalls,
		CBlueprintLight
	}
}
