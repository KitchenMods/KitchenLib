using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semver;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class SemVersionConverter : CustomConverter<SemVersion>
    {
        public override object Create(string str)
        {
            return SemVersion.Parse(str, SemVersionStyles.Any);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(((SemVersion)value).ToVersion().ToString());
            jToken.WriteTo(writer);
        }
    }
}
