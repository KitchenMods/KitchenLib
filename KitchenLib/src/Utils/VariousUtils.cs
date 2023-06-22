using System;
using System.Collections.Generic;
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
	}
}