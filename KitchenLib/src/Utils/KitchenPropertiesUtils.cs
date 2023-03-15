using Kitchen;
using KitchenData;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class KitchenPropertiesUtils
	{

		public static Process GetProcess()
		{
			Process result = new Process();

			//result.BasicEnablingAppliance = GDOUtils.GetExistingAppliance(-1248669347);
			result.BasicEnablingAppliance = (Appliance)GDOUtils.GetExistingGDO(-1248669347);
			result.CanObfuscateProgress = true;
			result.EnablingApplianceCount = 1;
			result.Icon = "<sprite name=\"chop\">";

			return result;
		}

		public static Item.ItemProcess GetItemProcess(Process process, Item processResult, float duration, bool isBad, bool requiresWrapepr)
		{
			Item.ItemProcess result = new Item.ItemProcess();

			result.Process = process;
			result.Result = processResult;
			result.Duration = duration;
			result.IsBad = isBad;
			result.RequiresWrapper = requiresWrapepr;

			return result;
		}

		public static CItemProvider GetUnlimitedCItemProvider(int id)
		{
			CItemProvider provider = new CItemProvider();
			object ptemp = provider;
			ReflectionUtils.GetField<CItemProvider>("Item").SetValue(ptemp, id);
			provider = (CItemProvider)ptemp;

			return provider;
		}

		public static CItemProvider GetLimitedCItemProvider(int id, int available, int maximum)
		{
			CItemProvider provider = new CItemProvider();
			object ptemp = provider;
			ReflectionUtils.GetField<CItemProvider>("Item").SetValue(ptemp, id);
			provider = (CItemProvider)ptemp;

			provider.Available = available;
			provider.Maximum = maximum;

			return provider;
		}

		public static CItemProvider GetCItemProvider(int id, int available, int maximum, bool directInsertOnly, bool emptyAtNight, bool preventReturns, bool destroyOnEmpty, bool autoGrabFromHolder, bool autoPlaceOnHolder, bool allowRefreshes)
		{
			CItemProvider result = new CItemProvider();
			object ptemp = result;
			ReflectionUtils.GetField<CItemProvider>("Item").SetValue(ptemp, id);
			result = (CItemProvider)ptemp;

			result.Available = available;
			result.Maximum = maximum;
			result.DirectInsertionOnly = directInsertOnly;
			result.EmptyAtNight = emptyAtNight;
			result.PreventReturns = preventReturns;
			result.DestroyOnEmpty = destroyOnEmpty;
			result.AutoGrabFromHolder = autoGrabFromHolder;
			result.AutoPlaceOnHolder = autoPlaceOnHolder;
			result.AllowRefreshes = allowRefreshes;

			return result;
		}

		public static CGivesDecoration GetCGivesDecoration(int affordable, int charming, int exclusive, int formal, int kitchen)
		{
			CGivesDecoration result = new CGivesDecoration();
			DecorationValues values = new DecorationValues();
			values.Affordable = affordable;
			values.Charming = charming;
			values.Exclusive = exclusive;
			values.Formal = formal;
			values.Kitchen = kitchen;
			result.DecorationValues = values;
			return result;
		}

		public static CConveyPushItems GetCConveyPushItems(float delay, bool push, bool grab, bool reversed, bool grabSpecificType, int specificType, ItemList specificComponents, bool ignoreProcessingItems, float progress, CConveyPushItems.ConveyState state)
		{
			CConveyPushItems result = new CConveyPushItems();
			result.Delay = delay;
			result.Push = push;
			result.Grab = grab;
			result.Reversed = reversed;
			result.GrabSpecificType = grabSpecificType;
			result.SpecificType = specificType;
			result.SpecificComponents = specificComponents;
			result.IgnoreProcessingItems = ignoreProcessingItems;
			result.Progress = progress;
			result.State = state;
			return result;
		}

		public static CConveyCooldown GetCConveyCooldown(float total, float remaining)
		{
			CConveyCooldown result = new CConveyCooldown();

			result.Total = total;
			result.Remaining = remaining;

			return result;
		}

		public static CApplianceBin GetCApplianceBin(int capacity, int currentAmmount, int emptyBinItem, float selfEmptyTime, float nextEmptyTime)
		{
			CApplianceBin result = new CApplianceBin();

			result.Capacity = capacity;
			result.CurrentAmount = currentAmmount;
			result.EmptyBinItem = emptyBinItem;
			result.SelfEmptyTime = selfEmptyTime;
			result.NextEmptyTime = nextEmptyTime;

			return result;
		}

		public static CCleanAppliance GetCCleanAppliance(int waterAppliance, bool canReplace)
		{
			CCleanAppliance result = new CCleanAppliance();

			result.WaterAppliance = waterAppliance;
			result.CanReplace = canReplace;

			return result;
		}

		public static CMobileAppliance GetCMobileAppliance(float speed, bool aimForDirt, Vector3 target)
		{
			CMobileAppliance result = new CMobileAppliance();

			result.Speed = speed;
			result.AimForDirt = aimForDirt;
			result.Target = target;

			return result;
		}

		public static CAutomatedInteractor GetCAutomatedInteractor(InteractionType type, bool isHeld, bool doNotReceive, bool transferOnly, TransferFlags requiredFlags)
		{
			CAutomatedInteractor result = new CAutomatedInteractor();

			result.Type = type;
			result.IsHeld = isHeld;
			result.DoNotReceive = doNotReceive;
			result.TransferOnly = transferOnly;
			result.RequiredFlags = requiredFlags;

			return result;
		}

		public static CItemTransferRestrictions GetCItemTransferRestrictions(bool allowWhenActive, bool allowWhenInactive)
		{
			CItemTransferRestrictions result = new CItemTransferRestrictions();

			result.AllowWhenActive = allowWhenActive;
			result.AllowWhenInactive = allowWhenInactive;

			return result;
		}

		public static CApplianceHostStand GetCApplianceHostStand(bool automatic)
		{
			CApplianceHostStand result = new CApplianceHostStand();

			result.Automatic = automatic;

			return result;
		}

		public static COrderEncourager GetCOrderEncourager(float probability)
		{
			COrderEncourager result = new COrderEncourager();

			result.Probability = probability;

			return result;
		}

		public static CSlowPlayer GetCSlowPlayer(float radius, float factor)
		{
			CSlowPlayer result = new CSlowPlayer();

			result.Radius = radius;
			result.Factor = factor;

			return result;
		}

		public static CTakesDuration GetCTakesDuration(float total, float remaining, bool active, bool manual, bool manualNeedsEmptyHands, DurationToolType relevantTool, InteractionMode mode, bool requiresRelease, bool preserveProgress, bool isInverse, bool isLocked, float currentChange)
		{
			CTakesDuration result = new CTakesDuration();

			result.Total = total;
			result.Remaining = remaining;
			result.Active = active;
			result.Manual = manual;
			result.ManualNeedsEmptyHands = manualNeedsEmptyHands;
			result.RelevantTool = relevantTool;
			result.Mode = mode;
			result.RequiresRelease = requiresRelease;
			result.PreserveProgress = preserveProgress;
			result.IsInverse = isInverse;
			result.IsLocked = isLocked;
			result.CurrentChange = currentChange;

			return result;
		}

		public static CDisplayDuration GetCDisplayDuration(bool isBad, int process, bool showWhenEmpty)
		{
			CDisplayDuration result = new CDisplayDuration();

			result.IsBad = isBad;
			result.Process = process;
			result.ShowWhenEmpty = showWhenEmpty;

			return result;
		}

		public static CStackableMess GetCStackableMess(int baseMess, int nextMess)
		{
			CStackableMess result = new CStackableMess();

			result.BaseMess = baseMess;
			result.NextMess = nextMess;

			return result;
		}

		public static CBlueprintStore GetCBlueprintStore(bool inUse, int applianceID, int price, int blueprintID, bool hasBeenUpgraded, bool hasBeenCopied, bool hasBeenMadeFree)
		{
			CBlueprintStore result = new CBlueprintStore();

			result.InUse = inUse;
			result.ApplianceID = applianceID;
			result.Price = price;
			result.BlueprintID = blueprintID;
			result.HasBeenUpgraded = hasBeenUpgraded;
			result.HasBeenCopied = hasBeenCopied;
			result.HasBeenMadeFree = hasBeenMadeFree;

			return result;
		}

		public static CCabinetModifier GetCCabinetModifier(bool upgrades, bool duplicates, bool makesFree, bool disableDeskAfterImprovement, bool defaultUpgrades, bool defaultDuplicates, bool defaultMakesFree, bool defaultDisablesDeskAfterImprovements)
		{
			CCabinetModifier result = new CCabinetModifier();

			result.Upgrades = upgrades;
			result.Duplicates = duplicates;
			result.MakesFree = makesFree;
			result.DisablesDeskAfterImprovement = disableDeskAfterImprovement;
			result.DefaultUpgrades = defaultUpgrades;
			result.DefaultDuplicates = defaultDuplicates;
			result.DefaultMakesFree = defaultMakesFree;
			result.DefaultDisablesDeskAfterImprovement = defaultDisablesDeskAfterImprovements;

			return result;
		}

		public static CDeskTarget GetCDeskTarget(bool requireUpgrade, bool requireCopyable, bool requireMakeFree, float retargetTime, float NextTarget, Entity target)
		{
			CDeskTarget result = new CDeskTarget();

			result.RequireUpgrade = requireUpgrade;
			result.RequireCopyable = requireCopyable;
			result.RequireMakeFree = requireMakeFree;
			result.RetargetTime = retargetTime;
			result.NextTarget = NextTarget;
			result.Target = target;

			return result;
		}

		public static CModifyBlueprintStoreAfterDuration GetCModifyBlueprintStoreAfterDuration(bool performUpgrade, bool performCopy, bool makeFree)
		{
			CModifyBlueprintStoreAfterDuration result = new CModifyBlueprintStoreAfterDuration();

			result.PerformUpgrade = performUpgrade;
			result.PerformCopy = performCopy;
			result.MakeFree = makeFree;

			return result;
		}

		public static CApplianceDeskIterate GetCApplianceDeskIterate(float averageTime, bool isLocked, bool requestUpdate, float nextUpdateTime)
		{
			CApplianceDeskIterate result = new CApplianceDeskIterate();

			result.AverageTime = averageTime;
			result.IsLocked = isLocked;
			result.RequestUpdate = requestUpdate;
			result.NextUpdateTime = nextUpdateTime;

			return result;
		}

		public static CGrantsShopDiscount GetCGrantsShopDiscount(float amount)
		{
			CGrantsShopDiscount result = new CGrantsShopDiscount();

			result.Amount = amount;

			return result;
		}

		public static CGrantsExtraBlueprint GetCGrantsExtraBlueprint(int id, bool isFree, bool canBeDuplicated)
		{
			CGrantsExtraBlueprint result = new CGrantsExtraBlueprint();

			result.ID = id;
			result.IsFree = isFree;
			result.CanBeDuplicated = canBeDuplicated;

			return result;
		}

		public static CRemovesShopBlueprint GetCRemovesShopBlueprint(int count)
		{
			CRemovesShopBlueprint result = new CRemovesShopBlueprint();

			result.Count = count;

			return result;
		}

		public static CDurationRequirement GetCDurationRequirement(bool needsScheduledCustomers, bool needsBeforeClosing)
		{
			CDurationRequirement result = new CDurationRequirement();

			result.NeedsScheduledCustomers = needsScheduledCustomers;
			result.NeedsBeforeClosing = needsBeforeClosing;

			return result;
		}

		public static CApplianceOrderMachine GetCApplianceOrderMachine(bool isReorderMachine)
		{
			CApplianceOrderMachine result = new CApplianceOrderMachine();

			result.IsReorderMachine = isReorderMachine;

			return result;
		}

		public static CItemStorage GetCItemStorage(int activeIndex, int capacity, bool isStack, bool preventManualCycling)
		{
			CItemStorage result = new CItemStorage();

			result.ActiveIndex = activeIndex;
			result.Capacity = capacity;
			result.IsStack = isStack;
			result.PreventManualCycling = preventManualCycling;

			return result;
		}

		public static CApplyProcessAfterDuration GetCApplyProcessAfterDuration(bool breakOnFailure)
		{
			CApplyProcessAfterDuration result = new CApplyProcessAfterDuration();

			result.BreakOnFailure = breakOnFailure;

			return result;
		}

		public static CSetEnabledAfterDuration GetCSetEnabledAfterDuration(bool activate)
		{
			CSetEnabledAfterDuration result = new CSetEnabledAfterDuration();

			result.Activate = activate;

			return result;
		}

		public static CBreakIfBadDuration GetCBreakIfBadDuration(bool catchFire, bool triggeredByNoProcess, bool triggeredByBadProcess)
		{
			CBreakIfBadDuration result = new CBreakIfBadDuration();

			result.CatchFire = catchFire;
			result.TriggeredByNoProcess = triggeredByNoProcess;
			result.TriggeredByBadProcess = triggeredByBadProcess;

			return result;
		}

		public static CRestrictProgressVisibility GetCRestrictProgressVisibility(bool hideWhenActive, bool hideWhenInactive, bool obfuscateWhenActive, bool obfuscateWhenInactive)
		{
			CRestrictProgressVisibility result = new CRestrictProgressVisibility();

			result.HideWhenActive = hideWhenActive;
			result.HideWhenInactive = hideWhenInactive;
			result.ObfuscateWhenActive = obfuscateWhenActive;
			result.ObfuscateWhenInactive = obfuscateWhenInactive;

			return result;
		}

		public static CDynamicMenuProvider GetCDynamicMenuProvider(DynamicMenuType type)
		{
			CDynamicMenuProvider result = new CDynamicMenuProvider();

			result.Type = type;

			return result;
		}

		public static CVariableProvider GetCVariableProvider(int current, int provide1, int provide2, int provide3)
		{
			CVariableProvider result = new CVariableProvider();

			result.Current = current;
			result.Provide1 = provide1;
			result.Provide2 = provide2;
			result.Provide3 = provide3;

			return result;
		}

		public static CDestroyApplianceAtDay GetCDestroyApplianceAtDay(bool hideBin)
		{
			CDestroyApplianceAtDay result = new CDestroyApplianceAtDay();

			result.HideBin = hideBin;

			return result;
		}

		public static CRequiresGenericInputIndicator GetCRequiresGenericInputIndicator(InputIndicatorMessage message)
		{
			CRequiresGenericInputIndicator result = new CRequiresGenericInputIndicator();

			result.Message = message;

			return result;
		}

		public static CItemHolderFilter GetCItemHolderFilter(ItemCategory category, bool allowAny, bool noDirectInsert)
		{
			CItemHolderFilter result = new CItemHolderFilter();

			result.Category = category;
			result.AllowAny = allowAny;
			result.NoDirectInsertion = noDirectInsert;

			return result;
		}

		public static CDynamicItemProvider GetCDynamicItemProvider(ItemStorage storageFlags)
		{
			CDynamicItemProvider result = new CDynamicItemProvider();

			result.StorageFlags = storageFlags;

			return result;
		}

		public static CFlowerProvider GetCFlowerProvider(int gardenProfile)
		{
			CFlowerProvider result = new CFlowerProvider();

			result.GardenProfile = gardenProfile;

			return result;
		}

		public static CApplianceChair GetCApplianceChair(bool isInUse, Entity occupant)
		{
			CApplianceChair result = new CApplianceChair();

			result.IsInUse = isInUse;
			result.Occupant = occupant;

			return result;
		}

		public static CApplianceTable GetCApplianceTable(bool isIndividualTable, bool isWaitingTable, bool preventSittingUp, bool preventSittingDown, bool preventSittingLeft, bool preventSittingRight, Orientation activeChairs)
		{
			CApplianceTable result = new CApplianceTable();

			result.IsIndividualTable = isIndividualTable;
			result.IsWaitingTable = isWaitingTable;
			result.PreventSittingUp = preventSittingUp;
			result.PreventSittingDown = preventSittingDown;
			result.PreventSittingLeft = preventSittingLeft;
			result.PreventSittingRight = preventSittingRight;
			result.ActiveChairs = activeChairs;

			return result;
		}

		public static CShoeSelector GetCShoeSelector(PlayerShoe shoe, int available, int max)
		{
			CShoeSelector result = new CShoeSelector();

			result.Shoe = shoe;
			result.Available = available;
			result.Max = max;

			return result;
		}

		public static CChangeProviderAfterDuration GetCChangeProviderAfterDuration(int replaceitem)
		{
			CChangeProviderAfterDuration result = new CChangeProviderAfterDuration();

			result.ReplaceItem = replaceitem;

			return result;
		}

		public static CChangeProviderWhenEmpty GetCChangeProviderWhenEmpty(int replaceitem)
		{
			CChangeProviderWhenEmpty result = new CChangeProviderWhenEmpty();

			result.ReplaceItem = replaceitem;

			return result;
		}

		public static CDurationRequiresProvider GetCDurationRequiresProvider(int requiredItem, int minimumItems)
		{
			CDurationRequiresProvider result = new CDurationRequiresProvider();

			result.RequiredItem = requiredItem;
			result.MinimumItems = minimumItems;

			return result;
		}

		public static CCausesSpills GetCCausesSpills(int id, float rate, bool overwriteOtherMesses)
		{
			CCausesSpills result = new CCausesSpills();

			result.ID = id;
			result.Rate = rate;
			result.OverwriteOtherMesses = overwriteOtherMesses;

			return result;
		}

	}
}
