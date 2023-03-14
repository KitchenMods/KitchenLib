using KitchenData;

namespace KitchenLib.src.JSON.Models.Containers
{
    public class ItemPropertyContainer
    {
        public ItemPropertyContext Type { get; set; }
        public IItemProperty Property { get; set; }
    }

    public enum ItemPropertyContext
    {
        CEffectCreator,
        CTriggerOrderReset,
        CTriggerPatienceReset,
        CTriggerLeaveHappy,
        CRefreshesFlowerProviders,
        CRefreshesProviderQuantity,
        CApplyDecor,
        CEquippableTool,
        CToolClean,
        CToolStorage,
        CDurationTool,
        CProcessTool,
        CReturnItem,
        CPreventItemTransfer,
        CPreventItemMerge,
        CSlowPlayer
    }
}
