using Newtonsoft.Json;
using System;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CBlockOutBackground : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CBlockOutBackground;
		[JsonIgnore]
		public virtual Color _Colour { get; set; } = Color.black;
		public float _ColourX = 0.0f;
		public float _ColourY = 0.0f;
		public float _ColourZ = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Block Out Background"));

			result.SetColor("_Colour", _Colour);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Colour = new Vector4(_ColourX, _ColourY, _ColourZ, 0);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			Vector4 _Colour = material.GetVector("_Colour");
			
			GUILayout.BeginArea(new Rect(0, 0, 159, 20));
			GUILayout.Label("Base Color");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 20, 159, 140));
			_Colour = mainColorPicker.DrawColorPicker(_Colour);
			material.SetVector("_Colour", _Colour);
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CBlockOutBackground result = new CBlockOutBackground();
				result._ColourX = material.GetVector("_Colour").x;
				result._ColourY = material.GetVector("_Colour").y;
				result._ColourZ = material.GetVector("_Colour").z;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
