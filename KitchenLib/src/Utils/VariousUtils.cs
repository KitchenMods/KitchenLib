using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

namespace KitchenLib.Utils
{
	public class VariousUtils
	{
		private static Dictionary<string, int> ids = new Dictionary<string, int>();
		public static int GetID(string name)
		{
			int ID = 0;
			if (ids.ContainsKey(name))
				return ids[name];
			while (ID == 0)
			{
				int x = GetInt32HashCode(name);
				if (!ids.ContainsValue(x))
				{
					ids.Add(name, x);
					ID = x;
				}
			}
			return ID;
		}

		public static Color HexToColor(string hex)
		{
			ColorUtility.TryParseHtmlString(hex, out Color color);
			return color;
		}
		
		private static System.Security.Cryptography.SHA1 hash = new System.Security.Cryptography.SHA1CryptoServiceProvider();
		private static int GetInt32HashCode(string strText)
		{
			if (string.IsNullOrEmpty(strText)) return 0;

			byte[] byteContents = Encoding.Unicode.GetBytes(strText);
			byte[] hashText = hash.ComputeHash(byteContents);
			uint hashCodeStart = BitConverter.ToUInt32(hashText, 0);
			uint hashCodeMedium = BitConverter.ToUInt32(hashText, 8);
			uint hashCodeEnd = BitConverter.ToUInt32(hashText, 16);
			var hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
			return BitConverter.ToInt32(BitConverter.GetBytes(uint.MaxValue - hashCode), 0);
		}
		internal static void CopyDirectory(string sourceDir, string destDir)
		{
			if (!Directory.Exists(sourceDir))
			{
				throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
			}

			if (!Directory.Exists(destDir))
			{
				Directory.CreateDirectory(destDir);
			}

			foreach (string file in Directory.GetFiles(sourceDir))
			{
				string destFile = Path.Combine(destDir, Path.GetFileName(file));
				File.Copy(file, destFile, true);
			}

			foreach (string subdir in Directory.GetDirectories(sourceDir))
			{
				string destSubdir = Path.Combine(destDir, Path.GetFileName(subdir));
				CopyDirectory(subdir, destSubdir);
			}
		}

		public static bool HasCommandlineArgument(string argument)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == argument)
				{
					return true;
				}
			}
			return false;
		}
	}
}