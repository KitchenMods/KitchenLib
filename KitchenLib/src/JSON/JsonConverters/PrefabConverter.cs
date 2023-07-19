using KitchenLib.JSON.Models.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.JSON.JsonConverters
{
	public class PrefabConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartObject)
			{
				return JObject.Load(reader).ToObject<PrefabContainer>();
			}
			else if (reader.TokenType == JsonToken.String)
			{
				return reader.Value;
			}
			else
			{
				throw new JsonReaderException($"Unexpected token type: {reader.TokenType}");
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{

		}
	}
}
