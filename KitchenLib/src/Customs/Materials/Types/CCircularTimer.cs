using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CCircularTimer : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CCircularTimer;
		[JsonIgnore]
		public virtual Color _Colour { get; set; } = Color.black;
		public float _ColourX = 0.0f;
		public float _ColourY = 0.0f;
		public float _ColourZ = 0.0f;
		
		public virtual Color _BackingColour { get; set; } = Color.black;
		public float _BackingColourX = 0.0f;
		public float _BackingColourY = 0.0f;
		public float _BackingColourZ = 0.0f;
		
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		
		public float _InnerRadius = 0.3f;
		public float _FadeInner = 0.03058824f;
		public float _BorderWidth = 0.02f;
		public float _OuterRadius = 0.5f;
		public float _IsWaitingPeriod = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Circular Timer"));

			result.SetColor("_Colour", _Colour);
			result.SetColor("_BackingColour", _BackingColour);
			result.SetColor("_Color0", _Color0);
			result.SetFloat("_InnerRadius", _InnerRadius);
			result.SetFloat("_FadeInner", _FadeInner);
			result.SetFloat("_BorderWidth", _BorderWidth);
			result.SetFloat("_OuterRadius", _OuterRadius);
			result.SetFloat("_IsWaitingPeriod", _IsWaitingPeriod);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Colour = new Vector4(_ColourX, _ColourY, _ColourZ, 0);
			_BackingColour = new Vector4(_BackingColourX, _BackingColourY, _BackingColourZ, 0);
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, 0);
		}
		IMColorPicker mainColorPicker;
		IMColorPicker backingColorPicker;
		IMColorPicker subColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(backingColorPicker == null)
				backingColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			Vector4 _Colour = material.GetVector("_Colour");
			Vector4 _BackingColour = material.GetVector("_BackingColour");
			Vector4 _Color0 = material.GetVector("_Color0");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Colour = mainColorPicker.DrawColorPicker(_Colour);
			material.SetVector("_Colour", _Colour);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 0, 159, 20));
			GUILayout.Label("Backing Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 20, 159, 140));
			_BackingColour = backingColorPicker.DrawColorPicker(_BackingColour);
			material.SetVector("_BackingColour", _BackingColour);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 0, 159, 20));
			GUILayout.Label("Sub Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 20, 159, 140));
			_Color0 = subColorPicker.DrawColorPicker(_Color0);
			material.SetVector("_Color0", _Color0);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 20, 110, 20));
			GUILayout.Label("Inner Radius");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 20, 49, 20));
			material.SetFloat("_InnerRadius", float.Parse(GUILayout.TextField(material.GetFloat("_InnerRadius").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 25, 318, 20));
			material.SetFloat("_InnerRadius", GUILayout.HorizontalSlider(material.GetFloat("_InnerRadius"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 40, 110, 20));
			GUILayout.Label("Fade Inner");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 40, 49, 20));
			material.SetFloat("_FadeInner", float.Parse(GUILayout.TextField(material.GetFloat("_FadeInner").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 45, 318, 20));
			material.SetFloat("_FadeInner", GUILayout.HorizontalSlider(material.GetFloat("_FadeInner"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 60, 110, 20));
			GUILayout.Label("Border Width");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 60, 49, 20));
			material.SetFloat("_BorderWidth", float.Parse(GUILayout.TextField(material.GetFloat("_BorderWidth").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 65, 318, 20));
			material.SetFloat("_BorderWidth", GUILayout.HorizontalSlider(material.GetFloat("_BorderWidth"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 80, 110, 20));
			GUILayout.Label("Outer Radius");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 80, 49, 20));
			material.SetFloat("_OuterRadius", float.Parse(GUILayout.TextField(material.GetFloat("_OuterRadius").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 85, 318, 20));
			material.SetFloat("_OuterRadius", GUILayout.HorizontalSlider(material.GetFloat("_OuterRadius"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 100, 110, 20));
			GUILayout.Label("Is Waiting Period");
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CCircularTimer result = new CCircularTimer();
				result._ColourX = material.GetVector("_Colour").x;
				result._ColourY = material.GetVector("_Colour").y;
				result._ColourZ = material.GetVector("_Colour").z;
				
				result._BackingColourX = material.GetVector("_BackingColour").x;
				result._BackingColourY = material.GetVector("_BackingColour").y;
				result._BackingColourZ = material.GetVector("_BackingColour").z;
				
				result._Color0X = material.GetVector("_Color0").x;
				result._Color0Y = material.GetVector("_Color0").y;
				result._Color0Z = material.GetVector("_Color0").z;

				result._InnerRadius = material.GetFloat("_InnerRadius");
				result._FadeInner = material.GetFloat("_FadeInner");
				result._BorderWidth = material.GetFloat("_BorderWidth");
				result._OuterRadius = material.GetFloat("_OuterRadius");
				result._IsWaitingPeriod = material.GetFloat("_IsWaitingPeriod");
				
				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
