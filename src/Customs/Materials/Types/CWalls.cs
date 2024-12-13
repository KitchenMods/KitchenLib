using KitchenLib.Utils;
using Newtonsoft.Json;
using System.IO;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CWalls : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CWalls;
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		public float _Color0A = 0.0f;
		[JsonIgnore]
		public virtual Color _Colour2 { get; set; } = Color.black;
		public float _Colour2X = 0.0f;
		public float _Colour2Y = 0.0f;
		public float _Colour2Z = 0.0f;
		public float _Colour2A = 0.0f;
		public virtual bool _Highlight { get; set; } = false;
		public virtual bool _TextureByUV { get; set; } = false;
		public virtual bool _VaryByUV { get; set; } = false;
		public virtual bool _HasTextureOverlay { get; set; } = false;
		public virtual float _Shininess { get; set; } = 0.0f;
		public virtual float _OverlayLowerBound { get; set; } = 0.0f;
		public virtual float _OverlayUpperBound { get; set; } = 0.0f;
		public virtual float _OverlayScale { get; set; } = 0.0f;
		public virtual float _OverlayMin { get; set; } = 0.0f;
		public virtual float _OverlayMax { get; set; } = 0.0f;
		[JsonIgnore]
		public virtual Vector4 _OverlayOffset { get; set; } = Vector4.zero;
		public float _OverlayOffsetX = 0.0f;
		public float _OverlayOffsetY = 0.0f;
		public float _OverlayOffsetZ = 0.0f;
		public float _OverlayOffsetA = 0.0f;
		[JsonIgnore]
		public virtual Vector4 _OverlayTextureScale { get; set; } = Vector4.zero;
		public float _OverlayTextureScaleX = 0.0f;
		public float _OverlayTextureScaleY = 0.0f;
		public float _OverlayTextureScaleZ = 0.0f;
		public float _OverlayTextureScaleA = 0.0f;
		public virtual float _Flatness { get; set; } = 0.0f;
		[JsonIgnore]
		public virtual Texture _Overlay { get; set; }
		public string _OverlayAsBase64 = "";

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Walls"));

			result.SetColor("_Color0", _Color0);
			result.SetColor("_Colour2", _Colour2);
			result.SetInt("_Highlight", _Highlight ? 1 : 0);
			result.SetInt("_HasTextureOverlay", _HasTextureOverlay ? 1 : 0);
			if (!_HasTextureOverlay)
				result.DisableKeyword("_HASTEXTUREOVERLAY_ON");
			else
				result.EnableKeyword("_HASTEXTUREOVERLAY_ON");
			
			
			result.SetInt("_TextureByUV", _TextureByUV ? 1 : 0);
			if (!_TextureByUV)
				result.DisableKeyword("_TEXTUREBYUV_ON");
			else
				result.EnableKeyword("_TEXTUREBYUV_ON");
			
			
			result.SetInt("_VaryByUV", _VaryByUV ? 1 : 0);
			if (!_VaryByUV)
				result.DisableKeyword("_VARYBYUV_ON");
			else
				result.EnableKeyword("_VARYBYUV_ON");
			result.SetTexture("_Overlay", _Overlay);
			result.SetFloat("_Shininess", _Shininess);
			result.SetFloat("_OverlayLowerBound", _OverlayLowerBound);
			result.SetFloat("_OverlayUpperBound", _OverlayUpperBound);
			result.SetFloat("_OverlayScale", _OverlayScale);
			result.SetFloat("_OverlayMin", _OverlayMin);
			result.SetFloat("_OverlayMax", _OverlayMax);
			result.SetVector("_OverlayOffset", _OverlayOffset);
			result.SetVector("_OverlayTextureScale", _OverlayTextureScale);
			result.SetFloat("_Flatness", _Flatness);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, _Color0A);
			_Colour2 = new Vector4(_Colour2X, _Colour2Y, _Colour2Z, _Colour2A);
			_OverlayOffset = new Vector4(_OverlayOffsetX, _OverlayOffsetY, _OverlayOffsetZ, _OverlayOffsetA);
			_OverlayTextureScale = new Vector4(_OverlayTextureScaleX, _OverlayTextureScaleY, _OverlayTextureScaleZ, _OverlayTextureScaleA);
			_Overlay = ResourceUtils.LoadTextureFromBase64(_OverlayAsBase64);
		}
		IMColorPicker mainColorPicker;
		IMColorPicker subColorPicker;
		private static string overlayFile = "";

		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			
			material.SetColor("_Colour2", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Colour2")));
			material.SetColor("_Color0", DrawColorModule(new Rect(152, 2, 146, 186), subColorPicker, "Secondary Color", material.GetVector("_Color0")));
			
			overlayFile = DrawTextureModule(new Rect(452, 2, 292, 207),"_Overlay", overlayFile, material, "_Overlay");
			
			material.SetFloat("_Shininess", DrawSliderModule(new Rect(2, 199, 446, 24), "_Shininess", material.GetFloat("_Shininess"), 0, 1));
			material.SetFloat("_OverlayLowerBound", DrawSliderModule(new Rect(2, 229, 446, 24), "_OverlayLowerBound", material.GetFloat("_OverlayLowerBound"), 0, 1));
			material.SetFloat("_OverlayUpperBound", DrawSliderModule(new Rect(2, 259, 446, 24), "_OverlayUpperBound", material.GetFloat("_OverlayUpperBound"), 0, 1));
			material.SetFloat("_OverlayScale", DrawSliderModule(new Rect(2, 289, 446, 24), "_OverlayScale", material.GetFloat("_OverlayScale"), 0, 1000));
			material.SetFloat("_OverlayMin", DrawSliderModule(new Rect(2, 319, 446, 24), "_OverlayMin", material.GetFloat("_OverlayMin"), 0, 1));
			material.SetFloat("_OverlayMax", DrawSliderModule(new Rect(2, 349, 446, 24), "_OverlayMax", material.GetFloat("_OverlayMax"), 0, 1));
			material.SetFloat("_Flatness", DrawSliderModule(new Rect(2, 379, 446, 24), "_Flatness", material.GetFloat("_Flatness"), 0, 1));

			_OverlayOffsetX = DrawSliderModule(new Rect(2, 419, 446, 24), "_OverlayOffsetX", material.GetVector("_OverlayOffset").x, 0, 1);
			_OverlayOffsetY = DrawSliderModule(new Rect(2, 449, 446, 24), "_OverlayOffsetY", material.GetVector("_OverlayOffset").y, 0, 1);
			_OverlayOffsetZ = DrawSliderModule(new Rect(2, 479, 446, 24), "_OverlayOffsetZ", material.GetVector("_OverlayOffset").z, 0, 1);
			_OverlayOffsetA = DrawSliderModule(new Rect(2, 509, 446, 24), "_OverlayOffsetA", material.GetVector("_OverlayOffset").w, 0, 1);

			_OverlayTextureScaleX = DrawSliderModule(new Rect(2, 549, 446, 24), "_OverlayTextureScaleX", material.GetVector("_OverlayTextureScale").x, 0, 100, 35, 15, 50);
			_OverlayTextureScaleY = DrawSliderModule(new Rect(2, 579, 446, 24), "_OverlayTextureScaleY", material.GetVector("_OverlayTextureScale").y, 0, 100, 35, 15, 50);
			_OverlayTextureScaleZ = DrawSliderModule(new Rect(2, 609, 446, 24), "_OverlayTextureScaleZ", material.GetVector("_OverlayTextureScale").z, 0, 100, 35, 15, 50);
			_OverlayTextureScaleA = DrawSliderModule(new Rect(2, 639, 446, 24), "_OverlayTextureScaleA", material.GetVector("_OverlayTextureScale").w, 0, 100, 35, 15, 50);
			
			material.SetVector("_OverlayTextureScale", new Vector4(_OverlayTextureScaleX, _OverlayTextureScaleY, _OverlayTextureScaleZ, _OverlayTextureScaleA));
			material.SetVector("_OverlayOffset", new Vector4(_OverlayOffsetX, _OverlayOffsetY, _OverlayOffsetZ, _OverlayOffsetA));
			
			material.SetFloat("_TextureByUV", DrawToggleModule(new Rect(462, 229, 146, 25), "_TextureByUV", material.GetFloat("_TextureByUV") == 1) ? 1 : 0);
			material.SetFloat("_VaryByUV", DrawToggleModule(new Rect(462, 259, 146, 25), "_VaryByUV", material.GetFloat("_VaryByUV") == 1) ? 1 : 0);
			material.SetFloat("_HasTextureOverlay", DrawToggleModule(new Rect(462, 289, 146, 25), "_HasTextureOverlay", material.GetFloat("_HasTextureOverlay") == 1) ? 1 : 0);
			
			
			if (material.GetInt("_TextureByUV") != 1)
				material.DisableKeyword("_TEXTUREBYUV_ON");
			else
				material.EnableKeyword("_TEXTUREBYUV_ON");
			
			
			if (material.GetInt("_VaryByUV") != 1)
				material.DisableKeyword("_VARYBYUV_ON");
			else
				material.EnableKeyword("_VARYBYUV_ON");
			
			
			if (material.GetInt("_HasTextureOverlay") != 1)
				material.DisableKeyword("_HASTEXTUREOVERLAY_ON");
			else
				material.EnableKeyword("_HASTEXTUREOVERLAY_ON");
		}

		public string Export(Material material)
		{
			CWalls result = new CWalls();
			result._Color0X = material.GetColor("_Color0").r;
			result._Color0Y = material.GetColor("_Color0").g;
			result._Color0Z = material.GetColor("_Color0").b;
			result._Color0A = material.GetColor("_Color0").a;

			result._Colour2X = material.GetColor("_Colour2").r;
			result._Colour2Y = material.GetColor("_Colour2").g;
			result._Colour2Z = material.GetColor("_Colour2").b;
			result._Colour2A = material.GetColor("_Colour2").a;

			result._Highlight = material.GetInt("_Highlight") == 1;
			result._HasTextureOverlay = material.GetInt("_HasTextureOverlay") == 1;
			result._TextureByUV = material.GetInt("_TextureByUV") == 1;
			result._VaryByUV = material.GetInt("_VaryByUV") == 1;
			result._Shininess = material.GetFloat("_Shininess");
			result._OverlayLowerBound = material.GetFloat("_OverlayLowerBound");
			result._OverlayUpperBound = material.GetFloat("_OverlayUpperBound");
			result._OverlayScale = material.GetFloat("_OverlayScale");
			result._OverlayMin = material.GetFloat("_OverlayMin");
			result._OverlayMax = material.GetFloat("_OverlayMax");
			result._OverlayOffsetX = material.GetVector("_OverlayOffset").x;
			result._OverlayOffsetY = material.GetVector("_OverlayOffset").y;
			result._OverlayOffsetZ = material.GetVector("_OverlayOffset").z;
			result._OverlayOffsetA = material.GetVector("_OverlayOffset").w;
			result._OverlayTextureScaleX = material.GetVector("_OverlayTextureScale").x;
			result._OverlayTextureScaleY = material.GetVector("_OverlayTextureScale").y;
			result._OverlayTextureScaleZ = material.GetVector("_OverlayTextureScale").z;
			result._OverlayTextureScaleA = material.GetVector("_OverlayTextureScale").w;
			result._Flatness = material.GetFloat("_Flatness");
			result._OverlayAsBase64 = imgtob64(material.GetTexture("_Overlay"));

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
