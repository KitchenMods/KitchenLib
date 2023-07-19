using KitchenData;
using KitchenLib.JSON.Models.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.JSON.JsonConverters
{
	public class IItemPropertyConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(IItemProperty);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			return new IItemPropertyContainer()
			{
				jObject = jObject
			};
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken jToken = JToken.FromObject(value);
			jToken.WriteTo(writer);
		}
	}
}
