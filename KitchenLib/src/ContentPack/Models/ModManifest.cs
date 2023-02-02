using Semver;
using System.Collections.Generic;

namespace KitchenLib.src.ContentPack.Models
{
    public class ModManifest
    {
        public string Description { get; set; }
        public string Author { get; set; }
        public SemVersion Version { get; set; }
        public List<string> ContentPackFor { get; set; }

        public bool IsTargetFor(string GUID)
        {
            return ContentPackFor.Contains(GUID);
        }
    }
}
