using KitchenLib.JSON.ContractResolver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Semver;
using KitchenLib.JSON.JsonConverters;
using KitchenLib.Customs;
using KitchenLib.JSON.Models.Jsons;
using KitchenLib.JSON.Models;
using KitchenLib.JSON.Enums;

namespace KitchenLib.JSON
{
	internal class JSONPackSerializer
	{
		internal static Dictionary<GDOType, Type> keyValuePairs = new Dictionary<GDOType, Type>()
		{
			{GDOType.Item, typeof(JsonItem) },
			{GDOType.ItemGroup, typeof(JsonItemGroup) },
			{GDOType.Dish, typeof(JsonDish) }
		};

		internal static JsonSerializer Serializer = JsonSerializer.Create(Settings);

		internal static JsonSerializerSettings Settings = new JsonSerializerSettings()
		{
			ContractResolver = new JSONPackContractResolver(),
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			Converters = new JsonConverter[]
			{
				new StringEnumConverter(),
				new GameDataObjectConverter(),
				new IItemPropertyConverter()
			},
			Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
			{
				Main.Logger.LogException(args.ErrorContext.Error);
				args.ErrorContext.Handled = true;
			}
		};

		internal static void InitialiseSerializer(string author, string modname)
		{
			Settings.Context = new StreamingContext(StreamingContextStates.Other, new Tuple<string, string>(author, modname));
			Serializer = JsonSerializer.Create(Settings);
		}

		internal static CustomGameDataObject DeserializeJson(JObject jObject, Type type)
		{
			try
			{
				return (CustomGameDataObject)Serializer.Deserialize(new JTokenReader(jObject), type);
			}
			catch (Exception e)
			{
				Main.Logger.LogException(e);
				return null;
			}
		}

		internal static Manifest DeserialiseManifest(JObject jObject)
		{
			try
			{
				return Serializer.Deserialize<Manifest>(new JTokenReader(jObject));
			}
			catch (Exception e)
			{
				Main.Logger.LogException(e);
				return null;
			}
		}

		internal static string SerializeJson(object JsonGDO)
		{
			JTokenWriter writer = new JTokenWriter();
			Serializer.Serialize(writer, JsonGDO);
			string output = writer.Token.ToString();
			writer.Close();
			return output;
		}

		internal static bool ValidateManifest(JObject jObject)
		{
			// Add schema validation
			if (jObject == null)
				return false;

			if (!jObject.ContainsKey("Author") ||
				!jObject.ContainsKey("ModName"))
				return false;

			if (jObject.TryGetValue("Dependencies", out JToken jToken))
			{
				Dictionary<string, SemVersion> dependencies = null;
				try
				{
					dependencies = jToken.ToObject<Dictionary<string, SemVersion>>(JSONPackSerializer.Serializer);
				}
				catch (Exception e)
				{
					Main.Logger.LogException(e);
				}

				if (dependencies == null)
					return false;
				else
				{
					// validate dependencies
				}
			}
			return true;
		}

		internal static bool ValidateJSONGDO(JObject jObject)
		{
			// Add schema validation
			if (jObject == null)
				return false;

			if (!jObject.ContainsKey("Type") ||
				!jObject.ContainsKey("Modification") ||
				!jObject.ContainsKey("UniqueNameID"))
				return false;

			return true;
		}
	}
}
