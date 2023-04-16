using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KitchenLib.JSON
{
	public class ContentPackManager
	{
		internal static void RegisterJSONGDO(GDOType type, JObject jObject)
		{
			Main.LogInfo($"{type}: {jObject}");
		}
	}
}
