using System;
using System.IO;
using imColorPicker;
using KitchenLib.Interfaces;
using KitchenLib.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CLakeSurface : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CLakeSurface;

		[JsonIgnore] public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		
		public float _TimeScale = 1.0f;
		
		[JsonIgnore] public virtual Color _Color1 { get; set; } = Color.black;
		public float _Color1X = 0.0f;
		public float _Color1Y = 0.0f;
		public float _Color1Z = 0.0f;
		
		[JsonIgnore] public virtual Vector4 _DaySpec { get; set; } = new Vector4(2.912527f,3.044249f,3.102792f,1f);
		public float _DaySpecX = 0.0f;
		public float _DaySpecY = 0.0f;
		public float _DaySpecZ = 0.0f;

		public float _Scale = 0.01f;
		
		[JsonIgnore] public virtual Vector4 _NightSpec { get; set; } = new Vector4(2.912527f,3.044249f,3.102792f,1f);
		public float _NightSpecX = 0.0f;
		public float _NightSpecY = 0.0f;
		public float _NightSpecZ = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Lake Surface"));

			result.SetColor("_Color0", _Color0);
			result.SetFloat("_TimeScale", _TimeScale);
			result.SetColor("_Color1", _Color1);
			result.SetVector("_DaySpec", _DaySpec);
			result.SetFloat("_Scale", _Scale);
			result.SetVector("_NightSpec", _NightSpec);
			
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, 0);
			_Color1 = new Vector4(_Color1X, _Color1Y, _Color1Z, 0);
			_DaySpec = new Vector4(_DaySpecX, _DaySpecY, _DaySpecZ, 1);
			_NightSpec = new Vector4(_NightSpecX, _NightSpecY, _NightSpecZ, 1);
		}
		static IMColorPicker mainColorPicker;
		static IMColorPicker subColorPicker;

		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			
			Vector4 _Color0 = material.GetVector("_Color0");
			Vector4 _Color1 = material.GetVector("_Color1");
			float _TimeScale = material.GetFloat("_TimeScale");
			Vector4 _DaySpec = material.GetVector("_DaySpec");
			float _Scale = material.GetFloat("_Scale");
			Vector4 _NightSpec = material.GetVector("_NightSpec");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Color0 = mainColorPicker.DrawColorPicker(_Color0);
			material.SetVector("_Color0", _Color0);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 0, 159, 20));
			GUILayout.Label("Sub Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 20, 159, 140));
			_Color1 = subColorPicker.DrawColorPicker(_Color1);
			material.SetVector("_Color1", _Color1);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 20, 110, 20));
			GUILayout.Label("Time Scale");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 20, 49, 20));
			material.SetFloat("_TimeScale", float.Parse(GUILayout.TextField(material.GetFloat("_TimeScale").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 25, 318, 20));
			material.SetFloat("_TimeScale", GUILayout.HorizontalSlider(material.GetFloat("_TimeScale"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 60, 110, 20));
			GUILayout.Label("Day Spec");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 60, 49, 20));
			_DaySpec.x = float.Parse(GUILayout.TextField(_DaySpec.x.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 65, 318, 20));
			_DaySpec.x = GUILayout.HorizontalSlider(_DaySpec.x, 0.0f, 10.0f);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 80, 49, 20));
			_DaySpec.y = float.Parse(GUILayout.TextField(_DaySpec.y.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 85, 318, 20));
			_DaySpec.y = GUILayout.HorizontalSlider(_DaySpec.y, 0.0f, 10.0f);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 100, 49, 20));
			_DaySpec.z = float.Parse(GUILayout.TextField(_DaySpec.z.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 105, 318, 20));
			_DaySpec.z = GUILayout.HorizontalSlider(_DaySpec.z, 0.0f, 10.0f);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 140, 110, 20));
			GUILayout.Label("Scale");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 140, 49, 20));
			material.SetFloat("_Scale", float.Parse(GUILayout.TextField(material.GetFloat("_Scale").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 145, 318, 20));
			material.SetFloat("_Scale", GUILayout.HorizontalSlider(material.GetFloat("_Scale"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 180, 110, 20));
			GUILayout.Label("Night Spec");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 180, 49, 20));
			_NightSpec.x = float.Parse(GUILayout.TextField(_NightSpec.x.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 185, 318, 20));
			_NightSpec.x = GUILayout.HorizontalSlider(_NightSpec.x, 0.0f, 10.0f);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 200, 49, 20));
			_NightSpec.y = float.Parse(GUILayout.TextField(_NightSpec.y.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 205, 318, 20));
			_NightSpec.y = GUILayout.HorizontalSlider(_NightSpec.y, 0.0f, 10.0f);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 220, 49, 20));
			_NightSpec.z = float.Parse(GUILayout.TextField(_NightSpec.z.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 225, 318, 20));
			_NightSpec.z = GUILayout.HorizontalSlider(_NightSpec.z, 0.0f, 10.0f);
			GUILayout.EndArea();
			
			material.SetVector("_DaySpec", _DaySpec);
			material.SetVector("_NightSpec", _NightSpec);
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CLakeSurface result = new CLakeSurface();
				
				result._Color0X = material.GetVector("_Color0").x;
				result._Color0Y = material.GetVector("_Color0").y;
				result._Color0Z = material.GetVector("_Color0").z;
				
				result._TimeScale = material.GetFloat("_TimeScale");
				
				result._Color1X = material.GetVector("_Color1").x;
				result._Color1Y = material.GetVector("_Color1").y;
				result._Color1Z = material.GetVector("_Color1").z;
				
				result._DaySpecX = material.GetVector("_DaySpec").x;
				result._DaySpecY = material.GetVector("_DaySpec").y;
				result._DaySpecZ = material.GetVector("_DaySpec").z;
				
				result._Scale = material.GetFloat("_Scale");
				
				result._NightSpecX = material.GetVector("_NightSpec").x;
				result._NightSpecY = material.GetVector("_NightSpec").y;
				result._NightSpecZ = material.GetVector("_NightSpec").z;
				
				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}