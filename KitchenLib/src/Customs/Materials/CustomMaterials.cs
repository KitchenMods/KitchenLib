using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
