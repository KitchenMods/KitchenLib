using KitchenLib.src.ContentPack.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace KitchenLib.src.ContentPack
{
    public class ContentPackUtils
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Populate,
            ContractResolver = new DefaultContractResolver
            {
                SerializeCompilerGeneratedMembers = true
            },
            Converters = new JsonConverter[]
            {
                new StringEnumConverter(),
                new SemVersionConverter(),
                new PrefabConverter(),
                new GameDataObjectConverter()
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
