using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class CustomSceneLoader : MonoBehaviour
{
    public string sceneName;
    private CoroutineManager coroutineManager;


    private void OnEnable()
    {

        StartCoroutine(Gaos.Device.Device.Registration.RegisterDevice());
        StartCoroutine(Gaos.User.User.GuestLogin.Login(OnGuestLoginComplete));
    }

    public async void LoadGuestAsync()
    {
        await LoadSceneUniTask(sceneName);
    }

    private async UniTask LoadSceneUniTask(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait for the scene to be fully loaded
        while (!asyncLoad.isDone)
        {
            // You can check the progress if needed
            //float progress = asyncLoad.progress;

            await UniTask.Yield(); // Yield to the Unity main thread
        }


        coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();

        await coroutineManager.LoadSaveSlots();
    }
    public void OnGuestLoginComplete()
    {
        if (Gaos.User.User.GuestLogin.IsLoggedIn == true)
        {
            Debug.Log($"Guest logged in: {Gaos.User.User.GuestLogin.GuestLoginResponse.UserName}");
            UserName.userName = Gaos.User.User.GuestLogin.GuestLoginResponse.UserName;

            LoadGuestAsync();
        }
        else
        {
            throw new System.Exception("Guest login failed");
        }

    }
}
