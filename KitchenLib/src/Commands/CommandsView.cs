using Kitchen;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using MessagePack;

namespace KitchenLib.Commands
{
	public class CommandManager : ResponsiveObjectView<CommandManager.ViewData, CommandManager.ResponseData>
	{
		protected override void UpdateData(ViewData data) { }

		public class UpdateView : ResponsiveViewSystemBase<CommandManager.ViewData, CommandManager.ResponseData>
		{
			protected override void OnUpdate()
			{
				var entities = GetEntityQuery(new QueryHelper().All(typeof(CLinkedView))).ToEntityArray(Allocator.TempJob);

				foreach (Entity entity in entities)
				{
					if (Require<CLinkedView>(entity, out CLinkedView linkedView))
					{
						ApplyUpdates(linkedView.Identifier, (data) =>
						{
							//Do something with data.Commands
						}, false);
					}
				}
			}
		}

		public override bool HasStateUpdate(out IResponseData state)
		{
			state = null;
			bool result;
			if (Response.Commands != null)
			{
				state = Response;
				Response = default(CommandManager.ResponseData);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private CommandManager.ResponseData Response;

		[MessagePackObject]
		public struct ViewData : IViewData, IViewResponseData, IViewData.ICheckForChanges<CommandManager.ViewData>
		{
			public bool IsChangedFrom(CommandManager.ViewData check)
			{
				return true;
			}
		}
		[MessagePackObject]
		public struct ResponseData : IResponseData, IViewResponseData
		{
			[Key(0)]
			public Dictionary<int, string> Commands;
		}
	}
}