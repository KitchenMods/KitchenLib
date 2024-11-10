#define USE_GZIP_COMPRESSION

using KitchenLib.Event;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace KitchenLib.Utils
{
	/*
     * Preferences System was 99% made by @R4wizard
     */

	[Obsolete("Please use PreferenceManager")]
    public static class PreferenceUtils {
	
	    public static Dictionary<string, BasePreference> Preferences = new Dictionary<string, BasePreference>();
    
	    public static Dictionary<string, Type> TypeMapping = new Dictionary<string, Type>();
	
	    public static T Register<T>(string modID, string key, string name) where T : BasePreference, new() {
			Main.LogInfo("Registering Preference: " + modID + ":" + key);
		    TypeMapping[typeof(T).FullName] = typeof(T);
			T instance = new T();
			instance.ModID = modID;
			instance.Key = key;
			instance.DisplayName = name;
			if (Preferences.ContainsKey(modID + ":" + key))
			{
				Main.LogWarning("[WARN] " + modID + ":" + key + " already exists! Has another mod already loaded it?");
			}
			else
			{
				Preferences.Add(modID + ":" + key, instance);
			}
			return instance;
		}

		public static T Get<T>(string modID, string key) where T : BasePreference
		{
			if (Preferences.ContainsKey(modID + ":" + key))
				return (T)Preferences[modID + ":" + key];
			return default(T);
		}

		public static void Load(string file = "UserData/KitchenLib/preferences.dat")
		{
			file = Path.Combine(Application.persistentDataPath, file);
			if (File.Exists(file))
				ReadFromFile(file);
		}

		public static void ReadFromFile(string file)
		{
#if USE_GZIP_COMPRESSION
			using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
			using (var stream = new GZipStream(fileStream, CompressionMode.Decompress))
#else
                using(var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
#endif
			using (var reader = new BinaryReader(stream))
			{
				if (Preferences == null)
					Preferences = new Dictionary<string, BasePreference>();

				byte[] magicBytes = reader.ReadBytes(4);
				if (magicBytes.Length == 4)
				{
					if (magicBytes[0] != 'K' || magicBytes[1] != 'L' || magicBytes[2] != 'I' || magicBytes[3] != 'B')
						throw new Exception("Not a valid KitchenLib settings file.");
				}
				else
					throw new Exception("Not a valid KitchenLib settings file.2");

				int count = reader.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					string type = reader.ReadString();
					string modID = reader.ReadString();
					string key = reader.ReadString();
					string DisplayName = reader.ReadString();
					int size = reader.ReadInt32();
					byte[] preferenceBytes = reader.ReadBytes(size);
					BasePreference pref = FromBytes(type, modID, key, DisplayName, preferenceBytes);
					if (pref != null)
					{
						if (Preferences.ContainsKey(pref.ModID + ":" + pref.Key))
							Preferences[pref.ModID + ":" + pref.Key] = pref;
						else
							Preferences.Add(pref.ModID + ":" + pref.Key, pref);
					}
				}
			}
		}
		public static void Save(string file = "UserData/KitchenLib/preferences.dat")
		{
			file = Path.Combine(Application.persistentDataPath, file);
			//Make sure file path Exists
			string path = Path.GetDirectoryName(file);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);


			WriteToFile(file);

			PreferencesSaveArgs mainMenuViewEvent = new PreferencesSaveArgs();
			EventUtils.InvokeEvent(nameof(Events.PreferencesSaveEvent), Events.PreferencesSaveEvent?.GetInvocationList(), null, mainMenuViewEvent);
		}
		public static void WriteToFile(string file)
		{
#if USE_GZIP_COMPRESSION
			using (var fileStream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
			using (var stream = new GZipStream(fileStream, CompressionMode.Compress))
#else
                using(var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
#endif
			using (var writer = new BinaryWriter(stream))
			{
				writer.Write((byte)'K');
				writer.Write((byte)'L');
				writer.Write((byte)'I');
				writer.Write((byte)'B');
				writer.Write((Int32)Preferences.Values.Count);
				foreach (var pref in Preferences.Values)
				{
					writer.Write(pref.GetType().FullName);
					writer.Write(pref.ModID);
					writer.Write(pref.Key);
					writer.Write(pref.DisplayName);
					byte[] bytes = ToBytes(pref);
					writer.Write(bytes.Length);
					writer.Write(bytes);
				}
			}
		}

		public static BasePreference FromBytes(string type, string modID, string key, string displayName, byte[] bytes)
		{
			if (!TypeMapping.ContainsKey(type))
				return null;

			Type prefType = TypeMapping[type];
			BasePreference pref = (BasePreference)Activator.CreateInstance(prefType);

			using (MemoryStream memoryStream = new MemoryStream(bytes))
			using (BinaryReader reader = new BinaryReader(memoryStream))
			{
				pref.ModID = modID;
				pref.Key = key;
				pref.DisplayName = displayName;
				pref.Deserialize(reader);
				return pref;
			}
		}

		public static byte[] ToBytes(BasePreference pref)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			using (BinaryWriter writer = new BinaryWriter(memoryStream))
			{
				pref.Serialize(writer);
				writer.Flush();
				return memoryStream.ToArray();
			}
		}

	}

	public static class BinaryReaderWriterExtensions
	{

		public static void Write<T>(this BinaryWriter writer, List<T> list) where T : IBinarySerializable
		{
			writer.Write((UInt32)list.Count);
			for (int i = 0; i < list.Count; i++)
				writer.Write(list[i]);
		}

		public static List<T> ReadList<T>(this BinaryReader reader) where T : IBinarySerializable, new()
		{
			var list = new List<T>();
			var count = reader.ReadUInt32();
			for (int i = 0; i < count; i++)
				list.Add(reader.Read<T>());
			return list;
		}

		public static void Write<T>(this BinaryWriter writer, T item) where T : IBinarySerializable
		{
			item.Serialize(writer);
		}

		public static T Read<T>(this BinaryReader reader) where T : IBinarySerializable, new()
		{
			var item = new T();
			item.Deserialize(reader);
			return item;
		}

	}
}