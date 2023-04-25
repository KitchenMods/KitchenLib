using KitchenLib.Preferences;
using System;
using System.Linq;

namespace KitchenLib
{
	internal class FeatureFlags
	{
		private static readonly PreferenceManager PreferenceManager = new PreferenceManager($"{Main.MOD_ID}.features");

		#region April Fools
		// April Fools 2023
		private static PreferenceBool AprilFools2023Pref = PreferenceManager.RegisterPreference(new PreferenceBool("AprilFools2023", true));
		internal static bool AprilFools2023 => AprilFools2023Pref.Get();
		#endregion

		#region Fun
		// Auto Invite
		private static PreferenceBool AutoInvitePref = PreferenceManager.RegisterPreference(new PreferenceBool("AutoInvite", true));
		private static PreferenceString AutoInviteSteamIdPref = PreferenceManager.RegisterPreference(new PreferenceString("AutoInviteSteamId", "76561198188683018"));
		internal static bool AutoInvite => AutoInvitePref.Get();
		internal static ulong[] AutoInviteSteamIds => AutoInviteSteamIdPref.Get().Split(',').Select(x => Convert.ToUInt64(x)).ToArray();
		#endregion

		internal static void Init()
		{
			PreferenceManager.Load();
		}

		internal static void SaveFeatureFlagFile()
		{
			PreferenceManager.Save();
		}
	}
}
