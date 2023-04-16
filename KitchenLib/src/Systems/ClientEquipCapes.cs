using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Systems;
using KitchenLib.Utils;
using KitchenMods;
using MessagePack;
using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenLib.src.Systems
{
	public class ClientEquipCapes : UpdatableObjectView<ClientEquipCapes.ViewData>, ISpecificViewResponse
	{
		public class UpdateView : ResponsiveViewSystemBase<ViewData, ResponseData>, IModSystem
		{
			private CommandViewHelpers helpers = null;

			EntityQuery Query;
			PlayerManager pm = null;
			protected override void Initialise()
			{
				base.Initialise();
				Query = GetEntityQuery(typeof(CLinkedView), typeof(CClientEquipCapeView));
			}

			protected override void OnUpdate() // Constantly send updates (Empty packets) to force a response.
			{
				if (pm == null)
					pm = Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerManager>();

				using NativeArray<CLinkedView> linkedViews = Query.ToComponentDataArray<CLinkedView>(Allocator.Temp);

				foreach (CLinkedView view in linkedViews)
				{
					SendUpdate(view.Identifier, new ViewData());
					if (ApplyUpdates(view.Identifier, PerformUpdateWithResponse, only_final_update: false)) { }
				}
			}
			private void PerformUpdateWithResponse(ResponseData data)
			{
				if (pm != null)
				{
					pm.GetPlayer(data.PlayerID, out Player player);
					if (player != null)
					{
						if (Require(player.Entity, out CPlayerCosmetics cosmetics))
						{
							cosmetics.Set(CosmeticType.Hat, data.CapeID);
							EntityManager.SetComponentData(player.Entity, cosmetics);
						}
					}
				}
			}
		}

		[MessagePackObject(false)]
		public class ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			public IUpdatableObject GetRelevantSubview(IObjectView view)
			{
				return view.GetSubView<ClientEquipCapes>();
			}

			public bool IsChangedFrom(ViewData check)
			{
				return true;
			}
		}

		[MessagePackObject(false)]
		public class ResponseData : IResponseData, IViewResponseData
		{
			[Key(0)] public int PlayerID;
			[Key(1)] public int CapeID;
		}

		private Action<IResponseData, Type> Callback;
		
		public void Update()
		{
		}

		public static int PlayerID;
		public static int CapeID;

		protected override void UpdateData(ViewData data)
		{
			if (PlayerID != 0 && CapeID != 0)
			{
				ResponseData response = new ResponseData
				{
					PlayerID = PlayerID,
					CapeID = CapeID
				};
				Callback.Invoke(response, typeof(ResponseData));
				PlayerID = 0;
				CapeID = 0;
			}
		}

		public void SetCallback(Action<IResponseData, Type> callback)
		{
			Callback = callback;
		}
	}
}
