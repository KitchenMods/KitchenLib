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
		public float _ColorA = 0.0f;
		
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		public float _Color0A = 0.0f;

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
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, _ColorA);
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
			
			material.SetColor("_Color", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Color")));
			material.SetColor("_Color0", DrawColorModule(new Rect(152, 2, 146, 186), subColorPicker, "Secondary Color", material.GetVector("_Color0")));

			material.SetFloat("_HasColour", DrawToggleModule(new Rect(2, 194, 146, 25), "_HasColour", material.GetFloat("_HasColour") == 1) ? 1 : 0);
			material.SetFloat("_IsCopy", DrawToggleModule(new Rect(152, 194, 146, 25), "_IsCopy", material.GetFloat("_IsCopy") == 1) ? 1 : 0);
		}

		public string Export(Material material)
		{
			CBlueprintLight result = new CBlueprintLight();
			result._ColorX = material.GetColor("_Color").r;
			result._ColorY = material.GetColor("_Color").g;
			result._ColorZ = material.GetColor("_Color").b;
			result._ColorA = material.GetColor("_Color").a;
				
			result._Color0X = material.GetColor("_Color0").r;
			result._Color0Y = material.GetColor("_Color0").g;
			result._Color0Z = material.GetColor("_Color0").b;
			result._Color0A = material.GetColor("_Color0").a;

			result._HasColour = material.GetFloat("_HasColour");

			result._IsCopy = material.GetFloat("_IsCopy");

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
