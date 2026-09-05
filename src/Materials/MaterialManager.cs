using System.Collections.Generic;
using KitchenLib.Utils;
using UnityEngine;

namespace KitchenLib.Materials
{
	public static class MaterialManager
	{
		public static Dictionary<string, Material> RegisteredCustomMaterials = new Dictionary<string, Material>();
		public static Dictionary<string, Material> LoadedVanillaMaterials = new Dictionary<string, Material>();

		#region Internal Setup

		/// <summary>
		/// Compiles a list of Materials pre-existing within the vanilla game
		/// </summary>
		internal static void CollectVanillaMaterials()
		{
			foreach (var material in Resources.FindObjectsOfTypeAll<Material>())
			{
				if (!LoadedVanillaMaterials.ContainsKey(material.name))
				{
					LoadedVanillaMaterials.Add(material.name, material);
				}
			}
		}
		
		/// <summary>
		/// Registered a custom material to the material index
		/// </summary>
		/// <param name="materialName">The name for the material being registered.</param>
		/// <param name="material">The material being registered</param>
		/// <returns>True when the material is added to the index, False if the material is already in the index</returns>
		internal static bool RegisterCustomMaterial(string materialName, Material material)
		{
			if (RegisteredCustomMaterials.ContainsKey(materialName)) return false;
			RegisteredCustomMaterials.Add(materialName, material);
			return true;
		}

		#endregion

		#region External Calls

		/// <summary>
		/// Used to get a complete list of all loaded Materials
		/// </summary>
		/// <param name="includeCustomMaterials">When True, the returned list will contain non-vanilla Materials</param>
		/// <returns>Returns a List of loaded Materials </returns>
		public static List<Material> GetAllMaterials(bool includeCustomMaterials = true)
		{
			List<Material> result = new List<Material>();
			
			result.AddRange(LoadedVanillaMaterials.Values);
			if (includeCustomMaterials)
			{
				result.AddRange(RegisteredCustomMaterials.Values);
			}

			return result;
		}

		/// <summary>
		/// Used to get a complete list of all loaded Materials which use a specified Shader
		/// </summary>
		/// <param name="shader">Decides which Materials should be returned</param>
		/// <param name="includeCustomMaterials">When True, the returned list will contain non-vanilla Materials</param>
		/// <returns>Returns a List of loaded Materials with a certain Shader</returns>
		public static List<Material> GetAllMaterialsOfShader(Shader shader, bool includeCustomMaterials = true)
		{
			List<Material> result = new List<Material>();

			foreach (Material material in LoadedVanillaMaterials.Values)
			{
				if (material.shader == shader)
					result.Add(material);
			}

			if (includeCustomMaterials)
			{
				foreach (Material material in RegisteredCustomMaterials.Values)
				{
					if (material.shader == shader)
						result.Add(material);
				}
			}

			return result;
		}

		/// <summary>
		/// Used to get a complete list of all loaded Materials which use a specified Shader
		/// </summary>
		/// <param name="shaders">Decides which Materials should be returned</param>
		/// <param name="includeCustomMaterials">When True, the returned list will contain non-vanilla Materials</param>
		/// <returns>Returns a List of loaded Materials with a certain Shader</returns>
		public static List<Material> GetAllMaterialsOfShader(List<Shader> shaders, bool includeCustomMaterials = true)
		{
			List<Material> result = new List<Material>();

			foreach (Material material in LoadedVanillaMaterials.Values)
			{
				if (shaders.Contains(material.shader))
					result.Add(material);
			}

			if (includeCustomMaterials)
			{
				foreach (Material material in RegisteredCustomMaterials.Values)
				{
					if (shaders.Contains(material.shader))
						result.Add(material);
				}
			}

			return result;
		}

		/// <summary>
		/// Used to get a complete list of all loaded Materials which use a specified Shader
		/// </summary>
		/// <param name="shaders">Decides which Materials should be returned</param>
		/// <param name="includeCustomMaterials">When True, the returned list will contain non-vanilla Materials</param>
		/// <returns>Returns a List of loaded Materials with a certain Shader</returns>
		public static List<Material> GetAllMaterialsOfShader(List<string> shaderNames, bool includeCustomMaterials = true)
		{
			List<Material> result = new List<Material>();
			List<Shader> _shaders = new List<Shader>();
			foreach (string shader in shaderNames)
			{
				_shaders.Add(Shader.Find(shader));
			}

			foreach (Material material in LoadedVanillaMaterials.Values)
			{
				if (_shaders.Contains(material.shader))
					result.Add(material);
			}

			if (includeCustomMaterials)
			{
				foreach (Material material in RegisteredCustomMaterials.Values)
				{
					if (_shaders.Contains(material.shader))
						result.Add(material);
				}
			}

			return result;
		}

		/// <summary>
		/// Used to get a specific Material
		/// </summary>
		/// <param name="materialName">The name of the Material to search for</param>
		/// <param name="includeCustomMaterials">When True, the returned list will contain non-vanilla Materials</param>
		/// <returns>Returns a Material</returns>
		public static Material GetMaterial(string materialName, bool includeCustomMaterials = true)
		{
			Material result = null;

			if (LoadedVanillaMaterials.ContainsKey(materialName))
				result = LoadedVanillaMaterials[materialName];

			if (result == null && includeCustomMaterials)
			{
				if (RegisteredCustomMaterials.ContainsKey(materialName))
					result = RegisteredCustomMaterials[materialName];
			}

			return result;
		}
		
		
		/// <summary>
		/// Replace Materials from the Unity Editor with Materials from PlateUp!
		/// </summary>
		/// <param name="gameObject">The GameObject to modify.</param>
		/// <returns>The modified GameObject</returns>
		public static GameObject AssignMaterialsByNames(this GameObject gameObject)
		{
			foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
			{
				List<Material> replacementMaterialList = new List<Material>();
				foreach (Material material in renderer.materials)
				{
					Material replacementMaterial = GetMaterial(material.name.Replace(" (Instance)", ""));
					replacementMaterialList.Add(replacementMaterial == null ? material : replacementMaterial);
				}
				renderer.materials = replacementMaterialList.ToArray();
			}
			return gameObject;
		}

		#endregion
	}
}