using KitchenData;
using KitchenLib.Customs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static KitchenLib.src.ContentPack.ContentPackUtils;

namespace KitchenLib.src.ContentPack.Models.Jsons
{
    public class JsonItem : CustomItem 
    {
        [JsonProperty("Prefab")]
        string PrefabStr { get; set; }
        [JsonProperty("Properties")]
        List<ItemPropertyContainer> ItemPropertyContainers { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Prefab = PrefabConverter(PrefabStr);
            if(ItemPropertyContainers is not null)
                Properties = ItemPropertyContainers.Select(p => p.Property).ToList();
        }
    }

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
