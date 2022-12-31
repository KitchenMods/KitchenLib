using KitchenLib.src.ContentPack.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KitchenLib.src.ContentPack
{
    public class ContentPackUtils
    {
        internal static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Converters = new JsonConverter[]
            {
                new StringEnumConverter(),
                new SemVersionConverter(),
                new AssetBundleConverter()
            }
        };
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
        GameDifficultySettings,
        GardenProfile,
        Item,
        ItemGroup,
        LevelUpgradeSet,
        ModularUnlockPack
    }
}
