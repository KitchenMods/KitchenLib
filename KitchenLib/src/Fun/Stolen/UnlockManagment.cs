using Kitchen;
using Kitchen.ShopBuilder;
using KitchenData;
using KitchenLib.Utils;
using KitchenMods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Fun
{
	public struct CUnlockModifications : IComponentData
	{
		public int UnlockID;
		public bool Remove;
	}
	[UpdateBefore(typeof(CreateCustomerSchedule))]
	public class UnlockManagment : GameSystemBase, IModSystem
	{
		private MethodInfo EntityManagerGetComponentData;

		private EntityQuery cUnlockModifications;
		private EntityQuery CurrentUnlocks;

		private EntityQuery MenuItems;
		private EntityQuery AvailableIngredients;
		private EntityQuery BlockedIngredients;
		private EntityQuery PossibleExtras;

		EntityQuery GlobalEffects;
		EntityQuery BonusStaples;
		EntityQuery RemoveBlueprints;
		EntityQuery ShopDiscounts;
		EntityQuery Duplicators;
		EntityQuery RandomiseShopPrices;
		EntityQuery UpgradedShops;
		EntityQuery Refreshes;
		EntityQuery Rebuyables;
		EntityQuery CustomerTypes;
		EntityQuery SpawnModifiers;
		protected override void Initialise()
		{
			EntityManagerGetComponentData = typeof(EntityManager).GetMethod("GetComponentData", BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(Entity) }, null);

			cUnlockModifications = GetEntityQuery(typeof(CUnlockModifications));
			CurrentUnlocks = GetEntityQuery(typeof(CProgressionUnlock));

			MenuItems = GetEntityQuery(typeof(CMenuItem));
			AvailableIngredients = GetEntityQuery(typeof(CAvailableIngredient));
			BlockedIngredients = GetEntityQuery(typeof(CBlockedIngredient));
			PossibleExtras = GetEntityQuery(typeof(CPossibleExtra));

			GlobalEffects = GetEntityQuery(typeof(CAppliesEffect), typeof(CEffectRangeGlobal));
			BonusStaples = GetEntityQuery(typeof(CShopStaple));
			RemoveBlueprints = GetEntityQuery(typeof(CRemovesShopBlueprint));
			ShopDiscounts = GetEntityQuery(typeof(CGrantsShopDiscount));
			Duplicators = GetEntityQuery(typeof(CBlueprintGrantDuplicator));
			RandomiseShopPrices = GetEntityQuery(typeof(CRandomiseShopPrices));
			UpgradedShops = GetEntityQuery(typeof(CUpgradedShopChance));
			Refreshes = GetEntityQuery(typeof(CBlueprintRefreshChance));
			Rebuyables = GetEntityQuery(typeof(CBlueprintRebuyableChance));
			CustomerTypes = GetEntityQuery(typeof(CCustomerType), typeof(CCustomerSpawnDefinition));
			SpawnModifiers = GetEntityQuery(typeof(CCustomerSpawnModifier));
		}

		protected override void OnUpdate()
		{
			int unlocksRemoved = 0;
			NativeArray<Entity> unlockModifications = cUnlockModifications.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> currentUnlocks = CurrentUnlocks.ToEntityArray(Allocator.Temp);
			foreach (Entity e in unlockModifications)
			{
				if (Require(e, out CUnlockModifications modifier))
				{
					if (!modifier.Remove)
					{
						AddUnlock(e, modifier.UnlockID);
					}
					else
					{
						RemoveUnlock(e, modifier.UnlockID);
						for (int i = 0; i < currentUnlocks.Length; i++)
						{
							if (Require(currentUnlocks[i], out CProgressionUnlock unlock))
							{
								if (unlock.ID == modifier.UnlockID)
								{
									EntityManager.DestroyEntity(currentUnlocks[i]);
									unlocksRemoved++;
								}
							}
						}
					}
				}
			}

			unlockModifications.Dispose();
			currentUnlocks.Dispose();
			
			if (unlocksRemoved > 0)
				RestoreSKitchenParameters();
		}

		public void RestoreSKitchenParameters()
		{
			if (!HasSingleton<SKitchenParameters>())
				return;
			
			SKitchenParameters defaults = SKitchenParameters.Defaults;
			SetSingleton(defaults);
			NativeArray<Entity> unlocks = CurrentUnlocks.ToEntityArray(Allocator.Temp);

			for (int i = 0; i < unlocks.Length; i++)
			{
				SKitchenParameters newParams = GetSingleton<SKitchenParameters>();
				if (Require(unlocks[i], out CProgressionUnlock cProgressionUnlock))
				{
					if (GameData.Main.TryGet<Unlock>(cProgressionUnlock.ID, out Unlock unlock2))
					{
						newParams.Parameters.CustomersPerHourReduction = newParams.Parameters.CustomersPerHourReduction + unlock2.CustomerMultiplier.Value();
						if (unlock2.GetType().Equals(typeof(UnlockCard)))
						{
							Params(unlock2.ID);
						}
					}
				}
				SetSingleton(newParams);
			}
		}

		private void Params(int unlockID)
		{
			if (GameData.Main.TryGet(unlockID, out UnlockCard card))
			{
				ParameterEffect effect = card.Effects[0] as ParameterEffect;
				if (effect != null)
				{
					SKitchenParameters parameters = GetSingleton<SKitchenParameters>();
					parameters.Parameters.Add(effect.Parameters);
					SetSingleton(parameters);
				}
			}
		}

		private void RemoveUnlock(Entity e, int UnlockID)
		{
			EntityManager.DestroyEntity(e);
			if (GameData.Main.TryGet(UnlockID, out Unlock unlock))
			{
				if (unlock.GetType().Equals(typeof(Dish)))
				{
					RemoveDishUnlock((Dish)unlock);
				}
				else if (unlock.GetType().Equals(typeof(ThemeUnlock)))
				{
					RemoveTheme(((ThemeUnlock)unlock).Type);
				}
				else if (unlock.GetType().Equals(typeof(UnlockCard)))
				{
					RemoveUnlockEffects((UnlockCard)unlock);
				}
			}
		}

		private void AddUnlock(Entity e, int UnlockID)
		{
			EntityManager.DestroyEntity(e);
		}

		private void RemoveUnlockEffects(UnlockCard unlock)
		{
			foreach (UnlockEffect unlockEffect in unlock.Effects)
			{
				if (!(unlockEffect is ParameterEffect))
				{
					if (!(unlockEffect is GlobalEffect))
					{
						if (!(unlockEffect is StatusEffect statusEffect))
						{
							if (!(unlockEffect is ThemeAddEffect themeAddEffect))
							{
								if (!(unlockEffect is ShopEffect effect))
								{
									if (!(unlockEffect is StartBonusEffect effect2))
									{
										if (!(unlockEffect is EnableGroupEffect enableGroupEffect))
										{
											if (unlockEffect is CustomerSpawnEffect customerSpawnEffect)
											{
												UndoCustomerSpawnEffect(customerSpawnEffect);
											}
										}
										else
										{
											UndoEnableGroupEffect(enableGroupEffect);
										}
									}
									else
									{
										UndoStartBonusEffect(effect2);
									}
								}
								else
								{
									UndoShopEffect(effect);
								}
							}
							else
							{
								RemoveTheme(themeAddEffect.AddsTheme);
							}
						}
						else
						{
							RemoveStatus(statusEffect.Status);
						}
					}
					else
					{
						UndoGlobalEffect((GlobalEffect)unlockEffect);
					}
				}
				else
				{
					UndoParameterEffect((ParameterEffect)unlockEffect);
				}
			}
		}

		#region Helpers

		private bool Require(Entity e, Type type, out object comp)
		{
			comp = null;
			if (!EntityManager.HasComponent(e, type))
			{
				return false;
			}
			MethodInfo getComponentDataGeneric = EntityManagerGetComponentData.MakeGenericMethod(type);
			comp = getComponentDataGeneric.Invoke(EntityManager, new object[] { e });
			return true;
		}

		private bool IsEqualCondition(object obj, object check)
		{
			if (obj.GetType() != check.GetType())
			{
				return false;
			}

			if (obj.GetType() == typeof(CEffectAlways))
				return EffectConditionComparer.CEffectAlwaysComparer.CompareEqual(obj, check);
			if (obj.GetType() == typeof(CEffectWhileBeingUsed))
				return EffectConditionComparer.CEffectWhileBeingUsedComparer.CompareEqual(obj, check);
			if (obj.GetType() == typeof(CEffectAtNight))
				return EffectConditionComparer.CEffectAtNightComparer.CompareEqual(obj, check);

			return false;
		}

		private bool IsEqualType(object obj, object check)
		{
			if (obj.GetType() != check.GetType())
			{
				return false;
			}

			if (obj.GetType() == typeof(CCabinetModifier))
				return EffectTypeComparer.CCabinetModifierComparer.CompareEqual(obj, check);
			if (obj.GetType() == typeof(CApplianceSpeedModifier))
				return EffectTypeComparer.CApplianceSpeedModifierComparer.CompareEqual(obj, check);
			if (obj.GetType() == typeof(CAppliesStatus))
				return EffectTypeComparer.CAppliesStatusComparer.CompareEqual(obj, check);
			if (obj.GetType() == typeof(CTableModifier))
				return EffectTypeComparer.CTableModifierComparer.CompareEqual(obj, check);
			if (obj.GetType() == typeof(CQueueModifier))
				return EffectTypeComparer.CQueueModifierComparer.CompareEqual(obj, check);

			return false;
		}
		#endregion

		#region DishRemover
		private void RemoveDishUnlock(Dish dish)
		{
			if (dish.UnlocksMenuItems != null)
			{
				foreach (Dish.MenuItem unlockMenuItem in dish.UnlocksMenuItems)
				{
					UndoUnlocksMenuItems(unlockMenuItem);
				}
			}

			if (dish.BlockProviders != null)
			{
				foreach (Item blockedItem in dish.BlockProviders)
				{
					UndoBlockedIngredient(blockedItem.ID);
				}
			}

			if (dish.UnlocksIngredients != null)
			{
				foreach (Dish.IngredientUnlock unlockIngredient in dish.UnlocksIngredients)
				{
					UndoAvailableIngredient(unlockIngredient.MenuItem.ID, unlockIngredient.Ingredient.ID);
				}
			}

			if (dish.ExtraOrderUnlocks != null)
			{
				foreach (Dish.IngredientUnlock extraOrderUnlock in dish.ExtraOrderUnlocks)
				{
					UndoPossibleExtra(extraOrderUnlock.MenuItem.ID, extraOrderUnlock.Ingredient.ID);
				}
			}
		}

		private bool UndoUnlocksMenuItems(Dish.MenuItem unlockMenuItem)
		{
			NativeArray<Entity> menuItems = MenuItems.ToEntityArray(Allocator.Temp);

			for (int i = 0; i < menuItems.Length; i++)
			{
				if (Require(menuItems[i], out CMenuItem cMenuItem))
				{
					if (cMenuItem.Item == unlockMenuItem.Item.ID)
					{
						EntityManager.DestroyEntity(menuItems[i]);
						menuItems.Dispose();
						return true;
					}
				}
			}
			menuItems.Dispose();
			return false;
		}

		private bool UndoAvailableIngredient(int unlockMenuItemID, int ingredient)
		{
			NativeArray<Entity> availableIngredients = AvailableIngredients.ToEntityArray(Allocator.Temp);

			for (int i = 0; i < availableIngredients.Length; i++)
			{
				if (Require(availableIngredients[i], out CAvailableIngredient cAvailableIngredient))
				{
					if (cAvailableIngredient.MenuItem == unlockMenuItemID && cAvailableIngredient.Ingredient == ingredient)
					{
						EntityManager.DestroyEntity(availableIngredients[i]);
						availableIngredients.Dispose();
						return true;
					}
				}
			}
			availableIngredients.Dispose();
			return false;
		}

		private bool UndoBlockedIngredient(int blockedIngredientID)
		{
			NativeArray<Entity> blockedIngredients = BlockedIngredients.ToEntityArray(Allocator.Temp);

			for (int i = 0; i < blockedIngredients.Length; i++)
			{
				if (Require(blockedIngredients[i], out CBlockedIngredient cBlockedIngredient))
				{
					if (cBlockedIngredient.Item == blockedIngredientID)
					{
						EntityManager.DestroyEntity(blockedIngredients[i]);
						blockedIngredients.Dispose();
						return true;
					}
				}
			}
			blockedIngredients.Dispose();
			return false;
		}

		private bool UndoPossibleExtra(int unlockMenuItemID, int ingredient)
		{
			NativeArray<Entity> possibleExtras = PossibleExtras.ToEntityArray(Allocator.Temp);

			for (int i = 0; i < possibleExtras.Length; i++)
			{
				if (Require(possibleExtras[i], out CPossibleExtra cPossibleExtra))
				{
					if (cPossibleExtra.MenuItem == unlockMenuItemID && cPossibleExtra.Ingredient == ingredient)
					{
						EntityManager.DestroyEntity(possibleExtras[i]);
						possibleExtras.Dispose();
						return true;
					}
				}
			}
			possibleExtras.Dispose();
			return false;
		}
		#endregion

		#region ThemeRemover
		//Theme Remover by IcedMilo
		private bool RemoveTheme(DecorationType theme)
		{
			SGlobalStatusList orCreate = GetOrCreate<SGlobalStatusList>();
			if (!orCreate.Theme.HasFlagFast(theme))
				return false;
			orCreate.Theme ^= theme;
			SetSingleton(orCreate);
			return true;
		}
		#endregion

		#region EffectRemovers
		private bool UndoCustomerSpawnEffect(CustomerSpawnEffect effect)
		{
			NativeArray<Entity> SpawnModifierEntities = SpawnModifiers.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < SpawnModifierEntities.Length; i++)
			{
				if (Require(SpawnModifierEntities[i], out CCustomerSpawnModifier modifier) &&
					modifier.BaseCustomerMultiplier.Value == effect.Base.Value &&
					modifier.PerDayCustomerMultiplier.Value == effect.PerDay.Value)
				{
					EntityManager.DestroyEntity(SpawnModifierEntities[i]);
					break;
				}
			}
			SpawnModifierEntities.Dispose();
			return true;
		}

		private bool UndoEnableGroupEffect(EnableGroupEffect effect)
		{
			NativeArray<Entity> CustomerTypeEntities = CustomerTypes.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < CustomerTypeEntities.Length; i++)
			{
				if (Require(CustomerTypeEntities[i], out CCustomerType customerType) &&
					customerType.Type == effect.EnableType.ID &&
					Require(CustomerTypeEntities[i], out CCustomerSpawnDefinition spawnDefinition) &&
					spawnDefinition.Probability == effect.Probability)
				{
					EntityManager.DestroyEntity(CustomerTypeEntities[i]);
					break;
				}
			}
			CustomerTypeEntities.Dispose();
			return true;
		}

		private bool UndoStartBonusEffect(StartBonusEffect effect)
		{
			return true;
		}

		private bool UndoShopEffect(ShopEffect effect)
		{
			NativeArray<Entity> BonusStapleEntities = BonusStaples.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> DuplicatorEntities = Duplicators.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> RemoveBlueprintEntities = RemoveBlueprints.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> ShopDiscountEntities = ShopDiscounts.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> RandomiseShopPricesEntities = RandomiseShopPrices.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> UpgradedShopEntities = UpgradedShops.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> RefreshEntities = Refreshes.ToEntityArray(Allocator.Temp);
			NativeArray<Entity> RebuyableEntities = Rebuyables.ToEntityArray(Allocator.Temp);

			if (effect.AddStaple != null)
			{
				for (int i = 0; i < BonusStapleEntities.Length; i++)
				{
					if (Require(BonusStapleEntities[i], out CShopStaple staples))
					{
						if (staples.Appliance == effect.AddStaple.ID)
						{
							EntityManager.DestroyEntity(BonusStapleEntities[i]);
							break;
						}
					}
				}
			}

			if (effect.BlueprintDeskAddition != 0)
			{
				int count = effect.BlueprintDeskAddition;
				for (int i = 0; i < DuplicatorEntities.Length; i++)
				{
					if (Require(DuplicatorEntities[i], out CBlueprintGrantDuplicator duplicator))
					{
						if (count > 0)
						{
							EntityManager.DestroyEntity(DuplicatorEntities[i]);
							count--;
						}
						else
							break;
					}
				}
			}

			if (effect.ExtraShopBlueprints != 0)
			{
				for (int i = 0; i < RemoveBlueprintEntities.Length; i++)
				{
					if (Require(RemoveBlueprintEntities[i], out CRemovesShopBlueprint cRemoves))
					{
						if (cRemoves.Count == -effect.ExtraShopBlueprints)
						{
							EntityManager.DestroyEntity(RemoveBlueprintEntities[i]);
							break;
						}
					}
				}
			}

			if (effect.ShopCostDecrease != 0)
			{
				for (int i = 0; i < ShopDiscountEntities.Length; i++)
				{
					if (Require(ShopDiscountEntities[i], out CGrantsShopDiscount cDiscount))
					{
						if (cDiscount.Amount == effect.ShopCostDecrease)
						{
							EntityManager.DestroyEntity(ShopDiscountEntities[i]);
							break;
						}
					}
				}
			}

			if (effect.RandomiseShopPrices)
			{
				for (int i = 0; i < RandomiseShopPricesEntities.Length; i++)
				{
					EntityManager.DestroyEntity(RandomiseShopPricesEntities[i]);
					break;
				}
			}

			if (effect.ExtraStartingBlueprints != 0)
			{
				// Not Implemented
			}

			if (effect.BlueprintRebuyableChance != 0)
			{
				for (int i = 0; i < RebuyableEntities.Length; i++)
				{
					if (Require(RebuyableEntities[i], out CBlueprintRebuyableChance chance))
					{
						if (chance.Chance == effect.BlueprintRefreshChance)
						{
							EntityManager.DestroyEntity(RebuyableEntities[i]);
							break;
						}
					}
				}
			}

			if (effect.BlueprintRefreshChance != 0)
			{
				for (int i = 0; i < RefreshEntities.Length; i++)
				{
					if (Require(RefreshEntities[i], out CBlueprintRefreshChance chance))
					{
						if (chance.Chance == effect.BlueprintRefreshChance)
						{
							EntityManager.DestroyEntity(RefreshEntities[i]);
							break;
						}
					}
				}
			}

			if (effect.UpgradedShopChance != 0)
			{
				for (int i = 0; i < UpgradedShopEntities.Length; i++)
				{
					if (Require(UpgradedShopEntities[i], out CUpgradedShopChance chance))
					{
						if (chance.Chance == effect.UpgradedShopChance)
						{
							EntityManager.DestroyEntity(UpgradedShopEntities[i]);
							break;
						}
					}
				}
			}

			BonusStapleEntities.Dispose();
			DuplicatorEntities.Dispose();
			RemoveBlueprintEntities.Dispose();
			ShopDiscountEntities.Dispose();
			RandomiseShopPricesEntities.Dispose();
			UpgradedShopEntities.Dispose();
			RefreshEntities.Dispose();
			RebuyableEntities.Dispose();

			return true;
		}

		private bool UndoGlobalEffect(GlobalEffect effect)
		{
			NativeArray<Entity> GlobalEffectEntities = GlobalEffects.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < GlobalEffectEntities.Length; i++)
			{
				if (Require(GlobalEffectEntities[i], effect.EffectCondition.GetType(), out var conditionComp) &&
					IsEqualCondition(conditionComp, effect.EffectCondition) &&
					Require(GlobalEffectEntities[i], effect.EffectType.GetType(), out var typeComp) &&
					IsEqualType(typeComp, effect.EffectType))
				{
					EntityManager.DestroyEntity(GlobalEffectEntities[i]);
					break;
				}
			}
			return true;
		}

		private bool UndoParameterEffect(ParameterEffect effect)
		{
			//RestoreSKitchenParameters();

			return true;
		}
		#endregion
	}
}