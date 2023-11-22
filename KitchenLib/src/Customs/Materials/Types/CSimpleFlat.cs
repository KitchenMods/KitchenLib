using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CSimpleFlat : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CSimpleFlat;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;
		public virtual bool _Highlight { get; set; } = false;
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
		[JsonIgnore]
		public virtual Vector4 _OverlayTextureScale { get; set; } = Vector4.zero;
		public float _OverlayTextureScaleX = 0.0f;
		public float _OverlayTextureScaleY = 0.0f;
		[JsonIgnore]
		public virtual Color _OverlayColour { get; set; } = Color.black;
		public float _OverlayColourX = 0.0f;
		public float _OverlayColourY = 0.0f;
		public float _OverlayColourZ = 0.0f;
		public virtual float _Flatness { get; set; } = 0.0f;
		[JsonIgnore]
		public virtual Texture _Overlay { get; set; }
		public string _OverlayAsBase64 = "";

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Simple Flat"));

			result.SetColor("_Color0", _Color);
			result.SetInt("_Highlight", _Highlight ? 1 : 0);
			result.SetInt("_HasTextureOverlay", _HasTextureOverlay ? 1 : 0);
			if (!_HasTextureOverlay)
				result.DisableKeyword("_HASTEXTUREOVERLAY_ON");
			else
				result.EnableKeyword("_HASTEXTUREOVERLAY_ON");
			result.SetTexture("_Overlay", _Overlay);
			result.SetFloat("_Shininess", _Shininess);
			result.SetFloat("_OverlayLowerBound", _OverlayLowerBound);
			result.SetFloat("_OverlayUpperBound", _OverlayUpperBound);
			result.SetFloat("_OverlayScale", _OverlayScale);
			result.SetFloat("_OverlayMin", _OverlayMin);
			result.SetFloat("_OverlayMax", _OverlayMax);
			result.SetVector("_OverlayOffset", _OverlayOffset);
			result.SetVector("_OverlayTextureScale", _OverlayTextureScale);
			result.SetColor("_OverlayColour", _OverlayColour);
			result.SetFloat("_Flatness", _Flatness);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, 0);
			_OverlayOffset = new Vector4(_OverlayOffsetX, _OverlayOffsetY, 0, 0);
			_OverlayTextureScale = new Vector4(_OverlayTextureScaleX, _OverlayTextureScaleY, 0, 0);
			_OverlayColour = new Vector4(_OverlayColourX, _OverlayColourY, _OverlayColourZ, 0);
			_Overlay = ResourceUtils.LoadTextureFromBase64(_OverlayAsBase64);
		}
		private static string overlayFile = "";
		static IMColorPicker mainColorPicker;
		static IMColorPicker overlayColorPicker;

		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(overlayColorPicker == null)
				overlayColorPicker = new IMColorPicker();
			
			Vector4 _Color0 = material.GetVector("_Color0");
			Vector4 _OverlayColour = material.GetVector("_OverlayColour");
			Vector4 _OverlayTextureScale = material.GetVector("_OverlayTextureScale");
			Vector4 _OverlayOffset = material.GetVector("_OverlayOffset");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Color0 = mainColorPicker.DrawColorPicker(_Color0);
			material.SetVector("_Color0", _Color0);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 0, 159, 20));
			GUILayout.Label("Overlay Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 20, 159, 140));
			_OverlayColour = overlayColorPicker.DrawColorPicker(_OverlayColour);
			material.SetVector("_OverlayColour", _OverlayColour);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 0, 477, 20));
			GUILayout.Label("----- Material Overlay -----");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 20, 159, 20));
			material.SetInt("_HasTextureOverlay", GUILayout.Toggle(material.GetInt("_HasTextureOverlay") == 1, "Has Overlay") ? 1 : 0);
			if (material.GetInt("_HasTextureOverlay") == 0)
				material.DisableKeyword("_HASTEXTUREOVERLAY_ON");
			else
				material.EnableKeyword("_HASTEXTUREOVERLAY_ON");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 20, 159, 20));
			GUILayout.Label("Texture File");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(636, 20, 159, 20));
			overlayFile = GUILayout.TextField(overlayFile);
			if (File.Exists(overlayFile))
				material.SetTexture("_Overlay", ResourceUtils.LoadTextureFromFile(overlayFile));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 40, 110, 20));
			GUILayout.Label("Lower Bound");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 40, 49, 20));
			material.SetFloat("_OverlayLowerBound", float.Parse(GUILayout.TextField(material.GetFloat("_OverlayLowerBound").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 45, 318, 20));
			material.SetFloat("_OverlayLowerBound", GUILayout.HorizontalSlider(material.GetFloat("_OverlayLowerBound"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 60, 110, 20));
			GUILayout.Label("Upper Bound");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 60, 49, 20));
			material.SetFloat("_OverlayUpperBound", float.Parse(GUILayout.TextField(material.GetFloat("_OverlayUpperBound").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 65, 318, 20));
			material.SetFloat("_OverlayUpperBound", GUILayout.HorizontalSlider(material.GetFloat("_OverlayUpperBound"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 80, 110, 20));
			GUILayout.Label("Scale");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 80, 49, 20));
			material.SetFloat("_OverlayScale", float.Parse(GUILayout.TextField(material.GetFloat("_OverlayScale").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 85, 318, 20));
			material.SetFloat("_OverlayScale", GUILayout.HorizontalSlider(material.GetFloat("_OverlayScale"), 0.0f, 10.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 100, 110, 20));
			GUILayout.Label("Min");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 100, 49, 20));
			material.SetFloat("_OverlayMin", float.Parse(GUILayout.TextField(material.GetFloat("_OverlayMin").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 105, 318, 20));
			material.SetFloat("_OverlayMin", GUILayout.HorizontalSlider(material.GetFloat("_OverlayMin"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 120, 110, 20));
			GUILayout.Label("Max");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 120, 49, 20));
			material.SetFloat("_OverlayMax", float.Parse(GUILayout.TextField(material.GetFloat("_OverlayMax").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 125, 318, 20));
			material.SetFloat("_OverlayMax", GUILayout.HorizontalSlider(material.GetFloat("_OverlayMax"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 140, 110, 20));
			GUILayout.Label("Texture Scale");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 140, 49, 20));
			_OverlayTextureScale.x = float.Parse(GUILayout.TextField(_OverlayTextureScale.x.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 145, 318, 20));
			_OverlayTextureScale.x = GUILayout.HorizontalSlider(_OverlayTextureScale.x, 0.0f, 100.0f);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 160, 49, 20));
			_OverlayTextureScale.y = float.Parse(GUILayout.TextField(_OverlayTextureScale.y.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 165, 318, 20));
			_OverlayTextureScale.y = GUILayout.HorizontalSlider(_OverlayTextureScale.y, 0.0f, 100.0f);
			GUILayout.EndArea();
			
			material.SetVector("_OverlayTextureScale", _OverlayTextureScale);
			
			GUILayout.BeginArea(new Rect(318, 180, 110, 20));
			GUILayout.Label("Overlay Offset");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 180, 49, 20));
			_OverlayOffset.x = float.Parse(GUILayout.TextField(_OverlayOffset.x.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 185, 318, 20));
			_OverlayOffset.x = GUILayout.HorizontalSlider(_OverlayOffset.x, 0.0f, 1.0f);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 200, 49, 20));
			_OverlayOffset.y = float.Parse(GUILayout.TextField(_OverlayOffset.y.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 205, 318, 20));
			_OverlayOffset.y = GUILayout.HorizontalSlider(_OverlayOffset.y, 0.0f, 1.0f);
			GUILayout.EndArea();
			
			material.SetVector("_OverlayOffset", _OverlayOffset);
			
			
			
			GUILayout.BeginArea(new Rect(318, 240, 477, 20));
			GUILayout.Label("----- Material Misc -----");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 260, 110, 20));
			GUILayout.Label("Shininess");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 260, 49, 20));
			material.SetFloat("_Shininess", float.Parse(GUILayout.TextField(material.GetFloat("_Shininess").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 265, 318, 20));
			material.SetFloat("_Shininess", GUILayout.HorizontalSlider(material.GetFloat("_Shininess"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 280, 110, 20));
			GUILayout.Label("Flatness");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 280, 49, 20));
			material.SetFloat("_Flatness", float.Parse(GUILayout.TextField(material.GetFloat("_Flatness").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 285, 318, 20));
			material.SetFloat("_Flatness", GUILayout.HorizontalSlider(material.GetFloat("_Flatness"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CSimpleFlat result = new CSimpleFlat();
				result._ColorX = material.GetVector("_Color0").x;
				result._ColorY = material.GetVector("_Color0").y;
				result._ColorZ = material.GetVector("_Color0").z;
				result._Highlight = material.GetInt("_Highlight") == 1;
				result._HasTextureOverlay = material.GetInt("_HasTextureOverlay") == 1;
				result._Shininess = material.GetFloat("_Shininess");
				result._OverlayLowerBound = material.GetFloat("_OverlayLowerBound");
				result._OverlayUpperBound = material.GetFloat("_OverlayUpperBound");
				result._OverlayScale = material.GetFloat("_OverlayScale");
				result._OverlayMin = material.GetFloat("_OverlayMin");
				result._OverlayMax = material.GetFloat("_OverlayMax");
				result._OverlayOffsetX = material.GetVector("_OverlayOffset").x;
				result._OverlayOffsetY = material.GetVector("_OverlayOffset").y;
				result._OverlayTextureScaleX = material.GetVector("_OverlayTextureScale").x;
				result._OverlayTextureScaleY = material.GetVector("_OverlayTextureScale").y;

				result._OverlayColourX = material.GetVector("_OverlayColour").x;
				result._OverlayColourY = material.GetVector("_OverlayColour").y;
				result._OverlayColourZ = material.GetVector("_OverlayColour").z;
				result._Flatness = material.GetFloat("_Flatness");
				result._OverlayAsBase64 = imgtob64(material.GetTexture("_Overlay"));

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
