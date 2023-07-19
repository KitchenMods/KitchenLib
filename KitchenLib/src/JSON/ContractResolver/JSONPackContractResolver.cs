using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace KitchenLib.JSON.ContractResolver
{
	internal class JSONPackContractResolver : DefaultContractResolver
	{
		internal JSONPackContractResolver()
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
