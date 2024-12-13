using KitchenLib.Preferences;

namespace KitchenLib
{
	internal class FeatureFlags
	{
		private static readonly PreferenceManager PreferenceManager = new PreferenceManager($"{Main.MOD_ID}.features");

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
