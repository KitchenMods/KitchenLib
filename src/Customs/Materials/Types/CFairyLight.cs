using Newtonsoft.Json;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CFairyLight : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CFairyLight;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;
		public float _ColorA = 0.0f;

		public float _Float0 = 1;
		

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Fairy Light"));

			result.SetColor("_Color0", _Color);
			result.SetFloat("_Float0", _Float0);
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
			
			material.SetColor("_Color0", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Color0")));
			// insensity slider?
			material.SetFloat("_Float0", DrawSliderModule(new Rect(2, 199, 446, 24), "_Float0", material.GetFloat("_Float0")));
		}

		public string Export(Material material)
		{
			CFairyLight result = new CFairyLight();
			result._ColorX = material.GetColor("_Color0").r;
			result._ColorY = material.GetColor("_Color0").g;
			result._ColorZ = material.GetColor("_Color0").b;
			result._ColorA = material.GetColor("_Color0").a;
			
			result._Float0 = material.GetFloat("_Float0");

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
