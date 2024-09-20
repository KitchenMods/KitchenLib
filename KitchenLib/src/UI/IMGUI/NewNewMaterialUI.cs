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
			ButtonName = "Materials";
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
			{"Simple Flat - Player", new CSimpleFlatPlayer()}
		};

		private static List<string> blenderShaderExports = new()
		{
			"Simple Flat",
			"Simple Transparent",
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
				string name = $"[{material.shader.name}] {material.name}";
				if (name.ToLower().Contains(materialSelectorSearchBar.ToLower()))
				{
					if (GUILayout.Button(name))
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
			List<string> scriptLines = new List<string>();
			List<string> existingMaterials = new List<string>();
			
			scriptLines.Add("import bpy");
			scriptLines.Add("");
			scriptLines.Add("");
			scriptLines.Add("def create_material(name, color):");
			scriptLines.Add("	if name in bpy.data.materials:");
			scriptLines.Add("		print(\"Material Already Found. Skipping\")");
			scriptLines.Add("	else:");
			scriptLines.Add("		material = bpy.data.materials.new(name)");
			scriptLines.Add("		material.diffuse_color = color");
			scriptLines.Add("		material.use_fake_user = 1");
			scriptLines.Add("");
			scriptLines.Add("create_material(\"ERROR_MATERIAL_MISSING\", (1,0,1,1))");
			scriptLines.Add("");

			int counter = 0;
			
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

				if (blenderShaderExports.Contains(material.shader.name))
				{
					string variableName = "material_" + counter;
					string materialName = material.name;

					if (!existingMaterials.Contains(materialName))
					{
						Color color = texture.Snapshot.GetPixel(5, 5);
				
						scriptLines.Add($"create_material(\"{materialName}\", ({color.r},{color.g},{color.b},{color.a}))");
						scriptLines.Add("");

						List<string> unityFile = CreateUnityMaterialFile(materialName, color);
						string path = Path.Combine(Application.persistentDataPath, "Debug/MaterialDumps/Unity");
						if (!Directory.Exists(path))
							Directory.CreateDirectory(path);
						string filePath = Path.Combine(Application.persistentDataPath, "Debug/MaterialDumps/Unity", materialName + ".mat");
						
						File.WriteAllLines(filePath, unityFile);
						
						counter++;
					}
				}
			}
			
			scriptLines.Add("");
			scriptLines.Add("");
			scriptLines.Add("error_material = bpy.data.materials[\"ERROR_MATERIAL_MISSING\"]");
			scriptLines.Add("");
			scriptLines.Add("for obj in bpy.data.objects:");
			scriptLines.Add("	if hasattr(obj.data, \"materials\"):");
			scriptLines.Add("		if len(obj.material_slots) == 0:");
			scriptLines.Add("			obj.data.materials.append(None)");
			scriptLines.Add("		for slot in obj.material_slots:");
			scriptLines.Add("			if slot.material is None:");
			scriptLines.Add("				slot.material = error_material");
			scriptLines.Add("			elif not slot.material.use_fake_user:");
			scriptLines.Add("				slot.material = error_material");
			scriptLines.Add("");
			scriptLines.Add("");
			
			string scriptPath = Path.Combine(Application.persistentDataPath, "Debug/MaterialDumps/blenderexport.py");
			File.WriteAllLines(scriptPath, scriptLines);
		}

		private List<string> CreateUnityMaterialFile(string name, Color color)
		{
			List<string> result = new List<string>();
			
			result.Add("%YAML 1.1");
			result.Add("%TAG !u! tag:unity3d.com,2011:");
			result.Add("--- !u!21 &2100000");
			result.Add("Material:");
			result.Add("  serializedVersion: 6");
			result.Add("  m_ObjectHideFlags: 0");
			result.Add("  m_CorrespondingSourceObject: {fileID: 0}");
			result.Add("  m_PrefabInstance: {fileID: 0}");
			result.Add("  m_PrefabAsset: {fileID: 0}");
			result.Add("  m_Name: " + name);
			result.Add("  m_Shader: {fileID: 46, guid: 0000000000000000f000000000000000, type: 0}");
			result.Add("  m_ShaderKeywords: ");
			result.Add("  m_LightmapFlags: 4");
			result.Add("  m_EnableInstancingVariants: 0");
			result.Add("  m_DoubleSidedGI: 0");
			result.Add("  m_CustomRenderQueue: -1");
			result.Add("  stringTagMap: {}");
			result.Add("  disabledShaderPasses: []");
			result.Add("  m_SavedProperties:");
			result.Add("    serializedVersion: 3");
			result.Add("    m_TexEnvs:");
			result.Add("    - _BumpMap:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _DetailAlbedoMap:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _DetailMask:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _DetailNormalMap:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _EmissionMap:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _MainTex:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _MetallicGlossMap:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _OcclusionMap:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    - _ParallaxMap:");
			result.Add("        m_Texture: {fileID: 0}");
			result.Add("        m_Scale: {x: 1, y: 1}");
			result.Add("        m_Offset: {x: 0, y: 0}");
			result.Add("    m_Floats:");
			result.Add("    - _BumpScale: 1");
			result.Add("    - _Cutoff: 0.5");
			result.Add("    - _DetailNormalMapScale: 1");
			result.Add("    - _DstBlend: 0");
			result.Add("    - _GlossMapScale: 1");
			result.Add("    - _Glossiness: 0.5");
			result.Add("    - _GlossyReflections: 1");
			result.Add("    - _Metallic: 0");
			result.Add("    - _Mode: 0");
			result.Add("    - _OcclusionStrength: 1");
			result.Add("    - _Parallax: 0.02");
			result.Add("    - _SmoothnessTextureChannel: 0");
			result.Add("    - _SpecularHighlights: 1");
			result.Add("    - _SrcBlend: 1");
			result.Add("    - _UVSec: 0");
			result.Add("    - _ZWrite: 1");
			result.Add("    m_Colors:");
			result.Add("    - _Color: {r: " + color.r + ", g: " + color.g + ", b: " + color.b + ", a: " + color.a + "}");
			result.Add("    - _EmissionColor: {r: 0, g: 0, b: 0, a: 1}");
			result.Add("  m_BuildTextureStacks: []");
			result.Add("");
			
			return result;
		}
	}
}
