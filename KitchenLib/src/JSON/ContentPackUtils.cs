using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using KitchenLib.JSON.Models.Jsons;
using Newtonsoft.Json.Linq;
using KitchenLib.Customs;
using KitchenLib.JSON.ContractResolver;
using KitchenLib.JSON.JsonConverters;
using System.IO;

namespace KitchenLib.JSON
{
	internal class ContentPackUtils
	{
		public static Dictionary<GDOType, Type> keyValuePairs = new Dictionary<GDOType, Type>()
		{
			{GDOType.Item, typeof(JsonItem) },
			{GDOType.ItemGroup, typeof(JsonItemGroup) },
			{GDOType.Dish, typeof(JsonDish) },
			{GDOType.Appliance, typeof(JsonAppliance) }
		};

		public static JsonSerializerSettings settings = new JsonSerializerSettings()
		{
			ContractResolver = new CustomContractResolver(),
			Converters = new JsonConverter[]
			{
				new StringEnumConverter(),
				new GameDataObjectConverter(),
				new ItemPropertyConverter(),
				new AppliancePropertyConverter()
			}
		};

		public static JsonSerializer serializer;

		public static CustomGameDataObject DeserializeJson(JObject jObject, Type type)
		{
			try
			{
				return (CustomGameDataObject)serializer.Deserialize(new JTokenReader(jObject), type);
			}
			catch (Exception e)
			{
				Main.LogError(e.Message);
				Main.LogError(e.StackTrace);
				return null;
			}
		}

		public static CustomGameDataObject DeserializeJson(string json, Type type)
		{
			return (CustomGameDataObject)serializer.Deserialize(new JsonTextReader(new StringReader(json)), type);
		}

		public static string SerializeJson(CustomGameDataObject Object)
		{
			JTokenWriter writer = new JTokenWriter();
			serializer.Serialize(writer, Object);
			string output = writer.Token.ToString();
			writer.Close();
			return output;
		}
	}

	public enum GDOType
	{
		Item,
		ItemGroup,
		Dish,
		Appliance
	}

	public enum ModificationType
	{
		Test,
		NewGDO,
		UpdateGDO
	}
}
