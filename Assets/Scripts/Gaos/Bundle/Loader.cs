using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{
    public static string BUNDLES_URL = Gaos.Environment.Environment.GetEnvironment()["BUNDLES_URL"];

    
    private string bundleUrl(string bundleName)
    {
        return $"{BUNDLES_URL}/{bundleName}";
    }

    IEnumerator LoadBundle(string bundleName)
    {
        string bundleUrl = this.bundleUrl(bundleName);

        UnityWebRequest wr = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);
        yield return wr.SendWebRequest();

        // Get an asset from the bundle and instantiate it.
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(wr);
        var loadAsset = bundle.LoadAssetAsync<GameObject>("Assets/Players/MainPlayer.prefab");
        yield return loadAsset;

        Instantiate(loadAsset.asset);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
