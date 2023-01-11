using KitchenData;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class PrefabConverter : GenericCustomConverter<GameObject>
    {
        public override object Create(string str)
        {
            return GetPrefab(GDOUtils.GetCustomGameDataObject(StringUtils.GetInt32HashCode(str)).GameDataObject);
        }

        public override object Create(int id)
        {
            return GetPrefab(GDOUtils.GetExistingGDO(id));
        }

        public GameObject GetPrefab(GameDataObject gdo)
        {
            return gdo switch
            {
                Item item => item.Prefab,
                Appliance appliance => appliance.Prefab,
                _ => null
            };
        }
    }
}
