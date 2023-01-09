using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class PrefabConverter : GenericCustomConverter<GameObject>
    {
        public override object Create(string str)
        {
            GameDataObject GDO;
            if (int.TryParse(str, out int ID))
                GDO = GDOUtils.GetExistingGDO(ID);
            else
                GDO = GDOUtils.GetCustomGameDataObject(StringUtils.GetInt32HashCode(str)).GameDataObject;

            return GDO switch
            {
                Item item => item.Prefab,
                Appliance appliance => appliance.Prefab,
                _ => null
            };
        }
    }
}
