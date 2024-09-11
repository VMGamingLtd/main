using Cysharp.Threading.Tasks;
using Gaos.Routes.Model.UserJson;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreenManager : MonoBehaviour
{
    public readonly static string CLASS_NAME = typeof(LoginScreenManager).Name;

    public GameObject mainUI;

    public TextMeshProUGUI errorText;

    public TMP_InputField userNameTextInput;
    public TMP_InputField passwordTextInput;

    public Button buttonLogin;
    public Button buttonBack;


    public void OnEnable()
    {
        ClearErrorText();

        userNameTextInput.text = "";
        passwordTextInput.text = "";

    }

    public void ClearErrorText()
    {
        errorText.text = "";
    }

    private string GetErrorMessage(Gaos.Routes.Model.UserJson.LoginResponseErrorKind? errorKind)
    {
        const string METHOD_NAME = "GetErrorMessage()";
        string msg = "Error!";
        switch (errorKind)
        {
            case LoginResponseErrorKind.IncorrectUserNameOrEmailError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Incorrect username";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Неверное имя пользователя";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "用户名不正确";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Nesprávne používateľské meno";
                        break;
                    default:
                        msg = "Incorrect username";
                        break;
                }
                break;
            case LoginResponseErrorKind.IncorrectPasswordError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Wrong password";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Неправильный пароль";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "密码错误";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Zlé heslo";
                        break;
                    default:
                        msg = "Wrong password";
                        break;
                }
                break;
            case LoginResponseErrorKind.InternalError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Internal error!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Internal error!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "内部错误！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Vnútorná chyba!";
                        break;
                    default:
                        msg = "Internal error!";
                        break;
                }
                break;
            default:
                msg = "Error!";
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: missing translation for: {errorKind}");
                break;

        }

        return msg;
    }

    public void StartLoginUser()
    {
        /*
        buttonBack.interactable = false;
        buttonLogin.interactable = false;

        StartCoroutine(Gaos.User.User.UserLogin.Login(userNameTextInput.text, passwordTextInput.text, OnUserLoginComplete));
        */
        DoLogin().Forget();
    }

    public async UniTaskVoid DoLogin()
    {
        const string METHOD_NAME = "DoLogin()";

        // Set both login and back buttons to be disabled so that user cannot play if there's an error while logging in
        // or else we might risk corruption of game data.
        // If the login is not successfull user have no other choice except of reaload the game.
        buttonBack.interactable = false;
        buttonLogin.interactable = false;

        await CustomSceneLoader.SaveGameDataAndStopSaveManager();

        await Gaos.User.User.UserLogin.Login(userNameTextInput.text, passwordTextInput.text);
        if (Gaos.User.User.UserLogin.IsLoggedIn == true)
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: User logged in.");


            CoroutineManager.registeredUser = true;
            UserName.userName = Gaos.User.User.UserLogin.LoginResponse.UserName;
            bool isGuest = (bool)Gaos.User.User.UserLogin.LoginResponse.IsGuest;
            Assets.Scripts.Login.UserChangedEvent.Emit(new Assets.Scripts.Login.UserChangedEventArgs { UserName = UserName.userName, IsGuest = isGuest });
            //mainUI.SetActive(true);
            //this.gameObject.SetActive(false);
            ModelsRx.ContextRx.UserRx.UserName = Gaos.User.User.UserLogin.LoginResponse.UserName;
            ModelsRx.ContextRx.UserRx.IsGuest = (bool)Gaos.User.User.UserLogin.LoginResponse.IsGuest; 
            //buttonBack.onClick.Invoke();

            await CustomSceneLoader.RestartGame();
        }
        else
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: User not logged in, erorr: {Gaos.User.User.UserLogin.ResponseErrorKind}");
            errorText.text = GetErrorMessage(Gaos.User.User.UserLogin.ResponseErrorKind);
        }
    }


    /*
    private void OnUserLoginComplete()
    {
        const string METHOD_NAME = "OnUserLoginComplete()";

        buttonBack.interactable = true;
        buttonLogin.interactable = true;

        if (Gaos.User.User.UserLogin.IsLoggedIn == true)
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: User logged in.");


            CoroutineManager.registeredUser = true;
            UserName.userName = Gaos.User.User.UserLogin.LoginResponse.UserName;
            bool isGuest = (bool)Gaos.User.User.UserLogin.LoginResponse.IsGuest;
            Assets.Scripts.Login.UserChangedEvent.Emit(new Assets.Scripts.Login.UserChangedEventArgs { UserName = UserName.userName, IsGuest = isGuest });
            //mainUI.SetActive(true);
            //this.gameObject.SetActive(false);
            ModelsRx.ContextRx.UserRx.UserName = Gaos.User.User.UserLogin.LoginResponse.UserName;
            ModelsRx.ContextRx.UserRx.IsGuest = (bool)Gaos.User.User.UserLogin.LoginResponse.IsGuest; 
            //buttonBack.onClick.Invoke();

            StartCoroutine(CustomSceneLoader.RestartGame());
        }
        else
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: User not logged in, erorr: {Gaos.User.User.UserLogin.ResponseErrorKind}");
            errorText.text = GetErrorMessage(Gaos.User.User.UserLogin.ResponseErrorKind);
        }
    }
    */

}
