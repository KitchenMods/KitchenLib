using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(-1)]
        public override int BaseGameDataObjectID { get; internal set; }
        [JsonProperty("ModName")]
        public new string ModName;
        [JsonProperty("Info")]
        public new LocalisationObject<ApplianceInfo> Info { get; internal set; }

        [JsonProperty("Prefab")]
        public override GameObject Prefab { get; internal set; }
        [JsonProperty("HeldAppliancePrefab")]
        public override GameObject HeldAppliancePrefab { get; internal set; }
        [JsonProperty("Processes")]
        public new List<Appliance.ApplianceProcesses> Processes { get; internal set; }
        [JsonProperty("Properties")]
        public new List<IApplianceProperty> Properties { get; internal set; }
        [JsonProperty("EffectRange")]
        public override IEffectRange EffectRange { get; internal set; }
        [JsonProperty("EffectCondition")]
        public override IEffectCondition EffectCondition { get; internal set; }
        [JsonProperty("EffectType")]
        public override IEffectType EffectType { get; internal set; }
        [JsonProperty("EffectRepresentation")]
        public override EffectRepresentation EffectRepresentation { get; internal set; }
        [JsonProperty("IsNonInteractive")]
        public override bool IsNonInteractive { get; internal set; }
        [JsonProperty("Layer")]
        public override OccupancyLayer Layer { get; internal set; }
        [JsonProperty("ForceHighInteractionPriority")]
        public override bool ForceHighInteractionPriority { get; internal set; }
        [JsonProperty("PurchaseCost")]
        [DefaultValue(0)]
        public override int PurchaseCost { get; internal set; }
        [JsonProperty("EntryAnimation")]
        public override EntryAnimation EntryAnimation { get; internal set; }
        [JsonProperty("ExitAnimation")]
        public override ExitAnimation ExitAnimation { get; internal set; }
        [JsonProperty("SkipRotationAnimation")]
        public override bool SkipRotationAnimation { get; internal set; }
        [JsonProperty("IsPurchasable")]
        [DefaultValue(false)]
        public override bool IsPurchasable { get; internal set; }
        [JsonProperty("IsPurchasableAsUpgrade")]
        public override bool IsPurchasableAsUpgrade { get; internal set; }
        [JsonProperty("ThemeRequired")]
        public override DecorationType ThemeRequired { get; internal set; }
        [JsonProperty("ShoppingTags")]
        [DefaultValue(ShoppingTags.None)]
        public override ShoppingTags ShoppingTags { get; internal set; }
        [JsonProperty("RarityTier")]
        [DefaultValue(RarityTier.Common)]
        public override RarityTier RarityTier { get; internal set; }
        [JsonProperty("PriceTier")]
        [DefaultValue(PriceTier.Medium)]
        public override PriceTier PriceTier { get; internal set; }
        [JsonProperty("ShopRequirementFilter")]
        public override ShopRequirementFilter ShopRequirementFilter {get; internal set;}
        [JsonProperty("RequiresForShop")]
        public new List<Appliance> RequiresForShop {get; internal set;}
        [JsonProperty("RequiresProcessForShop")]
        public new List<Process> RequiresProcessForShop {get; internal set;}
        public virtual bool StapleWhenMissing { get; internal set; }
        public virtual bool SellOnlyAsDuplicate { get; internal set; }
        public virtual bool PreventSale { get; internal set; }
        public virtual List<Appliance> Upgrades { get { return new List<Appliance>(); } }
        public virtual bool IsAnUpgrade { get; internal set; }
        public virtual bool IsNonCrated { get; internal set; }
        public virtual Item CrateItem { get; internal set; }
        public virtual string Name { get { return "Appliance"; } }
        public virtual string Description { get { return "A little something for your restaurant"; } }
        public virtual List<Appliance.Section> Sections { get { return new List<Appliance.Section>(); } }
        public virtual List<string> Tags { get { return new List<string>(); } }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (base.ModName != ModName)
                base.ModName = ModName;
            if (base.Info != Info)
                base.Info = Info;
            if (base.Processes != Processes)
                base.Processes = Processes;
            if (base.Properties != Properties)
                base.Properties = Properties;
            if(base.RequiresForShop != RequiresForShop)
                base.RequiresForShop = RequiresForShop;
        }

    }
}