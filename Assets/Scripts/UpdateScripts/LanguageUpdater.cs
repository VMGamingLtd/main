using UnityEngine;
using UnityEngine.SceneManagement;

public class LanguageUpdater : MonoBehaviour
{
    public void ChangeLanguage()
    {
        Gaos.Context.Authentication.SetLanguage(gameObject.name);
        CoroutineManager coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        coroutineManager.CallLanguageUpdate(gameObject.name);
        gameObject.transform.parent.gameObject.SetActive(false);
        GlobalCalculator.GameStarted = false;
        SceneManager.LoadScene("First");
    }
}
