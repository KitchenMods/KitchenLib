using KitchenData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.JSON.JsonConverters
{
    public class GameDataObjectConverter : JsonConverter
    {
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsSubclassOf(typeof(GameDataObject)) || objectType == typeof(GameDataObject);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return reader.Value.ToString();
		}

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(((GameDataObject)value).ID);
            jToken.WriteTo(writer);
        }
    }
}
