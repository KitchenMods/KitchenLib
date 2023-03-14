using KitchenData;
using KitchenLib.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KitchenLib.src.JSON.JsonConverters
{
    public class GameDataObjectConverter : DerivedClassConverter<GameDataObject>
    {
        public override object Create(string str)
        {
            return GDOUtils.GetCustomGameDataObject(StringUtils.GetInt32HashCode(str));
        }

        public override object Create(int id)
        {
            return GDOUtils.GetExistingGDO(id);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(((GameDataObject)value).ID);
            jToken.WriteTo(writer);
        }
    }
}
