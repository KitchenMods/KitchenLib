using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using UnityEngine;
using System.IO;

namespace KitchenLib.Customs
{
	public class CFoliage : CustomMaterial
	{
		public override JsonType Type => JsonType.CFoliage;
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;
		public virtual Color _Color1 { get; set; } = Color.black;
		public float _Color1X = 0.0f;
		public float _Color1Y = 0.0f;
		public float _Color1Z = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Foliage"));

			result.SetColor("_Color0", _Color0);
			result.SetColor("_Color1", _Color1);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, 0);
			_Color1 = new Vector4(_Color1X, _Color1Y, _Color1Z, 0);
		}
		public static void GUI(Material material)
		{
			Vector4 _Color0 = material.GetVector("_Color0");

			GUILayout.Label("_Color0");
			_Color0.x = GUILayout.HorizontalSlider(_Color0.x, 0.0f, 1.0f);
			_Color0.y = GUILayout.HorizontalSlider(_Color0.y, 0.0f, 1.0f);
			_Color0.z = GUILayout.HorizontalSlider(_Color0.z, 0.0f, 1.0f);
			material.SetVector("_Color0", _Color0);
			
			Vector4 _Color1 = material.GetVector("_Color1");

			GUILayout.Label("_Color1");
			_Color1.x = GUILayout.HorizontalSlider(_Color1.x, 0.0f, 1.0f);
			_Color1.y = GUILayout.HorizontalSlider(_Color1.y, 0.0f, 1.0f);
			_Color1.z = GUILayout.HorizontalSlider(_Color1.z, 0.0f, 1.0f);
			material.SetVector("_Color1", _Color1);
		}

		public static void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CFoliage result = new CFoliage();
				result._Color0X = material.GetVector("_Color0").x;
				result._Color0Y = material.GetVector("_Color0").y;
				result._Color0Z = material.GetVector("_Color0").z;
				
				result._Color1X = material.GetVector("_Color1").x;
				result._Color1Y = material.GetVector("_Color1").y;
				result._Color1Z = material.GetVector("_Color1").z;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
