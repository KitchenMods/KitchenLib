using KitchenData;
using KitchenLib.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.JSON.JsonConverters
{
    public class InfoTupleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ValueTuple<Locale, UnlockInfo>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject.TryGetValue("Locale", out JToken jLocale) &&
                jObject.TryGetValue("Name", out JToken jName) &&
                jObject.TryGetValue("Description", out JToken jDescription) &&
                jObject.TryGetValue("FlavourText", out JToken jFlavourText))
            {
                return (jLocale.ToObject<Locale>(), 
                        LocalisationUtils.CreateUnlockInfo(jName.ToString(), 
                            jDescription.ToString(), 
                            jFlavourText.ToString()
                        )
                );
            }
			return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
			Tuple<Locale, UnlockInfo> info = (Tuple<Locale, UnlockInfo>)value;
            writer.WriteStartObject();
            writer.WritePropertyName("Locale");
            writer.WriteValue(info.Item1);
            writer.WritePropertyName("Name");
            writer.WriteValue(info.Item2.Name);
            writer.WritePropertyName("Description");
            writer.WriteValue(info.Item2.Description);
            writer.WritePropertyName("FlavourText");
            writer.WriteValue(info.Item2.FlavourText);
            writer.WriteEnd();
            writer.WriteEndObject();
        }
    }
}