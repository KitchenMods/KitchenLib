using Kitchen;
using KitchenData;
using KitchenLib.src.JSON.Models.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.src.JSON.JsonConverters
{
    public class ItemPropertyConverter : JsonConverter
    {
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ItemPropertyContainer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject.TryGetValue("Type", out JToken type))
            {
                ItemPropertyContainer itemPropertyContainer = new ItemPropertyContainer();

                ItemPropertyContext ItemProperty = type.ToObject<ItemPropertyContext>();
                itemPropertyContainer.Type = ItemProperty;

                IItemProperty property = ItemProperty switch
                {
                    ItemPropertyContext.CEffectCreator => new CEffectCreator(),
                    ItemPropertyContext.CTriggerOrderReset => new CTriggerOrderReset(),
                    ItemPropertyContext.CTriggerPatienceReset => new CTriggerPatienceReset(),
                    ItemPropertyContext.CTriggerLeaveHappy => new CTriggerLeaveHappy(),
                    ItemPropertyContext.CRefreshesFlowerProviders => new CRefreshesFlowerProviders(),
                    ItemPropertyContext.CRefreshesProviderQuantity => new CRefreshesProviderQuantity(),
                    //ItemPropertyContext.CApplyDecor => new CApplyDecor(),
                    ItemPropertyContext.CEquippableTool => new CEquippableTool(),
                    ItemPropertyContext.CToolClean => new CToolClean(),
                    ItemPropertyContext.CToolStorage => new CToolStorage(),
                    ItemPropertyContext.CDurationTool => new CDurationTool(),
                    ItemPropertyContext.CProcessTool => new CProcessTool(),
                    ItemPropertyContext.CReturnItem => new CReturnItem(),
                    ItemPropertyContext.CPreventItemTransfer => new CPreventItemTransfer(),
                    //ItemPropertyContext.CPreventItemMerge => new CPreventItemMerge(),
                    ItemPropertyContext.CSlowPlayer => new CSlowPlayer(),
                    _ => null
                };
                serializer.Populate(jObject["Property"].CreateReader(), property);
                itemPropertyContainer.Property = property;

                return itemPropertyContainer;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(value);
            jToken.WriteTo(writer);
        }
    }
}
