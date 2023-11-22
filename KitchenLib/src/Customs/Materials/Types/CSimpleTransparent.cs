using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CSimpleTransparent : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CSimpleTransparent;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Simple Transparent"));

			result.SetColor("_Color", _Color);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, 0);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			
			Vector4 _Color = material.GetVector("_Color");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Color = mainColorPicker.DrawColorPicker(_Color);
			material.SetVector("_Color", _Color);
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CSimpleTransparent result = new CSimpleTransparent();
				result._ColorX = material.GetVector("_Color").x;
				result._ColorY = material.GetVector("_Color").y;
				result._ColorZ = material.GetVector("_Color").z;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
