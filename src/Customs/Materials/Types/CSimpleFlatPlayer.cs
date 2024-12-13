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
		public float _Colour2A = 0.0f;
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		public float _Color0A = 0.0f;

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
			_Colour2 = new Vector4(_Colour2X, _Colour2Y, _Colour2Z, _Colour2A);
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, _Color0A);
		}
		IMColorPicker mainColorPicker;
		IMColorPicker subColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			
			material.SetColor("_Color0", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Color0")));
			material.SetColor("_Colour2", DrawColorModule(new Rect(152, 2, 146, 186), subColorPicker, "Secondary Color", material.GetVector("_Colour2")));
			material.SetFloat("_Shininess", DrawSliderModule(new Rect(2, 199, 446, 24), "_Shininess", material.GetFloat("_Shininess"), 0, 1));
			material.SetFloat("_Flatness", DrawSliderModule(new Rect(2, 229, 446, 24), "_Flatness", material.GetFloat("_Flatness"), 0, 1));
			material.SetFloat("_MultiColour", DrawToggleModule(new Rect(2, 262, 146, 25), "_MultiColour", material.GetFloat("_MultiColour") == 1) ? 1 : 0);
		}

		public string Export(Material material)
		{
			CSimpleFlatPlayer result = new CSimpleFlatPlayer();
			result._Colour2X = material.GetColor("_Colour2").r;
			result._Colour2Y = material.GetColor("_Colour2").g;
			result._Colour2Z = material.GetColor("_Colour2").b;
			result._Colour2Z = material.GetColor("_Colour2").a;
				
			result._Color0X = material.GetColor("_Color0").r;
			result._Color0Y = material.GetColor("_Color0").g;
			result._Color0Z = material.GetColor("_Color0").b;
			result._Color0Z = material.GetColor("_Color0").a;
				
			result._Shininess = material.GetFloat("_Shininess");
				
			result._MultiColour = material.GetInt("_MultiColour") == 1;
				
			result._Flatness = material.GetFloat("_Flatness");

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
