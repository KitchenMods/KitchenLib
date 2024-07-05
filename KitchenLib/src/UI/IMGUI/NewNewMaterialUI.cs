using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kitchen;
using KitchenLib.Customs;
using KitchenLib.DevUI;
using KitchenLib.Interfaces;
using KitchenLib.Utils;
using TMPro;
using UnityEngine;

namespace KitchenLib.UI
{
	public class NewNewMaterialUI : BaseUI
	{
		public NewNewMaterialUI()
		{
			ButtonName = "Materials2";
		}

		private Material selectedMaterial;
		private Material defaultMaterial;
		
		
		private GameObject materialDisplay;
		private bool isNewMenuInstance = true;
		
		private static Vector2 templateScrollPosition;
		private static Vector2 materialSelectorScrollPosition;
		private static Vector2 customMaterialSelectorScrollPosition;

		private string materialSelectorSearchBar = "";
		private string customMaterialSelectorSearchBar = "";
		private static string customMaterialsPath = Path.Combine(Application.persistentDataPath, "ModData", "KitchenLib", "Materials");
		
		private List<Material> MaterialTemplates = new();
		private List<Material> CustomMaterials = new(); 

		private static Dictionary<string, IMaterialEditor> editors = new()
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
			if (materialDisplay == null)
			{
				materialDisplay = GameObject.Instantiate(Main.bundle.LoadAsset<GameObject>("Material Display"), new Vector3(0, 3, 0), Quaternion.identity);
				materialDisplay.SetActive(false);
			}

			if (defaultMaterial == null)
			{
				defaultMaterial = MaterialUtils.GetExistingMaterial("Wood - Default");
			}

			if (!Directory.Exists(customMaterialsPath))
			{
				Directory.CreateDirectory(customMaterialsPath);
			}

			SetupTemplates();
			SetupCustomMaterials();
		}
		
		public override void Setup()
		{
			if (isNewMenuInstance)
			{
				isNewMenuInstance = false;
				SetDisplayMaterial(defaultMaterial);
			}
			materialDisplay.SetActive(true);
			
			
			GUILayout.BeginArea(new Rect(0, 0, 397, 200));
			DrawShaderTemplates();
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(398, 0, 397, 200));
			DrawExistingMaterial();
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 201, 397, 200));
			DrawCustomMaterials();
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(398, 201, 397, 200));
			DrawMiscOptions();
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(0, 405, 795, 672));
			
			if (editors.ContainsKey(selectedMaterial.shader.name))
			{
				editors[selectedMaterial.shader.name].GUI(selectedMaterial);
			}
			
			GUILayout.EndArea();
			
		}
		
		public override void Disable()
		{
			isNewMenuInstance = true;
			materialDisplay.SetActive(false);
		}

		private void DrawShaderTemplates()
		{
			GUILayout.Label("Material Templates");
			templateScrollPosition = GUILayout.BeginScrollView(templateScrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			for (int i = 0; i < MaterialTemplates.Count; i++)
			{
				if (GUILayout.Button(MaterialTemplates[i].name))
				{
					SetDisplayMaterial(new Material(MaterialTemplates[i]));
				}
			}
			GUILayout.EndScrollView();
		}

		private void DrawExistingMaterial()
		{
			GUILayout.Label("Existing Materials");
			materialSelectorSearchBar = GUILayout.TextField(materialSelectorSearchBar);
			materialSelectorScrollPosition = GUILayout.BeginScrollView(materialSelectorScrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (Material material in MaterialUtils.GetAllMaterials(true, editors.Keys.ToList()))
			{
				if (material.name.ToLower().Contains(materialSelectorSearchBar.ToLower()))
				{
					if (GUILayout.Button(material.name))
					{
						SetDisplayMaterial(material);
					}
				}
			}
			GUILayout.EndScrollView();
		}

		private void DrawCustomMaterials()
		{
			GUILayout.Label("Custom Materials");
			customMaterialSelectorSearchBar = GUILayout.TextField(customMaterialSelectorSearchBar);
			customMaterialSelectorScrollPosition = GUILayout.BeginScrollView(customMaterialSelectorScrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			foreach (Material material in CustomMaterials)
			{
				if (material.name.ToLower().Contains(customMaterialSelectorSearchBar.ToLower()))
				{
					if (GUILayout.Button(material.name))
					{
						SetDisplayMaterial(material);
					}
				}
			}
			GUILayout.EndScrollView();
		}

		private void DrawMiscOptions()
		{
			GUILayout.BeginArea(new Rect(2, 150, 97, 22));
			GUILayout.Label("Material Name: ");
			GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(104, 150, 291, 22));
			selectedMaterial.name = GUILayout.TextField(selectedMaterial.name);
			GUILayout.EndArea();
			
			
			GUILayout.BeginArea(new Rect(2, 176, 393, 22));
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Save Material"))
			{
				if (editors.ContainsKey(selectedMaterial.shader.name))
				{
					string json = editors[selectedMaterial.shader.name].Export(selectedMaterial);
					File.WriteAllText(Path.Combine(customMaterialsPath, selectedMaterial.name + ".json"), json);
					SetupCustomMaterials();
				}
			}

			if (GUILayout.Button("Dump All Materials"))
			{
				GenerateMaterialDump();
			}

			if (GUILayout.Button("Reload Customs"))
			{
				SetupCustomMaterials();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		private void SetDisplayMaterial(Material material)
		{
			selectedMaterial = material;
			foreach (Renderer renderer in materialDisplay.GetComponentsInChildren<Renderer>())
			{
				renderer.material = selectedMaterial;
			}
		}

		private void SetupTemplates()
		{
			MaterialTemplates.Clear();
			foreach (string shader in editors.Keys)
			{
				MaterialTemplates.Add(new Material(Shader.Find(shader)));
			}
		}
		
		private void SetupCustomMaterials()
		{
			CustomMaterials.Clear();
			foreach (string file in Directory.GetFiles(customMaterialsPath))
			{
				string json = File.ReadAllText(file);
				Material material = JSONManager.LoadMaterialFromJson(json);
				CustomMaterials.Add(material);
			}
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
