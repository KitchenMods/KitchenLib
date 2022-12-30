using Newtonsoft.Json;
using Semver;

namespace KitchenLib.src.ContentPack.Models
{
    public class ModManifest
    {
        [JsonProperty("ModName", Required = Required.Always)]
        public string ModName { get; set; }
        [JsonProperty("Description", Required = Required.Always)]
        public string Description { get; set; }
        [JsonProperty("Author", Required = Required.Always)]
        public string Author { get; set; }
        [JsonProperty("Version", Required = Required.Always)]
        public SemVersion Version { get; set; }
        [JsonProperty("ContentPackFor", Required = Required.Always)]
        public ContentPackTarget ContentPackFor { get; set; }
    }

    public class ContentPackTarget
    {
        [JsonProperty("UniqueID", Required = Required.Always)]
        public string UniqueID { get; set; }

        public bool isTargetFor(string modName)
        {
            return UniqueID == modName;
        }
    }
}
