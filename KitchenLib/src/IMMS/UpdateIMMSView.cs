using Kitchen;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.IMMS
{
	public class UpdateIMMSView : ResponsiveViewSystemBase<IMMSView.ViewData, IMMSView.ResponseData>
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

				Router.BroadcastUpdate(linkedView, new IMMSView.ViewData { 
					Messages = IMMSManager.ClientboundMessages
				});
				IMMSManager.ClientboundMessages.Clear();

				ApplyUpdates(linkedView, (data) =>
				{
					IMMSManager.HandleIncomingHostboundNetworkMessage(data.Message);
				});
			}
		}
	}
}
