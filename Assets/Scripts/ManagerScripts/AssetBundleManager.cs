using UnityEngine;
using System.Collections.Generic;

public static class AssetBundleManager
{
    private static Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();
    private static string assetBundleDirectory = "Assets/AssetBundles"; // Update the path if needed

    public static T LoadAssetFromBundle<T>(string bundleName, string assetName) where T : Object
    {
        string assetBundlePath = assetBundleDirectory + "/" + bundleName.ToLower();

        if (loadedAssetBundles.ContainsKey(bundleName))
        {
            AssetBundle assetBundle = loadedAssetBundles[bundleName];
            T asset = assetBundle.LoadAsset<T>(assetName);
            return asset;
        }
        else
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            loadedAssetBundles.Add(bundleName, assetBundle);
            T asset = assetBundle.LoadAsset<T>(assetName);
            return asset;
        }
    }

    public static void UnloadAssetBundle(string bundleName)
    {
        if (loadedAssetBundles.TryGetValue(bundleName, out AssetBundle assetBundle))
        {
            assetBundle.Unload(false);
            loadedAssetBundles.Remove(bundleName);
            Debug.Log("Asset bundle '" + bundleName + "' unloaded.");
        }
        else
        {
            Debug.LogWarning("Asset bundle '" + bundleName + "' not found in loaded bundles.");
        }
    }
}
