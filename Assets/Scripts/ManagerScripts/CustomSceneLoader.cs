using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class CustomSceneLoader : MonoBehaviour
{

    public readonly static string CLASS_NAME = typeof(CustomSceneLoader).Name;
    public string sceneName;
    private CoroutineManager coroutineManager;


    private void OnEnable()
    {

        StartCoroutine(Gaos.Device.Device.Registration.RegisterDevice(OnRergisterDeviceComplete));
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

    public void OnRergisterDeviceComplete()
    {
        const string METOD_NAME = "OnRergisterDeviceComplete()";
        if (Gaos.Device.Device.Registration.IsDeviceRegistered == true)
        {
            if (Gaos.Context.Authentication.GetUserId() > -1)
            {
                var userName = Gaos.Context.Authentication.GetUserName();
                var isGuest = Gaos.Context.Authentication.GetIsGuest();
                Debug.Log($"{CLASS_NAME}:{METOD_NAME}: user is already logged in om this device: userName: {userName}, isGuest: {isGuest}");
                UserName.userName = userName;

                LoadGuestAsync();
            }
            else
            {
                StartCoroutine(Gaos.User.User.GuestLogin.Login(OnGuestLoginComplete));
            }
        }
        else
        {
            Debug.LogError($"{CLASS_NAME}:{METOD_NAME}: device registration failed");
            throw new System.Exception("device registration failed");
        }   

    }

    public void OnGuestLoginComplete()
    {
        const string METOD_NAME = "OnGuestLoginComplete()";
        if (Gaos.User.User.GuestLogin.IsLoggedIn == true)
        {
            var userName = Gaos.User.User.GuestLogin.GuestLoginResponse.UserName;
            Debug.Log($"{CLASS_NAME}:{METOD_NAME}: logged in guest user: {userName}");
            UserName.userName = userName;

            LoadGuestAsync();
        }
        else
        {
            Debug.LogError($"{CLASS_NAME}:{METOD_NAME}: guest login failed");
            throw new System.Exception("guest login failed");
        }

    }
}
