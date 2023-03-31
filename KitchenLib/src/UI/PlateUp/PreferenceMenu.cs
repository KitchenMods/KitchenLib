using Kitchen;
using Kitchen.Modules;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.UI
{
	public class PreferenceMenu<T> : KLMenu<T>
	{
		public PreferenceMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}

		public override void Setup(int player_id)
		{
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
			AddSelect<bool>(_over_13);
			_over_13.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("over13").Set(result);
			};

			New<SpacerElement>(true);

			AddLabel("Do you permit the collection data?");
			AddSelect<bool>(_data_consent);
			_data_consent.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("datacollection").Set(result);
			};

			New<SpacerElement>(true);

			if (pm != null && player != null)
			{
				if (Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpDeveloper").Value)
				{

					AddButton("Equip PlateUp! Cape", delegate (int i)
					{
						cosmetics.Set(CosmeticType.Hat, GDOUtils.GetCustomGameDataObject<PlateUp_Cape>().ID);
						pm.EntityManager.SetComponentData(player.Entity, cosmetics);
					}, 0, 1f, 0.2f);
					New<SpacerElement>(true);
				}

				if (Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpSupport").Value)
				{

					AddButton("Equip Support Cape", delegate (int i)
					{
						cosmetics.Set(CosmeticType.Hat, GDOUtils.GetCustomGameDataObject<PlateUp_Support_Cape>().ID);
						pm.EntityManager.SetComponentData(player.Entity, cosmetics);
					}, 0, 1f, 0.2f);
					New<SpacerElement>(true);
				}

				if (Main.cosmeticManager.GetPreference<PreferenceBool>("isPlateUpStaff").Value)
				{

					AddButton("Equip Staff Cape", delegate (int i)
					{
						cosmetics.Set(CosmeticType.Hat, GDOUtils.GetCustomGameDataObject<PlateUp_Staff_Cape>().ID);
						pm.EntityManager.SetComponentData(player.Entity, cosmetics);
					}, 0, 1f, 0.2f);
					New<SpacerElement>(true);
				}

				if (Main.cosmeticManager.GetPreference<PreferenceBool>("isKitchenLibDeveloper").Value)
				{
					AddButton("Equip KitchenLib Cape", delegate (int i)
					{
						cosmetics.Set(CosmeticType.Hat, GDOUtils.GetCustomGameDataObject<KitchenLib_Cape>().ID);
						pm.EntityManager.SetComponentData(player.Entity, cosmetics);
					}, 0, 1f, 0.2f);
					New<SpacerElement>(true);
				}
			}

			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				Main.manager.Save();
				this.RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

		private Option<bool> _over_13 = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("over13").Value, new List<string> { "Yes", "No" });
		private Option<bool> _data_consent = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("datacollection").Value, new List<string> { "Yes", "No" });
	}
}
