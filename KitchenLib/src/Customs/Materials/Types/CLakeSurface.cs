using System;
using imColorPicker;
using KitchenLib.Interfaces;
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
		public float _Color0A = 0.0f;
		
		public float _TimeScale = 1.0f;
		
		[JsonIgnore] public virtual Color _Color1 { get; set; } = Color.black;
		public float _Color1X = 0.0f;
		public float _Color1Y = 0.0f;
		public float _Color1Z = 0.0f;
		public float _Color1A = 0.0f;
		
		[JsonIgnore] public virtual Vector4 _DaySpec { get; set; } = new Vector4(2.912527f,3.044249f,3.102792f,1f);
		public float _DaySpecX = 0.0f;
		public float _DaySpecY = 0.0f;
		public float _DaySpecZ = 0.0f;
		public float _DaySpecW = 0.0f;

		public float _Scale = 0.01f;
		
		[JsonIgnore] public virtual Vector4 _NightSpec { get; set; } = new Vector4(2.912527f,3.044249f,3.102792f,1f);
		public float _NightSpecX = 0.0f;
		public float _NightSpecY = 0.0f;
		public float _NightSpecZ = 0.0f;
		public float _NightSpecW = 0.0f;

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
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, _Color0A);
			_Color1 = new Vector4(_Color1X, _Color1Y, _Color1Z, _Color1A);
			_DaySpec = new Vector4(_DaySpecX, _DaySpecY, _DaySpecZ, _DaySpecW);
			_NightSpec = new Vector4(_NightSpecX, _NightSpecY, _NightSpecZ, _NightSpecW);
		}
		static IMColorPicker mainColorPicker;
		static IMColorPicker subColorPicker;

		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			
			
			
			material.SetColor("_Color0", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Color0")));
			material.SetColor("_Color1", DrawColorModule(new Rect(152, 2, 146, 186), subColorPicker, "Secondary Color", material.GetVector("_Color1")));

			_DaySpecX = DrawSliderModule(new Rect(2, 199, 446, 24), "_DaySpecX", _DaySpecX, 0, 1);
			_DaySpecY = DrawSliderModule(new Rect(2, 229, 446, 24), "_DaySpecY", _DaySpecY, 0, 1);
			_DaySpecZ = DrawSliderModule(new Rect(2, 259, 446, 24), "_DaySpecZ", _DaySpecZ, 0, 1);
			_DaySpecW = DrawSliderModule(new Rect(2, 289, 446, 24), "_DaySpecW", _DaySpecW, 0, 1);
			
			material.SetVector("_DaySpec", new Vector4(_DaySpecX, _DaySpecY, _DaySpecZ, _DaySpecW));
			
			_NightSpecX = DrawSliderModule(new Rect(2, 349, 446, 24), "_NightSpecX", _NightSpecX, 0, 1);
			_NightSpecY = DrawSliderModule(new Rect(2, 379, 446, 24), "_NightSpecY", _NightSpecY, 0, 1);
			_NightSpecZ = DrawSliderModule(new Rect(2, 409, 446, 24), "_NightSpecZ", _NightSpecZ, 0, 1);
			_NightSpecW = DrawSliderModule(new Rect(2, 439, 446, 24), "_NightSpecW", _NightSpecW, 0, 1);
			
			material.SetVector("_NightSpec", new Vector4(_NightSpecX, _NightSpecY, _NightSpecZ, _NightSpecW));
			
			material.SetFloat("_TimeScale", DrawSliderModule(new Rect(2, 499, 446, 24), "_TimeScale", material.GetFloat("_TimeScale"), 0, 1));
			material.SetFloat("_Scale", DrawSliderModule(new Rect(2, 529, 446, 24), "_Scale", material.GetFloat("_Scale"), 0, 1));
		}

		public string Export(Material material)
		{
			CLakeSurface result = new CLakeSurface();
				
			result._Color0X = material.GetColor("_Color0").r;
			result._Color0Y = material.GetColor("_Color0").g;
			result._Color0Z = material.GetColor("_Color0").b;
			result._Color0A = material.GetColor("_Color0").a;
				
			result._TimeScale = material.GetFloat("_TimeScale");
				
			result._Color1X = material.GetColor("_Color1").r;
			result._Color1Y = material.GetColor("_Color1").g;
			result._Color1Z = material.GetColor("_Color1").b;
			result._Color1A = material.GetColor("_Color1").a;
				
			result._DaySpecX = material.GetVector("_DaySpec").x;
			result._DaySpecY = material.GetVector("_DaySpec").y;
			result._DaySpecZ = material.GetVector("_DaySpec").z;
				
			result._Scale = material.GetFloat("_Scale");
				
			result._NightSpecX = material.GetVector("_NightSpec").x;
			result._NightSpecY = material.GetVector("_NightSpec").y;
			result._NightSpecZ = material.GetVector("_NightSpec").z;
				
			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}