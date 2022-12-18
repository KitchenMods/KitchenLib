using Kitchen;
using Unity.Entities;
using System.Collections.Generic;
using Unity.Collections;
using System;

namespace KitchenLib
{
	public class CommandSystem : ResponsiveViewSystemBase<CommandSystem.ViewData, CommandSystem.ResponceData>
	{
		protected override void OnUpdate()
		{
			var Entities = GetEntityQuery(new QueryHelper()
					.All(typeof(CommandSystem.ViewData)));
			foreach (var entity in Entities.ToEntityArray(Allocator.TempJob))
			{
				CLinkedView linkedView;
				Require<CLinkedView>(entity, out linkedView);

				ApplyUpdates(linkedView.Identifier, (data) =>
				{
					foreach (string command in data.commands)
					{
						if (command.ToLower().Equals("spawn hob"))
						{
							//Spawn a hob
						}
					}
				}, false);
			}
		}

		public struct ViewData : IViewData, IViewResponseData, IViewData.ICheckForChanges<CommandSystem.ViewData>, IRollUp
		{
			public IUpdatableObject GetRelevantSubview(IObjectView view)
			{
				return view.GetSubView<CrateView>();
			}

			public bool IsChangedFrom(CommandSystem.ViewData check)
			{
				return false;
			}

		}

		public struct ResponceData : IResponseData, IViewResponseData, IRollUp
		{
			public List<string> commands;
		}
	}

	public class CommandView : ResponsiveObjectView<CommandSystem.ViewData, CommandSystem.ResponceData>
	{
		public override bool HasStateUpdate(out IResponseData state)
		{
			//Just here to prevent errors at the moment
			state = null;
			return false;
		}

		protected override void UpdateData(CommandSystem.ViewData data)
		{
		}
	}

}
