using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
	public struct MaterialsContainer
	{
		public string Path;
		public List<string> Materials;

		public Material[] Convert()
		{
			return Materials
				.Select(_ => JSONPackUtils.GetMaterialByName(_))
				.ToArray();
		}
	}
}
