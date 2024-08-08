using KitchenMods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class ResourceUtils
	{
		public static Texture2D LoadTextureFromBase64(string base64)
		{
			byte[] bytes = Convert.FromBase64String(base64);
			Texture2D image = LoadTextureRaw(bytes);
			return image;
		}

		public static Texture2D LoadTextureRaw(byte[] file)
		{
			if (file.Count() > 0)
			{
				Texture2D Tex2D = new Texture2D(2, 2);
				if (Tex2D.LoadImage(file))
				{
					return Tex2D;
				}
			}
			return null;
		}

		private static Dictionary<int, Texture2D> loadTextureFromFileCache = new Dictionary<int, Texture2D>(); 

		public static Texture2D LoadTextureFromFile(string FilePath)
		{
			if (loadTextureFromFileCache.ContainsKey(VariousUtils.GetID("FilePath")))
			{
				return loadTextureFromFileCache[VariousUtils.GetID("FilePath")];
			}
			else
			{
				if (File.Exists(FilePath))
				{
					loadTextureFromFileCache.Add(VariousUtils.GetID("FilePath"), LoadTextureRaw(File.ReadAllBytes(FilePath)));
					return loadTextureFromFileCache[VariousUtils.GetID("FilePath")];
				}
			}
			return null;
		}

		public static Texture2D LoadTextureFromResources(string resourcePath)
		{
			return LoadTextureRaw(GetResource(Assembly.GetCallingAssembly(), resourcePath));
		}

		public static Sprite LoadSpriteRaw(byte[] image, float PixelsPerUnit = 100.0f)
		{
			return LoadSpriteFromTexture(LoadTextureRaw(image), PixelsPerUnit);
		}

		public static Sprite LoadSpriteFromTexture(Texture2D SpriteTexture, float PixelsPerUnit = 100.0f)
		{
			if (SpriteTexture)
				return Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

			return null;
		}

		public static Sprite LoadSpriteFromFile(string FilePath, float PixelsPerUnit = 100.0f)
		{
			return LoadSpriteFromTexture(LoadTextureFromFile(FilePath), PixelsPerUnit);
		}

		public static Sprite LoadSpriteFromResources(string resourcePath, float PixelsPerUnit = 100.0f)
		{
			return LoadSpriteRaw(GetResource(Assembly.GetCallingAssembly(), resourcePath), PixelsPerUnit);
		}

		public static byte[] GetResource(Assembly asm, string ResourceName)
		{
			System.IO.Stream stream = asm.GetManifestResourceStream(ResourceName);
			byte[] data = new byte[stream.Length];
			stream.Read(data, 0, (int)stream.Length);
			return data;
		}

		public static string GetModsFolder()
		{
			return FolderModSource.ModsFolder;
		}

		public static string GetWorkshopFolder()
		{
			return "";
		}

		[Obsolete("User GetModsFolder or GetWorkshopFolder")]
		public static string FindModPath(Assembly assembly, AssetBundleLocation location)
		{
			string codeBase = assembly.CodeBase;
			UriBuilder uri = new UriBuilder(codeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			string mods = "";
			if (location == AssetBundleLocation.WorkshopFolder)
			{
				mods = Path.GetDirectoryName(path) + "\\..\\workshop\\content\\1599600\\";
			}
			if (location == AssetBundleLocation.ModsFolder)
			{
				mods = Path.GetDirectoryName(path) + "\\PlateUp\\PlateUp\\Mods\\";
			}

			if (!Directory.Exists(mods))
				return "";

			string[] modFolders = Directory.GetDirectories(mods);
			foreach (string folder in modFolders)
			{
				if (File.Exists(folder + "\\" + assembly.GetName().Name + ".dll"))
					return folder;
			}
			return "";
		}
	}

	public enum AssetBundleLocation
	{
		WorkshopFolder,
		ModsFolder
	}
}