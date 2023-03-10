using System.Collections.Generic;
using System.Linq;

namespace KitchenLib.Preferences
{

	public static class GlobalPreferences
	{
		private static PreferenceManager Manager = null;
		private static PreferenceDictionary<string, string> CurrentProfilePreference;
		private static PreferenceDictionary<string, string[]> ModProfilesPreference;

		public static void Setup()
		{
			if (Manager == null)
				Manager = new PreferenceManager("global");

			CurrentProfilePreference = Manager.RegisterPreference(new PreferenceDictionary<string, string>("modCurrentProfile", new Dictionary<string, string>()));
			ModProfilesPreference = Manager.RegisterPreference(new PreferenceDictionary<string, string[]>("modProfiles", new Dictionary<string, string[]>()));

			Manager.Load();
		}

		public static string[] GetProfiles(string mod_id)
		{
			if (Manager == null)
				Setup();
			if (ModProfilesPreference.Get().ContainsKey(mod_id))
				return ModProfilesPreference.Get()[mod_id];
			else
				return new string[] { };
		}
		public static void AddProfile(string mod_id, string profile)
		{
			if (Manager == null)
				Setup();
			List<string> profiles = GetProfiles(mod_id).ToList();
			profiles.Add(profile);
			ModProfilesPreference.Get()[mod_id] = profiles.ToArray();
			Manager.Save();
		}
		public static void RemoveProfile(string mod_id, string profile)
		{
			if (Manager == null)
				Setup();
			List<string> profiles = GetProfiles(mod_id).ToList();
			profiles.Remove(profile);
			ModProfilesPreference.Get()[mod_id] = profiles.ToArray();
			Manager.Save();
		}
		public static bool DoesProfileExist(string mod_id, string profile)
		{
			if (Manager == null)
				Setup();
			return GetProfiles(mod_id).Contains<string>(profile);
		}

		public static string GetProfile(string mod_id)
		{
			if (Manager == null)
				Setup();

			if (CurrentProfilePreference.Get().ContainsKey(mod_id))
				return CurrentProfilePreference.Get()[mod_id];
			else
				return "";
		}

		public static void SetProfile(string mod_id, string profile)
		{
			if (Manager == null)
				Setup();

			if (CurrentProfilePreference.Get().ContainsKey(mod_id))
				CurrentProfilePreference.Get()[mod_id] = profile;
			else
				CurrentProfilePreference.Get().Add(mod_id, profile);

			Manager.Save();
		}
	}
}
