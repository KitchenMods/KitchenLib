using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CMirrorBacking : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CMirrorBacking;
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;
		public float _ColorA = 0.0f;
		
		public float _TessPhongStrength = 0.5f;
		public float _TessValue = 16.0f;
		public float _TessMin = 10.0f;
		public float _TessMax = 25.0f;
		public float _TessEdgeLength = 16.0f;
		public float _TessMaxDisp = 25.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Mirror Backing"));

			result.SetColor("_Color0", _Color0);
			result.SetFloat("_TessPhongStrength", _TessPhongStrength);
			result.SetFloat("_TessValue", _TessValue);
			result.SetFloat("_TessMin", _TessMin);
			result.SetFloat("_TessMax", _TessMax);
			result.SetFloat("_TessEdgeLength", _TessEdgeLength);
			result.SetFloat("_TessMaxDisp", _TessMaxDisp);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color0 = new Color(_ColorX, _ColorY, _ColorZ, _ColorA);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			material.SetColor("_Color0", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Color0")));
			
			material.SetFloat("_TessPhongStrength", DrawSliderModule(new Rect(2, 199, 446, 24), "_TessPhongStrength", material.GetFloat("_TessPhongStrength")));
			material.SetFloat("_TessValue", DrawSliderModule(new Rect(2, 229, 446, 24), "_TessValue", material.GetFloat("_TessValue")));
			material.SetFloat("_TessMin", DrawSliderModule(new Rect(2, 259, 446, 24), "_TessMin", material.GetFloat("_TessMin")));
			material.SetFloat("_TessMax", DrawSliderModule(new Rect(2, 289, 446, 24), "_TessMax", material.GetFloat("_TessMax")));
			material.SetFloat("_TessEdgeLength", DrawSliderModule(new Rect(2, 319, 446, 24), "_TessEdgeLength", material.GetFloat("_TessEdgeLength")));
			material.SetFloat("_TessMaxDisp", DrawSliderModule(new Rect(2, 349, 446, 24), "_TessMaxDisp", material.GetFloat("_TessMaxDisp")));
		}

		public string Export(Material material)
		{
			CMirrorBacking result = new CMirrorBacking();
			result._ColorX = material.GetColor("_Color0").r;
			result._ColorY = material.GetColor("_Color0").g;
			result._ColorZ = material.GetColor("_Color0").b;
			result._ColorA = material.GetColor("_Color0").a;
			result._TessPhongStrength = material.GetFloat("_TessPhongStrength");
			result._TessValue = material.GetFloat("_TessValue");
			result._TessMin = material.GetFloat("_TessMin");
			result._TessMax = material.GetFloat("_TessMax");
			result._TessEdgeLength = material.GetFloat("_TessEdgeLength");
			result._TessMaxDisp = material.GetFloat("_TessMaxDisp");

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
