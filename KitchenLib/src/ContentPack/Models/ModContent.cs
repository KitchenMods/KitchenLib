using Semver;
using System.Collections.Generic;

namespace KitchenLib.src.ContentPack.Models
{
    public class ModContent
    {
        public SemVersion Format { get; set; }
        public Dictionary<string, SerializationContext> Changes { get; set; }
    }
}
