using KitchenLib.Customs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
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
			{ JsonType.ImageMaterial, typeof(CustomFlatImage) }
		};

		public static List<BaseJson> LoadedJsons = new List<BaseJson>();

		public static void LoadAllJsons(AssetBundle bundle)
		{
			/*
			foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
			{
				Main.instance.Log("Loading " + asset.name);
				BaseJson baseJson = null;
				try
				{
					baseJson = JsonConvert.DeserializeObject<BaseJson>(asset.text);
				}
				catch
				{
					Main.instance.Log(asset.name + " Could Not Be Loaded");
				}

				if (baseJson != null)
				{
					var newJson = JsonConvert.DeserializeObject(asset.text, keyValuePairs[baseJson.Type]);
					LoadedJsons.Add(newJson as BaseJson);
				}
			}
			*/
			foreach (TextAsset asset in bundle.LoadAllAssets<TextAsset>())
			{
				Main.instance.Log("Loading " + asset.name);
				if (Path.GetExtension(asset.name) == ".json")
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
