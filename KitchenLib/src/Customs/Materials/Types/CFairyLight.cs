using KitchenLib.Utils;
using Newtonsoft.Json;
using System;
using UnityEngine;
using System.IO;

namespace KitchenLib.Customs
{
	public class CFairyLight : CustomMaterial
	{
		public override JsonType Type => JsonType.CFairyLight;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Fairy Light"));

			result.SetColor("_Color0", _Color);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, 0);
		}
		public static void GUI(Material material)
		{
			Vector4 _Color = material.GetVector("_Color0");

			GUILayout.Label("_Color0");
			_Color.x = GUILayout.HorizontalSlider(_Color.x, 0.0f, 3.0f);
			_Color.y = GUILayout.HorizontalSlider(_Color.y, 0.0f, 3.0f);
			_Color.z = GUILayout.HorizontalSlider(_Color.z, 0.0f, 3.0f);
			material.SetVector("_Color0", _Color);
		}

		public static void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CFairyLight result = new CFairyLight();
				result._ColorX = material.GetVector("_Color0").x;
				result._ColorY = material.GetVector("_Color0").y;
				result._ColorZ = material.GetVector("_Color0").z;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
