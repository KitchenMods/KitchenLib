using Kitchen.Modules;
using Kitchen;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using KitchenLib.Views;
using KitchenMods;
using Steamworks.Data;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace KitchenLib.UI.PlateUp
{
	internal class ModSyncMenu : KLMenu<PauseMenuAction>
	{
		private List<ulong> installedMods = new List<ulong>();
		private Dictionary<(ulong, bool), Steamworks.Ugc.Item> missingMods = new Dictionary<(ulong, bool), Steamworks.Ugc.Item>();
		private List<ulong> successfulSubscriptions = new List<ulong>();
		private List<ulong> failedSubscriptions = new List<ulong>();
		private bool isInstalling = false;
		private int missingModCount = 0;
		private PanelElement element;
		public ModSyncMenu(Transform container, ModuleList module_list) : base(container, module_list)
		{
		}
		public override void Setup(int player_id)
		{

			//RequiresBackingPanel = false;
			element = new PanelElement();
			installedMods.Clear();
			missingMods.Clear();
			successfulSubscriptions.Clear();
			failedSubscriptions.Clear();
			isInstalling = false;
			missingModCount = 0;
			foreach (Mod _mod in ModPreload.Mods)
			{
				installedMods.Add(_mod.ID);
			}
			
			RedrawAsync();
			return;
		}

		private async Task RedrawAsync()
		{
			ModuleList.Clear();
			AddLabel("Mod Sync");
			if (missingMods.Count == 0)
			{
				AddInfo("Loading Mods... Please Wait");
				foreach (ulong mod in SyncMods._mods)
				{
					if (!installedMods.Contains(mod))
					{
						Steamworks.Ugc.Item item = new Steamworks.Ugc.Item(new PublishedFileId { Value = mod });
						var _mod = await Steamworks.Ugc.Item.GetAsync(item.Id);
						missingMods.Add((mod, !installedMods.Contains(mod)), _mod.Value);
						if (!installedMods.Contains(mod))
							missingModCount++;
					}
				}
				RedrawAsync();
				return;
			}
			else if (missingMods.Count > installedMods.Count && !(failedSubscriptions.Count > 0 || successfulSubscriptions.Count > 0) && !isInstalling)
			{
				//Confirm

				AddInfo($"Are you sure you would like to subscribe to {missingModCount} mods?");
				AddButton("Confirm", async delegate (int i)
				{
					isInstalling = true;
					RedrawAsync();
					return;
				}, 0, 1f, 0.2f);
			}
			else if (isInstalling)
			{
				AddInfo($"Installing {missingModCount} mods... Please wait.");
				foreach ((ulong, bool) item in missingMods.Keys)
				{
					if (item.Item2)
					{
						if (!await missingMods[item].Subscribe())
						{
							failedSubscriptions.Add(item.Item1);
						}
						else
						{
							successfulSubscriptions.Add(item.Item1);
						}
					}
				}
				isInstalling = false;
				RedrawAsync();
				return;
			}

			if (failedSubscriptions.Count > 0 || successfulSubscriptions.Count > 0)
			{
				if (failedSubscriptions.Count > 0)
				{
					AddInfo($"{failedSubscriptions.Count} Mods failed to subscribe.");
				}
				else
				{
					AddInfo("All Mods Subscribed, Please Restart.");
					AddButton("Restart", delegate (int i)
					{
						Application.Quit();
					}, 0, 1f, 0.2f);
				}
			}
			else
			{
				AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
				{
					this.RequestPreviousMenu();
				}, 0, 1f, 0.2f);
			}
			
			element.SetTarget(ModuleList);
			element.SetVisible(true);
		}
	}
}