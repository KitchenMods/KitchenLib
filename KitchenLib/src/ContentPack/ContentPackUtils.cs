﻿using KitchenLib.src.ContentPack.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KitchenLib.src.ContentPack
{
    public class ContentPackUtils
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            DefaultValueHandling = DefaultValueHandling.Populate,
            Converters = new JsonConverter[]
            {
                new StringEnumConverter(),
                new SemVersionConverter(),
                new AssetBundleConverter(),
                new PrefabConverter(),
                new ItemProcessConverter(),
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