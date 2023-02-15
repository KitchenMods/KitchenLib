using System.Collections.Generic;
using System.Linq;

namespace KitchenLib.Preferences
{

	public static class GlobalPreferences
	{
		private static PreferenceManager Manager = null;

		public static void Setup()
		{
			if (Manager == null)
				Manager = new PreferenceManager("global");

			Manager.RegisterPreference(new PreferenceDictionary<string, string>("modCurrentProfile")).Set(new Dictionary<string, string>());
			Manager.RegisterPreference(new PreferenceDictionary<string, string[]>("modProfiles")).Set(new Dictionary<string, string[]>());

			Manager.Load();
		}

		public static string[] GetProfiles(string mod_id)
		{
			if (Manager == null)
				Setup();
			if (((Dictionary<string, string[]>)Manager.Get<PreferenceDictionary<string, string[]>>("modProfiles")).ContainsKey(mod_id))
				return ((Dictionary<string, string[]>)Manager.Get<PreferenceDictionary<string, string[]>>("modProfiles"))[mod_id];
			else
				return new string[] { };
		}
		public static void AddProfile(string mod_id, string profile)
		{
			if (Manager == null)
				Setup();
			List<string> profiles = GetProfiles(mod_id).ToList();
			profiles.Add(profile);
			((Dictionary<string, string[]>)Manager.Get<PreferenceDictionary<string, string[]>>("modProfiles"))[mod_id] = profiles.ToArray();
			Manager.Save();
		}
		public static void RemoveProfile(string mod_id, string profile)
		{
			if (Manager == null)
				Setup();
			List<string> profiles = GetProfiles(mod_id).ToList();
			profiles.Remove(profile);
			((Dictionary<string, string[]>)Manager.Get<PreferenceDictionary<string, string[]>>("modProfiles"))[mod_id] = profiles.ToArray();
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

			if (((Dictionary<string, string>)Manager.Get<PreferenceDictionary<string, string>>("modCurrentProfile")).ContainsKey(mod_id))
				return ((Dictionary<string, string>)Manager.Get<PreferenceDictionary<string, string>>("modCurrentProfile"))[mod_id];
			else
				return "";
		}

		public static void SetProfile(string mod_id, string profile)
		{
			if (Manager == null)
				Setup();

			if (((Dictionary<string, string>)Manager.Get<PreferenceDictionary<string, string>>("modCurrentProfile")).ContainsKey(mod_id))
				((Dictionary<string, string>)Manager.Get<PreferenceDictionary<string, string>>("modCurrentProfile"))[mod_id] = profile;
			else
				((Dictionary<string, string>)Manager.Get<PreferenceDictionary<string, string>>("modCurrentProfile")).Add(mod_id, profile);

			Manager.Save();
		}
	}
}
