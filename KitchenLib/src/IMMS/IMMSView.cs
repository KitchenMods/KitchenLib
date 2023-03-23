using Kitchen;
using KitchenLib.Views;
using MessagePack;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.IMMS
{
	public class IMMSView : ResponsiveObjectView<IMMSView.ViewData, IMMSView.ResponseData>
	{
		public static readonly CustomViewType ViewType = new CustomViewType(627001, () =>
		{
			var res = new GameObject();

			res.AddComponent<IMMSView>();

			return res;
		});

		private int LastSeenMessageId = -1;
		private ViewData Data;

		[MessagePackObject]
		public struct ViewData : IViewData, IViewData.ICheckForChanges<ViewData>, IRollUp
		{
			[Key(0)]
			public List<IMMSNetworkMessage> Messages;

			public bool IsChangedFrom(ViewData check)
			{
				return Messages.Count != check.Messages.Count || (Messages.Count > 0 && Messages[0].Id != check.Messages[0].Id);
			}
		}

		[MessagePackObject]
		public struct ResponseData : IResponseData, IRollUp
		{
			[Key(0)]
			public IMMSNetworkMessage Message;
		}

		protected override void UpdateData(ViewData data)
		{
			Data = data;
			foreach (var message in Data.Messages)
			{
				if (message.Id > LastSeenMessageId)
				{
					IMMSManager.HandleIncomingClientboundNetworkMessage(message);
					LastSeenMessageId = message.Id;
				}
			}
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
