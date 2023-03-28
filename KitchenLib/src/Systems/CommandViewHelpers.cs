using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Systems;
using KitchenLib.Utils;
using KitchenLib.Views;
using KitchenMods;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class CommandViewHelpers : GameSystemBase, IModSystem
	{
		protected override void OnUpdate() { } // This is just here for the GameSystemBase

		public bool GetItemFromHolder(Entity entity, out Entity item, out CItem cItem)
		{
			if (Require(entity, out CItemHolder cItemHolder))
			{
				item = cItemHolder.HeldItem;
				Require(item, out cItem);
				return true;
			}

			cItem = new CItem();
			item = Entity.Null;
			return false;
		}

		public bool TryRunProcessOnItem(Entity holder, Process process)
		{
			if (GetItemFromHolder(holder, out Entity item, out CItem cItem))
			{
				Item newItem = GameData.Main.Get<Item>(cItem.ID);
				int id = GDOUtils.GetItemProcessResult(newItem, process);
				if (GameData.Main.Get<Item>(id) != null)
				{
					cItem.ID = id;
					EntityManager.SetComponentData(item, cItem);
					return true;
				}
			}
			return false;
		}

		public bool ToggleFire(Entity entity)
		{
			if (Has<CAppliance>(entity))
			{
				if (Has<CIsOnFire>(entity))
				{
					EntityManager.RemoveComponent<CIsOnFire>(entity);
					return true;
				}
				else
				{
					EntityManager.AddComponent<CIsOnFire>(entity);
					return true;
				}
			}
			return false;
		}

		public void AddCustomers(int amount, bool isCat)
		{
			GenerateCustomers.isCat = isCat;
			GenerateCustomers.AddCustomer = amount;
		}

		public void SpawnMess(Vector3 location, bool isKitchenMess, int level)
		{
			Entity entity = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition));
			if (isKitchenMess)
			{
				switch (level)
				{
					case 1:
						EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessKitchen1 });
						break;
					case 2:
						EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessKitchen2 });
						break;
					case 3:
						EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessKitchen3 });
						break;
				}
			}
			else
			{
				switch (level)
				{
					case 1:
						EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessCustomer1 });
						break;
					case 2:
						EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessCustomer2 });
						break;
					case 3:
						EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceReferences.MessCustomer3 });
						break;
				}
			}
			EntityManager.SetComponentData(entity, new CPosition(location));
		}

		public bool AssignDecorationValues(Entity entity, int exclusive, int affordable, int charming, int formal, int kitchen)
		{
			if (!Has<CAppliance>(entity))
				return false;
			if (!Has<CGivesDecoration>(entity))
				EntityManager.AddComponent<CGivesDecoration>(entity);
			CGivesDecoration cGivesDecoration = new CGivesDecoration();
			cGivesDecoration.DecorationValues = new DecorationValues
			{
				Exclusive = exclusive,
				Affordable = affordable,
				Charming = charming,
				Formal = formal,
				Kitchen = kitchen,
			};
			EntityManager.SetComponentData(entity, cGivesDecoration);
			return true;
		}

		public bool AssignPlayerCosmetics(Entity entity, int outfit, int hat)
		{
			if (Require(entity, out CPlayerCosmetics cosmetics))
			{
				cosmetics.Set(CosmeticType.Outfit, outfit);
				cosmetics.Set(CosmeticType.Hat, hat);
				EntityManager.SetComponentData(entity, cosmetics);
				return true;
			}
			return false;
		}

		public bool AssignPlayerColor(Entity entity, string hex)
		{
			if (Require(entity, out CPlayerColour color))
			{
				ColorUtility.TryParseHtmlString(hex, out color.Color);
				EntityManager.SetComponentData(entity, color);
				return true;
			}
			return false;
		}
		public void ToggleBlindness()
		{
			SendToClientView.UpdateView.isDark = !SendToClientView.UpdateView.isDark;
		}

		public void FillGarbage(Entity entity) 
		{
			if (Require(entity, out CApplianceBin bin))
			{
				bin.CurrentAmount++;
				if (bin.CurrentAmount > bin.Capacity)
					bin.CurrentAmount = 0;
				EntityManager.SetComponentData(entity, bin);
			}
		}

		public void AddToItemProvider(Entity entity)
		{
			if (Require(entity, out CItemProvider provider))
			{
				if (provider.Maximum == 0)
					return;
				if ((provider.Available + 1) > provider.Maximum)
					provider.Available = 0;
				else
					provider.Available += 1;

				EntityManager.SetComponentData(entity, provider);
			}
		}
	}
}
