using KitchenData;
using KitchenLib.Customs;
using KitchenLib.JSON.Interfaces;
using KitchenLib.JSON.JsonConverters;
using KitchenLib.JSON.Models.Containers;
using KitchenLib.JSON.Models.Views;
using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.JSON.Models.Jsons
{
	internal class JsonItemGroup : CustomItemGroup<JsonItemGroupView>, IHasSidePrefab
	{
		[field: JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }

		[JsonProperty("GDOName")]
		public string GDOName { get; set; }

		[JsonProperty("Materials")]
		public MaterialsContainer Materials { get; set; }

		[JsonProperty("View")]
		[JsonConverter(typeof(ViewConverter))]
		public ItemGroupViewContainer View { get; set; }

		[JsonIgnore]
		public override List<ItemGroup.ItemSet> Sets { get; protected set; } = new();
		[JsonProperty("Sets")]
		public List<ItemSetContainer> TempSets { get; set; } = new();

		[JsonIgnore]
		public override GameObject Prefab { get; protected set; }
		[JsonProperty("Prefab")]
		[JsonConverter(typeof(PrefabConverter))]
		public object TempPrefab { get; set; }

		[JsonIgnore]
		public override GameObject SidePrefab { get; protected set; }
		[JsonProperty("SidePrefab")]
		[JsonConverter(typeof(PrefabConverter))]
		public object TempSidePrefab { get; set; }

		[JsonIgnore]
		public override List<Item.ItemProcess> Processes { get; protected set; } = new();
		[JsonProperty("Processes")]
		public List<ItemProcessContainer> TempProcesses { get; set; } = new();

		[JsonIgnore]
		public override Item.ItemProcess AutomaticItemProcess { get; protected set; } = new();
		[JsonProperty("AutomaticItemProcess")]
		public ItemProcessContainer TempAutomaticItemProcess { get; set; } = new();

		[JsonIgnore]
		public override Item DirtiesTo { get; protected set; }
		[JsonProperty("DirtiesTo")]
		public string TempDirtiesTo { get; set; }

		[JsonIgnore]
		public override List<Item> MayRequestExtraItems { get; protected set; } = new();
		[JsonProperty("MayRequestExtraItems")]
		public List<string> TempMayRequestExtraItems { get; set; } = new();

		[JsonIgnore]
		public override Item SplitSubItem { get; protected set; }
		[JsonProperty("SplitSubItem")]
		public string TempSplitSubItem { get; set; }

		[JsonIgnore]
		public override List<Item> SplitDepletedItems { get; protected set; } = new();
		[JsonProperty("SplitDepletedItems")]
		public List<string> TempSplitDepletedItems { get; set; } = new();

		[JsonIgnore]
		public override Item RefuseSplitWith { get; protected set; }
		[JsonProperty("RefuseSplitWith")]
		public string TempRefuseSplitWith { get; set; }

		[JsonIgnore]
		public override Item DisposesTo { get; protected set; }
		[JsonProperty("DisposesTo")]
		public string TempDisposesTo { get; set; }

		[JsonIgnore]
		public override Appliance DedicatedProvider { get; protected set; }
		[JsonProperty("DedicatedProvider")]
		public string TempDedicatedProvider { get; set; }

		[JsonIgnore]
		public override Item ExtendedDirtItem { get; protected set; }
		[JsonProperty("ExtendedDirtItem")]
		public string TempExtendedDirtItem { get; set; }

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			Tuple<string, string> Context = (Tuple<string, string>)context.Context;
			ModName = Context.Item2;
			ModID = Context.Item2;
		}

		public override void OnRegister(ItemGroup gameDataObject)
		{
			gameDataObject.name = GDOName;

			Materials.Convert(gameDataObject.Prefab);

			gameDataObject.Prefab.GetComponent<JsonItemGroupView>().Setup(gameDataObject.Prefab, View);
		}
	}
}
