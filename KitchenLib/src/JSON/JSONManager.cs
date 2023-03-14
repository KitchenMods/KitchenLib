using KitchenLib.Customs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using KitchenLib.src.JSON;

namespace KitchenLib
{
    public static class JSONManager
	{
		public static Dictionary<JsonType, Type> keyValuePairs = new Dictionary<JsonType, Type>
		{
			{ JsonType.Base, typeof(BaseJson) },
			{ JsonType.FlatColorMaterial, typeof(CustomSimpleFlat) },
			{ JsonType.TransparentMaterial, typeof(CustomSimpleTransparent) },
			{ JsonType.ImageMaterial, typeof(CustomFlatImage) }
		};

		public static List<BaseJson> LoadedJsons = new List<BaseJson>();

		public static void LoadAllJsons(AssetBundle bundle)
		{
			foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
			{
				Main.instance.Log("Loading " + asset.name);
				if(Path.GetExtension(asset.name) == ".json")
				{
					try
					{
                        JObject jObject = JObject.Parse(asset.text);
                        if (jObject.TryGetValue("JSONType", out JToken jsonType))
                        {
							if (jsonType is JValue jValue)
							{
								var json = jObject.ToObject(keyValuePairs[jValue.ToObject<JsonType>()]);
                                LoadedJsons.Add(json as BaseJson);
                            }
							else if (jsonType is JObject Container)
							{
								ContentPackSystem.RawJObjects.Add(Main.instance.ModName, jObject);
							}
                        }
                    }
					catch(Exception e)
					{
                        Main.instance.Log(asset.name + " Could Not Be Loaded");
						Main.instance.Log(e.Message);
                    }
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
		ImageMaterial
	}
}
