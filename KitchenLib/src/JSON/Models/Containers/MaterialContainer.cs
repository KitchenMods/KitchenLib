using KitchenLib.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenLib.JSON.Models.Containers
{
	public struct MaterialsContainer
	{
		public JObject Tree;
		public JValue Base;

		public IEnumerable<KeyValuePair<string, IEnumerable<Material>>> GetLeafNodes(JToken token, string key = "")
		{
			if (token is JValue value)
			{
				yield return new KeyValuePair<string, IEnumerable<Material>>(key, new List<Material> { JSONPackUtils.GetMaterialByName(value.ToString()) });
			}
			else if (token is JObject obj)
			{
				foreach (var kvp in obj)
				{
					var newKey = string.IsNullOrEmpty(key) ? kvp.Key : $"{key}/{kvp.Key}";
					foreach (var result in GetLeafNodes(kvp.Value, newKey))
					{
						yield return result;
					}
				}
			}
			else if (token is JArray array)
			{
				var values = new List<string>();
				foreach (var item in array)
				{
					if (item is JValue arrayValue)
					{
						values.Add(arrayValue.ToString());
					}
				}
				yield return new KeyValuePair<string, IEnumerable<Material>>(key, values
					.Select(_ => JSONPackUtils.GetMaterialByName(_)));
			}
		}

		public void Convert(GameObject Prefab)
		{
			if (Base != null)
			{
				MaterialUtils.ApplyMaterial(Prefab, JSONPackUtils.GetMaterialByName(Base.ToString()));
			}
			else
			{
				foreach (KeyValuePair<string, IEnumerable<Material>> kvp in GetLeafNodes(Tree))
				{
					MaterialUtils.ApplyMaterial(
						Prefab,
						kvp.Key,
						kvp.Value.ToArray()
					);
				}
			}
		}
	}
}
