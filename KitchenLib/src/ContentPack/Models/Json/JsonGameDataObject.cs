using System.ComponentModel;
using System.Runtime.Serialization;
using KitchenLib.Customs;
using Newtonsoft.Json;

namespace KitchenLib.ContentPack.Models.Json
{
    public abstract class JsonGameDataObject : CustomGameDataObject
    {
        [JsonProperty("ID")]
        public override int ID { get; internal set; }
        [JsonProperty("UniqueNameID")]
        public override string UniqueNameID { get; internal set; }
        [JsonProperty("BaseGameDataObjectID")]
        [DefaultValue(-1)]
        public override int BaseGameDataObjectID { get; internal set; }
        [JsonProperty("ModName")]
        public new string ModName;
        
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if(base.ModName != ModName)
                base.ModName = ModName;
        }
    }
}