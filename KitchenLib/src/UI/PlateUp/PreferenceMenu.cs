using Kitchen;
using Kitchen.Modules;
using KitchenLib.Customs;
using KitchenLib.Customs.GDOs;
using KitchenLib.Preferences;
using KitchenLib.Systems;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.UI
{
	public class PreferenceMenu<T> : KLMenu<T>
	{
		public PreferenceMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		public Dictionary<(bool, int), string> Capes = new Dictionary<(bool, int), string>
		{
			{ (Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpDeveloper").Value, GDOUtils.GetCustomGameDataObject<PlateUpCape>().ID), "Equip PlateUp! Cape" },
			{ (Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpSupport").Value, GDOUtils.GetCustomGameDataObject<PlateUpSupportCape>().ID), "Equip Support Cape" },
			{ (Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpStaff").Value, GDOUtils.GetCustomGameDataObject<PlateUpStaffCape>().ID), "Equip Staff Cape" },
			{ (Main.cosmeticManager.GetPreference<PreferenceBool>("isKitchenLibDeveloper").Value, GDOUtils.GetCustomGameDataObject<KitchenLibCape>().ID), "Equip KitchenLib Cape" },
			{ (Main.cosmeticManager.GetPreference<PreferenceBool>("isTwitchStreamer").Value, GDOUtils.GetCustomGameDataObject<TwitchCape>().ID), "Equip Twitch Cape" },
			{ (Main.cosmeticManager.GetPreference<PreferenceBool>("isEasterChampion").Value, GDOUtils.GetCustomGameDataObject<Easter_Champion_Cape>().ID), "Equip Easter Champion Cape" },
		};

		private readonly List<int> capeIDs = new List<int>();
		private readonly List<string> capeNames = new List<string>();

		private void SetCosmetic(int playerID, int capeID)
		{
			ClientEquipCapes.CapeID = capeID;
			ClientEquipCapes.PlayerID = playerID;
		}

		public override void Setup(int player_id)
		{
			capeIDs.Clear();
			capeNames.Clear();
			foreach ((bool, int) key in Capes.Keys)
			{
				if (key.Item1)
				{
					capeIDs.Add(key.Item2);
					capeNames.Add(Capes[key]);
				}
			}

			Player player = null;
			CPlayerCosmetics cosmetics = new CPlayerCosmetics();
			PlayerManager pm = null;
			if (typeof(T) == typeof(PauseMenuAction))
			{
				pm = Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<PlayerManager>();
				if (pm != null)
				{
					pm.GetPlayer(player_id, out player, false);
					cosmetics = pm.EntityManager.GetComponentData<CPlayerCosmetics>(player.Entity);
				}
			}
			AddLabel("Are you over 13 years old?");
			AddSelect(_over_13);
			_over_13.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("over13").Set(result);
			};

			New<SpacerElement>(true);

			AddLabel("Do you permit the collection data?");
			AddSelect(_data_consent);
			_data_consent.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("datacollection").Set(result);
			};

			New<SpacerElement>(true);

			if (pm != null && player != null)
			{
				foreach ((bool, int) key in Capes.Keys)
				{
					if (key.Item1)
					{
						AddButton(Capes[key], delegate (int i)
						{
							SetCosmetic(player_id, key.Item2);
						}, 0, 1f, 0.2f);
						New<SpacerElement>(true);
					}
				}
			}

			AddButton(Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				Main.manager.Save();
				RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

		private Option<bool> _over_13 = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("over13").Value, new List<string> { "Yes", "No" });
		private Option<bool> _data_consent = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("datacollection").Value, new List<string> { "Yes", "No" });
	}
}
