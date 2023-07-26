using UnityEditor;
using System.IO;

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
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, buildTarget);
    }
}
