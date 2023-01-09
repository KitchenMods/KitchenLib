using KitchenLib.Customs;
using KitchenLib.src.ContentPack.Models.Json;
using Newtonsoft.Json;
using System.IO;
using static KitchenLib.src.ContentPack.ContentPackUtils;

namespace KitchenLib.src.ContentPack.Models
{
    public class ModChange
    {
        [JsonProperty("Type", Required = Required.Always)]
        public SerializationContext Type { get; set; }
        [JsonProperty("Location", Required = Required.Always)]
        public string Location { get; set; }

        public void Load()
        {
            CustomGameDataObject gdo = DeserializeGDO(Type);
            if (gdo != null)
            {
                CustomGDO.RegisterGameDataObject(gdo);
            }
        }

        public CustomGameDataObject DeserializeGDO(SerializationContext context)
        {
            string json = File.ReadAllText(Path.Combine(ContentPackManager.CurrentPack.ModDirectory, Path.Combine(Location)));
            return context switch
            {
                SerializationContext.Item => JsonConvert.DeserializeObject<JsonItem>(json, settings),
                SerializationContext.ItemGroup => JsonConvert.DeserializeObject<JsonItemGroup>(json, settings),
                _ => null
            };
        }
    }
}
