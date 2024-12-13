using System;
using System.Collections.Generic;
using System.IO;
using imColorPicker;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Customs
{
	public abstract class CustomMaterial : BaseJson
	{
		public virtual void ConvertMaterial(out Material material) { material = null; }
		public virtual void Deserialise() { }
		public virtual string Name { get; set; } = "";

		protected static string imgtob64(Texture texture, bool requiresFlip = false)
		{
			string enc = "";
			if (texture == null)
				return enc;
			
			if (texture is Texture2D texture2D)
			{
				if (!texture2D.isReadable)
				{
					texture2D = duplicateTexture(texture2D, requiresFlip);
				}
				enc = Convert.ToBase64String((texture2D).EncodeToPNG());
			}
			else
			{
				Debug.LogError("Texture is not Texture2D");
			}

			return enc;
		}
		
		private static Texture2D duplicateTexture(Texture2D source, bool requiresFlip = false)
		{
			RenderTexture renderTex = RenderTexture.GetTemporary(
				source.width,
				source.height,
				0,
				RenderTextureFormat.Default,
				RenderTextureReadWrite.sRGB);

			Graphics.Blit(source, renderTex);
			RenderTexture previous = RenderTexture.active;
			RenderTexture.active = renderTex;
			Texture2D readableText = new Texture2D(source.width, source.height);
			readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
			readableText.Apply();
			RenderTexture.active = previous;
			RenderTexture.ReleaseTemporary(renderTex);

			if (requiresFlip)
			{
				// Flip the texture vertically
				var pixels = readableText.GetPixels();
				System.Array.Reverse(pixels, 0, pixels.Length);
				readableText.SetPixels(pixels);
				readableText.Apply();
			}

			return readableText;
		}

		protected Color DrawColorModule(Rect rect, IMColorPicker module, string name, Color color)
		{
			GUILayout.BeginArea(rect);
			GUILayout.Label(name);
			color = module.DrawColorPicker(color);
			color.a = GUILayout.HorizontalSlider(color.a, 0, 1);
			GUILayout.EndArea();
			
			return color;
		}
		
		
		// This is currently unfinished and non-functional
		protected Color DrawHDRColorModule(Rect rect, IMColorPicker module, string name, Color color, float intensity, out float newIntensity)
		{
			GUILayout.BeginArea(rect);

			GUILayout.Label(name);
			color /= intensity;
			color = module.DrawColorPicker(color);
			GUILayout.BeginHorizontal();
			color.a = GUILayout.HorizontalSlider(color.a, 0, 1);
			intensity = GUILayout.HorizontalSlider(intensity, 1, 5);
			GUILayout.EndHorizontal();
			
			GUILayout.EndArea();

			color *= intensity;
			newIntensity = intensity;
			return color;
		}

		protected bool DrawToggleModule(Rect rect, string name, bool toggle)
		{
			GUILayout.BeginArea(rect);
			
			toggle = GUILayout.Toggle(toggle, name);
			
			GUILayout.EndArea();
			
			return toggle;
		}
		
		protected float DrawSliderModule(Rect rect, string name, float value, float min = 0, float max = 25, int labelWidthPercentage = 25, int typeBoxWidthPercentage = 15, int sliderWidthPercentage = 60)
		{
			GUILayout.BeginArea(rect);
			
			GUILayout.BeginArea(new Rect(0, 0, GetPercentageOf(rect.width, labelWidthPercentage), rect.height));
			GUILayout.Label(name);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(GetPercentageOf(rect.width, labelWidthPercentage), 0, GetPercentageOf(rect.width, typeBoxWidthPercentage), rect.height));
			value = float.Parse(GUILayout.TextField(value.ToString()));
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(GetPercentageOf(rect.width, labelWidthPercentage) + GetPercentageOf(rect.width, typeBoxWidthPercentage), 0, GetPercentageOf(rect.width, sliderWidthPercentage), rect.height));
			value = GUILayout.HorizontalSlider(value, min, max);
			GUILayout.EndArea();
			
			GUILayout.EndArea();

			return value;
		}
		
		protected string DrawTextureModule(Rect rect, string name, string texturePath, Material material = null, string textureName = "_Image")
		{
			GUILayout.BeginArea(rect);
			
			GUILayout.BeginArea(new Rect(2, 2, 288, 25));
			GUILayout.Label(name);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(2, 22, 288, 25));
			texturePath = GUILayout.TextField(texturePath);
			GUILayout.EndArea();

			if (File.Exists(texturePath))
			{
				Texture2D texture = ResourceUtils.LoadTextureFromFile(texturePath);
				GUI.DrawTexture(new Rect(2, 49, 156, 156), texture);
			}
			
			GUILayout.BeginArea(new Rect(160, 49, 130, 25));
			if (material != null)
			{
				if (GUILayout.Button("Apply Texture"))
				{
					if (File.Exists(texturePath))
					{
						material.SetTexture(textureName, ResourceUtils.LoadTextureFromFile(texturePath));
					}
				}
			}
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(160, 79, 130, 25));
			if (material != null)
			{
				if (GUILayout.Button("Clear Texture"))
				{
					material.SetTexture("_Image", null);
				}
			}
			GUILayout.EndArea();
			
			GUILayout.EndArea();
			
			return texturePath;
		}
		
		private float GetPercentageOf(float value, float percentage)
		{
			return value * percentage / 100;
		}
	}
}
