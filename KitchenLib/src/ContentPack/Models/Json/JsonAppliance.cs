using System.Collections.Generic;
using System.Runtime.Serialization;
using KitchenData;
using Newtonsoft.Json;
using UnityEngine;

namespace KitchenLib.Customs
{
    public class JsonAppliance : CustomAppliance
    {
        [JsonProperty("ID")]
        public override int ID { get; internal set; }
        [JsonProperty("UniqueNameID")]
        public override string UniqueNameID { get; internal set; }
        [JsonProperty("BaseGameDataObjectID")]
        public override int BaseGameDataObjectID { get; }
        [JsonProperty("ModName")]
        public new string ModName;
        [JsonProperty("Info")]
        public override LocalisationObject<ApplianceInfo> Info { get; internal set; }

        [JsonProperty("Prefab")]
        public override GameObject Prefab { get; internal set; }
        [JsonProperty("HeldAppliancePrefab")]
        public override GameObject HeldAppliancePrefab { get; internal set; }
        [JsonProperty("Processes")]
        public override List<Appliance.ApplianceProcesses> Processes { get; }
        [JsonProperty("EffectRepresentation")]
        public override EffectRepresentation EffectRepresentation { get; internal set; }
        [JsonProperty("IsNonInteractive")]
        public override bool IsNonInteractive { get; internal set; }
        [JsonProperty("Layer")]
        public override OccupancyLayer Layer { get; internal set; }
        [JsonProperty("ForceHighInteractionPriority")]
        public override bool ForceHighInteractionPriority { get; internal set; }
        [JsonProperty("PurchaseCost")]
        public override int PurchaseCost { get; }
        [JsonProperty("EntryAnimation")]
        public override EntryAnimation EntryAnimation { get; internal set; }
        [JsonProperty("ExitAnimation")]
        public override ExitAnimation ExitAnimation { get; internal set; }
        [JsonProperty("SkipRotationAnimation")]
        public override bool SkipRotationAnimation { get; internal set; }
        [JsonProperty("IsPurchasable")]
        public override bool IsPurchasable { get; }
        [JsonProperty("IsPurchasableAsUpgrade")]
        public override bool IsPurchasableAsUpgrade { get; internal set; }
        [JsonProperty("ThemeRequired")]
        public override DecorationType ThemeRequired { get; internal set; }
        [JsonProperty("ShoppingTags")]
        public override ShoppingTags ShoppingTags { get; }
        [JsonProperty("RarityTier")]
        public override RarityTier RarityTier { get; }
        [JsonProperty("PriceTier")]
        public override PriceTier PriceTier { get; }
        [JsonProperty("ShopRequirementFilter")]
        public override ShopRequirementFilter ShopRequirementFilter { get; internal set; }
        [JsonProperty("RequiresForShop")]
        public override List<Appliance> RequiresForShop { get; }
        [JsonProperty("RequiresProcessForShop")]
        public override List<Process> RequiresProcessForShop { get; }
        public override bool StapleWhenMissing { get; internal set; }
        public override bool SellOnlyAsDuplicate { get; internal set; }
        public override bool PreventSale { get; internal set; }
        public override List<Appliance> Upgrades { get; }
        public override bool IsAnUpgrade { get; internal set; }
        public override bool IsNonCrated { get; internal set; }
        public override Item CrateItem { get; internal set; }
        public override string Name { get; }
        public override string Description { get; }
        public override List<Appliance.Section> Sections { get; }
        public override List<string> Tags { get { return new List<string>(); } }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (base.ModName != ModName)
                base.ModName = ModName;
        }

    }
}