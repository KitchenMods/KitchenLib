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
		private static string imageFile = "";
		public void GUI(Material material)
		{
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Texture File");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 0, 318, 20));
			imageFile = GUILayout.TextField(imageFile);
			if (File.Exists(imageFile))
				material.SetTexture("_Image", ResourceUtils.LoadTextureFromFile(imageFile));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 110, 20));
			GUILayout.Label("Alpha");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(110, 20, 49, 20));
			material.SetFloat("_Alpha", float.Parse(GUILayout.TextField(material.GetFloat("_Alpha").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(208, 25, 318, 20));
			material.SetFloat("_Alpha", GUILayout.HorizontalSlider(material.GetFloat("_Alpha"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 40, 110, 20));
			GUILayout.Label("Blowout Scale");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(110, 40, 49, 20));
			material.SetFloat("_BlowoutScale1", float.Parse(GUILayout.TextField(material.GetFloat("_BlowoutScale1").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(208, 45, 318, 20));
			material.SetFloat("_BlowoutScale1", GUILayout.HorizontalSlider(material.GetFloat("_BlowoutScale1"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 60, 110, 20));
			GUILayout.Label("Blowout Offset");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(110, 60, 49, 20));
			material.SetFloat("_BlowoutOffset1", float.Parse(GUILayout.TextField(material.GetFloat("_BlowoutOffset1").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(208, 65, 318, 20));
			material.SetFloat("_BlowoutOffset1", GUILayout.HorizontalSlider(material.GetFloat("_BlowoutOffset1"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 80, 110, 20));
			GUILayout.Label("Is Blowout");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(110, 80, 49, 20));
			material.SetFloat("_IsBlowout", float.Parse(GUILayout.TextField(material.GetFloat("_IsBlowout").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(208, 85, 318, 20));
			material.SetFloat("_IsBlowout", GUILayout.HorizontalSlider(material.GetFloat("_IsBlowout"), 0.0f, 1.0f));
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CFlatImage result = new CFlatImage();
				result._Alpha = material.GetFloat("_Alpha");
				result._BlowoutScale1 = material.GetFloat("_BlowoutScale1");
				result._BlowoutOffset1 = material.GetFloat("_BlowoutOffset1");
				result._IsBlowout = material.GetFloat("_IsBlowout");
				result._ImageAsBase64 = imgtob64(material.GetTexture("_Image"));

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
