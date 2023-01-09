using KitchenData;
using KitchenLib.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class GameDataObjectConverter : CustomConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsSubclassOf(typeof(GameDataObject)) || objectType == typeof(GameDataObject);
        }

        public override object Create(string str)
        {
            if (int.TryParse(str, out int ID))
            {
                return GDOUtils.GetExistingGDO(ID);
            }
            return GDOUtils.GetCustomGameDataObject(StringUtils.GetInt32HashCode(str));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(((GameDataObject)value).ID);
            jToken.WriteTo(writer);
        }
    }
}
