using Kitchen;
using KitchenData;
using KitchenMods;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace KitchenLib.Systems
{
	public class PersistentViews : RestaurantSystem, IModSystem
	{
		protected override void Initialise()
		{
			commandViews = GetEntityQuery(typeof(CCommandView));
			infoViews = GetEntityQuery(typeof(CInfoView));
			sendToClientViews = GetEntityQuery(typeof(CSendToClientView));
		}
		protected override void OnUpdate()
		{
			EnsureView(commandViews.ToEntityArray(Allocator.Temp), Main.CommandViewHolder.ID, typeof(CCommandView));
			EnsureView(infoViews.ToEntityArray(Allocator.Temp), Main.InfoViewHolder.ID, typeof(CInfoView));
			EnsureView(sendToClientViews.ToEntityArray(Allocator.Temp), Main.SendToClientViewHolder.ID, typeof(CSendToClientView));
		}

		private void EnsureView(NativeArray<Entity> entities, int ApplianceID, Type marker)
		{
			using (entities)
			{
				if (entities.Length < 1)
				{
					Entity entity = EntityManager.CreateEntity(typeof(CCreateAppliance), typeof(CPosition), typeof(CDoNotPersist), marker);
					EntityManager.SetComponentData(entity, new CCreateAppliance { ID = ApplianceID });
					EntityManager.SetComponentData(entity, new CPosition(new Vector3(75, 0, 75)));
				}
				else
				{
					for (int i = 1; i < entities.Length; i++)
					{
						EntityManager.DestroyEntity(entities[i]);
					}
				}
			}
		}
		private EntityQuery commandViews;
		private EntityQuery infoViews;
		private EntityQuery sendToClientViews;
	}
	[Obsolete("No longer used")]
	public struct CViewHolder : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CCommandView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CInfoView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CSendToClientView : IApplianceProperty, IAttachableProperty, IComponentData { }
}