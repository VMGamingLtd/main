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
            if (assetBundle != null)
            {
                T asset = assetBundle.LoadAsset<T>(assetName);
                if (asset != null)
                {
                    Debug.Log("Loaded asset '" + assetName + "' from asset bundle '" + bundleName + "'.");
                    return asset;
                }
                else
                {
                    Debug.LogError("Failed to load asset '" + assetName + "' from asset bundle '" + bundleName + "'.");
                }
            }
        }
        else
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (assetBundle != null)
            {
                loadedAssetBundles.Add(bundleName, assetBundle);
                Debug.Log("Asset bundle '" + bundleName + "' loaded successfully.");

                T asset = assetBundle.LoadAsset<T>(assetName);
                if (asset != null)
                {
                    Debug.Log("Loaded asset '" + assetName + "' from asset bundle '" + bundleName + "'.");
                    return asset;
                }
                else
                {
                    Debug.LogError("Failed to load asset '" + assetName + "' from asset bundle '" + bundleName + "'.");
                }
            }
            else
            {
                Debug.LogError("Failed to load AssetBundle: " + assetBundlePath);
            }
        }

        return null;
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
