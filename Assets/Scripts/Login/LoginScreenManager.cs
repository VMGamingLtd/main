using Gaos.Routes.Model.UserJson;
using System.Collections;
using System.Collections.Generic;
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
        switch(errorKind)
        {
            case LoginResponseErrorKind.IncorrectUserNameOrEmailError:
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
        buttonBack.interactable = false;
        buttonLogin.interactable = false;

        StartCoroutine(Gaos.User.User.UserLogin.Login(userNameTextInput.text, passwordTextInput.text, OnUserLoginComplete));
    }


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
            //mainUI.SetActive(true);
            //this.gameObject.SetActive(false);
            buttonBack.onClick.Invoke();
        }
        else
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: User not logged in, erorr: {Gaos.User.User.UserLogin.ResponseErrorKind}");
            errorText.text = GetErrorMessage(Gaos.User.User.UserLogin.ResponseErrorKind);
        }
    }

}
