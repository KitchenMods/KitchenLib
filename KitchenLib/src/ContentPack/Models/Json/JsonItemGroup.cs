using KitchenLib.Customs;
using KitchenLib.src.ContentPack.Models.Interface;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace KitchenLib.src.ContentPack.Models.Json
{
    public class JsonItemGroup : CustomItemGroup
    {
        [JsonProperty("ID")]
        public override int ID { get; internal set; }
        [JsonProperty("UniqueNameID")]
        public string UniqueNameID { get; set; }
        [JsonProperty("BaseGameDataObjectID")]
        [DefaultValue(-1)]
        public int BaseGameDataObjectID { get; set; }


        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (base.ModName != ModName)
                base.ModName = ModName;
        }
    }
}
