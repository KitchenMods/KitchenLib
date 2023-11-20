using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CFoliage : CustomMaterial, IMaterialEditor
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
		IMColorPicker mainColorPicker;
		IMColorPicker subColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			if(subColorPicker == null)
				subColorPicker = new IMColorPicker();
			Vector4 _Color0 = material.GetVector("_Color0");
			Vector4 _Color1 = material.GetVector("_Color1");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Color0 = mainColorPicker.DrawColorPicker(_Color0);
			material.SetVector("_Color0", _Color0);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 0, 159, 20));
			GUILayout.Label("Sub Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 20, 159, 140));
			_Color1 = subColorPicker.DrawColorPicker(_Color1);
			material.SetVector("_Color1", _Color1);
			GUILayout.EndArea();
		}

		public void Export(Material material)
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
