using Cysharp.Threading.Tasks;
using Gaos.Routes.Model.UserJson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecoverPasswordUsernameScreenManager : MonoBehaviour
{
    public readonly static string CLASS_NAME = typeof(RecoverPasswordUsernameScreenManager).Name;

    public TextMeshProUGUI errorText;
    public TextMeshProUGUI infoText;

    public GameObject errorTextGameObject;
    public GameObject infoTextGameObject;

    public TMP_InputField passwordOrUsernameTextInput;

    public GameObject emailOrUserNameGameObject;

    public Button buttonRecover;
    public Button buttonBack;

    public void OnEnable()
    {
        ClearErrorText();

        passwordOrUsernameTextInput.text = "";

    }

    public void OnDisable()
    {
    }

    private void ClearErrorText()
    {
        errorText.text = "";
    }

    private void SetErrorText(string text)
    {
        errorText.text = text;
    }

    private void Deactivate(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }


    private void ClearInfoText()
    {
        infoText.text = "";
    }

    private void SetInfoText(string text)
    {
        infoText.text = text;
    }

    private void DisableButton(Button button)
    {
        button.interactable = false;
    }

    private void EnableButton(Button button)
    {
        button.interactable = true;
    }

    private string GetErrorMessage(Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeErrorKind errorKind)
    {
        string msg = "";
        switch (errorKind)
        {
            case RecoverPasswordSendVerificationCodeErrorKind.UserNameOrEmailNotFound:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Username or email not found.";
                        break;
                    case SystemLanguage.Russian:
                        msg =  "Имя пользователя или электронная почта не найдены.";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "用户名或电子邮件未找到。";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Užívateľské meno alebo e-mail sa nenašli.";
                        break;
                    default:
                        msg = "Username or email not found.";
                        break;
                }
                break;
            case RecoverPasswordSendVerificationCodeErrorKind.InternalError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Internal error.";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Внутренняя ошибка.";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "内部错误。";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Vnútorná chyba.";
                        break;
                    default:
                        msg = "Internal error.";
                        break;
                }
                break;
            default:
                msg = "Internal error.";
                break;
        }
        return msg;
    }

    private async UniTaskVoid OnRecoverButtonClickAsync()
    {
        DisableButton(buttonRecover);
        ClearErrorText();
        ClearInfoText();

        string passwordOrUsername = passwordOrUsernameTextInput.text;

        string msg;

        Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeResponse response = await Gaos.User.User.RecoverPassword.SendVerificationCode(passwordOrUsername);
        if (response == null)
        {
            msg = GetErrorMessage(Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeErrorKind.InternalError);
            SetErrorText(msg);
            EnableButton(buttonRecover);
            return;
        }
        if (response.IsError == true)
        {
            msg = GetErrorMessage(response.ErrorKind.Value);
            SetErrorText(msg);
            EnableButton(buttonRecover);
            return;
        }

        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                msg = "Verification code sent to your email.";
                break;
            case SystemLanguage.Russian:
                msg = "Код подтверждения отправлен на вашу электронную почту.";
                break;
            case SystemLanguage.Chinese:
                msg = "验证码已发送到您的电子邮件。";
                break;
            case SystemLanguage.Slovak:
                msg = "Overovací kód bol odoslaný na váš e-mail.";
                break;
            default:
                msg = "Verification code sent to your email.";
                break;
        }

        Deactivate(emailOrUserNameGameObject);
        Deactivate(buttonRecover.gameObject);
        Deactivate(errorTextGameObject);
        SetInfoText(msg);
    }

    public void OnRecoverButtonClick()
    {
        Debug.Log($"{CLASS_NAME}:OnRecoverButtonClick(): called, @@@@@@@@@@ cp 100");
        OnRecoverButtonClickAsync().Forget();
    }


}
