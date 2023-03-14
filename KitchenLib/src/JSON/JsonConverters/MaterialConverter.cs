using KitchenLib.Utils;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System;
using KitchenLib.src.JSON.Models.Containers;

namespace KitchenLib.src.JSON.JsonConverters
{
    public class MaterialConverter : CustomConverter<Material>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject.TryGetValue("Type", out JToken type))
            {
                MaterialType MaterialType = type.ToObject<MaterialType>();
                return MaterialType switch
                {
                    MaterialType.Existing => MaterialUtils.GetExistingMaterial(jObject["Name"].ToString()),
                    MaterialType.Custom => MaterialUtils.GetCustomMaterial(jObject["Name"].ToString()),
                    _ => null
                };
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(((Material)value).name);
            jToken.WriteTo(writer);
        }
    }
}
