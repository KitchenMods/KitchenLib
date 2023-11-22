using Newtonsoft.Json;
using System;
using System.IO;
using imColorPicker;
using KitchenLib.Interfaces;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CNewspaper : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CNewspaper;
		
		[JsonIgnore]
		public virtual Texture _Photograph { get; set; }
		public string _PhotographAsBase64 = "";
		private static string overlayFile = "";
		
		public float _Alpha = 1;
		public float _BlowoutScale = 0;
		public float _BlowoutOffset = 0;
		
		[JsonIgnore]
		public virtual Color _Colour { get; set; } = Color.black;
		public float _ColourX = 0.0f;
		public float _ColourY = 0.0f;
		public float _ColourZ = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Newspaper"));

			result.SetTexture("_Overlay", _Photograph);
			result.SetFloat("_Alpha", _Alpha);
			result.SetFloat("_BlowoutScale", _BlowoutScale);
			result.SetColor("_Colour", _Colour);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Photograph = ResourceUtils.LoadTextureFromBase64(_PhotographAsBase64);
			_Colour = new Color(_ColourX, _ColourY, _ColourZ);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			
			Vector4 _Colour = material.GetVector("_Colour");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Colour = mainColorPicker.DrawColorPicker(_Colour);
			material.SetVector("_Colour", _Colour);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 20, 159, 20));
			GUILayout.Label("Texture File");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 20, 159, 20));
			overlayFile = GUILayout.TextField(overlayFile);
			if (File.Exists(overlayFile))
				material.SetTexture("_Photograph", ResourceUtils.LoadTextureFromFile(overlayFile));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 40, 110, 20));
			GUILayout.Label("Alpha");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 40, 49, 20));
			material.SetFloat("_Alpha", float.Parse(GUILayout.TextField(material.GetFloat("_Alpha").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 45, 318, 20));
			material.SetFloat("_Alpha", GUILayout.HorizontalSlider(material.GetFloat("_Alpha"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 60, 110, 20));
			GUILayout.Label("Blowout Scale");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 60, 49, 20));
			material.SetFloat("_BlowoutScale", float.Parse(GUILayout.TextField(material.GetFloat("_BlowoutScale").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 65, 318, 20));
			material.SetFloat("_BlowoutScale", GUILayout.HorizontalSlider(material.GetFloat("_BlowoutScale"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 80, 110, 20));
			GUILayout.Label("Blowout Offset");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 80, 49, 20));
			material.SetFloat("_BlowoutOffset", float.Parse(GUILayout.TextField(material.GetFloat("_BlowoutOffset").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 85, 318, 20));
			material.SetFloat("_BlowoutOffset", GUILayout.HorizontalSlider(material.GetFloat("_BlowoutOffset"), 0.0f, 1.0f));
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CNewspaper result = new CNewspaper();
				result._ColourX = material.GetVector("_Colour").x;
				result._ColourY = material.GetVector("_Colour").y;
				result._ColourZ = material.GetVector("_Colour").z;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
