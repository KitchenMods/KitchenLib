using KitchenLib.Customs;
using KitchenLib.DevUI;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Kitchen;
using KitchenLib.Interfaces;
using TMPro;

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

		public static Dictionary<string, IMaterialEditor> editors = new Dictionary<string, IMaterialEditor>
		{
			{"Simple Flat", new CSimpleFlat()},
			{"Blueprint Light", new CBlueprintLight()},
			{"Fairy Light", new CFairyLight()},
			{"Flat", new CFlat()},
			{"Flat Image", new CFlatImage()},
			{"Foliage", new CFoliage()},
			{"Ghost", new CGhost()},
			{"Indicator Light", new CIndicatorLight()},
			{"Mirror", new CMirror()},
			{"Simple Transparent", new CSimpleTransparent()},
			{"Walls", new CWalls()},
			{"Block Out Background", new CBlockOutBackground()},
			{"Circular Timer", new CCircularTimer()},
			{"Lake Surface", new CLakeSurface()},
			{"Mirror Backing", new CMirrorBacking()},
			{"Mirror Surface", new CMirrorSurface()},
			{"Newspaper", new CNewspaper()},
			{"Ping", new CPing()},
			{"Preview Floor", new CPreviewFloor()},
			{"Simple Flat - Player", new CSimpleFlatPlayer()},
		};

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

			GUILayout.BeginArea(new Rect(0, 0, 397, 200)); //Default Material Selector
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

			GUILayout.BeginArea(new Rect(397, 0, 397, 200)); //Material Selector
			GUILayout.Label("Existing Materials");
			materialSelectorSearchBar = GUILayout.TextField(materialSelectorSearchBar);
			materialSelectorScrollPosition = GUILayout.BeginScrollView(materialSelectorScrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (Material material in MaterialUtils.GetAllMaterials(true, editors.Keys.ToList()))
			{
				if (material.name.ToLower().Contains(materialSelectorSearchBar.ToLower()))
				{
					if (GUILayout.Button(material.name))
					{
						SetCubeMaterial(material);
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 200, 397, 25));
			GUILayout.Label("Material Importer");
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(397, 200, 397, 25));
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 225, 397, 25));
			importFileText = GUILayout.TextField(importFileText);
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(397, 225, 397, 25));
			if (GUILayout.Button("Import"))
			{
				string json = "";
				if (File.Exists(importFileText))
				{
					json = File.ReadAllText(importFileText);
				}
				if (!string.IsNullOrEmpty(json))
				{
					Material material = JSONManager.LoadMaterialFromJson(json);
					SetCubeMaterial(material);
				}
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(397, 250, 397, 25));
			if (GUILayout.Button("Dump Existing Materials"))
			{
				GenerateMaterialDump();
			}
			GUILayout.EndArea();

			GUILayout.BeginArea(new Rect(0, 300, 795, 60)); //Material Name Editor

			GUILayout.Label("Material Name");
			selectedMaterial.name = GUILayout.TextField(selectedMaterial.name);
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 360, 795, 740)); //Material Value Editor

			if (editors.ContainsKey(selectedMaterial.shader.name))
			{
				GUILayout.BeginArea(new Rect(0, 0, 795, 20));
				editors[selectedMaterial.shader.name].Export(selectedMaterial);
				GUILayout.EndArea();
				GUILayout.BeginArea(new Rect(0, 40, 795, 700));
				editors[selectedMaterial.shader.name].GUI(selectedMaterial);
				GUILayout.EndArea();
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
			foreach (string shader in editors.Keys)
			{
				MaterialTemplates.Add(CreateTemplate(Shader.Find(shader)));
			}
			
			MaterialTemplates.Add(CreateTemplate(Shader.Find("Simple Flat Transparent")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("UI Card")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("UI Panel")));
			MaterialTemplates.Add(CreateTemplate(Shader.Find("XP Badge")));
		}
		private Material CreateTemplate(Shader shader)
		{
			Material tempMaterial = new Material(shader);
			tempMaterial.name = shader.name;
			return tempMaterial;
		}

		private void GenerateMaterialDump()
		{
			foreach (Material material in MaterialUtils.GetAllMaterials(false, editors.Keys.ToList()))
			{
				GameObject gameObject = Main.bundle.LoadAsset<GameObject>("Material Dump Cube");
				gameObject.transform.position = new Vector3(0, 5, 0);

				TextMeshPro title = gameObject.GetChild("TitleText").GetComponent<TextMeshPro>();
				
				MaterialUtils.ApplyMaterial(gameObject, "Cube", new Material[] { material } );
				title.text = material.name;
				
				Quaternion rotation = new Quaternion(0, 0, 0, 0);
				SnapshotTexture texture = Snapshot.RenderPrefabToTexture(512, 512, gameObject, rotation, 0.5f, 0.5f);
				byte[] bytes = null;
				if (texture != null)
					bytes = texture.Snapshot.EncodeToPNG();
				if (bytes != null)
				{
					string path = Path.Combine(Application.persistentDataPath, "Debug/MaterialDumps");
					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);
					File.WriteAllBytes(Path.Combine(path, material.name + ".png"), bytes);
				}
			}
		}
	}
}
