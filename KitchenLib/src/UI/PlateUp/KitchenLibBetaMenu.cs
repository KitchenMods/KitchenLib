using Kitchen;
using Kitchen.Modules;
using KitchenLib.Preferences;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using KitchenLib.Views;
using KitchenLib.UI.PlateUp;
using Steamworks.Data;

namespace KitchenLib.src.UI.PlateUp
{
	public class KitchenLibBetaMenu<T> : KLMenu<T>
	{
		public KitchenLibBetaMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

		public static Dictionary<ulong, bool> subscribedVersion = new Dictionary<ulong, bool>();

		private Option<bool> BetaInstalledOption = null;
		private static bool IsBeta = false;

		public override void Setup(int player_id)
		{
			subscribedVersion.Clear();

			Steamworks.Ugc.Item KLMain = new Steamworks.Ugc.Item(new PublishedFileId { Value = 2898069883 });
			Steamworks.Ugc.Item KLBeta = new Steamworks.Ugc.Item(new PublishedFileId { Value = 2932799348 });

			subscribedVersion.Add(2898069883, KLMain.IsSubscribed);
			subscribedVersion.Add(2932799348, KLBeta.IsSubscribed);

			BetaInstalledOption = new Option<bool>(new List<bool> { false, true }, IsBetaInstalled(), new List<string> { "Disabled", "Enabled" });

			IsBeta = IsBetaInstalled();

			AddLabel("Enable KitchenLib Beta");
			AddInfo("Note: PlateUp! Requires a restart after enabling KL Beta.");

			New<SpacerElement>(true);

			AddSelect(BetaInstalledOption);
			BetaInstalledOption.OnChanged += delegate (object _, bool result)
			{
				IsBeta = result;
			};

			New<SpacerElement>(true);

			AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i)
			{
				if (IsBetaInstalled() != IsBeta)
				{
					KLMain.Unsubscribe();
					KLBeta.Unsubscribe();

					if (IsBeta)
					{
						KLBeta.Subscribe();
					}
					else
					{
						KLMain.Subscribe();
					}
				}
				RequestPreviousMenu();
			}, 0, 1f, 0.2f);
		}

		private static bool IsBetaInstalled()
		{
			if (subscribedVersion.Count > 0)
			{
				if (subscribedVersion[2932799348])
				{
					return true;
				}
			}
			return false;
		}
	}
}