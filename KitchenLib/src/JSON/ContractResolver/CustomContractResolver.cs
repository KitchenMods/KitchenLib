using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace KitchenLib.src.JSON.ContractResolver
{
	public class CustomContractResolver : DefaultContractResolver
	{
		public CustomContractResolver()
		{
			SerializeCompilerGeneratedMembers = true;
		}

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var prop = base.CreateProperty(member, memberSerialization);
			prop.Writable = true;
			return prop;
		}
	}
}