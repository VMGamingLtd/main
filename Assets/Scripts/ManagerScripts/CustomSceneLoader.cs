using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CustomSceneLoader : MonoBehaviour
{
    public string sceneName;
    public static Gaos.WebSocket.WebSocketClientSharp Ws = new Gaos.WebSocket.WebSocketClientSharp();


    private void OnEnable()
    {

        StartCoroutine(Gaos.Device.Device.Registration.RegisterDevice());
        StartCoroutine(Gaos.User.User.GuestLogin.Login(OnGuestLoginComplete));
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
        if (Gaos.User.User.GuestLogin.IsLoggedIn == true)
        {
            Debug.Log($"Guest logged in: {Gaos.User.User.GuestLogin.GuestLoginResponse.UserName}");
            UserName.userName = Gaos.User.User.GuestLogin.GuestLoginResponse.UserName;

            LoadSceneAsync();
        }
        else
        {
            throw new System.Exception("Guest login failed");
        }

    }
}
