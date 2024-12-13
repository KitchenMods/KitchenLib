using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CustomMaterials
	{
		public static Dictionary<string, Material> CustomMaterialsIndex = new Dictionary<string, Material>();

		public static Material AddMaterial(string name, Material material)
		{
			if (CustomMaterialsIndex.ContainsKey(name))
				return null;

			CustomMaterialsIndex.Add(name, material);
			return material;
		}

		public static Material GetCustomMaterial(string materialName)
		{
			CustomMaterialsIndex.TryGetValue(materialName, out Material material);
			return material;
		}

		public static List<Material> GetCustomMaterials()
		{
			return CustomMaterialsIndex.Values.ToList<Material>();
		}
		public static Material LoadMaterialFromJson(string json)
		{
			CustomBaseMaterial baseMaterial = null;
			try { baseMaterial = JsonConvert.DeserializeObject<CustomBaseMaterial>(json); }
			catch { return null; }
			Material material = null;
			if (baseMaterial.Type == JsonType.FlatColorMaterial)
			{
				JsonConvert.DeserializeObject<CustomSimpleFlat>(json).ConvertMaterial(out material);
			}
			else if (baseMaterial.Type == JsonType.TransparentMaterial)
			{
				JsonConvert.DeserializeObject<CustomSimpleTransparent>(json).ConvertMaterial(out material);
			}
			else if (baseMaterial.Type == JsonType.ImageMaterial)
			{
				JsonConvert.DeserializeObject<CustomFlatImage>(json).ConvertMaterial(out material);
			}
			return material;
		}
	}
}
