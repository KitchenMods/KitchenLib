using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CSimpleFlat : CustomMaterial
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
		public static void GUI(Material material)
		{
			Vector4 _Color0 = material.GetVector("_Color0");

			GUILayout.Label("_Color0");
			_Color0.x = GUILayout.HorizontalSlider(_Color0.x, 0.0f, 1.0f);
			_Color0.y = GUILayout.HorizontalSlider(_Color0.y, 0.0f, 1.0f);
			_Color0.z = GUILayout.HorizontalSlider(_Color0.z, 0.0f, 1.0f);
			material.SetVector("_Color0", _Color0);

			GUILayout.Label("_Highlight");
			material.SetInt("_Highlight", GUILayout.Toggle(material.GetInt("_Highlight") == 1, "") ? 1 : 0);

			GUILayout.Label("_HasTextureOverlay");
			material.SetInt("_HasTextureOverlay", GUILayout.Toggle(material.GetInt("_HasTextureOverlay") == 1, "") ? 1 : 0);
			if (material.GetInt("_HasTextureOverlay") == 0)
				material.DisableKeyword("_HASTEXTUREOVERLAY_ON");
			else
				material.EnableKeyword("_HASTEXTUREOVERLAY_ON");

			GUILayout.Label("_Overlay");
			overlayFile = GUILayout.TextField(overlayFile);
			if (File.Exists(overlayFile))
				material.SetTexture("_Overlay", ResourceUtils.LoadTextureFromFile(overlayFile));

			GUILayout.Label("_Shininess");
			material.SetFloat("_Shininess", GUILayout.HorizontalSlider(material.GetFloat("_Shininess"), 0.0f, 1.0f));

			GUILayout.Label("_OverlayLowerBound");
			material.SetFloat("_OverlayLowerBound", GUILayout.HorizontalSlider(material.GetFloat("_OverlayLowerBound"), 0.0f, 1.0f));

			GUILayout.Label("_OverlayUpperBound");
			material.SetFloat("_OverlayUpperBound", GUILayout.HorizontalSlider(material.GetFloat("_OverlayUpperBound"), 0.0f, 1.0f));

			GUILayout.Label("_OverlayScale");
			material.SetFloat("_OverlayScale", GUILayout.HorizontalSlider(material.GetFloat("_OverlayScale"), 0.0f, 1.0f));

			GUILayout.Label("_OverlayMin");
			material.SetFloat("_OverlayMin", GUILayout.HorizontalSlider(material.GetFloat("_OverlayMin"), 0.0f, 1.0f));

			GUILayout.Label("_OverlayMax");
			material.SetFloat("_OverlayMax", GUILayout.HorizontalSlider(material.GetFloat("_OverlayMax"), 0.0f, 1.0f));

			Vector4 _OverlayOffset = material.GetVector("_OverlayOffset");

			GUILayout.Label("_OverlayOffset");
			_OverlayOffset.x = GUILayout.HorizontalSlider(_OverlayOffset.x, 0.0f, 1.0f);
			_OverlayOffset.y = GUILayout.HorizontalSlider(_OverlayOffset.y, 0.0f, 1.0f);
			material.SetVector("_OverlayOffset", _OverlayOffset);

			Vector4 _OverlayTextureScale = material.GetVector("_OverlayTextureScale");

			GUILayout.Label("_OverlayTextureScale");
			_OverlayTextureScale.x = GUILayout.HorizontalSlider(_OverlayTextureScale.x, 0.0f, 100.0f);
			_OverlayTextureScale.y = GUILayout.HorizontalSlider(_OverlayTextureScale.y, 0.0f, 100.0f);
			material.SetVector("_OverlayTextureScale", _OverlayTextureScale);

			Vector4 _OverlayColour = material.GetVector("_OverlayColour");

			GUILayout.Label("_OverlayColour");
			_OverlayColour.x = GUILayout.HorizontalSlider(_OverlayColour.x, 0.0f, 1.0f);
			_OverlayColour.y = GUILayout.HorizontalSlider(_OverlayColour.y, 0.0f, 1.0f);
			_OverlayColour.z = GUILayout.HorizontalSlider(_OverlayColour.z, 0.0f, 1.0f);
			material.SetVector("_OverlayColour", _OverlayColour);

			GUILayout.Label("_Flatness");
			material.SetFloat("_Flatness", GUILayout.HorizontalSlider(material.GetFloat("_Flatness"), 0.0f, 1.0f));
		}

		public static void Export(Material material)
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
