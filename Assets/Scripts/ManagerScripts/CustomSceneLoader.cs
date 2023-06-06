using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CustomSceneLoader : MonoBehaviour
{
    public string sceneName;
    public static Gaos.Websocket.WebSocketClientSharp Ws = new Gaos.Websocket.WebSocketClientSharp();


    private void OnEnable()
    {

        if (false)
        {
            Ws.Open();
            StartCoroutine(Ws.StartProcessing());
        }

        StartCoroutine(Gaos.Device.Manager.Registration.RegisterDevice());
        StartCoroutine(Gaos.User.Manager.GuestLogin.Login(OnGuestLoginComplete));
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

    public void OnGuestLoginComplete()
    {
        if (Gaos.User.Manager.GuestLogin.IsLoggedIn == true)
        {
            Debug.Log($"Guest logged in: {Gaos.User.Manager.GuestLogin.GuestLoginResponse.userName}");
            UserName.userName = Gaos.User.Manager.GuestLogin.GuestLoginResponse.userName; 

            LoadSceneAsync();
        }
        else
        {
            throw new System.Exception("Guest login failed");
        }

    }
}
