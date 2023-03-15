using Newtonsoft.Json;
using System;
using UnityEngine;

namespace KitchenLib.Customs
{
	public class CBlueprintLight: CustomMaterial
	{
		public override JsonType Type => JsonType.CBlueprintLight;
		[JsonIgnore]
		public virtual Color _Color { get; set; } = Color.black;
		public float _ColorX = 0.0f;
		public float _ColorY = 0.0f;
		public float _ColorZ = 0.0f;
		
		[JsonIgnore]
		public virtual Color _Color0 { get; set; } = Color.black;
		public float _Color0X = 0.0f;
		public float _Color0Y = 0.0f;
		public float _Color0Z = 0.0f;

		public virtual float _HasColour { get; set; } = 0.0f;
		public virtual float _IsCopy { get; set; } = 0.0f;

		public override void ConvertMaterial(out Material material)
		{
			Material result = new Material(Shader.Find("Blueprint Light"));

			result.SetColor("_Color", _Color);
			result.SetColor("_Color0", _Color0);
			result.SetFloat("_HasColour", _HasColour);
			result.SetFloat("_IsCopy", _IsCopy);
			result.name = Name;

			material = result;
		}

		public override void Deserialise()
		{
			_Color = new Vector4(_ColorX, _ColorY, _ColorZ, 0);
			_Color0 = new Vector4(_Color0X, _Color0Y, _Color0Z, 0);
		}
		public static void GUI(Material material)
		{
			Vector4 _Color = material.GetVector("_Color");

			GUILayout.Label("_Color");
			_Color.x = GUILayout.HorizontalSlider(_Color.x, 0.0f, 3.0f);
			_Color.y = GUILayout.HorizontalSlider(_Color.y, 0.0f, 3.0f);
			_Color.z = GUILayout.HorizontalSlider(_Color.z, 0.0f, 3.0f);
			material.SetVector("_Color", _Color);

			
			Vector4 _Color0 = material.GetVector("_Color0");

			GUILayout.Label("_Color0");
			_Color0.x = GUILayout.HorizontalSlider(_Color0.x, 0.0f, 3.0f);
			_Color0.y = GUILayout.HorizontalSlider(_Color0.y, 0.0f, 3.0f);
			_Color0.z = GUILayout.HorizontalSlider(_Color0.z, 0.0f, 3.0f);
			material.SetVector("_Color0", _Color0);

			float _HasColour = material.GetFloat("_HasColour");
			GUILayout.Label("_HasColour");
			_HasColour = GUILayout.HorizontalSlider(_HasColour, 0.0f, 1.0f);
			material.SetFloat("_HasColour", _HasColour);

			float _IsCopy = material.GetFloat("_IsCopy");
			GUILayout.Label("_IsCopy");
			_IsCopy = GUILayout.HorizontalSlider(_IsCopy, 0.0f, 1.0f);
			material.SetFloat("_IsCopy", _IsCopy);
		}

		public static void Export(Material material)
		{
			if (GUILayout.Button("Export"))
			{
				CBlueprintLight result = new CBlueprintLight();
				result._ColorX = material.GetVector("_Color").x;
				result._ColorY = material.GetVector("_Color").y;
				result._ColorZ = material.GetVector("_Color").z;
				
				result._Color0X = material.GetVector("_Color0").x;
				result._Color0Y = material.GetVector("_Color0").y;
				result._Color0Z = material.GetVector("_Color0").z;

				result._HasColour = material.GetFloat("_HasColour");

				result._IsCopy = material.GetFloat("_IsCopy");

				result.Name = material.name;

				string json = JsonConvert.SerializeObject(result, Formatting.Indented);
				System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"/{result.Name}.json", json);
			}
		}
	}
}
