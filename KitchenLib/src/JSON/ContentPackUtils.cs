using KitchenLib.src.JSON.ContractResolver;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using KitchenLib.JSON.Models.Jsons;
using Newtonsoft.Json.Linq;
using KitchenLib.Customs;
using KitchenLib.src.JSON.JsonConverters;
using KitchenData;
using Unity.Entities.UniversalDelegates;
using KitchenLib.Utils;

namespace KitchenLib.JSON
{
	internal class ContentPackUtils
	{
		public static Dictionary<GDOType, Type> keyValuePairs = new Dictionary<GDOType, Type>()
		{
			{GDOType.Item, typeof(JsonItem) },
			{GDOType.ItemGroup, typeof(JsonItemGroup) }
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

		public static T Find<T>(string str) where T : GameDataObject
		{
			if (int.TryParse(str, out int id))
				return (T)GDOUtils.GetExistingGDO(id);
			else
				return (T)GDOUtils.GetCustomGameDataObject(StringUtils.GetInt32HashCode(str)).GameDataObject;
		}

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
		ItemGroup
	}

	public enum ModificationType
	{
		Test,
		NewGDO,
		UpdateGDO
	}
}
