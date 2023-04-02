using Kitchen;

/*
 * ALL THIS CODE CAME FROM ICEDMILO'S CARD MANAGER
 * MASSIVE THANKS FOR MAKING THIS SO I DONT HAVE TO <3
 */
namespace KitchenLib.Fun
{
	internal static class EffectTypeComparer
	{
		public static class CCabinetModifierComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CCabinetModifier)x, (CCabinetModifier)y);
			}

			public static bool IsEqual(CCabinetModifier x, CCabinetModifier y)
			{
				return x.Upgrades == y.Upgrades
					&& x.Duplicates == y.Duplicates
					&& x.MakesFree == y.MakesFree
					&& x.DisablesDeskAfterImprovement == y.DisablesDeskAfterImprovement
					&& x.DefaultUpgrades == y.DefaultUpgrades
					&& x.DefaultDuplicates == y.DefaultDuplicates
					&& x.DefaultMakesFree == y.DefaultMakesFree
					&& x.DefaultDisablesDeskAfterImprovement == y.DefaultDisablesDeskAfterImprovement;
			}

			public static int GetHashCode(CCabinetModifier obj)
			{
				int hash = 17;
				hash = hash * 31 + obj.Upgrades.GetHashCode();
				hash = hash * 31 + obj.Duplicates.GetHashCode();
				hash = hash * 31 + obj.MakesFree.GetHashCode();
				hash = hash * 31 + obj.DisablesDeskAfterImprovement.GetHashCode();
				hash = hash * 31 + obj.DefaultUpgrades.GetHashCode();
				hash = hash * 31 + obj.DefaultDuplicates.GetHashCode();
				hash = hash * 31 + obj.DefaultMakesFree.GetHashCode();
				hash = hash * 31 + obj.DefaultDisablesDeskAfterImprovement.GetHashCode();
				return hash;
			}
		}

		public static class CApplianceSpeedModifierComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CApplianceSpeedModifier)x, (CApplianceSpeedModifier)y);
			}

			public static bool IsEqual(CApplianceSpeedModifier x, CApplianceSpeedModifier y)
			{
				return GetHashCode(x).Equals(GetHashCode(y));
			}

			public static int GetHashCode(CApplianceSpeedModifier obj)
			{
				int hash = 17;
				hash = hash * 31 + obj.AffectsAllProcesses.GetHashCode();
				hash = hash * 31 + obj.Process.GetHashCode();
				hash = hash * 31 + obj.Speed.GetHashCode();
				hash = hash * 31 + obj.BadSpeed.GetHashCode();
				return hash;
			}
		}

		public static class CAppliesStatusComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CAppliesStatus)x, (CAppliesStatus)y);
			}

			public static bool IsEqual(CAppliesStatus x, CAppliesStatus y)
			{
				return GetHashCode(x).Equals(GetHashCode(y));
			}

			public static int GetHashCode(CAppliesStatus obj)
			{
				return obj.Bonus.GetHashCode();
			}
		}

		public static class CTableModifierComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CTableModifier)x, (CTableModifier)y);
			}

			public static bool IsEqual(CTableModifier x, CTableModifier y)
			{
				return GetHashCode(x).Equals(GetHashCode(y));
			}

			public static int GetHashCode(CTableModifier obj)
			{
				int hash = 17;
				hash = hash * 31 + obj.PatienceModifiers.Eating.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.Thinking.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.Seating.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.Service.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.WaitForFood.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.GetFoodDelivered.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.DrinkDeliverBonus.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.ItemDeliverBonus.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.FoodDeliverBonus.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.SkipWaitPhase.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.InfinitePatienceIfQueue.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.DestroyTableIfLeave.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.BonusPatienceWhenNearby.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.ResetPatienceOption.GetHashCode();
				hash = hash * 31 + obj.PatienceModifiers.ProvidesQueuePatienceBoost.GetHashCode();

				hash = hash * 31 + obj.OrderingModifiers.StarterModifier.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.DessertModifier.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.SidesModifier.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.ChangeMindModifier.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.RepeatCourseModifier.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.GroupOrdersSame.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.SidesOptional.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.IsOnlyFlatFee.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.BonusPerDelivery.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.ConsumableReuseChance.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.MessFactor.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.PreventMess.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.MinimumShare.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.PriceModifier.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.FlatFee.GetHashCode();
				hash = hash * 31 + obj.OrderingModifiers.SeatWithoutClear.GetHashCode();


				hash = hash * 31 + obj.DecorationModifiers.Exclusive.GetHashCode();
				hash = hash * 31 + obj.DecorationModifiers.Affordable.GetHashCode();
				hash = hash * 31 + obj.DecorationModifiers.Charming.GetHashCode();
				hash = hash * 31 + obj.DecorationModifiers.Formal.GetHashCode();
				hash = hash * 31 + obj.DecorationModifiers.Kitchen.GetHashCode();

				hash = hash * 31 + obj.Attractiveness.GetHashCode();
				return hash;
			}
		}

		public class CQueueModifierComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CQueueModifier)x, (CQueueModifier)y);
			}

			public static bool IsEqual(CQueueModifier x, CQueueModifier y)
			{
				return GetHashCode(x).Equals(GetHashCode(y));
			}

			public static int GetHashCode(CQueueModifier obj)
			{
				return obj.PatienceFactor.Value.GetHashCode();
			}
		}
	}

	internal static class EffectConditionComparer
	{
		public static class CEffectAlwaysComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CEffectAlways)x, (CEffectAlways)y);
			}

			public static bool IsEqual(CEffectAlways x, CEffectAlways y)
			{
				return GetHashCode(x).Equals(GetHashCode(y));
			}

			public static int GetHashCode(CEffectAlways obj)
			{
				int hash = 17;
				return hash;
			}
		}

		public static class CEffectWhileBeingUsedComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CEffectWhileBeingUsed)x, (CEffectWhileBeingUsed)y);
			}

			public static bool IsEqual(CEffectWhileBeingUsed x, CEffectWhileBeingUsed y)
			{
				return GetHashCode(x).Equals(GetHashCode(y));
			}

			public static int GetHashCode(CEffectWhileBeingUsed obj)
			{
				int hash = 17;
				return hash;
			}
		}

		public static class CEffectAtNightComparer
		{
			public static bool CompareEqual(object x, object y)
			{
				return IsEqual((CEffectAtNight)x, (CEffectAtNight)y);
			}

			public static bool IsEqual(CEffectAtNight x, CEffectAtNight y)
			{
				return GetHashCode(x).Equals(GetHashCode(y));
			}

			public static int GetHashCode(CEffectAtNight obj)
			{
				return obj.DaytimeOnly.GetHashCode();
			}
		}
	}
}
