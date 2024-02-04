using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace KitchenLib.Preferences
{
    public class PreferenceManager
    {
        private readonly string OLD_PREFERENCE_FOLDER_PATH = Application.persistentDataPath + "/UserData/Preferences";
        private readonly string PREFERENCE_FOLDER_PATH = Application.persistentDataPath + "/ModData/Preferences";
        private string preferenceFilePath = "";
        private string fileType = ".json";

        private readonly string modId;
        private readonly Dictionary<(string, string), PreferenceBase> preferences = new();
        private string currentProfile = "";

        /// <summary>
        /// Create a preference manager attached to the given mod ID.
        /// </summary>
        /// <param name="modId">The mod ID.</param>
        public PreferenceManager(string modId)
        {
            this.modId = modId;
            Setup();
        }

        internal PreferenceManager(string modId, string fileType)
        {
	        this.fileType = fileType;
	        this.modId = modId;
	        Setup(true);
        }

        internal void Setup(bool isForcedFileType = false)
        {
	        if (Directory.Exists($"{OLD_PREFERENCE_FOLDER_PATH}/{this.modId}"))
	        {
		        Main.LogWarning($"Found old preferences folder for {this.modId}, moving to new location.");
		        if (!Directory.Exists($"{PREFERENCE_FOLDER_PATH}/{this.modId}"))
		        {
			        Directory.Move($"{OLD_PREFERENCE_FOLDER_PATH}/{this.modId}", $"{PREFERENCE_FOLDER_PATH}/{this.modId}");
		        }
		        else
		        {
			        Main.Logger.LogWarning($"Failed to move old preferences folder for {this.modId}, folder already exists.");
		        }
	        }
            
	        if (!isForcedFileType)
	        {
		        fileType = BaseMod.globalPreferences.GetPreference<PreferenceBool>("steamCloudPreferences").Value ? ".plateupsave" : ".json";
	        }
	        
	        if (!Directory.Exists($"{PREFERENCE_FOLDER_PATH}/{this.modId}"))
		        Directory.CreateDirectory($"{PREFERENCE_FOLDER_PATH}/{this.modId}");

	        preferenceFilePath = $"{PREFERENCE_FOLDER_PATH}/{this.modId}/{this.modId}{currentProfile}{fileType}";
        }
        
        /// <summary>
        /// Set the current mod-level preference profile of the preference manager.
        /// </summary>
        /// <param name="profile">The name of the profile.</param>
        public void SetProfile(string profile = "")
        {
            if (!string.IsNullOrEmpty(profile))
                profile = $"-{profile}";
            currentProfile = profile;
            preferenceFilePath = $"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}{fileType}";
        }

        /// <summary>
        /// Get the preference associated with a given key. Preferences need to be registered with 
        /// <see cref="RegisterPreference{T}(T)"/> before using this.
        /// </summary>
        /// <typeparam name="T">The type of the preference.</typeparam>
        /// <param name="key">The key of the preference.</param>
        /// <returns>The requested preference.</returns>
        public T GetPreference<T>(string key) where T : PreferenceBase
        {
            if (preferences.ContainsKey((key, typeof(T).Name)))
                return (T)preferences[(key, typeof(T).Name)];
            else
            {
                Main.LogWarning($"Unable to get preference with {key}, key not registered.");
                return null;
            }
        }

        /// <summary>
        /// Get the value associated with the preference with a given key. Preferences need to be 
        /// registered with <see cref="RegisterPreference{T}(T)"/> before using this. It is recommended
        /// to use <see cref="GetPreference{T}(string)"/> along with <see cref="PreferenceBase{T}.Get"/> 
        /// instead of this method.
        /// </summary>
        /// <typeparam name="T">The type of the preference.</typeparam>
        /// <param name="key">The key of the preference.</param>
        /// <returns>The value associated with the preference.</returns>
        public object Get<T>(string key) where T : PreferenceBase
        {
            if (preferences.ContainsKey((key, typeof(T).Name)))
                return ((dynamic)preferences[(key, typeof(T).Name)]).Get();
            else
            {
                Main.LogWarning($"Unable to get value of {key}, key not registered.");
                return null;
            }
        }

        /// <summary>
        /// Get the value associated with the preference with a given key. Preferences need to be 
        /// registered with <see cref="RegisterPreference{T}(T)"/> before using this. It is recommended
        /// to use <see cref="PreferenceBase{T}.Set(T)"/> instead of this method. Note that this method
        /// is not type safe and will throw an exception if the given value is not the correct type
        /// for the preference.
        /// </summary>
        /// <typeparam name="T">The type of the preference.</typeparam>
        /// <param name="key">The key of the preference.</param>
        /// <param name="value">The new value of the preference.</param>
        public void Set<T>(string key, object value)
        {
            if (preferences.ContainsKey((key, typeof(T).Name)))
                ((dynamic)preferences[(key, typeof(T).Name)]).Set((dynamic)value);
            else
            {
                Main.LogWarning($"Unable to set value of {key}, key not registered.");
                return;
            }
        }

        /// <summary>
        /// Save the current values of the preferences managed by this preference manager to the
        /// current profile's file on disk.
        /// </summary>
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

        private void CheckFileType()
        {
	        if (fileType == ".json")
	        {
		        if (!File.Exists($"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.plateupsave")) return;
				
		        if (!File.Exists($"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.json"))
		        {
			        File.Move($"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.plateupsave", $"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.json");
		        }
		        else
		        {
			        Main.Logger.LogWarning($"Failed to convert preferences file for {modId} from .plateupsave to .json, file already exists.");
		        }
	        }
	        else if (fileType == ".plateupsave")
	        {
		        if (!File.Exists($"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.json")) return;
				
		        if (!File.Exists($"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.plateupsave"))
		        {
			        File.Move($"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.json", $"{PREFERENCE_FOLDER_PATH}/{modId}/{modId}{currentProfile}.plateupsave");
		        }
		        else
		        {
			        Main.Logger.LogWarning($"Failed to convert preferences file for {modId} from .json to .plateupsave, file already exists.");
		        }
	        }
        }

        /// <summary>
        /// Load the values of the preferences managed by this preference manager from the
        /// current profile's file on disk.
        /// </summary>
        public void Load()
        {
	        try
	        {
		        CheckFileType();
	        }
	        catch (Exception e)
	        {
		        Console.WriteLine(e);
		        throw;
	        }
	        
	        try
	        {
		        string json = "";
		        if (File.Exists(preferenceFilePath))
			        json = File.ReadAllText(preferenceFilePath);
		        if (string.IsNullOrEmpty(json))
		        {
			        Main.LogWarning($"Unable to load preferences, file empty or not saved.");
			        return;
		        }

		        List<StoredPreference> storedPreferences = JsonConvert.DeserializeObject<List<StoredPreference>>(json);
		        foreach (StoredPreference pref in storedPreferences)
		        {
			        if (!preferences.ContainsKey((pref.Key, pref.Type)))
			        {
				        Main.LogWarning($"Unable to load {pref.Key}, key not registered.");
			        }
			        else
			        {
				        preferences[(pref.Key, pref.Type)].Deserialize(pref.Value);
			        }
		        }
		        storedPreferences.Clear();
	        }
	        catch
	        {
		        if (File.Exists(preferenceFilePath))
		        {
			        Main.LogWarning("Failed to load preferences file " + preferenceFilePath + ", backing up and replacing.");
			        File.Move(preferenceFilePath, preferenceFilePath + ".backup");
			        Save();
			        Load();
		        }
	        }
        }

        /// <summary>
        /// Register a preference with this preference manager.
        /// </summary>
        /// <typeparam name="T">the type of the preference.</typeparam>
        /// <param name="preference">the preference to register.</param>
        /// <returns>A reference to the input preference.</returns>
        public T RegisterPreference<T>(T preference) where T : PreferenceBase
        {
            if (preferences.ContainsKey((preference.Key, preference.GetType().Name)))
            {
                Main.LogWarning($"Unable to register {preference.Key}, key already registered.");
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
