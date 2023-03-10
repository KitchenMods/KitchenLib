using KitchenLib.Customs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KitchenLib
{
	public static class JSONManager
	{
		public static Dictionary<JsonType, Type> keyValuePairs = new Dictionary<JsonType, Type>
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

		public static List<BaseJson> LoadedJsons = new List<BaseJson>();

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
			Main.instance.Log("Unable to load JSON");
			return new Material(Shader.Find("Simple Flat"));

		}

		public static void LoadAllJsons(AssetBundle bundle)
		{
			foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
			{
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
						Main.instance.Log(asset.name + " Could Not Be Loaded");
						Main.instance.Log(e.Message);
					}
				}
				catch (Exception e)
				{
					Main.instance.Log(asset.name + " Could Not Be Loaded. Is it a JSON?");
				}
			}

			foreach (CustomMaterial material in LoadedJsons)
			{
				material.Deserialise();
				material.ConvertMaterial(out Material newMaterial);
				CustomMaterials.AddMaterial(material.Name, newMaterial);
			}
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
