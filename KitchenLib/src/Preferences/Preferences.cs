using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace KitchenLib
{
    public static class PreferencesRegistry {
	
	    public static Dictionary<string, BasePreference> Preferences = new Dictionary<string, BasePreference>();
    
	    public static Dictionary<string, Type> TypeMapping = new Dictionary<string, Type>();
	
	    public static T Register<T>(string modID, string key, string name) where T : BasePreference, new() {
		    TypeMapping[typeof(T).FullName] = typeof(T);
		    T instance = new T();
            instance.ModID = modID;
		    instance.Key = key;
            instance.DisplayName = name;
		    Preferences.Add(modID + ":" + key, instance);
		    return instance;
	    }		
	
	    public static T Get<T>(string modID, string key) where T : BasePreference {
		    return (T)Preferences[modID + ":" + key];
	    }

        public static void Load(string file = "UserData/KitchenLib/preferences.dat")
        {
            if (File.Exists(file))
                ReadFromFile(file);
        }

        private static void ReadFromFile(string file) {
		    //Preferences = new Dictionary<string, BasePreference>();
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            using(BinaryReader reader = new BinaryReader(fileStream))
            {
                int count = reader.ReadInt32();
                for(int i = 0; i < count; i++) {
				    string type = reader.ReadString();
                    string modID = reader.ReadString();
				    string key = reader.ReadString();
                    string DisplayName = reader.ReadString();
                    int size = reader.ReadInt32();
                    byte[] preferenceBytes = reader.ReadBytes(size);
				    BasePreference pref = FromBytes(type, modID, key, DisplayName, preferenceBytes);
                    if (Preferences.ContainsKey(pref.ModID + ":" + pref.Key))
                        Preferences[pref.ModID + ":" + pref.Key] = pref;
                    else
				        Preferences.Add(pref.ModID + ":" + pref.Key, pref);
                    Mod.Log("Loaded Value: " + ((BoolPreference)pref).Value);
                }
            }
        }
    
        public static void Save(string file = "UserData/KitchenLib/preferences.dat")
        {
            WriteToFile(file);
        }

        private static void WriteToFile(string file) {
            using(FileStream fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            using(BinaryWriter writer = new BinaryWriter(fileStream))
            {
			    writer.Write((Int32)Preferences.Values.Count);
			    foreach(var pref in Preferences.Values) {
				    writer.Write(pref.GetType().FullName);
                    writer.Write(pref.ModID);
				    writer.Write(pref.Key);
                    writer.Write(pref.DisplayName);
				    byte[] bytes = ToBytes(pref);
				    writer.Write(bytes.Length);
				    writer.Write(bytes);
                    Mod.Log("Saved Value: " + ((BoolPreference)pref).Value);
			    }
            }
        }
    
        public static BasePreference FromBytes(string type, string modID, string key, string displayName, byte[] bytes) {
            if(!TypeMapping.ContainsKey(type))
                return null;
		
		    Type prefType = TypeMapping[type];
		    BasePreference pref = (BasePreference)Activator.CreateInstance(prefType);
		
            using(MemoryStream memoryStream = new MemoryStream(bytes))
            using(BinaryReader reader = new BinaryReader(memoryStream))
            {
                pref.ModID = modID;
			    pref.Key = key;
                pref.DisplayName = displayName;
			    pref.Deserialize(reader);
                return pref;
            }
    }
    
        public static byte[] ToBytes(BasePreference pref) {
            using(MemoryStream memoryStream = new MemoryStream())
            using(BinaryWriter writer = new BinaryWriter(memoryStream))
            {
			    pref.Serialize(writer);
			    writer.Flush();
			    return memoryStream.ToArray();
            }
        }
    
}
    public abstract class BasePreference {
        public string ModID;
	    public string Key;
        public string DisplayName;
        public abstract void Deserialize(BinaryReader reader);
        public abstract void Serialize(BinaryWriter writer);   
	    public BasePreference() { } 
    }

    public class BoolPreference : BasePreference
    {
        public bool Value;
        public BoolPreference() : base() { }
        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Value);
        }
        public override void Deserialize(BinaryReader reader)
        {
            Value = reader.ReadBoolean();
        }
    }

    public class StringPreference : BasePreference
    {
        public string Value;
        public StringPreference() : base() { }
        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Value);
        }
        public override void Deserialize(BinaryReader reader)
        {
            Value = reader.ReadString();
        }
    }
}