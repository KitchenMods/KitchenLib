using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.src.JSON.JsonConverters
{
    public abstract class CustomConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string str = reader.Value.ToString();
            if (int.TryParse(str, out int id))
                return Create(id);
            return Create(str);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject("Not Supported");
            jToken.WriteTo(writer);
        }

        public virtual object Create(string str)
        {
            return null;
        }
        public virtual object Create(int id)
        {
            return null;
        }
    }
}
