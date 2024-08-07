using Newtonsoft.Json;
using System;
using KitchenLib.Interfaces;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CPreviewFloor : CustomMaterial, IMaterialEditor
	{
		public override JsonType Type => JsonType.CPreviewFloor;

		public float _LineRate = 3;
		public float _LineOffset = 1;
		public bool _IsKitchenFloor = false;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Preview Floor"));

			result.SetFloat("_LineRate", _LineRate);
			result.SetFloat("_LineOffset", _LineOffset);
			result.SetInt("_IsKitchenFloor", _IsKitchenFloor ? 1 : 0);
			if (!_IsKitchenFloor)
				result.DisableKeyword("_ISKITCHENFLOOR_ON");
			else
				result.EnableKeyword("_ISKITCHENFLOOR_ON");
			
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
		}
		public void GUI(Material material)
		{
			material.SetFloat("_LineRate", DrawSliderModule(new Rect(2, 2, 446, 24), "_LineRate", material.GetFloat("_LineRate"), 0, 30));
			material.SetFloat("_LineOffset", DrawSliderModule(new Rect(2, 32, 446, 24), "_LineOffset", material.GetFloat("_LineOffset"), 0, 1));
			material.SetFloat("_IsKitchenFloor", DrawToggleModule(new Rect(2, 66, 146, 25), "_IsKitchenFloor", material.GetFloat("_IsKitchenFloor") == 1) ? 1 : 0);
			
			if (material.GetInt("_IsKitchenFloor") != 1)
				material.DisableKeyword("_ISKITCHENFLOOR_ON");
			else
				material.EnableKeyword("_ISKITCHENFLOOR_ON");
		}

		public string Export(Material material)
		{
			CPreviewFloor result = new CPreviewFloor();
				
			result._LineRate = material.GetFloat("_LineRate");
			result._LineOffset = material.GetFloat("_LineOffset");
			result._IsKitchenFloor = material.GetInt("_IsKitchenFloor") == 1;

			result.Name = material.name;

			string json = JsonConvert.SerializeObject(result, Formatting.Indented);
			return json;
		}
	}
}
