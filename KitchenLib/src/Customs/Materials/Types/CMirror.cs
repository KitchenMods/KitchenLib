using System;
using KitchenLib.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CMirror : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CMirror;

		public float _Radius = 0.25f;
		[JsonIgnore]
		public virtual Vector4 _Centre { get; set; } = new Vector4(0.4f, 0f, 0.9f, 0f);
		public float _CentreX = 0.0f;
		public float _CentreY = 0.0f;
		public float _CentreZ = 0.0f;
		public float _CentreW = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Mirror"));

			result.SetVector("_Centre", _Centre);
			result.SetFloat("_Radius", _Radius);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Centre = new Vector4(_CentreX, _CentreY, _CentreZ, _CentreW);
		}

		public void GUI(Material material)
		{
			_CentreX = DrawSliderModule(new Rect(2, 2, 446, 24), "_CentreX", _CentreX, 0, 1);
			_CentreY = DrawSliderModule(new Rect(2, 32, 446, 24), "_CentreY", _CentreY, 0, 1);
			_CentreZ = DrawSliderModule(new Rect(2, 62, 446, 24), "_CentreZ", _CentreZ, 0, 1);
			_CentreW = DrawSliderModule(new Rect(2, 92, 446, 24), "_CentreW", _CentreW, 0, 1);
			
			material.SetVector("_Centre", new Vector4(_CentreX, _CentreY, _CentreZ, _CentreW));
			
			material.SetFloat("_Radius", DrawSliderModule(new Rect(2, 152, 446, 24), "_Radius", material.GetFloat("_Radius"), 0, 1));
		}

		public string Export(Material material)
		{
			CMirror result = new CMirror();
			result._CentreX = material.GetVector("_Centre").x;
			result._CentreY = material.GetVector("_Centre").y;
			result._CentreZ = material.GetVector("_Centre").z;
			result._CentreW = material.GetVector("_Centre").w;

			result._Radius = material.GetFloat("_Radius");
				
			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}