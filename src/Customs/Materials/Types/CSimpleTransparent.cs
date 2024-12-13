using Newtonsoft.Json;
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
		public float _ColorA = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Simple Transparent"));

			result.SetColor("_Color", _Color);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, _ColorA);
		}
		IMColorPicker mainColorPicker;
		public void GUI(Material material)
		{
			if(mainColorPicker == null)
				mainColorPicker = new IMColorPicker();
			
			material.SetColor("_Color", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Color")));
		}

		public string Export(Material material)
		{
			CSimpleTransparent result = new CSimpleTransparent();
			result._ColorX = material.GetColor("_Color").r;
			result._ColorY = material.GetColor("_Color").g;
			result._ColorZ = material.GetColor("_Color").b;
			result._ColorA = material.GetColor("_Color").a;

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
