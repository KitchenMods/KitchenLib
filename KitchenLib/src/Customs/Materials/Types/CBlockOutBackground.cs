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
		public float _ColourA = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Block Out Background"));

			result.SetColor("_Colour", _Colour);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Colour = new Vector4(_ColourX, _ColourY, _ColourZ, _ColourA);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();

			material.SetColor("_Colour", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Colour")));
		}

		public string Export(Material material)
		{
			CBlockOutBackground result = new CBlockOutBackground();
			result._ColourX = material.GetColor("_Colour").r;
			result._ColourY = material.GetColor("_Colour").g;
			result._ColourZ = material.GetColor("_Colour").b;
			result._ColourA = material.GetColor("_Colour").a;

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
