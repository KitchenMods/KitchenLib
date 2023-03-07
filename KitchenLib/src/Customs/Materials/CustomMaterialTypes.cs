using KitchenLib.Utils;
using System;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomBaseMaterial : BaseJson
	{
		public virtual void ConvertMaterial(out Material material) { material = null; }
	}
	public class CustomSimpleFlat : CustomBaseMaterial
	{
		public override JsonType Type { get => JsonType.FlatColorMaterial; }
		public string Name;

		public float _Color0W;
		public float _Color0X;
		public float _Color0Y;
		public float _Color0Z;

		public int _Highlight;
		public string _OverlayBase64 = "";
		public float _HasTextureOverlay;
		public float _Shininess;
		public float _OverlayLowerBound;
		public float _OverlayUpperBound;
		public float _OverlayScale;
		public float _OverlayMin;
		public float _OverlayMax;

		public float _OverlayOffsetW, _OverlayOffsetX, _OverlayOffsetY, _OverlayOffsetZ;

		public float _OverlayTextureScaleW, _OverlayTextureScaleX, _OverlayTextureScaleY, _OverlayTextureScaleZ;

		public float _OverlayColourW, _OverlayColourX, _OverlayColourY, _OverlayColourZ;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Simple Flat"));

			result.name = Name;
			result.SetVector("_Color0", new Vector4(_Color0X, _Color0Y, _Color0Z, _Color0W));
			result.SetInt("_Highlight", _Highlight);

			if (_OverlayBase64 != "" && _OverlayBase64 != String.Empty)
				result.SetTexture("_Overlay", ResourceUtils.LoadTextureFromBase64(_OverlayBase64));
			result.SetFloat("_HasTextureOverlay", _HasTextureOverlay);
			if (_HasTextureOverlay == 0)
				result.DisableKeyword("_HASTEXTUREOVERLAY_ON");
			else
				result.EnableKeyword("_HASTEXTUREOVERLAY_ON");

			result.SetFloat("_Shininess", _Shininess);
			result.SetFloat("_OverlayLowerBound", _OverlayLowerBound);
			result.SetFloat("_OverlayUpperBound", _OverlayUpperBound);
			result.SetFloat("_OverlayScale", _OverlayScale);
			result.SetFloat("_OverlayMin", _OverlayMin);
			result.SetFloat("_OverlayMax", _OverlayMax);
			result.SetVector("_OverlayOffset", new Vector4(_OverlayOffsetX, _OverlayOffsetY, _OverlayOffsetZ, _OverlayOffsetW));
			result.SetVector("_OverlayTextureScale", new Vector4(_OverlayTextureScaleX, _OverlayTextureScaleY, _OverlayTextureScaleZ, _OverlayTextureScaleW));
			result.SetVector("_OverlayColour", new Vector4(_OverlayColourX, _OverlayColourY, _OverlayColourZ, _OverlayColourW));

			material = result;
		}
	}

	public class CustomSimpleTransparent : CustomBaseMaterial
	{
		public override JsonType Type { get => JsonType.TransparentMaterial; }
		public string Name;

		public float _ColorW, _ColorX, _ColorY, _ColorZ;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Simple Transparent"));

			result.name = Name;
			result.SetVector("_Color", new Vector4(_ColorX, _ColorY, _ColorZ, _ColorW));

			material = result;
		}
	}

	public class CustomFlatImage : CustomBaseMaterial
	{
		public override JsonType Type { get => JsonType.ImageMaterial; }
		public string Name;

		public string _ImageBase64 = "";
		public float _Alpha;
		public float _BlowoutScale1;
		public float _BlowoutOffset1;
		public float _IsBlowout;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Flat Image"));

			result.name = Name;
			if (_ImageBase64 != "" && _ImageBase64 != String.Empty)
				result.SetTexture("_Image", ResourceUtils.LoadTextureFromBase64(_ImageBase64));
			result.SetFloat("_Alpha", _Alpha);
			result.SetFloat("_BlowoutScale1", _BlowoutScale1);
			result.SetFloat("_BlowoutOffset1", _BlowoutOffset1);
			result.SetFloat("_IsBlowout", _IsBlowout);

			material = result;
		}
	}
}
