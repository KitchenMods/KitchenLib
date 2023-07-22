using KitchenData;
using KitchenLib.Customs;
using KitchenLib.JSON.JsonConverters;
using KitchenLib.JSON.Models.Containers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenLib.src.JSON.Models.Jsons
{
	internal class JsonAppliance : CustomAppliance
	{
		[field: JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }

		[JsonProperty("GDOName")]
		public string GDOName { get; set; }

		[JsonProperty("Materials")]
		public List<MaterialsContainer> Materials { get; set; } = new();

		[JsonIgnore]
		public override GameObject Prefab { get; protected set; }
		[JsonProperty("Prefab")]
		[JsonConverter(typeof(PrefabConverter))]
		public object TempPrefab { get; set; }

		[JsonIgnore]
		public override GameObject HeldAppliancePrefab { get; protected set; }
		[JsonProperty("HeldAppliancePrefab")]
		[JsonConverter(typeof(PrefabConverter))]
		public object TempHeldAppliancePrefab { get; set; }
	}
}
