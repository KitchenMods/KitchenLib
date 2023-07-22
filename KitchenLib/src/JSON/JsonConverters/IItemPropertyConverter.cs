using Kitchen;
using KitchenData;
using KitchenLib.JSON.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.JSON.JsonConverters
{
	public class IItemPropertyConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(IItemProperty);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);

			if (jObject.TryGetValue("Type", out JToken type))
			{
				ItemPropertyContext context = type.ToObject<ItemPropertyContext>();

				IItemProperty property = context switch
				{
					ItemPropertyContext.CEffectCreator => new CEffectCreator(),
					ItemPropertyContext.CTriggerOrderReset => new CTriggerOrderReset(),
					ItemPropertyContext.CTriggerPatienceReset => new CTriggerPatienceReset(),
					ItemPropertyContext.CTriggerLeaveHappy => new CTriggerLeaveHappy(),
					ItemPropertyContext.CRefreshesFlowerProviders => new CRefreshesFlowerProviders(),
					ItemPropertyContext.CRefreshesProviderQuantity => new CRefreshesProviderQuantity(),
					ItemPropertyContext.CRefreshesSpecificProvider => new CRefreshesSpecificProvider(),
					ItemPropertyContext.CApplyDecor => new CApplyDecor(),
					ItemPropertyContext.CEquippableTool => new CEquippableTool(),
					ItemPropertyContext.CToolClean => new CToolClean(),
					ItemPropertyContext.CToolStorage => new CToolStorage(),
					ItemPropertyContext.CToolInteractionMemory => new CToolInteractionMemory(),
					ItemPropertyContext.CDurationTool => new CDurationTool(),
					ItemPropertyContext.CProcessTool => new CProcessTool(),
					ItemPropertyContext.CReturnItem => new CReturnItem(),
					ItemPropertyContext.CPreventItemTransfer => new CPreventItemTransfer(),
					ItemPropertyContext.CPreventItemMerge => new CPreventItemMerge(),
					ItemPropertyContext.CSlowPlayer => new CSlowPlayer()
				};

				serializer.Populate(jObject["Property"].CreateReader(), property);
				return property;
			};
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken jToken = JToken.FromObject(value);
			jToken.WriteTo(writer);
		}
	}
}
