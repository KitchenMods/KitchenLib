using Kitchen;
using KitchenData;
using KitchenLib.References;
using KitchenLib.Systems;
using KitchenLib.UI;
using KitchenLib.Utils;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Fun
{
	internal class CommandViewHelpers : GameSystemBase
	{

		public void test()
		{
			GenericPopupManager.CreatePopup(
						"Some mods are out of date!",
						$"Hello You're Gey",
						GenericChoiceType.OnlyAccept,
						null,
						null,
						TMPro.TextAlignmentOptions.Center,
						TMPro.TextAlignmentOptions.Center
					);
		}

		private Dictionary<int, Entity> currentPlayers = new Dictionary<int, Entity>();
		private EntityQuery playerQuery;
		private EntityQuery unlockQuery;
		private EntityQuery applianceQuery;
		private EntityQuery waiting;
		protected override void Initialise()
		{
			Main = this;
			playerQuery = EntityManager.CreateEntityQuery(typeof(CPlayer));
			unlockQuery = EntityManager.CreateEntityQuery(typeof(CProgressionUnlock));
			applianceQuery = EntityManager.CreateEntityQuery(typeof(CAppliance));
			waiting = EntityManager.CreateEntityQuery(typeof(CWaitingForItem));
		}
		protected override void OnUpdate()
		{
			currentPlayers.Clear();
			using NativeArray<Entity> players = playerQuery.ToEntityArray(Allocator.Temp);
			foreach (Entity player in players)
			{
				if (Require(player, out CPlayer cPlayer))
				{
					if (Has<CPlayerCosmetics>(player))
						currentPlayers.Add(cPlayer.ID, player);
				}
			}
		}
		public static CommandViewHelpers Main;

		public bool GetItemFromHolder(Vector3 location, out Entity item, out CItem cItem)
		{
			if (Require(GetOccupant(location), out CItemHolder cItemHolder))
			{
				item = cItemHolder.HeldItem;
				Require(item, out cItem);
				return true;
			}

			cItem = new CItem();
			item = Entity.Null;
			return false;
		}

		public bool TryRunProcessOnItem(Vector3 location, Process process)
		{
			if (process == null)
				return false;
			if (GetItemFromHolder(location, out Entity item, out CItem cItem))
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

		public void ToggleFireOnLocation(Vector3 location)
		{
			Entity e = GetOccupant(location, TileHighlighter.UpdateView.SelectedLayer);
			if (e == null)
				return;
			if (!Has<CIsOnFire>(e))
				EntityManager.AddComponent<CIsOnFire>(e);
			else
				EntityManager.RemoveComponent<CIsOnFire>(e);
		}

		public void SetPlayerCosmetic(CosmeticType type, int player, int outfit)
		{
			if (!currentPlayers.ContainsKey(player))
				return;
			Entity e = currentPlayers[player];
			if (e == null)
				return;
			if (Require(e, out CPlayerCosmetics cosmetics))
			{
				cosmetics.Set(type, outfit);
				EntityManager.SetComponentData(e, cosmetics);
			}
		}

		public void SetPlayerColor(int player, Color color)
		{
			if (!currentPlayers.ContainsKey(player))
				return;
			Entity e = currentPlayers[player];
			if (e == null)
				return;
			if (Require(e, out CPlayerColour cPlayerColour))
			{
				cPlayerColour.Color = color;
				EntityManager.SetComponentData(e, cPlayerColour);
			}
		}

		public void SetPlayerSpeedMultiplier(int player, float speed)
		{
			PlayerSpeedOverride.SetPlayerSpeedMultiplier(player, speed);
		}

		public void SetThemeLevel(Vector3 location, int exclusive, int affordable, int charming, int formal, int kitchen)
		{
			Entity e = GetOccupant(location);
			if (e == null)
				return;
			if (!Has<CAppliance>(e))
				return;
			
			if (!Has<CGivesDecoration>(e))
				EntityManager.AddComponent<CGivesDecoration>(e);
			CGivesDecoration cGivesDecoration = new CGivesDecoration();
			cGivesDecoration.DecorationValues = new DecorationValues
			{
				Exclusive = exclusive,
				Affordable = affordable,
				Charming = charming,
				Formal = formal,
				Kitchen = kitchen,
			};
			EntityManager.SetComponentData(e, cGivesDecoration);
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

		public void ToggleBlindness()
		{
			RefVars.IsBlind = !RefVars.IsBlind;
		}
		
		public void AdjustBin(Vector3 location, bool isUp, bool isDown)
		{
			Entity e = GetOccupant(location);
			if (e == null)
				return;
			if (!Has<CAppliance>(e))
				return;

			int moveCount = 0;

			if (isUp)
				moveCount = 1;
			if (isDown)
				moveCount = -1;

			if (Require(e, out CApplianceBin bin))
			{
				if ((bin.CurrentAmount + moveCount) > bin.Capacity)
				{
					bin.CurrentAmount = 0;
				}
				else if ((bin.CurrentAmount + moveCount) < 0)
				{
					bin.CurrentAmount = bin.Capacity;
				}
				else
					bin.CurrentAmount += moveCount;

				EntityManager.SetComponentData(e, bin);
			}
		}

		public void AdjustItemProvider(Vector3 location, bool isUp, bool isDown)
		{
			Entity e = GetOccupant(location);
			if (e == null)
				return;
			if (!Has<CAppliance>(e))
				return;

			int moveCount = 0;

			if (isUp)
				moveCount = 1;
			if (isDown)
				moveCount = -1;

			if (Require(e, out CItemProvider bin))
			{
				if ((bin.Available + moveCount) > bin.Maximum)
				{
					bin.Available = 0;
				}
				else if ((bin.Available + moveCount) < 0)
				{
					bin.Available = bin.Maximum;
				}
				else
					bin.Available += moveCount;

				EntityManager.SetComponentData(e, bin);
			}
		}

		public void ToggleDish(int id)
		{
			var unlocks = unlockQuery.ToEntityArray(Allocator.Temp);
			
			Entity e = Entity.Null;

			foreach (Entity entity in unlocks)
			{
				if (Require(entity, out CProgressionUnlock item))
				{
					if (item.ID == id)
					{
						e = entity;
						break;
					}
				}
			}

			if (e != Entity.Null)
			{
				RemoveUnlock(id);
			}

			if (e == Entity.Null)
			{
				e = EntityManager.CreateEntity(typeof(CProgressionOption));
				EntityManager.SetComponentData(e, new CProgressionOption
				{
					ID = id
				});
				EntityManager.AddComponent<CProgressionOption.Selected>(e);
			}
		}

		private void RemoveUnlock(int UnlockID)
		{
			Entity e = EntityManager.CreateEntity(typeof(CUnlockModifications));
			EntityManager.SetComponentData(e, new CUnlockModifications
			{
				Remove = true,
				UnlockID = UnlockID
			});
		}

		public void BurnEverything()
		{
			using NativeArray<Entity> appliances = applianceQuery.ToEntityArray(Allocator.Temp);
			foreach (Entity appliance in appliances)
			{
				EntityManager.AddComponent<CIsOnFire>(appliance);
			}
		}

		public void UnBurnEverything()
		{
			using NativeArray<Entity> appliances = applianceQuery.ToEntityArray(Allocator.Temp);
			foreach (Entity appliance in appliances)
			{
				EntityManager.RemoveComponent<CIsOnFire>(appliance);
			}
		}

		public void ResetOrder(Vector3 location)
		{
			Entity chair = GetOccupant(location);
			if (Require(chair, out CApplianceChair cApplianceChair))
			{
				Entity occupant = cApplianceChair.Occupant;
				if (Require(occupant, out CBelongsToGroup cBelongsToGroup))
				{
					Entity group = cBelongsToGroup.Group;
					EntityManager.AddComponent<CGroupForceChangedMind>(group);
				}
			}
		}

		public CPlayer getplayer(Entity entity)
		{
			if (Require(entity, out CPlayer player))
			{
				return player;
			}
			return new CPlayer();
		}


		public void SetMoney(int money)
		{
			if (HasSingleton<SMoney>())
			{
				SMoney moneySingleton = GetSingleton<SMoney>();
				moneySingleton.Amount = money;
				SetSingleton(moneySingleton);
			}
		}

		public bool New_TryRunProcessOnItem(Vector3 location, Process process, bool reversed = false)
		{
			if (process == null)
				return false;
			if (GetItemFromHolder(location, out Entity item, out CItem cItem))
			{
				Item newItem = GameData.Main.Get<Item>(cItem.ID);

				int id = 0;
				if (!reversed)
				{
					if (RefVars.ItemProcessResults.ContainsKey((cItem.ID, process)))
					{
						id = RefVars.ItemProcessResults[(cItem.ID, process)];
					}
				}
				else
				{
					if (RefVars.ReversedItemProcessResults.ContainsKey((cItem.ID, process)))
					{
						id = RefVars.ReversedItemProcessResults[(cItem.ID, process)];
					}
				}

				if (id == 0)
					return false;

				if (GameData.Main.Get<Item>(id) != null)
				{
					cItem.ID = id;
					EntityManager.SetComponentData(item, cItem);
					return true;
				}
			}
			return false;
		}

		public void DestroyAppliance(Vector3 location)
		{
			Entity e = GetOccupant(location, TileHighlighter.UpdateView.SelectedLayer);
			if (e == null)
				return;
			if (Has<CAppliance>(e))
				EntityManager.DestroyEntity(e);
		}

		public void ChangeOrder(Vector3 location, int itemID)
		{
			Entity chair = GetOccupant(location);
			if (Require(chair, out CApplianceChair cApplianceChair))
			{
				Entity customer = cApplianceChair.Occupant;
				if (EntityUtils.Main.GetGroupFromCustomer(customer, out Entity group, out int index))
				{
					EntityUtils.Main.AddItemToOrder(group, index, itemID);
				}
			}
		}
	}
}
