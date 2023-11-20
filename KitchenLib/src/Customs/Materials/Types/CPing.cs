using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CPing : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CPing;
		[JsonIgnore]
		public virtual Color _PlayerColour { get; set; } = Color.black;
		public float _PlayerColourX = 0.0f;
		public float _PlayerColourY = 0.0f;
		public float _PlayerColourZ = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Ping"));

			result.SetColor("_PlayerColour", _PlayerColour);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_PlayerColour = new Vector4(_PlayerColourX, _PlayerColourY, _PlayerColourZ, 0);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			Vector4 _PlayerColour = material.GetVector("_PlayerColour");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_PlayerColour = mainColorPicker.DrawColorPicker(_PlayerColour);
			material.SetVector("_PlayerColour", _PlayerColour);
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CPing result = new CPing();
				result._PlayerColourX = material.GetVector("_PlayerColour").x;
				result._PlayerColourY = material.GetVector("_PlayerColour").y;
				result._PlayerColourZ = material.GetVector("_PlayerColour").z;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
