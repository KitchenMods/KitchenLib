using Newtonsoft.Json;
using Semver;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.src.ContentPack.Models
{
    public class ModContent
    {
        [JsonProperty("Format", Required = Required.Always)]
        public SemVersion Format { get; set; }
        [JsonProperty("AssetBundle")]
        public AssetBundle Bundle { get; set; }
        [JsonProperty("Changes", Required = Required.Always)]
        public List<ModChange> Changes { get; set; }
    }
}