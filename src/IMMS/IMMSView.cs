using Kitchen;
using KitchenLib.Utils;
using KitchenLib.Views;
using MessagePack;
using System.Collections.Generic;

namespace KitchenLib.IMMS
{
	internal class IMMSView : ResponsiveObjectView<IMMSView.ViewData, IMMSView.ResponseData>
	{
		internal static CustomViewType ViewType => ViewUtils.GetViewType(Main.MOD_ID, "imms");

		private int LastSeenMessageId = -1;

		[MessagePackObject]
		public struct ViewData : IViewData, IViewData.ICheckForChanges<ViewData>, IRollUp
		{
			[Key(0)]
			public List<IMMSNetworkMessage> Messages;

			public bool IsChangedFrom(ViewData check)
			{
				if (Messages.Count != check.Messages.Count)
				{
					return true;
				}

				for (int i = 0; i < Messages.Count; i++)
				{
					if (Messages[i].Id != check.Messages[i].Id)
					{
						return true;
					}
				}

				return false;
			}
		}

		[MessagePackObject]
		public struct ResponseData : IResponseData
		{
			[Key(0)]
			public IMMSNetworkMessage Message;
		}

		protected override void UpdateData(ViewData data)
		{
			var maxId = 0;
			foreach (var message in data.Messages)
			{
				if (message.Id > LastSeenMessageId)
				{
					if (message.Id > maxId)
					{
						maxId = message.Id;
					}
					IMMSManager.HandleIncomingClientboundNetworkMessage(message);
				}
			}
			LastSeenMessageId = maxId;
		}

		public override bool HasStateUpdate(out IResponseData state)
		{
			state = null;
			if (IMMSManager.HostboundMessages.Count == 0)
			{
				return false;
			}

			state = new ResponseData
			{
				Message = IMMSManager.HostboundMessages.Dequeue()
			};
			return true;
		}
	}
}
