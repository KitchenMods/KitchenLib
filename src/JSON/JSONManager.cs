using KitchenLib.Customs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
			{ JsonType.CBlockOutBackground, typeof(CBlockOutBackground) },
			{ JsonType.CBlueprintLight, typeof(CBlueprintLight) },
			{ JsonType.CCircularTimer, typeof(CCircularTimer) },
			{ JsonType.CFairyLight, typeof(CFairyLight) },
			{ JsonType.CFlat, typeof(CFlat) },
			{ JsonType.CFlatImage, typeof(CFlatImage) },
			{ JsonType.CFoliage, typeof(CFoliage) },
			{ JsonType.CGhost, typeof(CGhost) },
			{ JsonType.CIndicatorLight, typeof(CIndicatorLight) },
			{ JsonType.CLakeSurface, typeof(CLakeSurface) },
			{ JsonType.CMirror, typeof(CMirror) },
			{ JsonType.CMirrorBacking, typeof(CMirrorBacking) },
			{ JsonType.CMirrorSurface, typeof(CMirrorSurface) },
			{ JsonType.CNewspaper, typeof(CNewspaper) },
			{ JsonType.CPing, typeof(CPing) },
			{ JsonType.CPreviewFloor, typeof(CPreviewFloor) },
			{ JsonType.CSimpleFlat, typeof(CSimpleFlat) },
			{ JsonType.CSimpleFlatPlayer, typeof(CSimpleFlatPlayer) },
			{ JsonType.CSimpleTransparent, typeof(CSimpleTransparent) },
			{ JsonType.CWalls, typeof(CWalls) },
		};

		public static List<BaseJson> LoadedJsons = new List<BaseJson>();

		public static T LoadJsonFromString<T>(string json) where T : BaseJson
		{
			var newJson = JsonConvert.DeserializeObject<T>(json);
			return newJson;
		}

		public static Material LoadJsonMaterial<T>(string json) where T : CustomMaterial
		{
			var newJson = JsonConvert.DeserializeObject<T>(json);
			newJson.Deserialise();
			newJson.ConvertMaterial(out Material material);
			return material;
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

		public static void LoadAllJsons(AssetBundle bundle)
		{
			foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
			{
				Main.LogInfo($"Loading JSON-based material asset '{asset.name}'");
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
					Main.LogWarning(e.Message);
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
		CBlueprintLight,
		CMirror,
		CBlockOutBackground,
		CCircularTimer,
		CLakeSurface,
		CMirrorBacking,
		CMirrorSurface,
		CNewspaper,
		CPing,
		CPreviewFloor,
		CSimpleFlatPlayer
	}
}
