using KitchenLib.Customs;
using KitchenLib.DevUI;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KitchenLib.UI
{
	public class NewMaterialUI : BaseUI
	{
		public NewMaterialUI()
		{
			ButtonName = "Materials";
		}

		public GameObject cube = null;
		public Material materialInstance = null;
		public Material selectedMaterial = null;

		private static Vector2 templateScrollPosition;
		private static Vector2 materialSelectorScrollPosition;
		private static string materialSelectorSearchBar = "";
		public static string importFileText = "";
		public static List<Material> MaterialTemplates = new List<Material>();

		public override void OnInit()
		{
			if (cube == null)
			{
				cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.position = new Vector3(0, 0, 0);
				cube.transform.localScale = new Vector3(5, 5, 5);
				cube.GetComponent<BoxCollider>().enabled = false;
				cube.SetActive(false);
			}
			else
			{
				cube.SetActive(true);
			}
			if (selectedMaterial == null)
				selectedMaterial = MaterialUtils.GetExistingMaterial("Wood - Default");

			UpdateMaterialInstance();
			UpdateMaterialCube();
			SetupDefaultTemplates();
		}
		public override void Setup()
		{
			if (IsEnabled)
				cube.SetActive(true);
			else
				cube.SetActive(false);

			GUILayout.BeginArea(new Rect(0, 0, 397, 100)); //Default Material Selector
			GUILayout.Label("Material Templates");
			templateScrollPosition = GUILayout.BeginScrollView(templateScrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			for (int i = 0; i < MaterialTemplates.Count; i++)
			{
				if (GUILayout.Button(MaterialTemplates[i].name))
				{
					SetCubeMaterial(MaterialTemplates[i]);
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(397, 0, 397, 100)); //Material Selector
			GUILayout.Label("Existing Materials");
			materialSelectorSearchBar = GUILayout.TextField(materialSelectorSearchBar);
			materialSelectorScrollPosition = GUILayout.BeginScrollView(materialSelectorScrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (Material material in MaterialUtils.GetAllMaterials(true, new List<string> { "Simple Flat", "Simple Transparent", "Flat", "Indicator Light", "Ghost", "Foliage", "Flat Image", "Fairy Lights", "Walls" }))
			{
				if (material.name.Contains(materialSelectorSearchBar))
				{
					if (GUILayout.Button(material.name))
					{
						SetCubeMaterial(material);
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 100, 397, 25));
			GUILayout.Label("Material Importer");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(397, 100, 397, 25));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 125, 397, 25));
			importFileText = GUILayout.TextField(importFileText);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(397, 125, 397, 25));
			if (GUILayout.Button("Import"))
			{
				string json = "";
				if (File.Exists(importFileText))
				{
					json = File.ReadAllText(importFileText);
				}
				if (!json.IsNullOrEmpty())
				{
					Material material = JSONManager.LoadMaterialFromJson(json);
					SetCubeMaterial(material);
				}
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 200, 795, 800)); //Material Value Editor

			GUILayout.Label("Material Name");
			selectedMaterial.name = GUILayout.TextField(selectedMaterial.name);

			switch (selectedMaterial.shader.name)
			{
				case "Simple Flat":
					CSimpleFlat.GUI(selectedMaterial);
					CSimpleFlat.Export(selectedMaterial);
					break;
				case "Simple Transparent":
					CSimpleTransparent.GUI(selectedMaterial);
					CSimpleTransparent.Export(selectedMaterial);
					break;
				case "Flat Image":
					CFlatImage.GUI(selectedMaterial);
					CFlatImage.Export(selectedMaterial);
					break;
				case "Flat":
					CFlat.GUI(selectedMaterial);
					CFlat.Export(selectedMaterial);
					break;
				case "Indicator Light":
					CIndicatorLight.GUI(selectedMaterial);
					CIndicatorLight.Export(selectedMaterial);
					break;
				case "Ghost":
					CGhost.GUI(selectedMaterial);
					CGhost.Export(selectedMaterial);
					break;
				case "Fairy Light":
					CFairyLight.GUI(selectedMaterial);
					CFairyLight.Export(selectedMaterial);
					break;
				case "Foliage":
					CFoliage.GUI(selectedMaterial);
					CFoliage.Export(selectedMaterial);
					break;
				case "Walls":
					CWalls.GUI(selectedMaterial);
					CWalls.Export(selectedMaterial);
					break;
			}
			SetCubeMaterial(selectedMaterial);
			GUILayout.EndArea();
		}

		public override void Disable()
		{
			cube.SetActive(false);
		}

		private void UpdateMaterialInstance()
		{
			if (materialInstance != selectedMaterial)
			{
				materialInstance = null;
				materialInstance = new Material(selectedMaterial);
			}
		}
		private void UpdateMaterialCube()
		{
			if (materialInstance != null)
				cube.GetComponent<Renderer>().material = materialInstance;
		}
		private void SetCubeMaterial(Material material)
		{
			selectedMaterial = material;
			UpdateMaterialInstance();
			UpdateMaterialCube();
		}
		private void SetupDefaultTemplates()
		{
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Simple Flat")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Simple Transparent")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Flat Image")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Flat")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Indicator Light")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Ghost")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Fairy Light")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Foliage")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Walls")));
		}
		private Material CreateTemplate(Shader shader)
		{
			Material tempMaterial = new Material(shader);
			tempMaterial.name = shader.name;
			return tempMaterial;
		}
	}
}
