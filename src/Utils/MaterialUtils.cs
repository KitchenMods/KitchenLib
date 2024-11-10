using KitchenLib.Customs;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenLib.Utils
{
    public static class MaterialUtils
	{
        private static readonly Dictionary<string, Material> MaterialIndex = new Dictionary<string, Material>();

        /// <summary>
        /// Apply a material array to a child renderer.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The material array to apply.</param>
		public static void ApplyMaterial(GameObject parent, string childPath, Material[] materials)
		{
            parent.GetChild(childPath).ApplyMaterial(materials);
		}

        /// <summary>
        /// Apply a material array to a child renderer.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The material array to apply.</param>
		public static void ApplyMaterial<T>(GameObject parent, string childPath, Material[] materials) where T : Renderer
		{
            parent.GetChild(childPath).ApplyMaterial<T>(materials);
        }

        /// <summary>
        /// Apply a material array to a GameObject's renderer.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="gameObject">The object to apply the material to.</param>
        /// <param name="materials">The material array to apply.</param>
        /// <returns>The input GameObject.</returns>
        public static GameObject ApplyMaterial<T>(this GameObject gameObject, params Material[] materials) where T : Renderer
        {
            var comp = gameObject?.GetComponent<T>();
            if (comp == null)
                return gameObject;

            comp.materials = materials;

            return gameObject;
        }

        /// <summary>
        /// Apply a material array to a GameObject's renderer.
        /// </summary>
        /// <param name="gameObject">The object to apply the material to.</param>
        /// <param name="materials">The material array to apply.</param>
        /// <returns>The input GameObject.</returns>
        public static GameObject ApplyMaterial(this GameObject gameObject, params Material[] materials)
        {
            return ApplyMaterial<MeshRenderer>(gameObject, materials);
        }

        /// <summary>
        /// Apply a material array to a GameObject's renderer.
        /// </summary>
        /// <param name="gameObject">The object to apply the material to.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The input GameObject.</returns>
        public static GameObject ApplyMaterial(this GameObject gameObject, params string[] materials)
        {
            return ApplyMaterial<MeshRenderer>(gameObject, GetMaterialArray(materials));
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="parent">The parent object.</param>
        /// <param name="nameContains">A filter that children names must contain to be modified.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChildren<T>(this GameObject parent, string nameContains, Material[] materials) where T : Renderer
        {
            for (int i = 0; i < parent.GetChildCount(); i++)
            {
                GameObject child = parent.GetChild(i);
                if (!child.name.ToLower().Contains(nameContains.ToLower()))
                    continue;
                child.ApplyMaterial<T>(materials);
            }

            return parent;
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="nameContains">A filter that children names must contain to be modified.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChildren(this GameObject parent, string nameContains, Material[] materials)
        {
            return ApplyMaterialToChildren<MeshRenderer>(parent, nameContains, materials);
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="nameContains">A filter that children names must contain to be modified.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChildren(this GameObject parent, string nameContains, params string[] materials)
        {
            return ApplyMaterialToChildren<MeshRenderer>(parent, nameContains, GetMaterialArray(materials));
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="parent">The parent object.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChildren<T>(this GameObject parent, Material[] materials) where T : Renderer
        {
            return ApplyMaterialToChildren<T>(parent, "", materials);
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChildren(this GameObject parent, Material[] materials)
        {
            return ApplyMaterialToChildren<MeshRenderer>(parent, "", materials);
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChildren(this GameObject parent, params string[] materials)
        {
            return ApplyMaterialToChildren<MeshRenderer>(parent, "", GetMaterialArray(materials));
        }

        /// <summary>
        /// Apply a material array to a child of a GameObject.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChild<T>(this GameObject parent, string childPath, Material[] materials) where T : Renderer
        {
            return parent.GetChild(childPath).ApplyMaterial<T>(materials);
        }

        /// <summary>
        /// Apply a material array to a child of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChild(this GameObject parent, string childPath, Material[] materials)
        {
            return parent.GetChild(childPath).ApplyMaterial(materials);
        }

        /// <summary>
        /// Apply a material array to a child of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        public static GameObject ApplyMaterialToChild(this GameObject parent, string childPath, params string[] materials)
        {
            return parent.GetChild(childPath).ApplyMaterial(GetMaterialArray(materials));
        }

        public static void SetupMaterialIndex()
        {
			if (MaterialIndex.Count > 0)
				return;

			foreach (Material material in Resources.FindObjectsOfTypeAll(typeof(Material)))
			{
				if (!MaterialIndex.ContainsKey(material.name))
				{
					MaterialIndex.Add(material.name, material);
				}
			}
		}

        /// <summary>
        /// Gets a list of all Materials.
        /// </summary>
        /// <param name="includeCustom">If the returned list should contain custom Materials.</param>
        /// <returns>The list of Materials.</returns>
		public static List<Material> GetAllMaterials(bool includeCustom)
		{
			List<Material> materials = new List<Material>();
			foreach (Material material in MaterialIndex.Values)
			{
				materials.Add(material);
			}

			return materials;
		}

        /// <summary>
        /// Gets a list of all Materials.
        /// </summary>
        /// <param name="includeCustom">If the returned list should contain custom Materials.</param>
        /// <param name="shaders">What shaders to filter by.</param>
        /// <returns>The list of Materials.</returns>
		public static List<Material> GetAllMaterials(bool includeCustom, List<string> shaders)
		{
			List<Material> materials = new List<Material>();
			foreach (Material material in MaterialIndex.Values)
			{
				if (shaders.Contains(material.shader.name))
					materials.Add(material);
			}

			foreach (Material material in CustomMaterials.GetCustomMaterials())
			{
				if (shaders.Contains(material.shader.name))
				{
					materials.Add(material);
				}
			}

			return materials;
		}

        /// <summary>
        /// Get a vanilla material by name.
        /// </summary>
        /// <param name="materialName">The name of the material to find.</param>
        /// <returns>The requested material or null if not found.</returns>
        public static Material GetExistingMaterial(string materialName)
        {
            if (MaterialIndex.ContainsKey(materialName))
                return MaterialIndex[materialName];
            else
                return null;
        }

        /// <summary>
        /// Get a custom material by name.
        /// </summary>
        /// <param name="materialName">The name of the material to find.</param>
        /// <returns>The requested material or null if not found.</returns>
        public static Material GetCustomMaterial(string materialName)
		{
			bool found = CustomMaterials.CustomMaterialsIndex.ContainsKey(materialName);
			if (found)
				return CustomMaterials.CustomMaterialsIndex[materialName];
			else
				return null;
        }

        /// <summary>
        /// Get a material array from a array of material names.
        /// </summary>
        /// <param name="materials">The names of the materials.</param>
        /// <returns>The corresponding material array.</returns>
        public static Material[] GetMaterialArray(params string[] materials)
        {
            List<Material> materialList = new();
            foreach (string matName in materials)
            {
                if (CustomMaterials.CustomMaterialsIndex.ContainsKey(matName))
                {
                    materialList.Add(CustomMaterials.CustomMaterialsIndex[matName]);
                }
                else
                {
                    materialList.Add(GetExistingMaterial(matName));
                }
            }
            return materialList.ToArray();
        }

        /// <summary>
        /// Get a Unity Color from a hex code.
        /// </summary>
        /// <param name="hex">The hex code.</param>
        /// <returns>The corresponding Color.</returns>
        public static Color ColorFromHex(int hex)
        {
            return new Color(((hex & 0xFF0000) >> 16) / 255.0f, ((hex & 0xFF00) >> 8) / 255.0f, (hex & 0xFF) / 255.0f);
        }

        /// <summary>
        /// Creates a new Material with the Simple Flat shader.
        /// </summary>
        /// <param name="name">The name for the Material.</param>
        /// <param name="color">The Color for the Material.</param>
        /// <param name="shininess">How shiny the Material should be.</param>
        /// <param name="overlayScale">The scale for the Material's overlay.</param>
        /// <returns>The created Material.</returns>
        public static Material CreateFlat(string name, Color color, float shininess = 0, float overlayScale = 10)
        {
            Material mat = new(Shader.Find("Simple Flat"))
            {
                name = name
            };
            color.a = 0;
            mat.SetColor("_Color0", color);
            mat.SetFloat("_Shininess", shininess);
            mat.SetFloat("_OverlayScale", overlayScale);
            return mat;
        }

        /// <summary>
        /// Creates a new Material with the Simple Flat shader.
        /// </summary>
        /// <param name="name">The name for the Material.</param>
        /// <param name="color">The hex code for the Material.</param>
        /// <param name="shininess">How shiny the Material should be.</param>
        /// <param name="overlayScale">The scale for the Material's overlay.</param>
        /// <returns>The created Material.</returns>
        public static Material CreateFlat(string name, int color, float shininess = 0, float overlayScale = 10)
        {
            return CreateFlat(name, ColorFromHex(color), shininess, overlayScale);
        }

        /// <summary>
        /// Creates a new Material with the Simple Transparent shader.
        /// </summary>
        /// <param name="name">The name for the Material.</param>
        /// <param name="color">The Color for the Material.</param>
        /// <returns>The created Material.</returns>
        public static Material CreateTransparent(string name, Color color)
        {
            Material mat = new(Shader.Find("Simple Transparent"))
            {
                name = name
            };
            mat.SetColor("_Color", color);
            return mat;
        }

        /// <summary>
        /// Creates a new Material with the Simple Transparent shader.
        /// </summary>
        /// <param name="name">The name for the Material.</param>
        /// <param name="color">The hex code for the Material.</param>
        /// <param name="opacity">The opacity for the Material.</param>
        /// <returns>The created Material.</returns>
        public static Material CreateTransparent(string name, int color, float opacity)
        {
            Color col = ColorFromHex(color);
            col.a = opacity;
            return CreateTransparent(name, col);
		}

		/// <summary>
		/// Replace Materials from the Unity Editor with Materials from PlateUp!
		/// </summary>
		/// <param name="gameObject">The GameObject to modify.</param>
		/// <returns>The modified GameObject</returns>
		public static GameObject AssignMaterialsByNames(this GameObject gameObject)
		{
			foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>())
			{
				if (transform.gameObject.HasComponent<Renderer>())
				{
					Renderer renderer = transform.gameObject.GetComponent<Renderer>();
					if (renderer != null)
					{
						Material[] materials = renderer.materials;
						List<Material> newMaterials = new List<Material>();
						foreach (Material material in materials)
						{
							Material mat = MaterialUtils.GetExistingMaterial(material.name.Replace(" (Instance)", ""));
							if (mat == null)
								mat = MaterialUtils.GetCustomMaterial(material.name.Replace(" (Instance)", ""));

							if (mat != null)
							{
								newMaterials.Add(mat);
							}
							else
							{
								newMaterials.Add(material);
							}
						}
						renderer.materials = newMaterials.ToArray();
					}
				}
			}
			return gameObject;
		}

    }
}
