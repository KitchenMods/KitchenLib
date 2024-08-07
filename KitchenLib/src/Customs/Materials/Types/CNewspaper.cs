using Newtonsoft.Json;
using System;
using System.IO;
using imColorPicker;
using KitchenLib.Interfaces;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CNewspaper : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CNewspaper;
		
		[JsonIgnore]
		public virtual Texture _Photograph { get; set; }
		public string _PhotographAsBase64 = "";
		private static string overlayFile = "";
		
		public float _Alpha = 1;
		public float _BlowoutScale = 0;
		public float _BlowoutOffset = 0;
		
		[JsonIgnore]
		public virtual Color _Colour { get; set; } = Color.black;
		public float _ColourX = 0.0f;
		public float _ColourY = 0.0f;
		public float _ColourZ = 0.0f;
		public float _ColourA = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Newspaper"));

			result.SetTexture("_Overlay", _Photograph);
			result.SetFloat("_Alpha", _Alpha);
			result.SetFloat("_BlowoutScale", _BlowoutScale);
			result.SetColor("_Colour", _Colour);
			result.SetFloat("_BlowoutOffset", _BlowoutOffset);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Photograph = ResourceUtils.LoadTextureFromBase64(_PhotographAsBase64);
			_Colour = new Color(_ColourX, _ColourY, _ColourZ, _ColourA);
		}
		IMColorPicker mainColorPicker;
		private string imageFile = "";
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			
			imageFile = DrawTextureModule(new Rect(2, 50, 292, 207),"_Photograph", imageFile, material, "_Photograph");
			
			material.SetFloat("_Alpha", DrawSliderModule(new Rect(2, 215, 446, 24), "_Alpha", material.GetFloat("_Alpha"), 0, 1));
			material.SetFloat("_BlowoutScale", DrawSliderModule(new Rect(2, 245, 446, 24), "_BlowoutScale", material.GetFloat("_BlowoutScale"), 0, 1));
			material.SetFloat("_BlowoutOffset", DrawSliderModule(new Rect(2, 275, 446, 24), "_BlowoutOffset", material.GetFloat("_BlowoutOffset"), 0, 1));
			
			material.SetColor("_Colour", DrawColorModule(new Rect(304, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Colour")));
		}

		public string Export(Material material)
		{
			CNewspaper result = new CNewspaper();
			result._ColourX = material.GetColor("_Colour").r;
			result._ColourY = material.GetColor("_Colour").g;
			result._ColourZ = material.GetColor("_Colour").b;
			result._ColourA = material.GetColor("_Colour").a;
			
			result._Alpha = material.GetFloat("_Alpha");
			result._BlowoutScale = material.GetFloat("_BlowoutScale");
			result._BlowoutOffset = material.GetFloat("_BlowoutOffset");
			result._PhotographAsBase64 = imgtob64(material.GetTexture("_Photograph"));

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
