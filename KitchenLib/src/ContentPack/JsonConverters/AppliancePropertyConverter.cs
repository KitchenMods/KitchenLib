using Kitchen;
using KitchenData;
using KitchenLib.src.ContentPack.Models.Containers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace KitchenLib.src.ContentPack.JsonConverters
{
    public class AppliancePropertyConverter : CustomConverter<AppliancePropertyContainer>
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            if (jObject.TryGetValue("Type", out JToken type))
            {
                AppliancePropertyContainer appliancePropertyContainer = new AppliancePropertyContainer();

                AppliancePropertyContext ApplianceProperty = type.ToObject<AppliancePropertyContext>();
                appliancePropertyContainer.Type = ApplianceProperty;

                IApplianceProperty property = ApplianceProperty switch
                {
                    AppliancePropertyContext.CAllowPlacingOver => new CAllowPlacingOver(),
                    AppliancePropertyContext.CDoesNotOccupy => throw new NotImplementedException(),
                    AppliancePropertyContext.CDisableAutomation => throw new NotImplementedException(),
                    AppliancePropertyContext.CGivesDecoration => throw new NotImplementedException(),
                    AppliancePropertyContext.CItemProvider => new CItemProvider(),
                    AppliancePropertyContext.CDynamicItemProvider => throw new NotImplementedException(),
                    AppliancePropertyContext.CDestroyApplianceAtNight => throw new NotImplementedException(),
                    AppliancePropertyContext.CDestroyApplianceAtDay => throw new NotImplementedException(),
                    AppliancePropertyContext.CItemStorage => throw new NotImplementedException(),
                    AppliancePropertyContext.CAcceptIntoStorage => throw new NotImplementedException(),
                    AppliancePropertyContext.CLockHolderIfStorage => throw new NotImplementedException(),
                    AppliancePropertyContext.CHolderFirstIfStorage => throw new NotImplementedException(),
                    AppliancePropertyContext.CDropFromStorage => throw new NotImplementedException(),
                    AppliancePropertyContext.CItemTransferRestrictions => throw new NotImplementedException(),
                    AppliancePropertyContext.CMustHaveWall => throw new NotImplementedException(),
                    AppliancePropertyContext.CStatic => throw new NotImplementedException(),
                    AppliancePropertyContext.CIsInactive => throw new NotImplementedException(),
                    AppliancePropertyContext.CDeactivateAtNight => throw new NotImplementedException(),
                    AppliancePropertyContext.CFixedRotation => throw new NotImplementedException(),
                    AppliancePropertyContext.CDestroyWhenNotHolding => throw new NotImplementedException(),
                    AppliancePropertyContext.CTriggerFullSaveLoad => throw new NotImplementedException(),
                    AppliancePropertyContext.CTriggerPracticeMode => throw new NotImplementedException(),
                    AppliancePropertyContext.CTriggerTutorial => throw new NotImplementedException(),
                    AppliancePropertyContext.CTriggerProfileEditor => throw new NotImplementedException(),
                    AppliancePropertyContext.CRequiresGenericInputIndicator => throw new NotImplementedException(),
                    AppliancePropertyContext.CDemoLock => throw new NotImplementedException(),
                    AppliancePropertyContext.CAutomatedInteractor => throw new NotImplementedException(),
                    AppliancePropertyContext.CIsInteractive => throw new NotImplementedException(),
                    AppliancePropertyContext.CInteractionProxy => throw new NotImplementedException(),
                    AppliancePropertyContext.CItemHolder => throw new NotImplementedException(),
                    AppliancePropertyContext.CItemHolderFilter => throw new NotImplementedException(),
                    AppliancePropertyContext.CCosmeticSelector => throw new NotImplementedException(),
                    AppliancePropertyContext.COutfitSelector => throw new NotImplementedException(),
                    AppliancePropertyContext.CColourSelector => throw new NotImplementedException(),
                    AppliancePropertyContext.CShoeSelector => throw new NotImplementedException(),
                    AppliancePropertyContext.CSlowPlayer => throw new NotImplementedException(),
                    AppliancePropertyContext.CAcceptsResearchValue => throw new NotImplementedException(),
                    AppliancePropertyContext.CBreakOnFailure => throw new NotImplementedException(),
                    AppliancePropertyContext.CNoBadProcesses => throw new NotImplementedException(),
                    AppliancePropertyContext.CPreservesContentsOvernight => throw new NotImplementedException(),
                    AppliancePropertyContext.CImmovable => throw new NotImplementedException(),
                    AppliancePropertyContext.CMobileAppliance => throw new NotImplementedException(),
                    AppliancePropertyContext.CSpawnMobileAppliance => throw new NotImplementedException(),
                    AppliancePropertyContext.CAllowMobilePathing => throw new NotImplementedException(),
                    AppliancePropertyContext.CMobileApplianceResetPoint => throw new NotImplementedException(),
                    AppliancePropertyContext.CCleanAppliance => throw new NotImplementedException(),
                    AppliancePropertyContext.CRequiresActivation => throw new NotImplementedException(),
                    AppliancePropertyContext.CRequiresHeldActivation => throw new NotImplementedException(),
                    AppliancePropertyContext.CRestrictProgressVisibility => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceIngredientSlot => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceBin => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceExternalBin => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceHostStand => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceTable => throw new NotImplementedException(),
                    AppliancePropertyContext.CTablePrioritiseCorrectGroups => throw new NotImplementedException(),
                    AppliancePropertyContext.CNoAttachedBonuses => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceGrabPoint => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceOrderMachine => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceDumbWaiter => throw new NotImplementedException(),
                    AppliancePropertyContext.CChangeProviderAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CChangeProviderWhenEmpty => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplyProcessAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CBreakIfBadDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CDestroyAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CRerollShopAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CPurchaseAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CSetEnabledAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CDurationRequiresProvider => throw new NotImplementedException(),
                    AppliancePropertyContext.CDurationRequirement => throw new NotImplementedException(),
                    AppliancePropertyContext.CTakesDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CLockDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CDurationInteractionProxy => throw new NotImplementedException(),
                    AppliancePropertyContext.CDisplayDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CLockedWhileDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CConveyPushItems => throw new NotImplementedException(),
                    AppliancePropertyContext.CConveyPushRotatable => throw new NotImplementedException(),
                    AppliancePropertyContext.CConveyCooldown => throw new NotImplementedException(),
                    AppliancePropertyContext.CConveyTeleport => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceChair => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceGhostChair => throw new NotImplementedException(),
                    AppliancePropertyContext.CVariableProvider => throw new NotImplementedException(),
                    AppliancePropertyContext.COutsideOnly => throw new NotImplementedException(),
                    AppliancePropertyContext.CGardenSpawn => throw new NotImplementedException(),
                    AppliancePropertyContext.CPreventGardenDespawn => throw new NotImplementedException(),
                    AppliancePropertyContext.CFlowerProvider => throw new NotImplementedException(),
                    AppliancePropertyContext.CCausesSpills => throw new NotImplementedException(),
                    AppliancePropertyContext.CCatchFireOnFailure => throw new NotImplementedException(),
                    AppliancePropertyContext.CCatchFireOnProcessComplete => throw new NotImplementedException(),
                    AppliancePropertyContext.CCatchFireDuringProcess => throw new NotImplementedException(),
                    AppliancePropertyContext.CRenameRestaurant => throw new NotImplementedException(),
                    AppliancePropertyContext.COrderEncourager => throw new NotImplementedException(),
                    AppliancePropertyContext.CChristmasShedPlaceholder => throw new NotImplementedException(),
                    AppliancePropertyContext.CStackableMess => throw new NotImplementedException(),
                    AppliancePropertyContext.CSnapToTile => throw new NotImplementedException(),
                    AppliancePropertyContext.CPreventGameOver => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceDeskIterate => throw new NotImplementedException(),
                    AppliancePropertyContext.CGrantsShopDiscount => throw new NotImplementedException(),
                    AppliancePropertyContext.CCountsAsBlueprintImprover => throw new NotImplementedException(),
                    AppliancePropertyContext.CCabinetModifier => throw new NotImplementedException(),
                    AppliancePropertyContext.CDeskTarget => throw new NotImplementedException(),
                    AppliancePropertyContext.CModifyBlueprintStoreAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CGrantMoneyAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CAccelerateTimeAfterDuration => throw new NotImplementedException(),
                    AppliancePropertyContext.CRemovesShopBlueprint => throw new NotImplementedException(),
                    AppliancePropertyContext.CGrantsExtraBlueprint => throw new NotImplementedException(),
                    AppliancePropertyContext.CBlueprintStore => throw new NotImplementedException(),
                    AppliancePropertyContext.CDurationRequiresDeskTarget => throw new NotImplementedException(),
                    AppliancePropertyContext.CApplianceDrinkDispenser => throw new NotImplementedException(),
                    AppliancePropertyContext.CFireImmune => throw new NotImplementedException(),
                    AppliancePropertyContext.CHighlyFlammable => throw new NotImplementedException(),
                    AppliancePropertyContext.CHideHeldProgressIndicator => throw new NotImplementedException(),
                    AppliancePropertyContext.CDynamicMenuProvider => throw new NotImplementedException(),
                    _ => null,
                };
                serializer.Populate(jObject["Property"].CreateReader(), property);
                appliancePropertyContainer.Property = property;

                return appliancePropertyContainer;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken jToken = JToken.FromObject(value);
            jToken.WriteTo(writer);
        }
    }
}
