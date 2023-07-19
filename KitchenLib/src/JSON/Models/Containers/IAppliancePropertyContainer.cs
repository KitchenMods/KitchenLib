using Kitchen;
using KitchenData;
using KitchenLib.JSON.Enums;
using KitchenLib.Utils;
using Newtonsoft.Json.Linq;

namespace KitchenLib.JSON.Models.Containers
{
	public struct IAppliancePropertyContainer
	{
		public JObject jObject;

		public IApplianceProperty Convert()
		{
			if (jObject.TryGetValue("Type", out JToken type))
			{
				AppliancePropertyContext context = type.ToObject<AppliancePropertyContext>();

				IApplianceProperty property = context switch
				{
					AppliancePropertyContext.CBreakOnFailure => new CBreakOnFailure(),
					AppliancePropertyContext.CNoBadProcesses => new CNoBadProcesses(),
					AppliancePropertyContext.CPreservesContentsOvernight => new CPreservesContentsOvernight(),
					AppliancePropertyContext.CImmovable => new CImmovable(),
					AppliancePropertyContext.CMobileAppliance => new CMobileAppliance(), //Vector3
					AppliancePropertyContext.CSpawnMobileAppliance => new CSpawnMobileAppliance(),
					AppliancePropertyContext.CAllowMobilePathing => new CAllowMobilePathing(),
					AppliancePropertyContext.CMobileApplianceResetPoint => new CMobileApplianceResetPoint(), //Vector3
					AppliancePropertyContext.CCleanAppliance => new CCleanAppliance(),
					AppliancePropertyContext.CRequiresActivation => new CRequiresActivation(),
					AppliancePropertyContext.CRequiresHeldActivation => new CRequiresHeldActivation(),
					AppliancePropertyContext.CRestrictProgressVisibility => new CRestrictProgressVisibility(),
					AppliancePropertyContext.CApplianceIngredientSlot => new CApplianceIngredientSlot(),
					AppliancePropertyContext.CApplianceBin => new CApplianceBin(),
					AppliancePropertyContext.CApplianceExternalBin => new CApplianceExternalBin(),
					AppliancePropertyContext.CApplianceHostStand => new CApplianceHostStand(),
					AppliancePropertyContext.CApplianceTable => new CApplianceTable(),
					AppliancePropertyContext.CTablePrioritiseCorrectGroups => new CTablePrioritiseCorrectGroups(),
					AppliancePropertyContext.CNoAttachedBonuses => new CNoAttachedBonuses(),
					AppliancePropertyContext.CApplianceGrabPoint => new CApplianceGrabPoint(),
					AppliancePropertyContext.CApplianceOrderMachine => new CApplianceOrderMachine(),
					AppliancePropertyContext.CApplianceDumbWaiter => new CApplianceDumbWaiter(),
					AppliancePropertyContext.CChangeProviderAfterDuration => new CChangeProviderAfterDuration(),
					AppliancePropertyContext.CChangeProviderWhenEmpty => new CChangeProviderWhenEmpty(),
					AppliancePropertyContext.CApplyProcessAfterDuration => new CApplyProcessAfterDuration(),
					AppliancePropertyContext.CBreakIfBadDuration => new CBreakIfBadDuration(),
					AppliancePropertyContext.CDestroyAfterDuration => new CDestroyAfterDuration(),
					AppliancePropertyContext.CRerollShopAfterDuration => new CRerollShopAfterDuration(),
					AppliancePropertyContext.CPurchaseAfterDuration => new CPurchaseAfterDuration(),
					AppliancePropertyContext.CSetEnabledAfterDuration => new CSetEnabledAfterDuration(),
					AppliancePropertyContext.CDurationRequiresProvider => new CDurationRequiresProvider(),
					AppliancePropertyContext.CDurationRequirement => new CDurationRequirement(),
					AppliancePropertyContext.CTakesDuration => new CTakesDuration(),
					AppliancePropertyContext.CLockDuration => new CLockDuration(),
					AppliancePropertyContext.CDurationInteractionProxy => new CDurationInteractionProxy(),
					AppliancePropertyContext.CDisplayDuration => new CDisplayDuration(),
					AppliancePropertyContext.CLockedWhileDuration => new CLockedWhileDuration(),
					AppliancePropertyContext.CConveyPushItems => new CConveyPushItems(),
					AppliancePropertyContext.CConveyPushRotatable => new CConveyPushRotatable(),
					AppliancePropertyContext.CConveyCooldown => new CConveyCooldown(),
					AppliancePropertyContext.CConveyTeleport => new CConveyTeleport(),
					AppliancePropertyContext.CApplianceChair => new CApplianceChair(),
					AppliancePropertyContext.CApplianceGhostChair => new CApplianceGhostChair(),
					AppliancePropertyContext.CVariableProvider => new CVariableProvider(),
					AppliancePropertyContext.COutsideOnly => new COutsideOnly(),
					AppliancePropertyContext.CGardenSpawn => new CGardenSpawn(),
					AppliancePropertyContext.CPreventGardenDespawn => new CPreventGardenDespawn(),
					AppliancePropertyContext.CFlowerProvider => new CFlowerProvider(),
					AppliancePropertyContext.CCausesSpills => new CCausesSpills(),
					AppliancePropertyContext.CCatchFireOnFailure => new CCatchFireOnFailure(),
					AppliancePropertyContext.CCatchFireOnProcessComplete => new CCatchFireOnProcessComplete(),
					AppliancePropertyContext.CCatchFireDuringProcess => new CCatchFireDuringProcess(),
					AppliancePropertyContext.CRenameRestaurant => new CRenameRestaurant(), //FixedString64
					AppliancePropertyContext.COrderEncourager => new COrderEncourager(),
					AppliancePropertyContext.CChristmasShedPlaceholder => new CChristmasShedPlaceholder(),
					AppliancePropertyContext.CStackableMess => new CStackableMess(),
					AppliancePropertyContext.CSnapToTile => new CSnapToTile(),
					AppliancePropertyContext.CPreventGameOver => new CPreventGameOver(),
					AppliancePropertyContext.CApplianceDeskIterate => new CApplianceDeskIterate(),
					AppliancePropertyContext.CGrantsShopDiscount => new CGrantsShopDiscount(),
					AppliancePropertyContext.CCountsAsBlueprintImprover => new CCountsAsBlueprintImprover(),
					AppliancePropertyContext.CCabinetModifier => new CCabinetModifier(),
					AppliancePropertyContext.CDeskTarget => new CDeskTarget(),
					AppliancePropertyContext.CModifyBlueprintStoreAfterDuration => new CModifyBlueprintStoreAfterDuration(),
					AppliancePropertyContext.CGrantMoneyAfterDuration => new CGrantMoneyAfterDuration(),
					AppliancePropertyContext.CAccelerateTimeAfterDuration => new CAccelerateTimeAfterDuration(),
					AppliancePropertyContext.CRemovesShopBlueprint => new CRemovesShopBlueprint(),
					AppliancePropertyContext.CGrantsExtraBlueprint => new CGrantsExtraBlueprint(),
					AppliancePropertyContext.CBlueprintStore => new CBlueprintStore(),
					AppliancePropertyContext.CDurationRequiresDeskTarget => new CDurationRequiresDeskTarget(),
					AppliancePropertyContext.CApplianceDrinkDispenser => new CApplianceDrinkDispenser(),
					AppliancePropertyContext.CFireImmune => new CFireImmune(),
					AppliancePropertyContext.CHighlyFlammable => new CHighlyFlammable(),
					AppliancePropertyContext.CHideHeldProgressIndicator => new CHideHeldProgressIndicator(),
					AppliancePropertyContext.CDynamicMenuProvider => new CDynamicMenuProvider(),
					AppliancePropertyContext.CAllowPlacingOver => new CAllowPlacingOver(),
					AppliancePropertyContext.CDoesNotOccupy => new CDoesNotOccupy(),
					AppliancePropertyContext.CDisableAutomation => new CDisableAutomation(),
					AppliancePropertyContext.CGivesDecoration => new CGivesDecoration(),
					AppliancePropertyContext.CProviderFillsOnAccept => new CProviderFillsOnAccept(),
					AppliancePropertyContext.CItemProvider => ConvertItemProvider(jObject["Property"]["Item"].ToObject<int>()),
					AppliancePropertyContext.CFillAtInterval => new CFillAtInterval(),
					AppliancePropertyContext.CDynamicItemProvider => new CDynamicItemProvider(),
					AppliancePropertyContext.CDestroyApplianceAtNight => new CDestroyApplianceAtNight(),
					AppliancePropertyContext.CDestroyApplianceAtDay => new CDestroyApplianceAtDay(),
					AppliancePropertyContext.CItemStorage => new CItemStorage(),
					AppliancePropertyContext.CAcceptIntoStorage => new CAcceptIntoStorage(),
					AppliancePropertyContext.CLockHolderIfStorage => new CLockHolderIfStorage(),
					AppliancePropertyContext.CHolderFirstIfStorage => new CHolderFirstIfStorage(),
					AppliancePropertyContext.CDropFromStorage => new CDropFromStorage(),
					AppliancePropertyContext.CItemTransferRestrictions => new CItemTransferRestrictions(),
					AppliancePropertyContext.CMustHaveWall => new CMustHaveWall(),
					AppliancePropertyContext.CStatic => new CStatic(),
					AppliancePropertyContext.CIsInactive => new CIsInactive(),
					AppliancePropertyContext.CDeactivateAtNight => new CDeactivateAtNight(),
					AppliancePropertyContext.CFixedRotation => new CFixedRotation(),
					AppliancePropertyContext.CDestroyWhenNotHolding => new CDestroyWhenNotHolding(),
					AppliancePropertyContext.CTriggerFullSaveLoad => new CTriggerFullSaveLoad(),
					AppliancePropertyContext.CTriggerPracticeMode => new CTriggerPracticeMode(),
					AppliancePropertyContext.CTriggerTutorial => new CTriggerTutorial(),
					AppliancePropertyContext.CTriggerProfileEditor => new CTriggerProfileEditor(),
					AppliancePropertyContext.CRequiresGenericInputIndicator => new CRequiresGenericInputIndicator(),
					AppliancePropertyContext.CDemoLock => new CDemoLock(),
					AppliancePropertyContext.CLocationChoice => new CLocationChoice(), // Seed, FixedString64
					AppliancePropertyContext.CAutomatedInteractor => new CAutomatedInteractor(),
					AppliancePropertyContext.CIsInteractive => new CIsInteractive(),
					AppliancePropertyContext.CInteractionProxy => new CInteractionProxy(),
					AppliancePropertyContext.CRequiresSpecificRefresher => new CRequiresSpecificRefresher(),
					AppliancePropertyContext.CItemHolder => new CItemHolder(),
					AppliancePropertyContext.CItemHolderFilter => new CItemHolderFilter(),
					AppliancePropertyContext.CCosmeticSelector => new CCosmeticSelector(),
					AppliancePropertyContext.COutfitSelector => new COutfitSelector(),
					AppliancePropertyContext.CColourSelector => new CColourSelector(),
					AppliancePropertyContext.CShoeSelector => new CShoeSelector(),
					AppliancePropertyContext.CSlowPlayer => new CSlowPlayer(),
					AppliancePropertyContext.CAcceptsResearchValue => new CAcceptsResearchValue(),
					AppliancePropertyContext.CSettingSelector => new CSettingSelector(),
					AppliancePropertyContext.CAchievementTracker => new CAchievementTracker(),
					AppliancePropertyContext.CExpViewer => new CExpViewer(),
					AppliancePropertyContext.CUpgradesTracker => new CUpgradesTracker(),
				};

				JSONPackSerializer.Serializer.Populate(jObject["Property"].CreateReader(), property);
				return property;
			}
			return null;
		}

		public CItemProvider ConvertItemProvider(int id)
		{
			CItemProvider result = new CItemProvider();
			object ptemp = result;
			ReflectionUtils.GetField<CItemProvider>("Item").SetValue(ptemp, id);
			result = (CItemProvider)ptemp;
			return result;
		}
	}
}
