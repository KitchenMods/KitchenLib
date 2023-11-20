using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CBlueprintLight: CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CBlueprintLight;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;
		
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;

		public virtual float _HasColour { get; set; } = 0.0f;
		public virtual float _IsCopy { get; set; } = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Blueprint Light"));

			result.SetColor("_Color", _Color);
			result.SetColor("_Color0", _Color0);
			result.SetFloat("_HasColour", _HasColour);
			result.SetFloat("_IsCopy", _IsCopy);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, 0);
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, 0);
		}
		IMColorPicker mainColorPicker;
		IMColorPicker subColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			
			Vector4 _Color = material.GetVector("_Color");
			Vector4 _Color0 = material.GetVector("_Color0");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Color = mainColorPicker.DrawColorPicker(_Color);
			material.SetVector("_Color", _Color);
			GUILayout.EndArea();
			material.SetVector("_Color", _Color);
			
			GUILayout.BeginArea(new Rect(159, 0, 159, 20));
			GUILayout.Label("Sub Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 20, 159, 140));
			_Color0 = subColorPicker.DrawColorPicker(_Color0);
			material.SetVector("_Color0", _Color0);
			GUILayout.EndArea();
			material.SetVector("_Color0", _Color0);
			
			GUILayout.BeginArea(new Rect(318, 20, 110, 20));
			GUILayout.Label("Has Sub Colour");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 20, 49, 20));
			material.SetFloat("_HasColour", float.Parse(GUILayout.TextField(material.GetFloat("_HasColour").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 25, 318, 20));
			material.SetFloat("_HasColour", GUILayout.HorizontalSlider(material.GetFloat("_HasColour"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 40, 110, 20));
			GUILayout.Label("Is Copy");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 40, 49, 20));
			material.SetFloat("_IsCopy", float.Parse(GUILayout.TextField(material.GetFloat("_IsCopy").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 45, 318, 20));
			material.SetFloat("_IsCopy", GUILayout.HorizontalSlider(material.GetFloat("_IsCopy"), 0.0f, 1.0f));
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CBlueprintLight result = new CBlueprintLight();
				result._ColorX = material.GetVector("_Color").x;
				result._ColorY = material.GetVector("_Color").y;
				result._ColorZ = material.GetVector("_Color").z;
				
				result._Color0X = material.GetVector("_Color0").x;
				result._Color0Y = material.GetVector("_Color0").y;
				result._Color0Z = material.GetVector("_Color0").z;

				result._HasColour = material.GetFloat("_HasColour");

				result._IsCopy = material.GetFloat("_IsCopy");

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
