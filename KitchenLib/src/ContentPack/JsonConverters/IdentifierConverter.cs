using Newtonsoft.Json;
using System;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public abstract class IdentifierConverter<T> : CustomConverter<T>
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(T)) || objectType == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string str = reader.Value.ToString();
            if (int.TryParse(str, out int id))
                return Create(id);
            return Create(str);
        }
    }
}
