using Kitchen;
using KitchenLib.Systems;
using KitchenLib.Utils;
using KitchenMods;
using MessagePack;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;

namespace KitchenLib.Views
{
	internal class InfoView : UpdatableObjectView<InfoView.ViewData>
	{
		#region ECS View System (Runs on host and updates views to be broadcasted to clients)
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			private EntityQuery Views;
			private EntityQuery PlayersQuery;
			private static Dictionary<int, string> playerCache = new Dictionary<int, string>();

			protected override void Initialise()
			{
				base.Initialise();
				Views = GetEntityQuery(typeof(CLinkedView), typeof(CInfoView));
				PlayersQuery = GetEntityQuery(typeof(CPlayer));
			}
			//Dictionary<int, string> _players = new Dictionary<int, string>();
			public List<int> PlayerIDs = new List<int>();
			public List<string> PlayerNames = new List<string>();
			protected override void OnUpdate()
			{
				using var entities = Views.ToEntityArray(Allocator.Temp);
				using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				using NativeArray<CPlayer> players = PlayersQuery.ToComponentDataArray<CPlayer>(Allocator.Temp);

				for (var i = 0; i < views.Length; i++)
				{

					var view = views[i];

					foreach (CPlayer p in players)
					{
						if (!playerCache.ContainsKey(p.ID))
							playerCache.Add(p.ID, Players.Main.Get(p.ID).Profile.Name);
						else if (string.IsNullOrEmpty(playerCache[p.ID]))
							playerCache[p.ID] = Players.Main.Get(p.ID).Profile.Name;
					}

					foreach (CPlayer p in players)
					{
						ViewData data = new ViewData
						{
							playerID = p.ID,
							playerName = playerCache[p.ID]
						};

						SendUpdate(view, data);
					}
				}
			}
		}
		#endregion

		
		#region Message Packet
		[MessagePackObject(false)]
		public struct ViewData : ISpecificViewData, IViewData.ICheckForChanges<ViewData>
		{
			[Key(0)] public int playerID;
			[Key(1)] public string playerName;

			public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<InfoView>();

			public bool IsChangedFrom(ViewData cached)
			{
				return playerID != cached.playerID || playerName != cached.playerName;
			}
		}
		#endregion

		private int playerID;
		private string playerName;

		protected override void UpdateData(ViewData view_data)
		{
			playerID = view_data.playerID;
			playerName = view_data.playerName;
		}

		void Update()
		{
			if (CommandView.players.ContainsKey(playerID))
			{
				CommandView.players[playerID] = playerName;
			}
			else
			{
				CommandView.players.Add(playerID, playerName);
			}
		}
	}
}