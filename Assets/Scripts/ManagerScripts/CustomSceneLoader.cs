using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CustomSceneLoader : MonoBehaviour
{
    public string sceneName;

    private void OnEnable()
    {
        LoadSceneAsync();
    }
    public void LoadSceneAsync()
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        yield return null;
    }
}
