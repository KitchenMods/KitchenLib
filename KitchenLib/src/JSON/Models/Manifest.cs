using Newtonsoft.Json;
using Semver;
using System.Collections.Generic;

namespace KitchenLib.JSON.Models
{
	internal class Manifest
	{
		[JsonProperty("Author", Required = Required.Always)]
		public string Author { get; set; }

		[JsonProperty("ModName", Required = Required.Always)]
		public string ModName { get; set; }

		[JsonProperty("Debug")]
		public bool Debug { get; set; }

		[JsonProperty("Dependencies")]
		public Dictionary<string, SemVersion> Dependencies { get; set; } = new Dictionary<string, SemVersion>();
	}
}
