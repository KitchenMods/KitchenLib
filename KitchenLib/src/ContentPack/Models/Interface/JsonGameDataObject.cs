using System.ComponentModel;
using System.Runtime.Serialization;
using KitchenLib.Customs;
using Newtonsoft.Json;

namespace KitchenLib.src.ContentPack.Models.Interface
{
    public abstract class JsonGameDataObject : CustomGameDataObject
    {
        [JsonProperty("ID")]
        public override int ID { get; internal set; }
        [JsonProperty("UniqueNameID")]
        public override string UniqueNameID { get; internal set; }
        [JsonProperty("BaseGameDataObjectID")]
        public override int BaseGameDataObjectID { get; internal set; } = -1;
        [JsonProperty("ModName")]
        public new string ModName;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if(context.Context is string modName)
                base.ModName = modName;
            else if (base.ModName != ModName)
                base.ModName = ModName;
        }
    }
}