using KitchenLib.Customs;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
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
		};

		public static List<BaseJson> LoadedJsons = new List<BaseJson>();

		public static void LoadAllJsons(AssetBundle bundle)
		{
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
		CWalls
	}
}
