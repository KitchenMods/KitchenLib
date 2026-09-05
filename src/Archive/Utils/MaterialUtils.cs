using System;
using System.Collections.Generic;
using KitchenLib.Materials;
using UnityEngine;

namespace KitchenLib.Utils
{
	[Obsolete("Please use MaterialManager.")]
    public static class MaterialUtils
	{
		#region Obsolete Code

		[Obsolete("SetupMaterialIndex is also Obsolete.")]
		private static readonly Dictionary<string, Material> MaterialIndex = new Dictionary<string, Material>();
		
		[Obsolete("This was never intended on being public in the first place.")]
		public static void SetupMaterialIndex()
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "SetupMaterialIndex");
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
        /// Apply a material array to a child renderer.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The material array to apply.</param>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
		public static void ApplyMaterial(GameObject parent, string childPath, Material[] materials)
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterial");
            parent.GetChild(childPath).ApplyMaterial(materials);
		}

        /// <summary>
        /// Apply a material array to a child renderer.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The material array to apply.</param>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
		public static void ApplyMaterial<T>(GameObject parent, string childPath, Material[] materials) where T : Renderer
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterial<T>");
            parent.GetChild(childPath).ApplyMaterial<T>(materials);
        }

        /// <summary>
        /// Apply a material array to a GameObject's renderer.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="gameObject">The object to apply the material to.</param>
        /// <param name="materials">The material array to apply.</param>
        /// <returns>The input GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterial<T>(this GameObject gameObject, params Material[] materials) where T : Renderer
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterial<T>");
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
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterial(this GameObject gameObject, params Material[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterial");
            return ApplyMaterial<MeshRenderer>(gameObject, materials);
        }

        /// <summary>
        /// Apply a material array to a GameObject's renderer.
        /// </summary>
        /// <param name="gameObject">The object to apply the material to.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The input GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterial(this GameObject gameObject, params string[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterial");
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
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChildren<T>(this GameObject parent, string nameContains, Material[] materials) where T : Renderer
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChildren<T>");
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
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChildren(this GameObject parent, string nameContains, Material[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChildren");
            return ApplyMaterialToChildren<MeshRenderer>(parent, nameContains, materials);
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="nameContains">A filter that children names must contain to be modified.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChildren(this GameObject parent, string nameContains, params string[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChildren");
            return ApplyMaterialToChildren<MeshRenderer>(parent, nameContains, GetMaterialArray(materials));
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <typeparam name="T">The type of the renderer.</typeparam>
        /// <param name="parent">The parent object.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChildren<T>(this GameObject parent, Material[] materials) where T : Renderer
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChildren<T>");
            return ApplyMaterialToChildren<T>(parent, "", materials);
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChildren(this GameObject parent, Material[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChildren");
            return ApplyMaterialToChildren<MeshRenderer>(parent, "", materials);
        }

        /// <summary>
        /// Apply a material array to all children of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChildren(this GameObject parent, params string[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChildren");
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
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChild<T>(this GameObject parent, string childPath, Material[] materials) where T : Renderer
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChild<T>");
            return parent.GetChild(childPath).ApplyMaterial<T>(materials);
        }

        /// <summary>
        /// Apply a material array to a child of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChild(this GameObject parent, string childPath, Material[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChild");
            return parent.GetChild(childPath).ApplyMaterial(materials);
        }

        /// <summary>
        /// Apply a material array to a child of a GameObject.
        /// </summary>
        /// <param name="parent">The parent object.</param>
        /// <param name="childPath">The path to the child object.</param>
        /// <param name="materials">The names of the materials to apply.</param>
        /// <returns>The parent GameObject.</returns>
        [Obsolete("Please add Materials to GameObjects within Unity.")]
        public static GameObject ApplyMaterialToChild(this GameObject parent, string childPath, params string[] materials)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ApplyMaterialToChild");
            return parent.GetChild(childPath).ApplyMaterial(GetMaterialArray(materials));
        }

        /// <summary>
        /// Get a Unity Color from a hex code.
        /// </summary>
        /// <param name="hex">The hex code.</param>
        /// <returns>The corresponding Color.</returns>
        [Obsolete("Please create custom Materials within KitchenLib's Material Editor, and load them in Unity.")]
        public static Color ColorFromHex(int hex)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "ColorFromHex");
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
        [Obsolete("Please create custom Materials within KitchenLib's Material Editor, and load them in Unity.")]
        public static Material CreateFlat(string name, Color color, float shininess = 0, float overlayScale = 10)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "CreateFlat");
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
        [Obsolete("Please create custom Materials within KitchenLib's Material Editor, and load them in Unity.")]
        public static Material CreateFlat(string name, int color, float shininess = 0, float overlayScale = 10)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "CreateFlat");
            return CreateFlat(name, ColorFromHex(color), shininess, overlayScale);
        }

        /// <summary>
        /// Creates a new Material with the Simple Transparent shader.
        /// </summary>
        /// <param name="name">The name for the Material.</param>
        /// <param name="color">The Color for the Material.</param>
        /// <returns>The created Material.</returns>
        [Obsolete("Please create custom Materials within KitchenLib's Material Editor, and load them in Unity.")]
        public static Material CreateTransparent(string name, Color color)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "CreateTransparent");
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
        [Obsolete("Please create custom Materials within KitchenLib's Material Editor, and load them in Unity.")]
        public static Material CreateTransparent(string name, int color, float opacity)
        {
	        BaseMod.ObsoleteCodeWarning("MaterialUtils", "CreateTransparent");
            Color col = ColorFromHex(color);
            col.a = opacity;
            return CreateTransparent(name, col);
		}

		/// <summary>
		/// Gets a list of all Materials.
		/// </summary>
		/// <param name="includeCustom">If the returned list should contain custom Materials.</param>
		/// <returns>The list of Materials.</returns>
		[Obsolete("Please use MaterialManager.GetAllMaterials() instead")]
		public static List<Material> GetAllMaterials(bool includeCustom)
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "GetAllMaterials");
			return MaterialManager.GetAllMaterials(includeCustom);
		}

		/// <summary>
		/// Gets a list of all Materials.
		/// </summary>
		/// <param name="includeCustom">If the returned list should contain custom Materials.</param>
		/// <param name="shaders">What shaders to filter by.</param>
		/// <returns>The list of Materials.</returns>
		[Obsolete("Please use MaterialManager.GetAllMaterialsOfShader() instead")]
		public static List<Material> GetAllMaterials(bool includeCustom, List<string> shaders)
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "GetAllMaterials");
			List<Shader> _tempShaders = new List<Shader>();
			foreach (string shaderName in shaders)
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					foreach (Shader loadedShader in Resources.FindObjectsOfTypeAll<Shader>())
					{
						if (loadedShader.name == shaderName)
							shader =  loadedShader;
					}
				}
				
				if (shader != null)
					_tempShaders.Add(shader);
			}

			return MaterialManager.GetAllMaterialsOfShader(_tempShaders, includeCustom);
		}

		/// <summary>
		/// Get a vanilla material by name.
		/// </summary>
		/// <param name="materialName">The name of the material to find.</param>
		/// <returns>The requested material or null if not found.</returns>
		[Obsolete("Please use MaterialManager.GetMaterial() instead")]
		public static Material GetExistingMaterial(string materialName)
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "GetExistingMaterial");
			return MaterialManager.GetMaterial(materialName);
		}

		/// <summary>
		/// Get a custom material by name.
		/// </summary>
		/// <param name="materialName">The name of the material to find.</param>
		/// <returns>The requested material or null if not found.</returns>
		[Obsolete("Please use MaterialManager.GetMaterial() instead")]
		public static Material GetCustomMaterial(string materialName)
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "GetCustomMaterial");
			return MaterialManager.GetMaterial(materialName);
		}

		/// <summary>
		/// Get a material array from a array of material names.
		/// </summary>
		/// <param name="materials">The names of the materials.</param>
		/// <returns>The corresponding material array.</returns>
		[Obsolete("Please use MaterialManager.GetMaterial() instead")]
		public static Material[] GetMaterialArray(params string[] materials)
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "GetMaterialArray");
			List<Material> result = new List<Material>();
			foreach (string materialName in materials)
			{
				Material foundMaterial = MaterialManager.GetMaterial(materialName);
				if (foundMaterial == null) continue;
				result.Add(foundMaterial);
			}
			return result.ToArray();
		}

		/// <summary>
		/// Replace Materials from the Unity Editor with Materials from PlateUp!
		/// </summary>
		/// <param name="gameObject">The GameObject to modify.</param>
		/// <returns>The modified GameObject</returns>
		[Obsolete("Please use MaterialManager.AssignMaterialsByNames() instead")]
		public static GameObject AssignMaterialsByNames(this GameObject gameObject)
		{
			BaseMod.ObsoleteCodeWarning("MaterialUtils", "AssignMaterialsByNames");
			return MaterialManager.AssignMaterialsByNames(gameObject);
		}

		#endregion
    }
}
