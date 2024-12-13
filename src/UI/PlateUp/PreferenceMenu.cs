using Kitchen.Modules;
using System.IO;
using UnityEngine;
using KitchenLib.Views;
using KitchenLib.UI.PlateUp;

namespace KitchenLib.UI
{
	internal class PreferenceMenu : KLMenu
	{
		public PreferenceMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		public override void Setup(int player_id)
		{
			AddLabel("KitchenLib");
			New<SpacerElement>(true);

			AddButton("User Options", delegate (int i)
			{
				RequestSubMenu(typeof(UserOptions));
			}, 0, 1f, 0.2f);

			AddButton("Developer Options", delegate (int i)
			{
				RequestSubMenu(typeof(DeveloperOptions));
			}, 0, 1f, 0.2f);

			New<SpacerElement>(true);
			if (SyncMods.MissingMods.Count > 0)
			{
				AddButton("Sync Mods", delegate (int i)
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
