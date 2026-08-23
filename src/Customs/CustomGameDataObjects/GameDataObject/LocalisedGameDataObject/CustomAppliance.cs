using System;
using System.Collections.Generic;
using System.Linq;
using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace KitchenLib.Customs
{
	public abstract class CustomAppliance : CustomLocalisedGameDataObject<Appliance, ApplianceInfo>, ICustomHasPrefab
	{

		#region Base Game Variables

		public virtual GameObject Prefab { get; protected set; }
		public virtual GameObject HeldAppliancePrefab { get; protected set; }
		public virtual List<Appliance.ApplianceProcesses> Processes { get; protected set; } = new();
		public virtual List<IApplianceProperty> Properties { get; protected set; } = new();
		public virtual List<UnlockEffect> AppliesEffects { get; protected set; } = new List<UnlockEffect>();
		public virtual IEffectRange EffectRange { get; protected set; }
		public virtual IEffectCondition EffectCondition { get; protected set; }
		public virtual IEffectType EffectType { get; protected set; }
		public virtual EffectRepresentation EffectRepresentation { get; protected set; }
		public virtual bool IsNonInteractive { get; protected set; }
		public virtual OccupancyLayer Layer { get; protected set; }
		public virtual bool ForceHighInteractionPriority { get; protected set; }
		public virtual int PurchaseCost { get; protected set; } = 1;
		public virtual EntryAnimation EntryAnimation { get; protected set; }
		public virtual ExitAnimation ExitAnimation { get; protected set; }
		public virtual bool SkipRotationAnimation { get; protected set; }
		public virtual bool IsPurchasable { get; protected set; }
		public virtual Season RestrictedToSeason { get; protected set; }
		public virtual bool IsPurchasableAsUpgrade { get; protected set; }
		public virtual DecorationType ThemeRequired { get; protected set; }
		public virtual ShoppingTags ShoppingTags { get; protected set; }
		public virtual RarityTier RarityTier { get; protected set; }
		public virtual PriceTier PriceTier { get; protected set; } = PriceTier.Medium;
		public virtual ShopRequirementFilter ShopRequirementFilter { get; protected set; }
		public virtual List<Appliance> RequiresForShop { get; protected set; } = new();
		public virtual List<Process> RequiresProcessForShop { get; protected set; } = new();
		public virtual List<Item> RequiresIngredientForShop { get; protected set; } = new();
		public virtual List<MenuPhase> RequiresPhaseForShop { get; protected set; } = new();
		public virtual bool StapleWhenMissing { get; protected set; }
		public virtual bool SellOnlyAsDuplicate { get; protected set; }
		public virtual bool SellOnlyAsUnique { get; protected set; }
		public virtual bool SellOnlyIfPlatesNeeded { get; protected set; }
		public virtual bool PreventSale { get; protected set; }
		public virtual List<Appliance> Upgrades { get; protected set; } = new();
		public virtual List<Appliance> Enchantments { get; protected set; } = new();
		public virtual bool IsAnUpgrade { get; protected set; }
		public virtual bool IsNonCrated { get; protected set; }
		public virtual Item CrateItem { get; protected set; }
		public virtual Appliance WeakVariant { get; protected set; }

		#region Obsolete

		[Obsolete("Please set your Name in Info")]
		public virtual string Name { get; protected set; } = "Appliance";

		[Obsolete("Please set your Description in Info")]
		public virtual string Description { get; protected set; } = "A little something for your restaurant";

		[Obsolete("Please set your Sections in Info")]
		public virtual List<Appliance.Section> Sections { get; protected set; } = new();

		[Obsolete("Please set your Tags in Info")]
		public virtual List<string> Tags { get; protected set; } = new();

		#endregion

		#endregion

		#region KitchenLib Variables

		public virtual bool AutoGenerateNavMeshObject { get; protected set; } = true;
		public virtual int PurchaseCostOverride { get; protected set; } = -1;

		#region Obsolete

		[Obsolete("Please create a custom system for rotations")]
		public virtual bool ForceIsRotationPossible()
		{
			return false;
		}

		[Obsolete("Please create a custom system for rotations")]
		public virtual bool IsRotationPossible(InteractionData data)
		{
			return true;
		}

		[Obsolete("Please create a custom system for rotations")]
		public virtual bool PreRotate(InteractionData data, bool isSecondary = false)
		{
			return false;
		}

		[Obsolete("Please create a custom system for rotations")]
		public virtual void PostRotate(InteractionData data)
		{
		}

		[Obsolete("Please create a custom system for interactions")]
		public virtual bool ForceIsInteractionPossible()
		{
			return false;
		}

		[Obsolete("Please create a custom system for interactions")]
		public virtual bool IsInteractionPossible(InteractionData data)
		{
			return true;
		}

		[Obsolete("Please create a custom system for interactions")]
		public virtual bool PreInteract(InteractionData data, bool isSecondary = false)
		{
			return false;
		}

		[Obsolete("Please create a custom system for interactions")]
		public virtual void PostInteract(InteractionData data)
		{
		}

		#endregion

		
		#endregion
		
		[Obsolete("Please use OnRegister")]
		public virtual void SetupPrefab(GameObject prefab) { }

		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			base.Convert(gameData, out gameDataObject);

			if (gameDataObject is Appliance appliance)
			{
				#region Backwards Compatibility

				#region Localisation

				if (InfoList.Count <= 0)
				{
					appliance.Info = new LocalisationObject<ApplianceInfo>();
					if (!appliance.Info.Has(Locale.English))
					{
						var applianceInfo = ScriptableObject.CreateInstance<ApplianceInfo>();
						applianceInfo.Name = Name;
						applianceInfo.Description = Description;
						applianceInfo.Sections = Sections;
						applianceInfo.Tags = Tags;
						appliance.Info.Add(Locale.English, applianceInfo);
					}
				}

				#endregion

				#endregion

				#region Apply Properties

				OverrideVariable(appliance, "Prefab", Prefab);
				OverrideVariable(appliance, "HeldAppliancePrefab", HeldAppliancePrefab);
				OverrideVariable(appliance, "EffectRange", EffectRange);
				OverrideVariable(appliance, "EffectCondition", EffectCondition);
				OverrideVariable(appliance, "EffectType", EffectType);
				OverrideVariable(appliance, "IsNonInteractive", IsNonInteractive);
				OverrideVariable(appliance, "Layer", Layer);
				OverrideVariable(appliance, "ForceHighInteractionPriority", ForceHighInteractionPriority);
				OverrideVariable(appliance, "PurchaseCost", PurchaseCost);
				OverrideVariable(appliance, "EntryAnimation", EntryAnimation);
				OverrideVariable(appliance, "ExitAnimation", ExitAnimation);
				OverrideVariable(appliance, "SkipRotationAnimation", SkipRotationAnimation);
				OverrideVariable(appliance, "IsPurchasable", IsPurchasable);
				OverrideVariable(appliance, "RestrictedToSeason", RestrictedToSeason);
				OverrideVariable(appliance, "IsPurchasableAsUpgrade", IsPurchasableAsUpgrade);
				OverrideVariable(appliance, "ThemeRequired", ThemeRequired);
				OverrideVariable(appliance, "ShoppingTags", ShoppingTags);
				OverrideVariable(appliance, "RarityTier", RarityTier);
				OverrideVariable(appliance, "PriceTier", PriceTier);
				OverrideVariable(appliance, "ShopRequirementFilter", ShopRequirementFilter);
				OverrideVariable(appliance, "RequiresPhaseForShop", RequiresPhaseForShop);
				OverrideVariable(appliance, "StapleWhenMissing", StapleWhenMissing);
				OverrideVariable(appliance, "SellOnlyAsDuplicate", SellOnlyAsDuplicate);
				OverrideVariable(appliance, "SellOnlyAsUnique", SellOnlyAsUnique);
				OverrideVariable(appliance, "SellOnlyIfPlatesNeeded", SellOnlyIfPlatesNeeded);
				OverrideVariable(appliance, "PreventSale", PreventSale);
				OverrideVariable(appliance, "IsAnUpgrade", IsAnUpgrade);
				OverrideVariable(appliance, "IsNonCrated", IsNonCrated);

				#endregion

				// Used to override the purchase price of this Appliance.
				if (PurchaseCostOverride != -1)
				{
					BaseMod.InternalLogger.LogDebug($"Assigning : {PurchaseCostOverride} >> PurchaseCostOverride");
					ApplianceOverrides.AddPurchaseCostOverride(appliance.ID, PurchaseCostOverride);
				}

				// Used to automatically generate a NavMeshObstacle component on this Appliance if not already present.
				if (AutoGenerateNavMeshObject && appliance.Prefab != null)
				{
					BaseMod.InternalLogger.LogDebug("Setting up NavMeshObstacle");
					if (appliance.Prefab.GetComponentsInChildren<NavMeshObstacle>().Length == 0)
					{
						var counter = gameData.Get<Appliance>().FirstOrDefault(a => a.ID == ApplianceReferences.Countertop);
						foreach (Transform t in counter.Prefab.GetComponentInChildren<Transform>())
						{
							if (t.gameObject.HasComponent<NavMeshObstacle>())
							{
								GameObjectUtils.CopyComponent(t.gameObject.GetComponent<NavMeshObstacle>(), appliance.Prefab);
								break;
							}
						}
					}
				}
			}
		}

		public override void AttachDependentProperties(GameData gameData, GameDataObject gameDataObject)
		{
			base.AttachDependentProperties(gameData, gameDataObject);

			if (gameDataObject is Appliance appliance)
			{
				#region Apply Properties

				OverrideVariable(appliance, "Properties", Properties);
				OverrideVariable(appliance, "AppliesEffects", AppliesEffects);
				OverrideVariable(appliance, "Processes", Processes);
				OverrideVariable(appliance, "EffectRepresentation", EffectRepresentation);
				OverrideVariable(appliance, "RequiresForShop", RequiresForShop);
				OverrideVariable(appliance, "RequiresProcessForShop", RequiresProcessForShop);
				OverrideVariable(appliance, "RequiresIngredientForShop", RequiresIngredientForShop);
				OverrideVariable(appliance, "Upgrades", Upgrades);
				OverrideVariable(appliance, "Enchantments", Enchantments);
				OverrideVariable(appliance, "CrateItem", CrateItem);
				OverrideVariable(appliance, "WeakVariant", WeakVariant);

				#endregion
			}
		}

		public override void OnRegister(GameDataObject gameDataObject)
		{
			var gdo = gameDataObject as IHasPrefab;
			if (gdo?.Prefab != null)
			{
				SetupPrefab(gdo.Prefab);
			}

			base.OnRegister(gameDataObject);
		}
	}
}