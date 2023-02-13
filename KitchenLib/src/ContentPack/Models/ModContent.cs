using Semver;
using System.Collections.Generic;
using static KitchenLib.ContentPack.ContentPackUtils;

namespace KitchenLib.ContentPack.Models
{
    public class ModContent
    {
        public SemVersion Format { get; set; }
        public Dictionary<string, SerializationContext> Changes { get; set; }
    }
}
