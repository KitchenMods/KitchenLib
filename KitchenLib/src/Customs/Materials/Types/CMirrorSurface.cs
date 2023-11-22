using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CMirrorSurface : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CMirrorSurface;
		[JsonIgnore]
		public virtual Color _Color1 { get; set; } = Color.black;
		public float _Color1X = 0.0f;
		public float _Color1Y = 0.0f;
		public float _Color1Z = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Mirror Surface"));

			result.SetColor("_Color1", _Color1);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color1 = new Vector4(_Color1X, _Color1Y, _Color1Z, 0);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			Vector4 _Color1 = material.GetVector("_Color1");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Color1 = mainColorPicker.DrawColorPicker(_Color1);
			material.SetVector("_Color1", _Color1);
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CMirrorSurface result = new CMirrorSurface();
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
