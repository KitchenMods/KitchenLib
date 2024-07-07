using Newtonsoft.Json;
using imColorPicker;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CGhost : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CGhost;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;
		public float _ColorA = 0.0f;

		public bool _Hatched = false;
		

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Ghost"));

			result.SetColor("_Colour", _Color);
			result.SetFloat("_Hatched", _Hatched ? 1 : 0);
			
			if (!_Hatched)
				result.DisableKeyword("_HATCHED_ON");
			else
				result.EnableKeyword("_HATCHED_ON");
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
			
			material.SetColor("_Colour", DrawColorModule(new Rect(2, 2, 146, 186), mainColorPicker, "Primary Color", material.GetVector("_Colour")));
			material.SetFloat("_Hatched", DrawToggleModule(new Rect(2, 195, 146, 25), "_Hatched", material.GetFloat("_Hatched") == 1) ? 1 : 0);
			
			if (material.GetFloat("_Hatched") == 1)
				material.EnableKeyword("_HATCHED_ON");
			else
				material.DisableKeyword("_HATCHED_ON");
			
		}

		public string Export(Material material)
		{
			CGhost result = new CGhost();
			result._ColorX = material.GetColor("_Colour").r;
			result._ColorY = material.GetColor("_Colour").g;
			result._ColorZ = material.GetColor("_Colour").b;
			result._ColorA = material.GetColor("_Colour").a;
			
			result._Hatched = material.GetFloat("_Hatched") == 1;

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
