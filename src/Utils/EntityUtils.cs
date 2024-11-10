using Kitchen;
using KitchenData;
using Unity.Entities;

namespace KitchenLib.Utils
{
	public class EntityUtils : GameSystemBase
	{
		public static EntityUtils Main;
		protected override void Initialise() { Main = this; }
		protected override void OnUpdate() { }
		
		public static EntityManager GetEntityManager()
		{
			return Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<Kitchen.PlayerManager>().EntityManager;
		}

		public bool GetGroupFromCustomer(Entity customer, out Entity group, out int index)
		{
			group = Entity.Null;
			index = 0;
			if (GetEntityManager().HasComponent<CBelongsToGroup>(customer))
			{
				group = GetEntityManager().GetComponentData<CBelongsToGroup>(customer).Group;
				if (HasBuffer<CGroupMember>(group))
				{
					DynamicBuffer<CGroupMember> members = GetBuffer<CGroupMember>(group);
					for (int i = 0; i < members.Length; i++)
					{
						if (members[i].Customer == customer)
						{
							index = i;
							break;
						}
					}
				}
				return true;
			}
			return false;
		}
		public void AddItemToOrder(Entity e, int memberIndex, int itemID)
		{
			if (e == null)
				return;
			if (HasBuffer<CWaitingForItem>(e))
			{
				if (GameData.Main.TryGet<Item>(itemID, out Item item))
				{
					EntityManager.GetBuffer<CWaitingForItem>(e).Add(new CWaitingForItem
					{
						ItemID = itemID,
						Item = CreateItem(itemID),
						Reward = 0,
						MemberIndex = memberIndex,
						IsSide = GameData.Main.Get<Item>(itemID).IsMergeableSide,
						DirtItem = 0,
						Extra = 0,
						SourceMenuItem = 0
					});
				}
			}
		}

		public void RemoveItemFromOrder(Entity e, int memberIndex, int itemID)
		{
			if (e == null)
				return;
			if (HasBuffer<CWaitingForItem>(e))
			{
				DynamicBuffer<CWaitingForItem> cWaitingForItems = EntityManager.GetBuffer<CWaitingForItem>(e);
				for (int i = 0; i < cWaitingForItems.Length; i++)
				{
					if (cWaitingForItems[i].ItemID == itemID && cWaitingForItems[i].MemberIndex == memberIndex)
					{
						cWaitingForItems.RemoveAt(i);
						break;
					}
				}
			}
		}

		public void RemoveItemFromOrder(Entity e, int itemID)
		{
			if (e == null)
				return;
			if (HasBuffer<CWaitingForItem>(e))
			{
				DynamicBuffer<CWaitingForItem> cWaitingForItems = EntityManager.GetBuffer<CWaitingForItem>(e);
				for (int i = 0; i < cWaitingForItems.Length; i++)
				{
					if (cWaitingForItems[i].ItemID == itemID)
					{
						cWaitingForItems.RemoveAt(i);
						break;
					}
				}
			}
		}

		public Entity CreateItem(int id)
		{
			Entity e = EntityManager.CreateEntity();
			if (GameData.Main.TryGet<Item>(id, out Item item))
			{
				EntityManager.AddComponentData(e, new CItem
				{
					ID = id,
					IsPartial = false,
					IsTransient = false,
					IsGroup = false,
					Category = item.ItemCategory,
					Items = new ItemList(id)
				});
				EntityManager.AddComponentData(e, new CRequiresView
				{
					Type = ViewType.Item
				});
			}
			return e;
		}
	}
}
