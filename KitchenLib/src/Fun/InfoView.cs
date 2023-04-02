using Kitchen;
using KitchenLib.Systems;
using KitchenMods;
using MessagePack;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Fun
{
	internal class InfoView : UpdatableObjectView<InfoView.ViewData>
	{
		#region ECS View System (Runs on host and updates views to be broadcasted to clients)
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			private EntityQuery Views;
			private EntityQuery PlayersQuery;

			protected override void Initialise()
			{
				base.Initialise();
				Views = GetEntityQuery(typeof(CLinkedView), typeof(CInfoView));
				PlayersQuery = GetEntityQuery(typeof(CPlayer));
			}

			private FixedListInt64 playerIDs = new FixedListInt64();
			private List<string> playerNames = new List<string>();
			protected override void OnUpdate()
			{
				using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				using NativeArray<CPlayer> players = PlayersQuery.ToComponentDataArray<CPlayer>(Allocator.Temp);

				for (var i = 0; i < views.Length; i++)
				{
					var view = views[i];
					playerIDs.Clear();
					playerNames.Clear();
					foreach (CPlayer p in players)
					{
						playerIDs.Add(p.ID);
						playerNames.Add(Players.Main.Get(p.ID).Profile.Name);
					}

					ViewData data = new ViewData
					{
						PlayerIDs = playerIDs,
						PlayerNames = string.Join(",", playerNames)
					};

					SendUpdate(view, data);
				}
			}
		}
		#endregion


		#region Message Packet
		[MessagePackObject(false)]
		public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<InfoView>();

			[Key(0)]
			public FixedListInt64 PlayerIDs;
			[Key(1)]
			public string PlayerNames;

			public bool IsChangedFrom(ViewData cached)
			{
				return PlayerIDs != cached.PlayerIDs ||
					PlayerNames != cached.PlayerNames;
			}
		}
		#endregion

		private FixedListInt64 playerIDs = new FixedListInt64();
		private List<string> playerNames = new List<string>();

		protected override void UpdateData(ViewData view_data)
		{
			playerIDs = view_data.PlayerIDs;
			playerNames = view_data.PlayerNames.Split(',').ToList();
		}

		void Update()
		{
			RefVars.CurrentPlayers.Clear();
			for (var i = 0; i < playerIDs.Length; i++)
			{
				RefVars.CurrentPlayers.Add(playerIDs[i], playerNames[i]);
			}
		}
	}
}