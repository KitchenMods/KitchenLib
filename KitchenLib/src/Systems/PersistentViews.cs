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
		/*
		 * As of 23/JUNE/2023
		 * KitchenLib has removed it's secret "Fun Menu" although these views are still here in an attempt to keep player's save files safe.
		 * The "Fun Menu" is nolonger in KitchenLib, and the functionality is no longer present.
		 */
		protected override void Initialise()
		{
			commandViews = GetEntityQuery(typeof(CCommandView));
			infoViews = GetEntityQuery(typeof(CInfoView));
			sendToClientViews = GetEntityQuery(typeof(CSendToClientView));
			tileHightlighterView = GetEntityQuery(typeof(CTileHightlighterView));
			clientEquipCapeView = GetEntityQuery(typeof(CClientEquipCapeView));
			syncModsView = GetEntityQuery(typeof(CSyncModsView));
		}
		protected override void OnUpdate()
		{
			EnsureView(commandViews.ToEntityArray(Allocator.Temp), Main.CommandViewHolder.ID, typeof(CCommandView));
			EnsureView(infoViews.ToEntityArray(Allocator.Temp), Main.InfoViewHolder.ID, typeof(CInfoView));
			EnsureView(sendToClientViews.ToEntityArray(Allocator.Temp), Main.SendToClientViewHolder.ID, typeof(CSendToClientView));
			EnsureView(tileHightlighterView.ToEntityArray(Allocator.Temp), Main.TileHighlighterViewHolder.ID, typeof(CTileHightlighterView));
			EnsureView(clientEquipCapeView.ToEntityArray(Allocator.Temp), Main.ClientEquipCapeViewHolder.ID, typeof(CClientEquipCapeView));
			EnsureView(syncModsView.ToEntityArray(Allocator.Temp), Main.SyncModsViewHolder.ID, typeof(CSyncModsView));
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
		private EntityQuery tileHightlighterView;
		private EntityQuery clientEquipCapeView;
		private EntityQuery syncModsView;
	}
	[Obsolete("No longer used")]
	public struct CViewHolder : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CCommandView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CInfoView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CSendToClientView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CTileHightlighterView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CClientEquipCapeView : IApplianceProperty, IAttachableProperty, IComponentData { }
	public struct CSyncModsView : IApplianceProperty, IAttachableProperty, IComponentData { }
}