using Newtonsoft.Json;
using System;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public abstract class CustomConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string str = (string)reader.Value;
            return Create(str);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { }

        public abstract object Create(string str);
    }
}
