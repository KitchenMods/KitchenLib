using Kitchen;
using Kitchen.Modules;
using System.Collections.Generic;
using KitchenLib.Preferences;
using UnityEngine;
using Steamworks.Data;

namespace KitchenLib.UI.PlateUp
{
	public class DeveloperOptions<T> : KLMenu<T>
	{
		public DeveloperOptions(Transform container, ModuleList module_list) : base(container, module_list) { }

		public static Dictionary<ulong, bool> subscribedVersion = new Dictionary<ulong, bool>();

		private Option<bool> BetaInstalledOption = null;
		private Option<bool> forceLocalDishes = new Option<bool>(new List<bool>{true, false}, Main.manager.GetPreference<PreferenceBool>("forceLocalDishes").Value, new List<string>{"Enabled", "Disabled"});
		private static bool IsBeta = false;

		public override void Setup(int player_id)
		{
			subscribedVersion.Clear();

			Steamworks.Ugc.Item KLMain = new Steamworks.Ugc.Item(new PublishedFileId { Value = 2898069883 });
			Steamworks.Ugc.Item KLBeta = new Steamworks.Ugc.Item(new PublishedFileId { Value = 2932799348 });

			subscribedVersion.Add(2898069883, KLMain.IsSubscribed);
			subscribedVersion.Add(2932799348, KLBeta.IsSubscribed);

			BetaInstalledOption = new Option<bool>(new List<bool> { true, false }, IsBetaInstalled(), new List<string> { "Enabled", "Disabled" });

			IsBeta = IsBetaInstalled();

			AddLabel("Spawn Local Dishes (Requires Restart)");

			New<SpacerElement>(true);

			AddSelect(forceLocalDishes);
			forceLocalDishes.OnChanged += delegate (object _, bool result)
			{
				Main.manager.GetPreference<PreferenceBool>("forceLocalDishes").Set(result);
			};

			New<SpacerElement>(true);

			AddLabel("Enable KitchenLib Beta (Requires Restart)");

			New<SpacerElement>(true);

			AddSelect(BetaInstalledOption);
			BetaInstalledOption.OnChanged += delegate (object _, bool result)
			{
				IsBeta = result;
			};

			New<SpacerElement>(true);
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