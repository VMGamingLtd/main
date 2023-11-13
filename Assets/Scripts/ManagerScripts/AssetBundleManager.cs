using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class AssetBundleManager
{
    private static Dictionary<string, AssetBundle> loadedAssetBundles = new();
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

    public async static UniTask LoadAllAssetBundles()
    {
        AssetBundle bundle;

        bundle = await loadAssetBundleWebgl("buildingicons");
        loadedAssetBundles.Add("buildingicons", bundle);
        bundle = await loadAssetBundleWebgl("resourceicons");
        loadedAssetBundles.Add("resourceicons", bundle);
        bundle = await loadAssetBundleWebgl("skillicons");
        loadedAssetBundles.Add("skillicons", bundle);
    }

    public async static UniTask<AssetBundle> loadAssetBundleWebgl(string bundleName)
    {
        //const string METHOD_NAME = "loadAssetBundleWebgl()";
        //Debug.Log($"{CLASS_NAME}:${METHOD_NAME}: loading bundle '{bundleName}' ....");
        string assetBundlePath = "AssetBundles" + "/" + bundleName.ToLower();
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundlePath);
        await www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError($"{CLASS_NAME}:${METHOD_NAME}: failed to load bundle '{bundleName}': {www.error}, url: {www.url}, status: {www.responseCode}");
            return null;
        }
        else
        {
            //Debug.Log($"{CLASS_NAME}:${METHOD_NAME}: loaded bundle '{bundleName}'");
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            return bundle;
        }

    }
}
