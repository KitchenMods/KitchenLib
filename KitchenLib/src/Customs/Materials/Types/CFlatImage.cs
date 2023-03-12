using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CFlatImage : CustomMaterial
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
			Material result = new Material(Shader.Find("Simple Flat"));
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Image = ResourceUtils.LoadTextureFromBase64(_ImageAsBase64);
		}
		private static string imageFile = "";
		public static void GUI(Material material)
		{

			GUILayout.Label("_Image");
			imageFile = GUILayout.TextField(imageFile);
			if (File.Exists(imageFile))
				material.SetTexture("_Image", ResourceUtils.LoadTextureFromFile(imageFile));

			GUILayout.Label("_Alpha");
			material.SetFloat("_Alpha", GUILayout.HorizontalSlider(material.GetFloat("_Alpha"), 0.0f, 1.0f));

			GUILayout.Label("_BlowoutScale1");
			material.SetFloat("_BlowoutScale1", GUILayout.HorizontalSlider(material.GetFloat("_BlowoutScale1"), 0.0f, 1.0f));

			GUILayout.Label("_BlowoutOffset1");
			material.SetFloat("_BlowoutOffset1", GUILayout.HorizontalSlider(material.GetFloat("_BlowoutOffset1"), 0.0f, 1.0f));

			GUILayout.Label("_IsBlowout");
			material.SetFloat("_IsBlowout", GUILayout.HorizontalSlider(material.GetFloat("_IsBlowout"), 0.0f, 1.0f));
		}

		public static void Export(Material material)
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
