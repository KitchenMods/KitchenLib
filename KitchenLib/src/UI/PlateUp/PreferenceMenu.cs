using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using KitchenLib.Views;
using KitchenLib.UI.PlateUp;
using KitchenLib.Systems;

namespace KitchenLib.UI
{
	internal class PreferenceMenu<T> : KLMenu<T>
	{
		public PreferenceMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		private List<int> capeIDs = new List<int>();
		private List<string> capeNames = new List<string>();
		private int selectedID;
		private Dictionary<ulong, Steamworks.Ugc.Item> Mods = new Dictionary<ulong, Steamworks.Ugc.Item>();

		private Option<int> capeSelector;

		private void SetCosmetic(int playerID, int capeID)
		{
			ClientEquipCapes.CapeID = capeID;
			ClientEquipCapes.PlayerID = playerID;
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

			AddLabel("Changing Main Menu");
			AddSelect(scrollingMenu);
			scrollingMenu.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("enableChangingMenu").Set(result);
			};

			New<SpacerElement>(true);

			AddButton("Dump Details", delegate (int i)
			{
				if (Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp"))
					DeleteDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp");

				if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp.zip"))
					File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp.zip");

				CopyFilesRecursively(Application.persistentDataPath, System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp");

				ZipFile.CreateFromDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp.zip");

				if (Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp"))
					DeleteDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/PlateUp");
			}, 0, 1f, 0.2f);

			New<SpacerElement>(true);
			if (SyncMods._isMissingMod)
			{
				AddButton("Sync Mods", async delegate (int i)
				{
					RequestSubMenu(typeof(ModSyncMenu));
				}, 0, 1f, 0.2f);

				New<SpacerElement>(true);
			}
			
			capeIDs.Clear();
			capeNames.Clear();
			foreach ((string, int) key in DataCollector.Capes.Keys)
			{
				if (Main.cosmeticManager.GetPreference<PreferenceBool>(key.Item1).Value)
				{
					capeIDs.Add(key.Item2);
					capeNames.Add(DataCollector.Capes[key]);
				}
			}

			if (capeIDs.Count > 0)
			{
				capeIDs.Add(-1);
				capeNames.Add("Disable Cape");
				AddLabel("Equip Cape");
				if (capeIDs.Count > 0)
				{
					capeSelector = new Option<int>(capeIDs, capeIDs.First(), capeNames);
					selectedID = capeIDs.First();
					capeSelector.OnChanged += (s, args) =>
					{
						selectedID = args;
					};
				}
				AddSelect<int>(capeSelector);
				AddButton("Equip Cape", delegate (int i)
				{
					SetCosmetic(player_id, selectedID);
				}, 0, 1f, 0.2f);
				New<SpacerElement>(true);
			}

			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				Main.manager.Save();
				RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

		private Option<bool> _over_13 = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("over13").Value, new List<string> { "Yes", "No" });
		private Option<bool> _data_consent = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("datacollection").Value, new List<string> { "Yes", "No" });
		private Option<bool> scrollingMenu = new Option<bool>(new List<bool> { true, false }, Main.manager.GetPreference<PreferenceBool>("enableChangingMenu").Value, new List<string> { "Enabled", "Disabled" });

		private static void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			//Now Create all of the directories
			foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
			}

			//Copy all the files & Replaces any files with the same name
			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
			}
		}

		private static void DeleteDirectory(string target_dir)
		{
			string[] files = Directory.GetFiles(target_dir);
			string[] dirs = Directory.GetDirectories(target_dir);

			foreach (string file in files)
			{
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (string dir in dirs)
			{
				DeleteDirectory(dir);
			}

			Directory.Delete(target_dir, false);
		}
	}
}
