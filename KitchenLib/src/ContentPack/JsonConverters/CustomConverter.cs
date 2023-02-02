using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public abstract class CustomConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value switch
            {
                int id => Create(id),
                string str => Create(str),
                _ => null
            }; ;
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
