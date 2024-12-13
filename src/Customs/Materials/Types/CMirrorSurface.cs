using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CMirrorSurface : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CMirrorSurface;
		[JsonIgnore]
		public virtual Color _Color1 { get; set; } = Color.black;
		public float _Color1X = 0.0f;
		public float _Color1Y = 0.0f;
		public float _Color1Z = 0.0f;
		public float _Color1W = 0.0f;
		
		public bool _Highlight = false;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Mirror Surface"));

			result.SetColor("_Color1", _Color1);
			result.SetFloat("_Highlight", _Highlight ? 1 : 0);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color1 = new Vector4(_Color1X, _Color1Y, _Color1Z, _Color1W);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			material.SetColor("_Color1", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Color1")));
			
			material.SetFloat("_Highlight", DrawToggleModule(new Rect(2, 194, 146, 25), "_Highlight", material.GetFloat("_Highlight") == 1) ? 1 : 0);
		}

		public string Export(Material material)
		{
			CMirrorSurface result = new CMirrorSurface();
			result._Color1X = material.GetColor("_Color1").r;
			result._Color1Y = material.GetColor("_Color1").g;
			result._Color1Z = material.GetColor("_Color1").b;
			result._Color1W = material.GetColor("_Color1").a;
			
			result._Highlight = material.GetFloat("_Highlight") == 1;

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
