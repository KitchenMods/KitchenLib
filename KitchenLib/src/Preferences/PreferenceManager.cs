using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace KitchenLib.Preferences
{
	public class PreferenceManager
	{
		private readonly string preference_path = Application.persistentDataPath + "/UserData/Preferences/";
		private string mod_id;
		private Dictionary<(string, string), PreferenceBase> preferences = new Dictionary<(string, string), PreferenceBase>();
		private Dictionary<string, Type> preferenceTypes = new Dictionary<string, Type>();
		public PreferenceManager(string MOD_ID)
		{
			mod_id = MOD_ID;
			if (!Directory.Exists(preference_path))
				Directory.CreateDirectory(preference_path);
		}

		public object Get<T>(string key)
		{
			if (preferences.ContainsKey((key, typeof(T).Name)))
				return preferences[(key, typeof(T).Name)].Get();
			else
			{
				Main.instance.Warning($"Unable to get value of {key}, key not registered.");
				return null;
			}
		}

		public void Get<T>(string key, object value)
		{
			if (preferences.ContainsKey((key, typeof(T).Name)))
				preferences[(key, typeof(T).Name)].Set(value);
			else
			{
				Main.instance.Warning($"Unable to set value of {key}, key not registered.");
				return;
			}
		}

		public void Save()
		{
			List<StoredPreference> storedPreferences = new List<StoredPreference>();

			foreach ((string, string) key in preferences.Keys)
			{
				preferences[key].Serialize();
				storedPreferences.Add(new StoredPreference
				{
					Key = key.Item1,
					raw_Value = preferences[key].raw_value,
					Type = key.Item2
				});
			}

			string json = JsonConvert.SerializeObject(storedPreferences, Formatting.Indented);
			File.WriteAllText(preference_path + mod_id + ".json", json);
		}
		public void Load()
		{
			List<StoredPreference> storedPreferences = new List<StoredPreference>();
			string json = "";
			if (File.Exists(preference_path + mod_id + ".json"))
				json = File.ReadAllText(preference_path + mod_id + ".json");
			if (string.IsNullOrEmpty(json))
			{
				Main.instance.Warning($"Unable to load preferences, file empty or not saved.");
				return;
			}

			storedPreferences = JsonConvert.DeserializeObject<List<StoredPreference>>(json);
			foreach (StoredPreference pref in storedPreferences)
			{
				if (!preferences.ContainsKey((pref.Key, pref.Type)))
				{
					Main.instance.Warning($"Unable to load {pref.Key}, key not registered.");
				}
				else
				{
					preferences[(pref.Key, pref.Type)].raw_value = pref.raw_Value;
					preferences[(pref.Key, pref.Type)].Deserialize();
				}
			}
			storedPreferences.Clear();
		}
		public PreferenceBase RegisterPreference(PreferenceBase preference)
		{
			if (preferences.ContainsKey((preference.Key, preference.GetType().Name)))
			{
				Main.instance.Warning($"Unable to register {preference.Key}, key already registered.");
				return null;
			}
			preferences.Add((preference.Key, preference.GetType().Name), preference);

			return preference;
		}
		private class StoredPreference
		{
			public string Key;
			public string raw_Value;
			public string Type;
		}
	}
}
