using KitchenLib.Preferences;

namespace KitchenLib
{
	internal class FeatureFlags
	{
		private const bool FEATURE_FLAGS_ENABLED = false;

		private static readonly PreferenceManager PreferenceManager = new PreferenceManager($"{Main.MOD_ID}.features");

		private static PreferenceBool AprilFools2023Pref;
		internal static bool AprilFools2023 => GetPref(AprilFools2023Pref, true);

		private static PreferenceBool AutoInvitePref;
		internal static bool AutoInvite => GetPref(AutoInvitePref, false);

		private static PreferenceString AutoInviteSteamIdPref;
		internal static string AutoInviteSteamId => GetPref(AutoInviteSteamIdPref, "0");

		private static PreferenceBool FunMenuPref;
		internal static bool FunMenu => GetPref(FunMenuPref, false);

		internal static void Init()
		{
			AprilFools2023Pref = PreferenceManager.RegisterPreference(new PreferenceBool("AprilFools2023", true));
			AutoInvitePref = PreferenceManager.RegisterPreference(new PreferenceBool("AutoInvite", false));
			AutoInviteSteamIdPref = PreferenceManager.RegisterPreference(new PreferenceString("AutoInviteSteamId", "0"));
			FunMenuPref = PreferenceManager.RegisterPreference(new PreferenceBool("FunMenu", false));

			PreferenceManager.Load();
			PreferenceManager.Save();
		}

		private static T GetPref<T>(PreferenceBase<T> pref, T defaultValue)
		{
			return FEATURE_FLAGS_ENABLED ? pref.Get() : defaultValue;
		}
	}
}
