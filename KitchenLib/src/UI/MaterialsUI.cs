using KitchenLib.DevUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using KitchenLib.Customs;
using System.IO;
using KitchenLib.Utils;

namespace KitchenLib.UI
{
	/*
	public class MaterialsUI : BaseUI
	{
		public GameObject MaterialDisplay = null;
		public MaterialsUI()
		{
			ButtonName = "Materials";
		}

		private string importText = string.Empty;
		public static bool isMenuEnabled = false;
		
		private static Vector2 searchScrollPosition;
		private static string searchText = string.Empty;

		public static Dictionary<string, Material> Materials = new Dictionary<string, Material>();
		public static List<string> MaterialNames = new List<string>();

		private static string overlayFile = "";
		private static string imageFile = "";
		private static string name = "";
		
		public static Material SelectedMaterial = null;

		public override void OnInit()
		{
			foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
			{
				if (material.shader.name.ToLower().Contains("simple") || material.shader.name.ToLower().Contains("image"))
				{
					int counter = 1;
					if (Materials.ContainsKey(material.name))
					{
						while (Materials.ContainsKey(material.name + counter))
						{
							counter++;
						}
						Materials.Add(material.name + counter, material);
						MaterialNames.Add(material.name + counter);
					}
					else
					{
						Materials.Add(material.name, material);
						MaterialNames.Add(material.name);
					}
				}
			}
			if (MaterialDisplay == null)
			{
				MaterialDisplay = GameObject.CreatePrimitive(PrimitiveType.Cube);
				MaterialDisplay.transform.localScale = new Vector3(5, 5, 5);
				MaterialDisplay.GetComponent<BoxCollider>().enabled = false;
				MaterialDisplay.SetActive(false);
			}
		}

		public override void Setup()
		{
			if (IsEnabled)
			{
				MaterialDisplay.SetActive(true);
			}
			else
			{
				MaterialDisplay.SetActive(false);
			}

			GUILayout.BeginArea(new Rect(0, 0, 795, 250));

			importText = GUILayout.TextField(importText);
			if (GUILayout.Button("Import"))
			{

				if (File.Exists(importText))
				{
					string jsontext = File.ReadAllText(importText);
					CustomBaseMaterial baseMaterial = JsonConvert.DeserializeObject<CustomBaseMaterial>(jsontext);
					Material material = null;
					if (baseMaterial.Type == JsonType.FlatColorMaterial)
					{
						JsonConvert.DeserializeObject<CustomSimpleFlat>(jsontext).ConvertMaterial(out material);
					}
					else if (baseMaterial.Type == JsonType.TransparentMaterial)
					{
						JsonConvert.DeserializeObject<CustomSimpleTransparent>(jsontext).ConvertMaterial(out material);
					}
					else if (baseMaterial.Type == JsonType.ImageMaterial)
					{
						JsonConvert.DeserializeObject<CustomFlatImage>(jsontext).ConvertMaterial(out material);
					}

					if (material != null)
					{
						SelectedMaterial = material;
						MaterialDisplay.GetComponent<MeshRenderer>().material = material;
					}
				}
			}

			searchText = GUILayout.TextField(searchText);
			searchScrollPosition = GUILayout.BeginScrollView(searchScrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			for (int i = 0; i < MaterialNames.Count; i++)
			{
				if (string.IsNullOrEmpty(searchText) || MaterialNames[i].ToLower().Contains(searchText.ToLower()))
				{
					if (GUILayout.Button(MaterialNames[i]))
					{
						MaterialDisplay.GetComponent<MeshRenderer>().material = Materials[MaterialNames[i]];
						SelectedMaterial = Materials[MaterialNames[i]];
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			GUILayout.BeginArea(new Rect(0, 250, 795, 750));

			if (SelectedMaterial != null)
			{
				if (SelectedMaterial.shader == null)
					return;

				Shader shader = SelectedMaterial.shader;
				if (shader.name.ToLower().Contains("simple") && shader.name.ToLower().Contains("flat"))
				{
					//Simple Flat
					SetupSimpleFlat(SelectedMaterial);
				}
				else if (shader.name.ToLower().Contains("flat") && shader.name.ToLower().Contains("image"))
				{
					//Flat Image
					SetupFlatImage(SelectedMaterial);
				}
				else if (shader.name.ToLower().Contains("simple") && shader.name.ToLower().Contains("transparent"))
				{
					//Simple Transparent
					SetupSimpleTransparent(SelectedMaterial);
				}
			}
			GUILayout.EndArea();
		}

		public override void Disable()
		{
			MaterialDisplay.SetActive(false);
		}

		private static void SetupSimpleFlat(Material material)
		{
			if (name == "")
				name = material.name;
			Vector4 _Color0 = material.GetVector("_Color0");
			int _Highlight = material.GetInt("_Highlight");
			Texture2D _Overlay = (Texture2D)material.GetTexture("_Overlay");
			int _HasTextureOverlay = material.GetInt("_HasTextureOverlay");
			float _Shininess = material.GetFloat("_Shininess");
			float _OverlayLowerBound = material.GetFloat("_OverlayLowerBound");
			float _OverlayUpperBound = material.GetFloat("_OverlayUpperBound");
			float _OverlayScale = material.GetFloat("_OverlayScale");
			float _OverlayMin = material.GetFloat("_OverlayMin");
			float _OverlayMax = material.GetFloat("_OverlayMax");
			Vector4 _OverlayOffset = material.GetVector("_OverlayOffset");
			Vector4 _OverlayTextureScale = material.GetVector("_OverlayTextureScale");
			Vector4 _OverlayColour = material.GetVector("_OverlayColour");

			GUILayout.Label("Name");
			name = GUILayout.TextField(name);
			GUILayout.Label("_Color0");
			_Color0.x = GUILayout.HorizontalSlider(_Color0.x, 0, 1);
			_Color0.y = GUILayout.HorizontalSlider(_Color0.y, 0, 1);
			_Color0.z = GUILayout.HorizontalSlider(_Color0.z, 0, 1);
			GUILayout.Label("_Highlight");
			_Highlight = (int)GUILayout.HorizontalSlider(_Highlight, 0, 1);
			GUILayout.Label("_Overlay");
			overlayFile = GUILayout.TextField(overlayFile);
			if (File.Exists(overlayFile))
				_Overlay = ResourceUtils.LoadTextureFromFile(overlayFile);
			GUILayout.Label("_HasTextureOverlay");
			_HasTextureOverlay = (int)GUILayout.HorizontalSlider(_HasTextureOverlay, 0, 1);
			GUILayout.Label("_Shininess");
			_Shininess = GUILayout.HorizontalSlider(_Shininess, 0, 1);
			GUILayout.Label("_OverlayLowerBound");
			_OverlayLowerBound = GUILayout.HorizontalSlider(_OverlayLowerBound, 0, 1);
			GUILayout.Label("_OverlayUpperBound");
			_OverlayUpperBound = GUILayout.HorizontalSlider(_OverlayUpperBound, 0, 1);
			GUILayout.Label("_OverlayScale");
			_OverlayScale = GUILayout.HorizontalSlider(_OverlayScale, 0, 1000);
			GUILayout.Label("_OverlayMin");
			_OverlayMin = GUILayout.HorizontalSlider(_OverlayMin, 0, 1);
			GUILayout.Label("_OverlayMax");
			_OverlayMax = GUILayout.HorizontalSlider(_OverlayMax, 0, 1);
			GUILayout.Label("_OverlayOffset");
			_OverlayOffset.x = GUILayout.HorizontalSlider(_OverlayOffset.x, 0, 1);
			_OverlayOffset.y = GUILayout.HorizontalSlider(_OverlayOffset.y, 0, 1);
			GUILayout.Label("_OverlayTextureScale");
			_OverlayTextureScale.x = GUILayout.HorizontalSlider(_OverlayTextureScale.x, 0, 100);
			_OverlayTextureScale.y = GUILayout.HorizontalSlider(_OverlayTextureScale.y, 0, 100);
			GUILayout.Label("_OverlayColour");
			_OverlayColour.x = GUILayout.HorizontalSlider(_OverlayColour.x, 0, 1);
			_OverlayColour.y = GUILayout.HorizontalSlider(_OverlayColour.y, 0, 1);
			_OverlayColour.z = GUILayout.HorizontalSlider(_OverlayColour.z, 0, 1);

			material.SetVector("_Color0", _Color0);
			material.SetInt("_Highlight", _Highlight);
			material.SetTexture("_Overlay", _Overlay);
			material.SetInt("_HasTextureOverlay", _HasTextureOverlay);
			if (_HasTextureOverlay == 0)
				material.DisableKeyword("_HASTEXTUREOVERLAY_ON");
			else
				material.EnableKeyword("_HASTEXTUREOVERLAY_ON");
			material.SetFloat("_Shininess", _Shininess);
			material.SetFloat("_OverlayLowerBound", _OverlayLowerBound);
			material.SetFloat("_OverlayUpperBound", _OverlayUpperBound);
			material.SetFloat("_OverlayScale", _OverlayScale);
			material.SetFloat("_OverlayMin", _OverlayMin);
			material.SetFloat("_OverlayMax", _OverlayMax);
			material.SetVector("_OverlayOffset", _OverlayOffset);
			material.SetVector("_OverlayTextureScale", _OverlayTextureScale);
			material.SetVector("_OverlayColour", _OverlayColour);
			if (GUILayout.Button("Export"))
			{
				CustomSimpleFlat mat = new CustomSimpleFlat()
				{
					Name = name,
					_Color0W = _Color0.w,
					_Color0X = _Color0.x,
					_Color0Y = _Color0.y,
					_Color0Z = _Color0.z,
					_Highlight = _Highlight,
					_OverlayBase64 = imgtob64(_Overlay),
					_HasTextureOverlay = _HasTextureOverlay,
					_Shininess = _Shininess,
					_OverlayLowerBound = _OverlayLowerBound,
					_OverlayUpperBound = _OverlayUpperBound,
					_OverlayScale = _OverlayScale,
					_OverlayMin = _OverlayMin,
					_OverlayMax = _OverlayMax,
					_OverlayOffsetW = _OverlayOffset.w,
					_OverlayOffsetX = _OverlayOffset.x,
					_OverlayOffsetY = _OverlayOffset.y,
					_OverlayOffsetZ = _OverlayOffset.z,
					_OverlayTextureScaleW = _OverlayTextureScale.w,
					_OverlayTextureScaleX = _OverlayTextureScale.x,
					_OverlayTextureScaleY = _OverlayTextureScale.y,
					_OverlayTextureScaleZ = _OverlayTextureScale.z,
					_OverlayColourW = _OverlayColour.w,
					_OverlayColourX = _OverlayColour.x,
					_OverlayColourY = _OverlayColour.y,
					_OverlayColourZ = _OverlayColour.z,
				};
				string json = JsonConvert.SerializeObject(mat, Formatting.Indented);
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + name + ".json", json);
			}
		}

		private static string imgtob64(Texture tex)
		{
			if (tex == null)
				return "";
			if (!tex.isReadable)
				return "";
			string enc = Convert.ToBase64String(((Texture2D)tex).EncodeToPNG());
			return enc;
		}

		private static void SetupFlatImage(Material material)
		{
			if (name == "")
				name = material.name;
			Texture _Image = material.GetTexture("_Image");
			float _Alpha = material.GetFloat("_Alpha");
			float _BlowoutScale1 = material.GetFloat("_BlowoutScale1");
			float _BlowoutOffset1 = material.GetFloat("_BlowoutOffset1");
			float _IsBlowout = material.GetFloat("_IsBlowout");

			GUILayout.Label("Name");
			name = GUILayout.TextField(name);

			GUILayout.Label("_Image");
			imageFile = GUILayout.TextField(imageFile);
			if (File.Exists(imageFile))
				_Image = (Texture)ResourceUtils.LoadTextureFromFile(imageFile);
			GUILayout.Label("_Alpha");
			_Alpha = GUILayout.HorizontalSlider(_Alpha, 0, 1);
			GUILayout.Label("_BlowoutScale1");
			_BlowoutScale1 = GUILayout.HorizontalSlider(_BlowoutScale1, 0, 1);
			GUILayout.Label("_BlowoutOffset1");
			_BlowoutOffset1 = GUILayout.HorizontalSlider(_BlowoutOffset1, 0, 1);
			GUILayout.Label("_IsBlowout");
			_IsBlowout = GUILayout.HorizontalSlider(_IsBlowout, 0, 1);

			material.SetTexture("_Image", _Image);
			material.SetFloat("_Alpha", _Alpha);
			material.SetFloat("_BlowoutScale1", _BlowoutScale1);
			material.SetFloat("_BlowoutOffset1", _BlowoutOffset1);
			material.SetFloat("_IsBlowout", _IsBlowout);
			if (GUILayout.Button("Export"))
			{
				CustomFlatImage mat = new CustomFlatImage()
				{
					Name = name,
					_ImageBase64 = imgtob64(_Image),
					_Alpha = _Alpha,
					_BlowoutScale1 = _BlowoutScale1,
					_BlowoutOffset1 = _BlowoutOffset1,
					_IsBlowout = _IsBlowout,
				};
				string json = JsonConvert.SerializeObject(mat, Formatting.Indented);
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + name + ".json", json);
			}
		}

		private static void SetupSimpleTransparent(Material material)
		{
			if (name == "")
				name = material.name;
			Vector4 _Color = material.GetVector("_Color");

			GUILayout.Label("Name");
			name = GUILayout.TextField(name);

			GUILayout.Label("_Color");
			_Color.w = GUILayout.HorizontalSlider(_Color.w, 0, 1);
			_Color.x = GUILayout.HorizontalSlider(_Color.x, 0, 1);
			_Color.y = GUILayout.HorizontalSlider(_Color.y, 0, 1);
			_Color.z = GUILayout.HorizontalSlider(_Color.z, 0, 1);

			material.SetVector("_Color", _Color);
			if (GUILayout.Button("Export"))
			{
				CustomSimpleTransparent mat = new CustomSimpleTransparent()
				{
					Name = name,
					_ColorW = _Color.w,
					_ColorX = _Color.x,
					_ColorY = _Color.y,
					_ColorZ = _Color.z,
				};
				string json = JsonConvert.SerializeObject(mat, Formatting.Indented);
				File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + name + ".json", json);
			}
		}
	}
	*/
}
