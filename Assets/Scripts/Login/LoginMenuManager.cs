using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginMenuManager : MonoBehaviour
{
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

    public TMP_InputField EmailInputField;
    public TMP_InputField UserNameInputField;
    public TMP_InputField PasswordInputField;

    public TextMeshProUGUI errorText;

    public readonly static string CLASS_NAME = typeof(Register).Name;

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

    public void StartRegisterUser()
    {
        StartCoroutine(RegisterUser());
    }

    private IEnumerator RegisterUser()
    {
        yield return new WaitUntil(() => Gaos.Device.Device.Registration.IsDeviceRegistered == true);
        StartCoroutine(Gaos.User.User.UserRegister.Register(EmailInputField.text, UserNameInputField.text, PasswordInputField.text, OnUserRegisterComplete));


    }
    public void OnUserRegisterComplete()
    {
        const string METHOD_NAME = "OnUserRegisterComplete()";

        if (Gaos.User.User.UserRegister.IsRegistered == true)
        {
            Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: User registered: {Gaos.User.User.UserRegister.RegisterResponse.Jwt}");
            CoroutineManager.registeredUser = true;
        }
        else
        {
            throw new System.Exception("User registration failed");
        }
    }
}
