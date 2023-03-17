using KitchenData;
using KitchenLib.Customs;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using static KitchenLib.src.JSON.ContentPackUtils;

namespace KitchenLib.src.JSON.Models.Jsons
{
    public class JsonDish : CustomDish
    {
		[JsonProperty("UniqueNameID")]
		public override string UniqueNameID { get; internal set; } = "";
		[JsonProperty("GDOName")]
        string GDOName { get; set; } = "";
        [JsonProperty("IconPrefab")]
        string IconPrefabStr { get; set; } = "";
        [JsonProperty("DisplayPrefab")]
        string DisplayPrefabStr { get; set; } = "";

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            IconPrefab = PrefabConverter(IconPrefabStr);
            DisplayPrefab = PrefabConverter(DisplayPrefabStr);
        }

        public override void OnRegister(Dish gameDataObject)
        {
            gameDataObject.name = GDOName;
        }
    }
}
