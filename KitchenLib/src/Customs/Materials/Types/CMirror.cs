using System;
using System.IO;
using imColorPicker;
using KitchenLib.Interfaces;
using KitchenLib.Utils;
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
			Vector4 _Centre = material.GetVector("_Centre");
			_Centre.x = GUILayout.HorizontalSlider(_Centre.x, 0, 1);
			_Centre.y = GUILayout.HorizontalSlider(_Centre.y, 0, 1);
			_Centre.z = GUILayout.HorizontalSlider(_Centre.z, 0, 1);
			_Centre.w = GUILayout.HorizontalSlider(_Centre.w, 0, 1);
			
			material.SetFloat("_Radius", GUILayout.HorizontalSlider(material.GetFloat("_Radius"), 0, 1));
			material.SetVector("_Centre", _Centre);
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CMirror result = new CMirror();
				result._CentreX = material.GetVector("_Centre").x;
				result._CentreY = material.GetVector("_Centre").y;
				result._CentreZ = material.GetVector("_Centre").z;
				result._CentreW = material.GetVector("_Centre").w;

				result._Radius = material.GetFloat("_Radius");
				
				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}