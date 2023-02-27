using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using UnityEngine;
using System.IO;

namespace KitchenLib.Customs
{
	public class CGhost : CustomMaterial
	{
		public override JsonType Type => JsonType.CGhost;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Ghost"));

			result.SetColor("_Colour", _Color);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, 0);
		}
		public static void GUI(Material material)
		{
			Vector4 _Color = material.GetVector("_Colour");

			GUILayout.Label("_Colour");
			_Color.x = GUILayout.HorizontalSlider(_Color.x, 0.0f, 3.0f);
			_Color.y = GUILayout.HorizontalSlider(_Color.y, 0.0f, 3.0f);
			_Color.z = GUILayout.HorizontalSlider(_Color.z, 0.0f, 3.0f);
			material.SetVector("_Colour", _Color);
		}

		public static void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CGhost result = new CGhost();
				result._ColorX = material.GetVector("_Colour").x;
				result._ColorY = material.GetVector("_Colour").y;
				result._ColorZ = material.GetVector("_Colour").z;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
