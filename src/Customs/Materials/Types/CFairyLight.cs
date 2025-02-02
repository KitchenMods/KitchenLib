using System.Collections.Generic;
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

		public float _Intensity = 1;
		public float _Float0 = 1;

		[JsonIgnore] private readonly Dictionary<Material, Color> baseColorCache = new Dictionary<Material, Color>();
		

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

			if (!baseColorCache.ContainsKey(material))
				baseColorCache.Add(material, material.GetVector("_Color0"));
			
			baseColorCache[material] = DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", baseColorCache[material]);
			_Intensity = DrawSliderModule(new Rect(2, 199, 446, 24), "_Intensity", _Intensity, 1, 25);
			
			material.SetColor("_Color0", baseColorCache[material] * _Intensity);
			material.SetFloat("_Float0", DrawSliderModule(new Rect(2, 223, 446, 24), "_Float0", material.GetFloat("_Float0")));
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
