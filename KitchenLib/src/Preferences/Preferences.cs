using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace KitchenLib
{
    public static class PreferencesRegistry {
	
	    public static Dictionary<string, ModPreference> Preferences = new Dictionary<string, ModPreference>();
    
	    private static Dictionary<string, Type> TypeMapping = new Dictionary<string, Type>();
	
	    public static T Register<T>(string key) where T : ModPreference, new() {
		    TypeMapping[typeof(T).FullName] = typeof(T);
		    T instance = new T();
		    instance.Key = key;
		    Preferences.Add(key, instance);
		    return instance;
	    }		
	
	    public static T Get<T>(string key) where T : ModPreference {
		    return (T)Preferences[key];
	    }
    
        public static void ReadFromFile(string file) {
		    Preferences = new Dictionary<string, ModPreference>();
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            using(BinaryReader reader = new BinaryReader(fileStream))
            {
                int count = reader.ReadInt32();
                for(int i = 0; i < count; i++) {
				    string type = reader.ReadString();
				    string key = reader.ReadString();
                    int size = reader.ReadInt32();
                    byte[] preferenceBytes = reader.ReadBytes(size);
				    ModPreference pref = FromBytes(type, key, preferenceBytes);
				    Preferences.Add(pref.Key, pref);
                }
            }
        }
	
        public static void WriteToFile(string file) {
            using(FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            using(BinaryWriter writer = new BinaryWriter(fileStream))
            {
			    writer.Write((Int32)Preferences.Values.Count);
			    foreach(var pref in Preferences.Values) {
				    writer.Write(pref.GetType().FullName);
				    writer.Write(pref.Key);
				    byte[] bytes = ToBytes(pref);
				    writer.Write(bytes.Length);
				    writer.Write(bytes);
			    }
            }
        }
    
        public static ModPreference FromBytes(string type, string key, byte[] bytes) {
            if(!TypeMapping.ContainsKey(type))
                return null;
		
		    Type prefType = TypeMapping[type];
		    ModPreference pref = (ModPreference)Activator.CreateInstance(prefType);
		
            using(MemoryStream memoryStream = new MemoryStream(bytes))
            using(BinaryReader reader = new BinaryReader(memoryStream))
            {
			    pref.Key = key;
			    pref.Deserialize(reader);
                return pref;
            }
    }
    
        public static byte[] ToBytes(ModPreference pref) {
            using(MemoryStream memoryStream = new MemoryStream())
            using(BinaryWriter writer = new BinaryWriter(memoryStream))
            {
			    pref.Serialize(writer);
			    writer.Flush();
			    return memoryStream.ToArray();
            }
        }
    
}
    public abstract class ModPreference {
	    public string Key;
        public abstract void Deserialize(BinaryReader reader);
        public abstract void Serialize(BinaryWriter writer);   
	    public ModPreference() { } 
    }
}