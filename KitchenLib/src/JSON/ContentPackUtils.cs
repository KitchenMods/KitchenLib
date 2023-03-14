using KitchenData;
using KitchenLib.src.JSON.ContractResolver;
using KitchenLib.src.JSON.JsonConverters;
using KitchenLib.src.JSON.Models.Jsons;
using KitchenLib.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.src.JSON
{
    public class ContentPackUtils
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            ContractResolver = new ProtectedFieldResolver(),
            Converters = new JsonConverter[]
            {
                new StringEnumConverter(),
                new SemVersionConverter(),
                new GameDataObjectConverter(),
                new ItemPropertyConverter(),
                new AppliancePropertyConverter(),
                new MaterialConverter()
            }
        };

        public static Dictionary<SerializationContext, Type> ContextType = new Dictionary<SerializationContext, Type>
        {
            {SerializationContext.Appliance, typeof(Appliance)},
            {SerializationContext.Item, typeof(Item)},
            {SerializationContext.ItemGroup, typeof(ItemGroup)},
            {SerializationContext.Dish, typeof(Dish)},
        };

        public static Dictionary<SerializationContext, Type> JSONType = new Dictionary<SerializationContext, Type>
        {
            {SerializationContext.Appliance, typeof(JsonAppliance)},
            {SerializationContext.Item, typeof(JsonItem)},
            {SerializationContext.ItemGroup, typeof(JsonItemGroup)},
            {SerializationContext.Dish, typeof(JsonDish)},
        };

        public static GameObject PrefabConverter(string str)
        {
            GameDataObject customGDO = null;
            if (int.TryParse(str, out var num))
            {
                customGDO = GDOUtils.GetExistingGDO(num);
            }
            else
            {
                customGDO = GDOUtils.GetCustomGameDataObject(StringUtils.GetInt32HashCode(str)).GameDataObject;
            }
            return customGDO switch
            {
                Item customItem => customItem.Prefab,
                _ => null
            };
        }

        public enum SerializationContext
        {
            Appliance,
            Dish,
            Item,
            ItemGroup
        }
    }
}
