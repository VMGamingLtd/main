using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            await AssetBundleManager.LoadAllAssetBundles();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            await UniTask.Yield();
        }

        coroutineManager = GameObject.Find(Constants.CoroutineManager).GetComponent<CoroutineManager>();

        await UniTask.Delay(TimeSpan.FromSeconds(1));

        await coroutineManager.LoadSaveSlots();

        _ = SceneManager.UnloadSceneAsync("First");
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
                UserName.userName = userName;
                Assets.Scripts.Login.UserChangedEvent.Emit(new Assets.Scripts.Login.UserChangedEventArgs { UserName = UserName.userName, IsGuest = isGuest });
                ModelsRx.ContextRx.UserRx.UserName = userName;
                ModelsRx.ContextRx.UserRx.IsGuest = isGuest;

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
            UserName.userName = userName;
            bool isGuest = (bool)Gaos.User.User.GuestLogin.GuestLoginResponse.IsGuest;
            Assets.Scripts.Login.UserChangedEvent.Emit(new Assets.Scripts.Login.UserChangedEventArgs { UserName = UserName.userName, IsGuest = isGuest });
            ModelsRx.ContextRx.UserRx.UserName = Gaos.User.User.GuestLogin.GuestLoginResponse.UserName;
            ModelsRx.ContextRx.UserRx.IsGuest = (bool)Gaos.User.User.GuestLogin.GuestLoginResponse.IsGuest;
            LoadGuestAsync();
        }
        else
        {
            Debug.LogError($"{CLASS_NAME}:{METOD_NAME}: guest login failed");
            throw new System.Exception("guest login failed");
        }

    }
}
