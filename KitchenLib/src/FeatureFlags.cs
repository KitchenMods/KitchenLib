using KitchenLib.Preferences;

namespace KitchenLib
{
	internal class FeatureFlags
	{
#if WORKSHOP___FEATURE_FLAGS_ENABLED
		private const bool FEATURE_FLAGS_ENABLED = true;
#else
		private const bool FEATURE_FLAGS_ENABLED = false;
#endif

		private static readonly PreferenceManager PreferenceManager = new PreferenceManager($"{Main.MOD_ID}.features");

		// April Fools 2023
		private static PreferenceBool AprilFools2023Pref = PreferenceManager.RegisterPreference(new PreferenceBool("AprilFools2023", true));
		internal static bool AprilFools2023 => GetPref(AprilFools2023Pref, true);

		// Auto Invite
		private static PreferenceBool AutoInvitePref = PreferenceManager.RegisterPreference(new PreferenceBool("AutoInvite", false));
		internal static bool AutoInvite => GetPref(AutoInvitePref, false);

		// Auto Invite (Steam ID)
		private static PreferenceString AutoInviteSteamIdPref = PreferenceManager.RegisterPreference(new PreferenceString("AutoInviteSteamId", "0"));
		internal static string AutoInviteSteamId => GetPref(AutoInviteSteamIdPref, "0");

		// Fun Menu
		private static PreferenceBool FunMenuPref = PreferenceManager.RegisterPreference(new PreferenceBool("FunMenu", false));
		internal static bool FunMenu => GetPref(FunMenuPref, false);

		internal static void Init()
		{
			PreferenceManager.Load();
			PreferenceManager.Save();
		}

		private static T GetPref<T>(PreferenceBase<T> pref, T defaultValue)
		{
			return FEATURE_FLAGS_ENABLED ? pref.Get() : defaultValue;
		}
	}
}
