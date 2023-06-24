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
		
		private Dictionary<ulong, Steamworks.Ugc.Item> Mods = new Dictionary<ulong, Steamworks.Ugc.Item>();

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

			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				Main.manager.Save();
				RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}
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
