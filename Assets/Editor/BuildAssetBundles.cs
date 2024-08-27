using UnityEditor;
using UnityEngine.Windows;

public class CreateAssetBundles
{
    private const string assetBundleDirectory = "Assets/AssetBundles";

    [MenuItem("Assets/Build Windows Asset Bundles")]
    static void BuildPCAssetBundles()
    {
        BuildAssetBundles(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Assets/Build Android Asset Bundles")]
    static void BuildAndroidAssetBundles()
    {
        BuildAssetBundles(BuildTarget.Android);
    }

    [MenuItem("Assets/Build macOS Asset Bundles")]
    static void BuildMacOSAssetBundles()
    {
        BuildAssetBundles(BuildTarget.StandaloneOSX);
    }

    [MenuItem("Assets/Build iOS Asset Bundles")]
    static void BuildiOSAssetBundles()
    {
        BuildAssetBundles(BuildTarget.iOS);
    }

    [MenuItem("Assets/Build WebGL Asset Bundles")]
    static void BuildWebGLAssetBundles()
    {
        BuildAssetBundles(BuildTarget.WebGL);
    }

    static void BuildAssetBundles(BuildTarget buildTarget)
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        //// Manually ensure dependencies are correctly assigned to the AssetBundle
        //EnsureDependencies("Assets/abilityprefabs");

        //IncludeShaders();

        // Build the AssetBundles
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, buildTarget);
    }

    //static void EnsureDependencies(string folderPath)
    //{
    //    // Get all asset paths in the specified folder
    //    string[] assetPaths = AssetDatabase.GetDependencies(folderPath, true);

    //    // Assign each asset to the 'abilityprefabs' AssetBundle
    //    foreach (string assetPath in assetPaths)
    //    {
    //        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
    //        if (assetImporter != null)
    //        {
    //            assetImporter.SetAssetBundleNameAndVariant("abilityprefabs", "");
    //        }
    //    }
    //}

    //static void IncludeShaders()
    //{
    //    // Include any shaders that may be used
    //    Shader[] allShaders = Resources.FindObjectsOfTypeAll<Shader>();
    //    foreach (Shader shader in allShaders)
    //    {
    //        string shaderPath = AssetDatabase.GetAssetPath(shader);
    //        if (!string.IsNullOrEmpty(shaderPath))
    //        {
    //            AssetImporter shaderImporter = AssetImporter.GetAtPath(shaderPath);
    //            if (shaderImporter != null)
    //            {
    //                shaderImporter.SetAssetBundleNameAndVariant("abilityprefabs", "");
    //            }
    //        }
    //    }
    //}
}
