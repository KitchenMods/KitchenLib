using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CFlatImage : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CFlatImage;

		[JsonIgnore]
		public virtual Texture _Image { get; set; }
		public string _ImageAsBase64 = "";

		public virtual float _Alpha { get; set; } = 1.0f;
		public virtual float _BlowoutScale1 { get; set; } = 0.0f;
		public virtual float _BlowoutOffset1 { get; set; } = 0.0f;
		public virtual float _IsBlowout { get; set; } = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Flat Image"));
			
			result.name = Name;
			result.SetFloat("_Alpha", _Alpha);
			result.SetFloat("_BlowoutScale1", _BlowoutScale1);
			result.SetFloat("_BlowoutOffset1", _BlowoutOffset1);
			result.SetFloat("_IsBlowout", _IsBlowout);
			result.SetTexture("_Image", _Image);

			material = result;
		}

		public override void Deserialise()
		{
			_Image = ResourceUtils.LoadTextureFromBase64(_ImageAsBase64);
		}
		private string imageFile = "";
		public void GUI(Material material)
		{
			imageFile = DrawTextureModule(new Rect(2, 50, 292, 207),"Texture", imageFile, material);
			
			material.SetFloat("_Alpha", DrawSliderModule(new Rect(2, 215, 446, 24), "_Alpha", material.GetFloat("_Alpha"), 0, 1));
			material.SetFloat("_BlowoutScale1", DrawSliderModule(new Rect(2, 245, 446, 24), "_BlowoutScale1", material.GetFloat("_BlowoutScale1"), 0, 1));
			material.SetFloat("_BlowoutOffset1", DrawSliderModule(new Rect(2, 275, 446, 24), "_BlowoutOffset1", material.GetFloat("_BlowoutOffset1"), 0, 1));
			
			material.SetFloat("_IsBlowout", DrawToggleModule(new Rect(2, 305, 146, 25), "_IsBlowout", material.GetFloat("_IsBlowout") == 1) ? 1 : 0);
		}

		public string Export(Material material)
		{
			CFlatImage result = new CFlatImage();
			result._Alpha = material.GetFloat("_Alpha");
			result._BlowoutScale1 = material.GetFloat("_BlowoutScale1");
			result._BlowoutOffset1 = material.GetFloat("_BlowoutOffset1");
			result._IsBlowout = material.GetFloat("_IsBlowout");
			result._ImageAsBase64 = imgtob64(material.GetTexture("_Image"), true);

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
