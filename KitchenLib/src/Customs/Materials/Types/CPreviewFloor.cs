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
			GUILayout.BeginArea(new Rect(0, 20, 110, 20));
			GUILayout.Label("Line Rate");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(110, 20, 49, 20));
			material.SetFloat("_LineRate", float.Parse(GUILayout.TextField(material.GetFloat("_LineRate").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 25, 318, 20));
			material.SetFloat("_LineRate", GUILayout.HorizontalSlider(material.GetFloat("_LineRate"), 0.0f, 1.0f));
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(0, 40, 110, 20));
			GUILayout.Label("Line Offset");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(110, 40, 49, 20));
			material.SetFloat("_LineOffset", float.Parse(GUILayout.TextField(material.GetFloat("_LineOffset").ToString())));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(159, 45, 318, 20));
			material.SetFloat("_LineOffset", GUILayout.HorizontalSlider(material.GetFloat("_LineOffset"), 0.0f, 1.0f));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 60, 159, 20));
			material.SetInt("_IsKitchenFloor", GUILayout.Toggle(material.GetInt("_IsKitchenFloor") == 1, "Is Kitchen Floor") ? 1 : 0);
			if (material.GetInt("_IsKitchenFloor") == 0)
				material.DisableKeyword("_ISKITCHENFLOOR_ON");
			else
				material.EnableKeyword("_ISKITCHENFLOOR_ON");
			GUILayout.EndArea();
		}

		public void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CPreviewFloor result = new CPreviewFloor();
				
				result._LineRate = material.GetFloat("_LineRate");
				result._LineOffset = material.GetFloat("_LineOffset");
				result._IsKitchenFloor = material.GetInt("_IsKitchenFloor") == 1;

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
