using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetBundler
{
    /// <summary>
    /// The types of assets to search for in checks.
    /// </summary>
    private static readonly string ASSET_SEARCH_QUERY = "t:prefab,t:textAsset,t:audioclip";

    /// <summary>
    /// Temporary location for building AssetBundles.
    /// </summary>
    private static readonly string TEMP_BUILD_FOLDER = "Temp/AssetBundles";

    /// <summary>
    /// Name of the output bundle file. This needs to match the bundle that you tag your assets with.
    /// </summary>
    private static readonly string BUNDLE_FILENAME = "mod.assets";

    /// <summary>
    /// The output folder to place the completed bundle in.
    /// </summary>
    private static readonly string OUTPUT_FOLDER = "content";

    /// <summary>
    /// The folders to not search for assets in.
    /// </summary>
    private static readonly string[] EXCLUDED_FOLDERS = new string[] { "Assets/Editor", "Packages" };

    /// <summary>
    /// The build target of the asset bundle. Should either be StandaloneWindows or StandaloneOSX, depending on your platform.
    /// </summary>
    private BuildTarget Target = BuildTarget.StandaloneWindows;

    /// <summary>
    /// Number of warnings encountered.
    /// </summary>
    private int NumWarnings;

    /// <summary>
    /// Number of warnings encountered.
    /// </summary>
    private string GeneratedAssetBundleTag;

    [MenuItem("PlateUp!/Build Asset Bundle _F6")]
    public static void BuildAssetBundle()
    {
        Debug.LogFormat("Creating 1\"{0}\" AssetBundle...", BUNDLE_FILENAME);

		AssetBundler bundler = new AssetBundler();

        if (Application.platform == RuntimePlatform.OSXEditor) bundler.Target = BuildTarget.StandaloneOSX;

        // Randomly generate the resulting name of the asset bundle
        bundler.GenerateRandomAssetBundleTag();

        bool success = false;
        try
        {
            // Check for assets
            bundler.WarnIfAssetsAreNotTagged();
            bundler.WarnIfZeroAssetsAreTagged();
            bundler.WarnIfMeshAssetsAreTagged();
            // bundler.WarnIfMaterialsAreTaggedOrIncluded();

            // Delete the contents of OUTPUT_FOLDER
            bundler.CleanBuildFolder();

            // Temporarily move the tagged assets to the temporary tag
            bundler.MoveAssetsToTemporaryAssetBundle();

            // Lastly, create the asset bundle itself and copy it to the output folder
            bundler.CreateAssetBundle();

            success = true;
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("Failed to build AssetBundle: {0}\n{1}", e.Message, e.StackTrace);
        }

        // Return assets to the original asset bundle tag
        bundler.RestoreAssetBundleTags();
        AssetDatabase.RemoveUnusedAssetBundleNames();

        if (success)
        {
            Debug.LogFormat("[{0}] Build complete with {1} warnings! Output: {2} (temporary ID: {3})", DateTime.Now.ToLocalTime(), bundler.NumWarnings, OUTPUT_FOLDER + "/" + BUNDLE_FILENAME, bundler.GeneratedAssetBundleTag);
        }
    }

    /// <summary>
    /// Generate the random asset bundle tag to use when building the asset bundle.
    /// </summary>
    private void GenerateRandomAssetBundleTag()
    {
        System.Random rand = new System.Random();
        GeneratedAssetBundleTag = $"mod-{rand.Next(0, int.MaxValue)}.assets";
    }

    /// <summary>
    /// Move assets tagged with BUNDLE_FILENAME to the temporary asset bundle
    /// </summary>
    private void MoveAssetsToTemporaryAssetBundle()
    {
        SubstituteAssetBundleTags(BUNDLE_FILENAME, GeneratedAssetBundleTag);
    }

    /// <summary>
    /// Move assets tagged with the temporary asset bundle back to BUNDLE_FILENAME
    /// </summary>
    private void RestoreAssetBundleTags()
    {
        SubstituteAssetBundleTags(GeneratedAssetBundleTag, BUNDLE_FILENAME);
    }

    /// <summary>
    /// Find all assets tagged with a certain asset bundle tag and replace them with another tag
    /// </summary>
    /// <param name="from">The asset bundle tag to search for</param>
    /// <param name="to">The new asset bundle tag</param>
    private void SubstituteAssetBundleTags(string from, string to)
    {
        string[] assetGUIDs = AssetDatabase.FindAssets($"b:{from}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            var importer = AssetImporter.GetAtPath(path);
            importer.assetBundleName = to;
        }
    }

    /// <summary>
    /// Delete and recreate the OUTPUT_FOLDER to ensure a clean build.
    /// </summary>
    protected void CleanBuildFolder()
    {
        Debug.LogFormat("Cleaning {0}...", OUTPUT_FOLDER);

        if (Directory.Exists(OUTPUT_FOLDER))
        {
            Directory.Delete(OUTPUT_FOLDER, true);
        }

        Directory.CreateDirectory(OUTPUT_FOLDER);
    }

    /// <summary>
    /// Build the AssetBundle itself and copy it to the OUTPUT_FOLDER.
    /// </summary>
    protected void CreateAssetBundle()
    {
        Debug.Log("Building AssetBundle...");

        // Build all AssetBundles to the TEMP_BUILD_FOLDER
        if (!Directory.Exists(TEMP_BUILD_FOLDER))
        {
            Directory.CreateDirectory(TEMP_BUILD_FOLDER);
        }

#pragma warning disable 618
        // Build the asset bundle with the CollectDependencies flag. This is necessary or else ScriptableObjects will
        // not be accessible within the asset bundle. Unity has deprecated this flag claiming it is now always active,
        // but due to a bug we must still include it (and ignore the warning).
        BuildPipeline.BuildAssetBundles(
            TEMP_BUILD_FOLDER,
            BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.CollectDependencies,
            Target);
#pragma warning restore 618

        // We are only interested in the BUNDLE_FILENAME bundle (and not any extra AssetBundle or the manifest files
        // that Unity makes), so just copy that to the final output folder
        string srcPath = Path.Combine(TEMP_BUILD_FOLDER, GeneratedAssetBundleTag);
        string destPath = Path.Combine(OUTPUT_FOLDER, BUNDLE_FILENAME);
        File.Copy(srcPath, destPath, true);
    }

    /// <summary>
    /// Checks if the given path is a search path.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>true if the given path is a search path, otherwise false.</returns>
    protected static bool IsIncludedAssetPath(string path)
    {
        foreach (string excludedPath in EXCLUDED_FOLDERS)
        {
            if (path.StartsWith(excludedPath))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Log a warning for all potential assets that are not currently tagged to be in this AssetBundle.
    /// </summary>
    protected void WarnIfAssetsAreNotTagged()
    {
        string[] assetGUIDs = AssetDatabase.FindAssets(ASSET_SEARCH_QUERY);
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            var importer = AssetImporter.GetAtPath(path);
            if (!importer.assetBundleName.Equals(BUNDLE_FILENAME))
            {
                Debug.LogWarningFormat("Asset \"{0}\" is not tagged with \"{1}\" and will not be included in the AssetBundle!", path, BUNDLE_FILENAME);
                ++NumWarnings;
            }
        }
    }

    /// <summary>
    /// Verify that there is at least one asset to be included in the asset bundle.
    /// </summary>
    protected void WarnIfZeroAssetsAreTagged()
    {
        string[] assetsInBundle = AssetDatabase.FindAssets($"{ASSET_SEARCH_QUERY},b:{BUNDLE_FILENAME}");
        if (assetsInBundle.Length == 0)
        {
            throw new Exception(string.Format("No assets have been tagged for inclusion in the {0} AssetBundle.", BUNDLE_FILENAME));
        }
    }

    /// <summary>
    /// Warn if there are any mesh assets tagged. If so, the user probably meant to tag a prefab instead.
    /// </summary>
    protected void WarnIfMeshAssetsAreTagged()
    {
        string[] assetGUIDs = AssetDatabase.FindAssets($"t:mesh,b:{BUNDLE_FILENAME}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            Debug.LogWarningFormat("Mesh asset \"{0}\" is tagged for inclusion in the {1} AssetBundle! This is likely a mistake. You should include a prefab instead.", path, BUNDLE_FILENAME);
            ++NumWarnings;
        }
    }

    /// <summary>
    /// Warn if there are any material assets tagged. If so, the user probably meant to tag a prefab instead.
    /// </summary>
    protected void WarnIfMaterialsAreTaggedOrIncluded()
    {
        // Check for directly tagged materials
        string[] assetGUIDs = AssetDatabase.FindAssets($"t:material,b:{BUNDLE_FILENAME}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            Debug.LogWarningFormat("Material asset \"{0}\" is tagged for inclusion in the {1} AssetBundle! This is likely a mistake. You should use generate materials using the vanilla shaders instead.", path, BUNDLE_FILENAME);
            ++NumWarnings;
        }

        // Check for materials assigned to prefabs
        assetGUIDs = AssetDatabase.FindAssets($"t:prefab,b:{BUNDLE_FILENAME}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.sharedMaterials.Any(m => m != null))
                {
                    Debug.LogWarningFormat("Material found attached to bundle prefab in \"{0}\" at \"<root>/{1}\"! This is likely a mistake. To avoid log spam and texturing issues, you should remove these materials or set them to \"None\".", path, GetGameObjectPath(renderer.transform).Split(new char[] { '/' }, 3)[2]);
                    ++NumWarnings;
                }
            }
        }
    }

    public static string GetGameObjectPath(Transform current)
    {
        if (current.parent == null)
            return "/" + current.name;
        return GetGameObjectPath(current.parent) + "/" + current.name;
    }

    [MenuItem("PlateUp!/Preparation/Strip Materials From Prefabs")]
    public static void RemoveAllPrefabMaterials()
    {
        if (!EditorUtility.DisplayDialog("Confirm", "Stripping materials from prefabs is an irreversible process. Perform at your own risk.", "Proceed", "Cancel"))
        {
            return;
        }

        string[] assetGUIDs = AssetDatabase.FindAssets($"t:prefab,b:{BUNDLE_FILENAME}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.sharedMaterials.Length > 0)
                {
                    renderer.sharedMaterials = new Material[renderer.sharedMaterials.Length];
                    Debug.LogFormat("Stripped materials from \"{0}\" at \"<root>/{1}\".", path, GetGameObjectPath(renderer.transform).Split(new char[] { '/' }, 3)[2]);
                }
            }
        }

        Debug.LogFormat("[{0}] Done stripping materials.", DateTime.Now.ToLocalTime());
    }



    [MenuItem("PlateUp!/Preparation/Set Prefab Materials to Default")]
    public static void SetAllPrefabMaterialsToDefault()
    {
        if (!EditorUtility.DisplayDialog("Confirm", "Changing the materials of prefabs is an irreversible process. Perform at your own risk.", "Proceed", "Cancel"))
        {
            return;
        }

        string[] assetGUIDs = AssetDatabase.FindAssets($"t:prefab,b:{BUNDLE_FILENAME}");
        foreach (var assetGUID in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(assetGUID);
            if (!IsIncludedAssetPath(path))
            {
                continue;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            MeshRenderer[] renderers = prefab.GetComponentsInChildren<MeshRenderer>();
            Material defaultMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

            foreach (MeshRenderer renderer in renderers)
            {
                if (renderer.sharedMaterials.Length > 0)
                {
                    var newMaterials = new Material[renderer.sharedMaterials.Length];

                    for (int i = 0; i < newMaterials.Length; i++)
                    {
                        newMaterials[i] = defaultMaterial;
                    }

                    renderer.sharedMaterials = newMaterials;

                    Debug.LogFormat("Set materials from \"{0}\" at \"<root>/{1}\".", path, GetGameObjectPath(renderer.transform).Split(new char[] { '/' }, 3)[2]);
                }
            }
        }

        Debug.LogFormat("[{0}] Done setting materials.", DateTime.Now.ToLocalTime());
    }
}
