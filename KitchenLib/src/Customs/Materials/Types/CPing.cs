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
		public float _PlayerColourA = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Ping"));

			result.SetColor("_PlayerColour", _PlayerColour);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_PlayerColour = new Vector4(_PlayerColourX, _PlayerColourY, _PlayerColourZ, _PlayerColourA);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			
			material.SetColor("_PlayerColour", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_PlayerColour")));
		}

		public string Export(Material material)
		{
			CPing result = new CPing();
			result._PlayerColourX = material.GetColor("_PlayerColour").r;
			result._PlayerColourY = material.GetColor("_PlayerColour").g;
			result._PlayerColourZ = material.GetColor("_PlayerColour").b;
			result._PlayerColourA = material.GetColor("_PlayerColour").a;

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
