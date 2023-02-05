using KitchenData;
using KitchenLib.src.ContentPack.ContractResolver;
using KitchenLib.src.ContentPack.JsonConverters;
using KitchenLib.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace KitchenLib.src.ContentPack
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

        public static GameObject PrefabConverter(string str)
        {
            GameDataObject customGDO = null;
            if(int.TryParse(str, out var num))
            {
                customGDO = GDOUtils.GetExistingGDO(num);
            } else
            {
                customGDO = GDOUtils.GetCustomGameDataObject(StringUtils.GetInt32HashCode(str)).GameDataObject;
            }
            return customGDO switch
            {
                Item customItem => customItem.Prefab,
                _ => null
            };
        }

        public static string Serialize(object Object, bool indented = true)
        {
            if(indented)
                return $"\n{JsonConvert.SerializeObject(Object, Formatting.Indented, settings)}";
            return JsonConvert.SerializeObject(Object, settings);
        }

        public static T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text, settings);
        }

        public enum SerializationContext
        {
            ModManifest,
            ModContent,
            Appliance,
            CompositeUnlockPack,
            CrateSet,
            Decor,
            Dish,
            Effect,
            EffectRepresentation,
            GameDataObject,
            GameDifficultySettings,
            GardenProfile,
            Item,
            ItemGroup,
            LayoutProfile,
            LevelUpgradeSet,
            LocalisedGameDataObject,
            ModularUnlockPack,
            PlayerCosmetic,
            Process,
            RandomUpgradeSet,
            Research,
            Shop,
            ThemeUnlock,
            Unlock,
            UnlockCard,
            UnlockPack,
            WorkshopRecipe,
            ApplianceProcess,
            ItemProcess,
            SubProcess
        }
    }
}
