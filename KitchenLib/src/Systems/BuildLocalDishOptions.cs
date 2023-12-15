using System.Collections.Generic;
using Kitchen;
using KitchenData;
using KitchenLib.Preferences;
using KitchenMods;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class BuildLocalDishOptions : FranchiseFirstFrameSystem, IModSystem
	{
		public static List<int> MenuOptions = new List<int>();

		protected override void Initialise()
		{
			base.Initialise();
		}
		
		private Vector3[] defaultPositions = new Vector3[]
		{
			new Vector3(0, 0, -2),
			new Vector3(-1, 0, -2),
			new Vector3(1, 0, -2),
			new Vector3(-2, 0, -2),
			new Vector3(2, 0, -2),
			new Vector3(0, 0, -3),
			new Vector3(-1, 0, -3),
			new Vector3(1, 0, -3),
			new Vector3(-2, 0, -3),
			new Vector3(2, 0, -3)
		};
			
		private Vector3[] integratedHQPositions = new Vector3[]
		{
			new Vector3(0, 0, -2),
			new Vector3(-1, 0, -2),
			new Vector3(1, 0, -2),
			new Vector3(-2, 0, -2),
			new Vector3(2, 0, -2),
			new Vector3(-4, 0, -2),
			new Vector3(4, 0, -2),
		};
		
		protected override void OnUpdate()
		{
			if (!Main.manager.GetPreference<PreferenceBool>("forceLocalDishes").Value)
				return;
			
			Vector3 office = LobbyPositionAnchors.Office;

			Vector3[] positions;
			
			if (GetOccupant(office + new Vector3(0, 0, -3)) != Entity.Null)
				positions = integratedHQPositions;
			else
				positions = defaultPositions;
			
			for (int i = 0; i < MenuOptions.Count; i++)
			{
				if (positions.Length <= i)
				{
					Main.LogError("Not enough positions for all the dishes!");
					break;
				}

				CreateFoodSource(office + positions[i], GameData.Main.Get<Dish>(MenuOptions[i]));
			}
			
		}
		
		private void CreateFoodSource(Vector3 location, Dish dish)
		{
			Entity entity = EntityManager.CreateEntity(new ComponentType[]
			{
				typeof(CCreateAppliance),
				typeof(CPosition),
				typeof(CDishSource),
				typeof(CItemHolder)
			});
			EntityManager.SetComponentData<CCreateAppliance>(entity, new CCreateAppliance
			{
				ID = AssetReference.DishPedestal
			});
			EntityManager.SetComponentData<CPosition>(entity, new CPosition(location));
			if (dish != null)
			{
				Entity entity2 = EntityManager.CreateEntity(new ComponentType[]
				{
					typeof(CCreateItem),
					typeof(CHeldBy),
					typeof(CHome)
				});
				EntityManager.SetComponentData<CCreateItem>(entity2, new CCreateItem
				{
					ID = AssetReference.DishPaper
				});
				EntityManager.SetComponentData<CHeldBy>(entity2, new CHeldBy
				{
					Holder = entity
				});
				EntityManager.SetComponentData<CItemHolder>(entity, new CItemHolder
				{
					HeldItem = entity2
				});
				EntityManager.SetComponentData<CHome>(entity2, entity);
				EntityManager.AddComponentData<CDishChoice>(entity2, new CDishChoice
				{
					Dish = dish.ID
				});
			}
		}
	}
}