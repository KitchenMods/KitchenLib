using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CFoliage : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CFoliage;
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		public float _Color0A = 0.0f;
		[JsonIgnore]
		public virtual Color _Color1 { get; set; } = Color.black;
		public float _Color1X = 0.0f;
		public float _Color1Y = 0.0f;
		public float _Color1Z = 0.0f;
		public float _Color1A = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Foliage"));

			result.SetColor("_Color0", _Color0);
			result.SetColor("_Color1", _Color1);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, _Color0A);
			_Color1 = new Vector4(_Color1X, _Color1Y, _Color1Z, _Color1A);
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
			material.SetColor("_Color1", DrawColorModule(new Rect(152, 2, 146, 186), subColorPicker, "Secondary Color", material.GetVector("_Color1")));
		}

		public string Export(Material material)
		{
			CFoliage result = new CFoliage();
			result._Color0X = material.GetColor("_Color0").r;
			result._Color0Y = material.GetColor("_Color0").g;
			result._Color0Z = material.GetColor("_Color0").b;
			result._Color0A = material.GetColor("_Color0").a;

			result._Color1X = material.GetColor("_Color1").r;
			result._Color1Y = material.GetColor("_Color1").g;
			result._Color1Z = material.GetColor("_Color1").b;
			result._Color1A = material.GetColor("_Color1").a;

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
