using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KitchenLib.Utils
{
	public class FontUtils
	{
		private static readonly Dictionary<string, Font> FontIndex = new Dictionary<string, Font>();
		private static readonly Dictionary<string, TMP_FontAsset> TMPFontIndex = new Dictionary<string, TMP_FontAsset>();
		public static void SetupFontIndex()
		{
			if (FontIndex.Count > 0 && TMPFontIndex.Count > 0)
				return;

			FontIndex.Clear();
			TMPFontIndex.Clear();

			foreach (Font font in Resources.FindObjectsOfTypeAll(typeof(Font)))
			{
				if (!FontIndex.ContainsKey(font.name))
				{
					FontIndex.Add(font.name, font);
				}
			}

			foreach (TMP_FontAsset font in Resources.FindObjectsOfTypeAll(typeof(TMP_FontAsset)))
			{
				if (!TMPFontIndex.ContainsKey(font.name))
				{
					TMPFontIndex.Add(font.name, font);
				}
			}
		}


		public static TMP_FontAsset GetExistingTMPFont(string font)
		{
			if (TMPFontIndex.ContainsKey(font))
			{
				return TMPFontIndex[font];
			}
			return null;
		}

		public static Font GetExistingFont(string font)
		{
			if (FontIndex.ContainsKey(font))
			{
				return FontIndex[font];
			}
			return null;
		}
	}
}
