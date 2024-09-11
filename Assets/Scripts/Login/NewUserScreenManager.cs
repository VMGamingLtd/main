using Cysharp.Threading.Tasks;
using Gaos.Routes.Model.UserJson;
using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewUserScreen : MonoBehaviour

{
    public readonly static string CLASS_NAME = typeof(NewUserScreen).Name;

    private enum ErrorKind
    {
        PasswordsDoNotMatch,
    }

    //public GameObject mainUI;

    public TextMeshProUGUI errorText;

    public TMP_InputField emailTextInput;
    public TMP_InputField userNameTextInput;
    public TMP_InputField passwordTextInput;
    public TMP_InputField passwordVerifyTextInput;

    public Button buttonCreate;
    public Button buttonBack;

    public void OnEnable()
    {
        ClearErrorText();

        emailTextInput.text = "";
        userNameTextInput.text = "";
        passwordTextInput.text = "";
        passwordVerifyTextInput.text = "";

    }

    public void ClearErrorText()
    {
        errorText.text = "";
    }

    private string GetErrorMessage(Gaos.Routes.Model.UserJson.RegisterResponseErrorKind? errorKind)
    {
        const string METHOD_NAME = "GetErrorMessage()";
        string msg = "Error!";
        switch (errorKind)
        {
            case RegisterResponseErrorKind.UsernameExistsError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Please choose a different username!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пожалуйста, выберите другое имя пользователя!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "请选择一个不同的用户名！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Prosím, zvoľte iné používateľské meno!";
                        break;
                    default:
                        msg = "Please choose a different username!";
                        break;
                }
                break;
            case RegisterResponseErrorKind.UserNameIsEmptyError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Please enter your username!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пожалуйста, введите Ваш логин!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "请输入您的用户名！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Zadajte svoje používateľské meno!";
                        break;
                    default:
                        msg = "Please enter your username!";
                        break;
                }
                break;
            case RegisterResponseErrorKind.EmailIsEmptyError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Please enter your email!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пожалуйста, введите свой адрес электронной почты!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "请输入您的电子邮件！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Zadajte svoj e-mail!";
                        break;
                    default:
                        msg = "Please enter your email!";
                        break;
                }
                break;
            case RegisterResponseErrorKind.IncorrectEmailError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "The email has wrong format!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Письмо имеет неверный формат!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "邮件格式错误！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "E-mail má nesprávny formát!";
                        break;
                    default:
                        msg = "The email has wrong format!";
                        break;
                }
                break;
            case RegisterResponseErrorKind.EmailExistsError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Please choose a diffrent email!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пожалуйста, выберите другой адрес электронной почты!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "请选择不同的电子邮件！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Zvolte si iný e-mail!";
                        break;
                    default:
                        msg = "Please choose a diffrent email!";
                        break;
                }
                break;
            case RegisterResponseErrorKind.PasswordIsEmptyError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Please enter the password!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пожалуйста, введите пароль!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "请输入密码！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Zadajte heslo!";
                        break;
                    default:
                        break;
                }
                break;
            case RegisterResponseErrorKind.PasswordsDoNotMatchError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Passwords do not match!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пароли не совпадают!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "密码不匹配！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Heslá sa nezhodujú!";
                        break;
                    default:
                        msg = "Passwords do not match!";
                        break;
                }
                break;
            case RegisterResponseErrorKind.InternalError:
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

    private string GetErrorMessae(ErrorKind errorKimd)
    {
        string msg = "Error!";
        switch (errorKimd)
        {
            case ErrorKind.PasswordsDoNotMatch:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Passwords do not match!";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пароли не совпадают!";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "密码不匹配！";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Heslá sa nezhodujú!";
                        break;
                    default:
                        msg = "Passwords do not match!";
                        break;
                }
                break;
            default:
                msg = "Error!";
                break;
        }

        return msg;

    }


    private bool VerifyIfPasswordsAreSame()
    {
        bool result = false;

        if (passwordTextInput.text == passwordVerifyTextInput.text)
        {
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public void StartRegisterUser()
    {
        if (!VerifyIfPasswordsAreSame())
        {
            errorText.text = GetErrorMessae(ErrorKind.PasswordsDoNotMatch);
            return;
        }

        /*
        buttonCreate.interactable = false;
        buttonBack.interactable = false;
        StartCoroutine(Gaos.User.User.UserRegister.Register(userNameTextInput.text, emailTextInput.text, passwordTextInput.text, OnUserRegisterComplete));
        */

        DoRegisterUser().Forget();
    }

    public async UniTaskVoid DoRegisterUser()
    {
        string METHOD_NAME = "DoRegisterUser()";

        // Set both create and back buttons to be disabled so that user cannot play if there's an error while registering 
        // or else we might risk corruption of game data.
        // If the register is not successfull user have no other choice except of reaload the game.
        buttonCreate.interactable = false;
        buttonBack.interactable = false;

        await CustomSceneLoader.SaveGameDataAndStopSaveManager();

        await Gaos.User.User.UserRegister.Register(userNameTextInput.text, emailTextInput.text, passwordTextInput.text);
        if (Gaos.User.User.UserRegister.IsRegistered == true)
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: User registered.");


            /*
            CoroutineManager.registeredUser = true;
            UserName.userName = Gaos.User.User.UserRegister.RegisterResponse.User.Name;
            bool isGuest = (bool)Gaos.User.User.UserRegister.RegisterResponse.User.IsGuest;
            Assets.Scripts.Login.UserChangedEvent.Emit(new Assets.Scripts.Login.UserChangedEventArgs { UserName = UserName.userName, IsGuest = isGuest });
            ModelsRx.ContextRx.UserRx.UserName = Gaos.User.User.UserRegister.RegisterResponse.User.Name;
            ModelsRx.ContextRx.UserRx.IsGuest = (bool)Gaos.User.User.UserRegister.RegisterResponse.User.IsGuest;
            //buttonBack.onClick.Invoke();
            */

            await CustomSceneLoader.RestartGame();

        }
        else
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: User not registered, erorr: {Gaos.User.User.UserRegister.ResponseErrorKind}");
            errorText.text = GetErrorMessage(Gaos.User.User.UserRegister.ResponseErrorKind);
        }
    }

    public void OnUserRegisterComplete()
    {
        const string METHOD_NAME = "OnUserRegisterComplete()";

        buttonCreate.interactable = true;
        buttonBack.interactable = true;

        if (Gaos.User.User.UserRegister.IsRegistered == true)
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: User registered.");


            CoroutineManager.registeredUser = true;
            UserName.userName = Gaos.User.User.UserRegister.RegisterResponse.User.Name;
            bool isGuest = (bool)Gaos.User.User.UserRegister.RegisterResponse.User.IsGuest;
            Assets.Scripts.Login.UserChangedEvent.Emit(new Assets.Scripts.Login.UserChangedEventArgs { UserName = UserName.userName, IsGuest = isGuest });
            ModelsRx.ContextRx.UserRx.UserName = Gaos.User.User.UserRegister.RegisterResponse.User.Name;
            ModelsRx.ContextRx.UserRx.IsGuest = (bool)Gaos.User.User.UserRegister.RegisterResponse.User.IsGuest;
            //buttonBack.onClick.Invoke();

            StartCoroutine(CustomSceneLoader.RestartGame());

        }
        else
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: User not registered, erorr: {Gaos.User.User.UserRegister.ResponseErrorKind}");
            errorText.text = GetErrorMessage(Gaos.User.User.UserRegister.ResponseErrorKind);
        }
    }


    public void Display()
    {

    }

}
