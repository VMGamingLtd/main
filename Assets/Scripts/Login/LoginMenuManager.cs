using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Gaos.Routes.Model.UserJson;

public class LoginMenuManager : MonoBehaviour
{
    public readonly static string CLASS_NAME = typeof(LoginMenuManager).Name;

    public GameObject email;
    public GameObject username;
    public GameObject password;
    public GameObject createAccount;
    public GameObject login;
    public GameObject next;
    public GameObject register;
    public GameObject recoverPassword;
    public GameObject backToEmail;
    public GameObject back;
    public GameObject mainUI;
    public GameObject registerButton;

    public TMP_InputField EmailInputField;
    public TMP_InputField UserNameInputField;
    public TMP_InputField PasswordInputField;

    public TextMeshProUGUI errorText;


    public void BackToEmail()
    {
        backToEmail.SetActive(false);
        password.SetActive(false);
        login.SetActive(false);
        recoverPassword.SetActive(false);
        register.SetActive(false);

        createAccount.SetActive(true);
        next.SetActive(true);
        back.SetActive(true);

        errorText.text = string.Empty;
        EmailInputField.text = null;
    }

    public void CreateNewAccount()
    {
        password.SetActive(true);
        username.SetActive(true);
        email.SetActive(true);
        backToEmail.SetActive(true);
        register.SetActive(true);

        back.SetActive(false);
        recoverPassword.SetActive(false);
        createAccount.SetActive(false);
        next.SetActive(false);
        login.SetActive(false);
    }
    public void ValidateEmail()
    {
        // <-- INSERT HERE server callback to check if the e-mail exists and the if should be changed to relevant data
        if (EmailInputField.text != null && EmailInputField.text.Contains('@'))
        {
            password.SetActive(true);
            login.SetActive(true);
            recoverPassword.SetActive(true);
            backToEmail.SetActive(true);

            createAccount.SetActive(false);
            register.SetActive(false);
            next.SetActive(false);
            back.SetActive(false);
        }
        else
        {
            errorText.text = IncorrectEmailError();

            password.SetActive(false);
            login.SetActive(false);
            recoverPassword.SetActive(false);
            backToEmail.SetActive(false);
        }
    }

    public void ValidatePassword()
    {
        // <-- INSERT HERE server callback to check if the e-mail exists and the if should be changed to relevant data
        if (PasswordInputField.text != null)
        {

        }
        else
        {
            errorText.text = IncorrectPasswordError();
        }
    }


    public string IncorrectEmailError()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            return "Incorrect e-mail!";
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            return "Неверный почтовый ящик!";
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            return "不正确的电子邮件！";
        }
        else if (Application.systemLanguage == SystemLanguage.Slovak)
        {
            return "Nesprávny e-mail!";
        }
        else { return null; }
    }

    public string IncorrectPasswordError()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            return "Incorrect password!";
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            return "Неправильный пароль!";
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            return "密码错误！";
        }
        else if (Application.systemLanguage == SystemLanguage.Slovak)
        {
            return "Nesprávne heslo!";
        }
        else { return null; }
    }

    public string UsernameExistsError()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            return "Please choose a different username!";
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            return "Пожалуйста, выберите другое имя пользователя!";
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            return "请选择一个不同的用户名！";
        }
        else if (Application.systemLanguage == SystemLanguage.Slovak)
        {
            return "Prosím, zvoľte iné používateľské meno!";
        }
        else { return null; }
    }

    public string GetErrorMessage(Gaos.Routes.Model.UserJson.RegisterResponseErrorKind? errorKind)
    {
        const string METHOD_NAME = "GetErrorMessage()";
        string msg = "Error!";
        switch(errorKind)
        {
            case RegisterResponseErrorKind.UsernameExistsError:
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
                switch(Application.systemLanguage)
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
                        break;
                        msg = "Internal error!";
                }
                break;
            default:
                msg = "Error!";
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: missing translation for: {errorKind}");
                break;  

        }

        return msg;
    }

    public void ProcessRegistrationError(Gaos.Routes.Model.UserJson.RegisterResponseErrorKind errorKing)
    {
        /*
                UsernameExistsError,
        UserNameIsEmptyError,
        EmailIsEmptyError,
        IncorrectEmailError,
        EmailExistsError,
        PasswordIsEmptyError,
        PasswordsDoNotMatchError,
        InternalError,
        */

    }

    public void StartRegisterUser()
    {
        StartCoroutine(RegisterUser());
    }

    private IEnumerator RegisterUser()
    {
        yield return new WaitUntil(() => Gaos.Device.Device.Registration.IsDeviceRegistered == true);
        StartCoroutine(Gaos.User.User.UserRegister.Register(UserNameInputField.text, EmailInputField.text, PasswordInputField.text, OnUserRegisterComplete));


    }
    public void OnUserRegisterComplete()
    {
        const string METHOD_NAME = "OnUserRegisterComplete()";

        if (Gaos.User.User.UserRegister.IsRegistered == true)
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: User registered: {Gaos.User.User.UserRegister.RegisterResponse.Jwt}");



            CoroutineManager.registeredUser = true;
            UserName.userName = Gaos.User.User.UserRegister.RegisterResponse.User.Name;  
            mainUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: User not registered, erorr: {Gaos.User.User.UserRegister.ResponseErrorKind}");
            errorText.text = GetErrorMessage(Gaos.User.User.UserRegister.ResponseErrorKind);
            //throw new System.Exception("User registration failed");
        }
    }
}
