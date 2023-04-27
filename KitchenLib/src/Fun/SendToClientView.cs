using Kitchen;
using KitchenData;
using KitchenLib.Systems;
using KitchenMods;
using MessagePack;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using System.Collections.Generic;

namespace KitchenLib.Fun
{
	public class SendToClientView : UpdatableObjectView<SendToClientView.ViewData>
	{
		#region ECS View System (Runs on host and updates views to be broadcasted to clients)
		public class UpdateView : IncrementalViewSystemBase<ViewData>, IModSystem
		{
			public static bool isDark = false;
			private EntityQuery Views;
			private EntityQuery Unlocks;
			private static Dictionary<int, string> LocalAppliances = new Dictionary<int, string>();
			private static Dictionary<int, string> LocalHats = new Dictionary<int, string>();
			private static Dictionary<int, string> LocalOutfits = new Dictionary<int, string>();
			private static Dictionary<int, string> LocalUnlocks = new Dictionary<int, string>();
			private static List<int> LocalActiveUnlocks = new List<int>();

			protected override void Initialise()
			{
				base.Initialise();

				Views = GetEntityQuery(new QueryHelper().All(typeof(CSendToClientView), typeof(CLinkedView)));
				Unlocks = GetEntityQuery(new QueryHelper().All(typeof(CProgressionUnlock)));

				LocalAppliances.Clear();
				LocalHats.Clear();
				LocalOutfits.Clear();
				LocalUnlocks.Clear();


				foreach (Appliance appliance in GameData.Main.Get<Appliance>())
				{
					LocalAppliances.Add(appliance.ID, appliance.Name);
				}
				foreach (PlayerCosmetic cosmetic in GameData.Main.Get<PlayerCosmetic>())
				{
					if (cosmetic.CosmeticType == CosmeticType.Outfit)
					{
						if (cosmetic.name.Contains(" - "))
							LocalOutfits.Add(cosmetic.ID, cosmetic.name.Split('-')[1].Replace(" ", ""));
						else
							LocalOutfits.Add(cosmetic.ID, cosmetic.name.Replace(" ", ""));
					}
					else if (cosmetic.CosmeticType == CosmeticType.Hat)
					{
						if (cosmetic.name.Contains(" - "))
							LocalHats.Add(cosmetic.ID, cosmetic.name.Split('-')[1].Replace(" ", ""));
						else
							LocalHats.Add(cosmetic.ID, cosmetic.name.Replace(" ", ""));
					}
				}

				foreach (Unlock unlock in GameData.Main.Get<Unlock>())
				{
					LocalUnlocks.Add(unlock.ID, unlock.Name);
				}
			}

			protected override void OnUpdate()
			{
				using var entities = Views.ToEntityArray(Allocator.Temp);
				using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
				using var activeUnlocks = Unlocks.ToComponentDataArray<CProgressionUnlock>(Allocator.Temp);

				LocalActiveUnlocks.Clear();

				for (int i = 0; i < activeUnlocks.Length; i++)
				{
					LocalActiveUnlocks.Add(activeUnlocks[i].ID);
				}

				//////////////////////////////////////////////////

				for (var i = 0; i < views.Length; i++)
				{
					var view = views[i];

					ViewData data = new ViewData
					{
						isDark = RefVars.IsBlind,
						LocalAppliances = LocalAppliances,
						LocalOutfits = LocalOutfits,
						LocalHats = LocalHats,
						LocalUnlocks = LocalUnlocks,
						LocalActiveUnlocks = LocalActiveUnlocks
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
			[Key(0)] public bool isDark;
			[Key(1)] public Dictionary<int, string> LocalAppliances;
			[Key(2)] public Dictionary<int, string> LocalOutfits;
			[Key(3)] public Dictionary<int, string> LocalHats;
			[Key(4)] public Dictionary<int, string> LocalUnlocks;
			[Key(5)] public List<int> LocalActiveUnlocks;

			public IUpdatableObject GetRelevantSubview(IObjectView view) => view.GetSubView<SendToClientView>();

			public bool IsChangedFrom(ViewData cached)
			{
				return isDark != cached.isDark ||
					LocalAppliances != cached.LocalAppliances ||
					LocalOutfits != cached.LocalOutfits ||
					LocalHats != cached.LocalHats ||
					LocalUnlocks != cached.LocalUnlocks ||
					LocalActiveUnlocks != cached.LocalActiveUnlocks;
			}
		}
		#endregion

		protected bool isDark = false;
		protected GameObject Light = null;
		protected Dictionary<int, string> _dishes = new Dictionary<int, string>();
		protected override void UpdateData(ViewData view_data)
		{
			_dishes.Clear();
			isDark = view_data.isDark;
			foreach (int applianceID in view_data.LocalAppliances.Keys)
			{
				RefVars.SetKeyPair(applianceID, view_data.LocalAppliances[applianceID]);
			}
			RefVars.AvailableAppliances = view_data.LocalAppliances;

			foreach (int outfitID in view_data.LocalOutfits.Keys)
			{
				RefVars.SetKeyPair(outfitID, view_data.LocalOutfits[outfitID]);
			}
			RefVars.AvailableOutfits = view_data.LocalOutfits;

			foreach (int hatID in view_data.LocalHats.Keys)
			{
				RefVars.SetKeyPair(hatID, view_data.LocalHats[hatID]);
			}
			RefVars.AvailableHats = view_data.LocalHats;

			foreach (int unlockID in view_data.LocalUnlocks.Keys)
			{
				RefVars.SetKeyPair(unlockID, view_data.LocalUnlocks[unlockID]);
			}
			RefVars.AvailableUnlocks = view_data.LocalUnlocks;
			RefVars.ActiveUnlocks = view_data.LocalActiveUnlocks;
		}

		void Update()
		{
			if (Light == null)
				Light = GameObject.Find("Directional Light");
			if (isDark)
				Light.SetActive(false);
			else
				Light.SetActive(true);
		}
	}
}