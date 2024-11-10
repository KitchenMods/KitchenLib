using Kitchen;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.IMMS
{
	internal class UpdateIMMSView : ResponsiveViewSystemBase<IMMSView.ViewData, IMMSView.ResponseData>
	{
		private EntityQuery Views;

		protected override void Initialise()
		{
			Views = GetEntityQuery(new QueryHelper().All(
				typeof(CLinkedView),
				typeof(SIMMSManager)
			));
		}

		protected override void OnUpdate()
		{
			using var entities = Views.ToEntityArray(Allocator.Temp);
			using var linkedViews = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);

			for (var i = 0; i < entities.Length; ++i)
			{
				var entity = entities[i];
				var linkedView = linkedViews[i];

				// Send current messages
				SendUpdate(linkedView.Identifier, new IMMSView.ViewData
				{
					Messages = new List<IMMSNetworkMessage>(IMMSManager.ClientboundMessages)
				});

				// Clear out old messages
				var fiveSecondsAgo = DateTime.UtcNow.AddSeconds(-5).Ticks;
				while (IMMSManager.ClientboundMessages.Count > 0 && IMMSManager.ClientboundMessages.Peek().Timestamp < fiveSecondsAgo)
				{
					IMMSManager.ClientboundMessages.Dequeue();
				}

				ApplyUpdates(linkedView, (data) =>
				{
					IMMSManager.HandleIncomingHostboundNetworkMessage(data.Message);
				}, only_final_update: false);
			}
		}
	}
}
