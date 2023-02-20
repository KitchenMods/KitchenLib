using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace KitchenLib.Preferences
{
    public class PreferenceManager
    {
        private readonly string PREFERENCE_FOLDER_PATH = Application.persistentDataPath + "/UserData/Preferences";
        private string preferenceFilePath = "";

        private readonly string modId;
        private readonly Dictionary<(string, string), PreferenceBase> preferences = new();
        private string currentProfile = "";

        public PreferenceManager(string modId)
        {
            this.modId = modId;
            if (!Directory.Exists($"{PREFERENCE_FOLDER_PATH}/{this.modId}"))
                Directory.CreateDirectory($"{PREFERENCE_FOLDER_PATH}/{this.modId}");

            preferenceFilePath = $"{PREFERENCE_FOLDER_PATH}/{this.modId}/{this.modId}{currentProfile}.json";
        }

        public void SetProfile(string profile = "")
        {
            if (!string.IsNullOrEmpty(profile))
                profile = $"-{profile}";
            currentProfile = profile;
            preferenceFilePath = $"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.json";
        }

        public T GetPreference<T>(string key) where T : PreferenceBase
        {
            if (preferences.ContainsKey((key, typeof(T).Name)))
                return (T)preferences[(key, typeof(T).Name)];
            else
            {
                Main.instance.Warning($"Unable to get value of {key}, key not registered.");
                return null;
            }
        }

        [Obsolete("Use GetPreference<T>(string key) and PreferenceBase<T>.Get() instead")]
        public object Get<T>(string key)
        {
            if (preferences.ContainsKey((key, typeof(T).Name)))
                return preferences[(key, typeof(T).Name)];
            else
            {
                Main.instance.Warning($"Unable to get value of {key}, key not registered.");
                return null;
            }
        }

        [Obsolete("Use PreferenceBase<T>.Set(T value) instead")]
        public void Set<T>(string key, object value)
        {
            if (preferences.ContainsKey((key, typeof(T).Name)))
                ((dynamic)preferences[(key, typeof(T).Name)]).Set((dynamic)value);
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
                storedPreferences.Add(new StoredPreference
                {
                    Key = key.Item1,
                    Value = preferences[key].Serialize(),
                    Type = key.Item2
                });
            }

            string json = JsonConvert.SerializeObject(storedPreferences, Formatting.Indented);
            File.WriteAllText(preferenceFilePath, json);
        }

        public void Load()
        {
            string json = "";
            if (File.Exists(preferenceFilePath))
                json = File.ReadAllText(preferenceFilePath);
            if (string.IsNullOrEmpty(json))
            {
                Main.instance.Warning($"Unable to load preferences, file empty or not saved.");
                return;
            }

            List<StoredPreference> storedPreferences = JsonConvert.DeserializeObject<List<StoredPreference>>(json);
            foreach (StoredPreference pref in storedPreferences)
            {
                if (!preferences.ContainsKey((pref.Key, pref.Type)))
                {
                    Main.instance.Warning($"Unable to load {pref.Key}, key not registered.");
                }
                else
                {
                    preferences[(pref.Key, pref.Type)].Deserialize(pref.Value);
                }
            }
            storedPreferences.Clear();
        }

        public T RegisterPreference<T>(T preference) where T : PreferenceBase
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
            public string Value;
            public string Type;
        }
    }
}
