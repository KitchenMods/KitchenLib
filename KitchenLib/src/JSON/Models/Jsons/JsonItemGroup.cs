using KitchenLib.Customs;
using Newtonsoft.Json;

namespace KitchenLib.JSON.Models.Jsons
{
	public class JsonItemGroup : CustomItemGroup
	{
		[JsonProperty("UniqueNameID")]
		public override string UniqueNameID => "";
	}
}
