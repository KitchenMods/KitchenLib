using Newtonsoft.Json;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CCircularTimer : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CCircularTimer;
		[JsonIgnore]
		public virtual Color _Colour { get; set; } = Color.black;
		public float _ColourX = 0.0f;
		public float _ColourY = 0.0f;
		public float _ColourZ = 0.0f;
		public float _ColourA = 0.0f;
		[JsonIgnore]
		
		public virtual Color _BackingColour { get; set; } = Color.black;
		public float _BackingColourX = 0.0f;
		public float _BackingColourY = 0.0f;
		public float _BackingColourZ = 0.0f;
		public float _BackingColourA = 0.0f;
		[JsonIgnore]
		
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		public float _Color0A = 0.0f;
		
		public float _InnerRadius = 0.3f;
		public float _FadeInner = 0.03058824f;
		public float _BorderWidth = 0.02f;
		public float _OuterRadius = 0.5f;
		public float _IsWaitingPeriod = 0.0f;
		public float _Value = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Circular Timer"));

			result.SetColor("_Colour", _Colour);
			result.SetColor("_BackingColour", _BackingColour);
			result.SetColor("_Color0", _Color0);
			result.SetFloat("_InnerRadius", _InnerRadius);
			result.SetFloat("_FadeInner", _FadeInner);
			result.SetFloat("_BorderWidth", _BorderWidth);
			result.SetFloat("_OuterRadius", _OuterRadius);
			result.SetFloat("_IsWaitingPeriod", _IsWaitingPeriod);
			result.SetFloat("_Value", _Value);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Colour = new Vector4(_ColourX, _ColourY, _ColourZ, _ColourA);
			_BackingColour = new Vector4(_BackingColourX, _BackingColourY, _BackingColourZ, _BackingColourA);
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, _Color0A);
		}
		IMColorPicker mainColorPicker;
		IMColorPicker backingColorPicker;
		IMColorPicker subColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(backingColorPicker == null)
				backingColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			
			
			material.SetColor("_Colour", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Colour")));
			material.SetColor("_BackingColour", DrawColorModule(new Rect(152, 2, 146, 186), backingColorPicker, "Backing Color", material.GetVector("_BackingColour")));
			material.SetColor("_Color0", DrawColorModule(new Rect(302, 2, 146, 186), subColorPicker, "Secondary Color", material.GetVector("_Color0")));
			
			material.SetFloat("_Value", DrawSliderModule(new Rect(2, 199, 446, 24), "_Value", material.GetFloat("_Value"), 0, 1));
			material.SetFloat("_InnerRadius", DrawSliderModule(new Rect(2, 229, 446, 24), "_InnerRadius", material.GetFloat("_InnerRadius"), 0, 1));
			material.SetFloat("_FadeInner", DrawSliderModule(new Rect(2, 259, 446, 24), "_FadeInner", material.GetFloat("_FadeInner"), 0, 1));
			material.SetFloat("_BorderWidth", DrawSliderModule(new Rect(2, 289, 446, 24), "_BorderWidth", material.GetFloat("_BorderWidth"), 0, 1));
			material.SetFloat("_OuterRadius", DrawSliderModule(new Rect(2, 319, 446, 24), "_OuterRadius", material.GetFloat("_OuterRadius"), 0, 1));
			
			material.SetFloat("_IsWaitingPeriod", DrawToggleModule(new Rect(2, 353, 146, 25), "_IsWaitingPeriod", material.GetFloat("_IsWaitingPeriod") == 1) ? 1 : 0);
		}

		public string Export(Material material)
		{
			CCircularTimer result = new CCircularTimer();
			result._ColourX = material.GetColor("_Colour").r;
			result._ColourY = material.GetColor("_Colour").g;
			result._ColourZ = material.GetColor("_Colour").b;
			result._ColourA = material.GetColor("_Colour").a;
				
			result._BackingColourX = material.GetColor("_BackingColour").r;
			result._BackingColourY = material.GetColor("_BackingColour").g;
			result._BackingColourZ = material.GetColor("_BackingColour").b;
			result._BackingColourA = material.GetColor("_BackingColour").a;
				
			result._Color0X = material.GetColor("_Color0").r;
			result._Color0Y = material.GetColor("_Color0").g;
			result._Color0Z = material.GetColor("_Color0").b;
			result._Color0A = material.GetColor("_Color0").a;

			result._InnerRadius = material.GetFloat("_InnerRadius");
			result._FadeInner = material.GetFloat("_FadeInner");
			result._BorderWidth = material.GetFloat("_BorderWidth");
			result._OuterRadius = material.GetFloat("_OuterRadius");
			result._IsWaitingPeriod = material.GetFloat("_IsWaitingPeriod");
			result._Value = material.GetFloat("_Value");
				
			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
