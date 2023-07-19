namespace KitchenLib.JSON.Enums
{
	// Kitchen.CBreakOnFailure
	// Kitchen.CNoBadProcesses
	// Kitchen.CPreservesContentsOvernight
	// Kitchen.CImmovable
	// Kitchen.CMobileAppliance
	// Kitchen.CSpawnMobileAppliance
	// Kitchen.CAllowMobilePathing
	// Kitchen.CMobileApplianceResetPoint
	// Kitchen.CCleanAppliance
	// Kitchen.CRequiresActivation
	// Kitchen.CRequiresHeldActivation
	// Kitchen.CRestrictProgressVisibility
	// Kitchen.CApplianceIngredientSlot
	// Kitchen.CApplianceBin
	// Kitchen.CApplianceExternalBin
	// Kitchen.CApplianceHostStand
	// Kitchen.CApplianceTable
	// Kitchen.CTablePrioritiseCorrectGroups
	// Kitchen.CNoAttachedBonuses
	// Kitchen.CApplianceGrabPoint
	// Kitchen.CApplianceOrderMachine
	// Kitchen.CApplianceDumbWaiter
	// Kitchen.CChangeProviderAfterDuration
	// Kitchen.CChangeProviderWhenEmpty
	// Kitchen.CApplyProcessAfterDuration
	// Kitchen.CBreakIfBadDuration
	// Kitchen.CDestroyAfterDuration
	// Kitchen.CRerollShopAfterDuration
	// Kitchen.CPurchaseAfterDuration
	// Kitchen.CSetEnabledAfterDuration
	// Kitchen.CDurationRequiresProvider
	// Kitchen.CDurationRequirement
	// Kitchen.CTakesDuration
	// Kitchen.CLockDuration
	// Kitchen.CDurationInteractionProxy
	// Kitchen.CDisplayDuration
	// Kitchen.CLockedWhileDuration
	// Kitchen.CConveyPushItems
	// Kitchen.CConveyPushRotatable
	// Kitchen.CConveyCooldown
	// Kitchen.CConveyTeleport
	// Kitchen.CApplianceChair
	// Kitchen.CApplianceGhostChair
	// Kitchen.CVariableProvider
	// Kitchen.COutsideOnly
	// Kitchen.CGardenSpawn
	// Kitchen.CPreventGardenDespawn
	// Kitchen.CFlowerProvider
	// Kitchen.CCausesSpills
	// Kitchen.CCatchFireOnFailure
	// Kitchen.CCatchFireOnProcessComplete
	// Kitchen.CCatchFireDuringProcess
	// Kitchen.CRenameRestaurant
	// Kitchen.COrderEncourager
	// Kitchen.CChristmasShedPlaceholder
	// Kitchen.CStackableMess
	// Kitchen.CSnapToTile
	// Kitchen.CPreventGameOver
	// Kitchen.CApplianceDeskIterate
	// Kitchen.CGrantsShopDiscount
	// Kitchen.CCountsAsBlueprintImprover
	// Kitchen.CCabinetModifier
	// Kitchen.CDeskTarget
	// Kitchen.CModifyBlueprintStoreAfterDuration
	// Kitchen.CGrantMoneyAfterDuration
	// Kitchen.CAccelerateTimeAfterDuration
	// Kitchen.CRemovesShopBlueprint
	// Kitchen.CGrantsExtraBlueprint
	// Kitchen.CBlueprintStore
	// Kitchen.CDurationRequiresDeskTarget
	// Kitchen.CApplianceDrinkDispenser
	// Kitchen.CFireImmune
	// Kitchen.CHighlyFlammable
	// Kitchen.CHideHeldProgressIndicator
	// Kitchen.CDynamicMenuProvider
	// Kitchen.CAllowPlacingOver
	// Kitchen.CDoesNotOccupy
	// Kitchen.CDisableAutomation
	// Kitchen.CGivesDecoration
	// Kitchen.CProviderFillsOnAccept
	// Kitchen.CItemProvider
	// Kitchen.CFillAtInterval
	// Kitchen.CDynamicItemProvider
	// Kitchen.CDestroyApplianceAtNight
	// Kitchen.CDestroyApplianceAtDay
	// Kitchen.CItemStorage
	// Kitchen.CAcceptIntoStorage
	// Kitchen.CLockHolderIfStorage
	// Kitchen.CHolderFirstIfStorage
	// Kitchen.CDropFromStorage
	// Kitchen.CItemTransferRestrictions
	// Kitchen.CMustHaveWall
	// Kitchen.CStatic
	// Kitchen.CIsInactive
	// Kitchen.CDeactivateAtNight
	// Kitchen.CFixedRotation
	// Kitchen.CDestroyWhenNotHolding
	// Kitchen.CTriggerFullSaveLoad
	// Kitchen.CTriggerPracticeMode
	// Kitchen.CTriggerTutorial
	// Kitchen.CTriggerProfileEditor
	// Kitchen.CRequiresGenericInputIndicator
	// Kitchen.CDemoLock
	// Kitchen.CLocationChoice
	// Kitchen.CAutomatedInteractor
	// Kitchen.CIsInteractive
	// Kitchen.CInteractionProxy
	// Kitchen.CRequiresSpecificRefresher
	// Kitchen.CItemHolder
	// Kitchen.CItemHolderFilter
	// Kitchen.CCosmeticSelector
	// Kitchen.COutfitSelector
	// Kitchen.CColourSelector
	// Kitchen.CShoeSelector
	// Kitchen.CSlowPlayer
	// Kitchen.CAcceptsResearchValue
	// Kitchen.CSettingSelector
	// Kitchen.CAchievementTracker
	// Kitchen.CExpViewer
	// Kitchen.CUpgradesTracker

	public enum AppliancePropertyContext
	{
		CBreakOnFailure,
		CNoBadProcesses,
		CPreservesContentsOvernight,
		CImmovable,
		CMobileAppliance,
		CSpawnMobileAppliance,
		CAllowMobilePathing,
		CMobileApplianceResetPoint,
		CCleanAppliance,
		CRequiresActivation,
		CRequiresHeldActivation,
		CRestrictProgressVisibility,
		CApplianceIngredientSlot,
		CApplianceBin,
		CApplianceExternalBin,
		CApplianceHostStand,
		CApplianceTable,
		CTablePrioritiseCorrectGroups,
		CNoAttachedBonuses,
		CApplianceGrabPoint,
		CApplianceOrderMachine,
		CApplianceDumbWaiter,
		CChangeProviderAfterDuration,
		CChangeProviderWhenEmpty,
		CApplyProcessAfterDuration,
		CBreakIfBadDuration,
		CDestroyAfterDuration,
		CRerollShopAfterDuration,
		CPurchaseAfterDuration,
		CSetEnabledAfterDuration,
		CDurationRequiresProvider,
		CDurationRequirement,
		CTakesDuration,
		CLockDuration,
		CDurationInteractionProxy,
		CDisplayDuration,
		CLockedWhileDuration,
		CConveyPushItems,
		CConveyPushRotatable,
		CConveyCooldown,
		CConveyTeleport,
		CApplianceChair,
		CApplianceGhostChair,
		CVariableProvider,
		COutsideOnly,
		CGardenSpawn,
		CPreventGardenDespawn,
		CFlowerProvider,
		CCausesSpills,
		CCatchFireOnFailure,
		CCatchFireOnProcessComplete,
		CCatchFireDuringProcess,
		CRenameRestaurant,
		COrderEncourager,
		CChristmasShedPlaceholder,
		CStackableMess,
		CSnapToTile,
		CPreventGameOver,
		CApplianceDeskIterate,
		CGrantsShopDiscount,
		CCountsAsBlueprintImprover,
		CCabinetModifier,
		CDeskTarget,
		CModifyBlueprintStoreAfterDuration,
		CGrantMoneyAfterDuration,
		CAccelerateTimeAfterDuration,
		CRemovesShopBlueprint,
		CGrantsExtraBlueprint,
		CBlueprintStore,
		CDurationRequiresDeskTarget,
		CApplianceDrinkDispenser,
		CFireImmune,
		CHighlyFlammable,
		CHideHeldProgressIndicator,
		CDynamicMenuProvider,
		CAllowPlacingOver,
		CDoesNotOccupy,
		CDisableAutomation,
		CGivesDecoration,
		CProviderFillsOnAccept,
		CItemProvider,
		CFillAtInterval,
		CDynamicItemProvider,
		CDestroyApplianceAtNight,
		CDestroyApplianceAtDay,
		CItemStorage,
		CAcceptIntoStorage,
		CLockHolderIfStorage,
		CHolderFirstIfStorage,
		CDropFromStorage,
		CItemTransferRestrictions,
		CMustHaveWall,
		CStatic,
		CIsInactive,
		CDeactivateAtNight,
		CFixedRotation,
		CDestroyWhenNotHolding,
		CTriggerFullSaveLoad,
		CTriggerPracticeMode,
		CTriggerTutorial,
		CTriggerProfileEditor,
		CRequiresGenericInputIndicator,
		CDemoLock,
		CLocationChoice,
		CAutomatedInteractor,
		CIsInteractive,
		CInteractionProxy,
		CRequiresSpecificRefresher,
		CItemHolder,
		CItemHolderFilter,
		CCosmeticSelector,
		COutfitSelector,
		CColourSelector,
		CShoeSelector,
		CSlowPlayer,
		CAcceptsResearchValue,
		CSettingSelector,
		CAchievementTracker,
		CExpViewer,
		CUpgradesTracker
	}
}
