using KitchenLib.src.JSON.ContractResolver;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace KitchenLib.JSON
{
	internal class ContentPackUtils
	{
		public static JsonSerializerSettings settings = new JsonSerializerSettings()
		{
			ContractResolver = new ProtectedFieldResolver(),
			Converters = new JsonConverter[]
			{
				new StringEnumConverter()
			}
		};
	}

	public enum GDOType
	{
		ItemGroup
	}
}
