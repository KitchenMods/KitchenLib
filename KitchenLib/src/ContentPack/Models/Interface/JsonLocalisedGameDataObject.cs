using System.ComponentModel;
using System.Runtime.Serialization;
using KitchenData;
using KitchenLib.Customs;
using Newtonsoft.Json;

namespace KitchenLib.src.ContentPack.Models.Interface
{
    public abstract class JsonLocalisedGameDataObject<T> : CustomGameDataObject where T : Localisation
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
        [JsonProperty("Info")]
        public virtual LocalisationObject<T> Info { get; internal set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (base.ModName != ModName)
                base.ModName = ModName;
        }
    }
}