using Kitchen;
using KitchenData;
using KitchenLib.JSON.Enums;
using Newtonsoft.Json.Linq;

namespace KitchenLib.JSON.Models.Containers
{
	public struct IItemPropertyContainer
	{
		public JObject jObject;

		public IItemProperty Convert()
		{
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

				JSONPackSerializer.Serializer.Populate(jObject["Property"].CreateReader(), property);
				return property;
			}
			return null;
		}
	}
}
