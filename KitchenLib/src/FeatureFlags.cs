using KitchenLib.Preferences;

namespace KitchenLib
{
	internal class FeatureFlags
	{
		private static PreferenceManager PreferenceManager = new PreferenceManager($"{Main.MOD_ID}.features");

		private static PreferenceBool AprilFoolsPref;
		internal static bool AprilFools => AprilFoolsPref.Get();

		private static PreferenceBool AutoInvitePref;
		internal static bool AutoInvite => AutoInvitePref.Get();

		private static PreferenceString DebugSteamIdPref;
		internal static string DebugSteamId => DebugSteamIdPref.Get();

		private static PreferenceBool FunMenuPref;
		internal static bool FunMenu => FunMenuPref.Get();

		internal static void Init()
		{
			AprilFoolsPref = PreferenceManager.RegisterPreference(new PreferenceBool("AprilFools", true));
			AutoInvitePref = PreferenceManager.RegisterPreference(new PreferenceBool("AutoInvite", false));
			DebugSteamIdPref = PreferenceManager.RegisterPreference(new PreferenceString("DebugSteamId", ""));
			FunMenuPref = PreferenceManager.RegisterPreference(new PreferenceBool("FunMenu", false));

			PreferenceManager.Load();
			PreferenceManager.Save();
		}
	}
}
