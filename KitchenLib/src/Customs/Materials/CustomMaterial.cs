using System;
using UnityEngine;
using KitchenLib.Utils;

namespace KitchenLib.Customs
{
	public enum MaterialType
	{
		SimpleFlat,
		SimpleTransparent,
		FlatImage
	}
	public abstract class CustomMaterial : BaseJson
	{
		public virtual void ConvertMaterial(out Material material) { material = null; }
		public override JsonType Type => JsonType.CustomMaterial;
		public virtual MaterialType MaterialType { get; set; }
		public virtual string Name { get; set; } = "";

		protected static string imgtob64(Texture texture)
		{
			if (texture == null)
				return "";
			if (!texture.isReadable)
				return "";
			string enc = Convert.ToBase64String(((Texture2D)texture).EncodeToPNG());
			return enc;
		}
	}
}
