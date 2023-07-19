using KitchenData;
using KitchenLib.Customs;
using KitchenLib.JSON.Interfaces;
using KitchenLib.JSON.JsonConverters;
using KitchenLib.JSON.Models.Containers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace KitchenLib.JSON.Models.Jsons
{
	internal class JsonDish : CustomDish, IHasDisplayPrefab, IHasIconPrefab
	{
		[field: JsonProperty("UniqueNameID", Required = Required.Always)]
		[JsonIgnore]
		public override string UniqueNameID { get; }

		[JsonProperty("GDOName")]
		public string GDOName { get; set; }

		[JsonIgnore]
		public override HashSet<Dish.IngredientUnlock> ExtraOrderUnlocks { get; protected set; } = new();
		[JsonProperty("ExtraOrderUnlocks")]
		public List<IngredientUnlockContainer> TempExtraOrderUnlocks { get; set; } = new();

		[JsonIgnore]
		public override HashSet<Item> MinimumIngredients { get; protected set; } = new();
		[JsonProperty("MinimumIngredients")]
		public List<string> TempMinimumIngredients { get; set; } = new();

		[JsonIgnore]
		public override HashSet<Process> RequiredProcesses { get; protected set; } = new();
		[JsonProperty("RequiredProcesses")]
		public List<string> TempRequiredProcesses { get; set; } = new List<string>();

		[JsonIgnore]
		public override HashSet<Item> BlockProviders { get; protected set; } = new();
		[JsonProperty("BlockProviders")]
		public List<string> TempBlockProviders { get; set; } = new();

		[JsonIgnore]
		public override GameObject IconPrefab { get; protected set; }
		[JsonProperty("IconPrefab")]
		[JsonConverter(typeof(PrefabConverter))]
		public object TempIconPrefab { get; set; }

		[JsonIgnore]
		public override GameObject DisplayPrefab { get; protected set; }
		[JsonProperty("DisplayPrefab")]
		[JsonConverter(typeof(PrefabConverter))]
		public object TempDIsplayPrefab { get; set; }

		[JsonIgnore]
		public override List<Dish.MenuItem> ResultingMenuItems { get; protected set; } = new();
		[JsonProperty("ResultingMenuItem")]
		public List<MenuItemContainer> TempResultingMenuItems { get; set; } = new();

		[JsonIgnore]
		public override HashSet<Dish.IngredientUnlock> IngredientsUnlocks { get; protected set; } = new();
		[JsonProperty("IngredientsUnlocks")]
		public List<IngredientUnlockContainer> TempIngredientsUnlocks = new();

		[JsonIgnore]
		public override Item RequiredDishItem { get; protected set; }
		[JsonProperty("RequiredDishItem")]
		public string TempRequiredDishItem { get; set; }

		[JsonIgnore]
		public override List<Unlock> AllowedFoods { get; protected set; } = new();
		[JsonProperty("AllowedFoods")]
		public List<string> TempAllowedFoods { get; set; } = new();

		[JsonIgnore]
		public override RestaurantSetting ForceFranchiseSetting { get; protected set; }
		[JsonProperty("ForceFranchiseSetting")]
		public string TempForceFranchiseSetting { get; set; }

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			Tuple<string, string> Context = (Tuple<string, string>)context.Context;
			ModName = Context.Item2;
			ModID = Context.Item2;
		}

		public override void OnRegister(Dish gameDataObject)
		{
			gameDataObject.name = GDOName;
		}
	}
}
