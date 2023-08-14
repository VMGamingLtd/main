using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{
    public readonly static string CLASS_NAME = typeof(Loader).Name;

    public static string BUNDLES_URL = Gaos.Environment.Environment.GetEnvironment()["BUNDLES_URL"];

    Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

    bool IsError = false;


    private string bundleUrl(string bundleName)
    {
        return $"{BUNDLES_URL}/{bundleName}";
    }

    IEnumerator LoadBundle(string bundleName)
    {
        if (IsError == true)
        {
            yield break;
        }

        string bundleUrl = this.bundleUrl(bundleName);

        UnityWebRequest wr = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return wr.SendWebRequest();

        if (wr.result == UnityWebRequest.Result.Success)
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(wr);
            bundles[bundleName] = bundle;
            Debug.Log($"{CLASS_NAME}: loaded bundle '{bundleName}'");
        }
        else
        {
            Debug.LogError($"{CLASS_NAME}: failed to load bundle '{bundleName}': {wr.error}, url: {wr.url}, status: {wr.responseCode}");
        }
    }

    IEnumerator LoadBundles(string[] bundleNames)
    {
        for (int i = 0; i < bundleNames.Length; i++)
        {
            string bundleName = bundleNames[i];
            yield return this.LoadBundle(bundleName);
        }

    }
}
