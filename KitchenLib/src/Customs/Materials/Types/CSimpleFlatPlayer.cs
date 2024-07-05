using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CSimpleFlatPlayer : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CSimpleFlatPlayer;
		[JsonIgnore]
		public virtual Color _Colour2 { get; set; } = Color.black;
		public float _Colour2X = 0.0f;
		public float _Colour2Y = 0.0f;
		public float _Colour2Z = 0.0f;
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;

		public float _Shininess = 0;
		public bool _MultiColour = false;
		public float _Flatness = 0;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Simple Flat - Player"));

			result.SetColor("_Colour2", _Colour2);
			result.SetColor("_Color0", _Color0);
			result.SetFloat("_Shininess", _Shininess);
			result.SetInt("_MultiColour", _MultiColour ? 1 : 0);
			if (!_MultiColour)
				result.DisableKeyword("_MULTICOLOUR_ON");
			else
				result.EnableKeyword("_MULTICOLOUR_ON");
			result.SetFloat("_Flatness", _Flatness);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Colour2 = new Vector4(_Colour2X, _Colour2Y, _Colour2Z, 0);
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
			Vector4 _Colour2 = material.GetVector("_Colour2");
			Vector4 _Color0 = material.GetVector("_Color0");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Colour2 = mainColorPicker.DrawColorPicker(_Colour2);
			material.SetVector("_Colour2", _Colour2);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 0, 159, 20));
			GUILayout.Label("Overlay Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 20, 159, 140));
			_Color0 = subColorPicker.DrawColorPicker(_Color0);
			material.SetVector("_Color0", _Color0);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 20, 110, 20));
			GUILayout.Label("Shininess");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 20, 49, 20));
			material.SetFloat("_Shininess", float.Parse(GUILayout.TextField(material.GetFloat("_Shininess").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 25, 318, 20));
			material.SetFloat("_Shininess", GUILayout.HorizontalSlider(material.GetFloat("_Shininess"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 40, 159, 20));
			material.SetInt("_MultiColour", GUILayout.Toggle(material.GetInt("_MultiColour") == 1, "Multi Colour") ? 1 : 0);
			if (material.GetInt("_MultiColour") == 0)
				material.DisableKeyword("_MULTICOLOUR_ON");
			else
				material.EnableKeyword("_MULTICOLOUR_ON");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(318, 60, 110, 20));
			GUILayout.Label("Flatness");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(428, 60, 49, 20));
			material.SetFloat("_Flatness", float.Parse(GUILayout.TextField(material.GetFloat("_Flatness").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(477, 65, 318, 20));
			material.SetFloat("_Flatness", GUILayout.HorizontalSlider(material.GetFloat("_Flatness"), 0.0f, 1.0f));
			GUILayout.EndArea();
		}

		public string Export(Material material)
		{
			CSimpleFlatPlayer result = new CSimpleFlatPlayer();
			result._Colour2X = material.GetVector("_Colour2").x;
			result._Colour2Y = material.GetVector("_Colour2").y;
			result._Colour2Z = material.GetVector("_Colour2").z;
				
			result._Color0X = material.GetVector("_Color0").x;
			result._Color0Y = material.GetVector("_Color0").y;
			result._Color0Z = material.GetVector("_Color0").z;
				
			result._Shininess = material.GetFloat("_Shininess");
				
			result._MultiColour = material.GetInt("_MultiColour") == 1;
				
			result._Flatness = material.GetFloat("_Flatness");

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
