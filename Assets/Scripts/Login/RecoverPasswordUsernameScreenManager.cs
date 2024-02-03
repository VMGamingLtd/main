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
    public TMP_InputField verificationCodeTextInput;
    public TMP_InputField newPasswordTextInput;
    public TMP_InputField newPasswordVerifyTextInput;

    public GameObject emailOrUserNameInputGameObject;
    public GameObject verificationCodeGameInputObject;
    public GameObject newPasswordInputGeameObject;
    public GameObject newPasswordVerifyInputGeameObject;

    public GameObject buttonSendGameObject;
    public GameObject buttonBackGameObject;

    public Button buttonSend;
    public Button buttonBack;

    public void OnEnable()
    {
        passwordOrUsernameTextInput.text = "";
        setIinitialSate();

    }

    public void OnDisable()
    {
    }

    public void ClearErrorText()
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

    private void Activate(GameObject gameObject)
    {
        gameObject.SetActive(true);
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

    private void setIinitialSate()
    {
        FormState = FormStateEnum.Initial;
        Deactivate(verificationCodeGameInputObject);
        Deactivate(newPasswordInputGeameObject);
        Deactivate(newPasswordVerifyInputGeameObject);

        Activate(emailOrUserNameInputGameObject);
        Activate(buttonSendGameObject);
        EnableButton(buttonSend);
        Activate(buttonBackGameObject);
        EnableButton(buttonBack);

        ClearErrorText();
        ClearInfoText();
        passwordOrUsernameTextInput.text = "";
        verificationCodeTextInput.text = "";
        newPasswordTextInput.text = "";
        newPasswordVerifyTextInput.text = "";
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
                        msg = "Имя пользователя или электронная почта не найдены.";
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

    private string GetErrorMessage(Gaos.Routes.Model.UserJson.RecoverPasswordVerifyCodeReplyErrorKind errorKind)
    {
        string msg = "";
        switch (errorKind)
        {
            case RecoverPasswordVerifyCodeReplyErrorKind.InternalError:
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

    private string GetErrorMessage(Gaos.Routes.Model.UserJson.RecoverPasswordChangePassworErrorKind errorKind)
    {
        string msg = "";
        switch (errorKind)
        {
            case RecoverPasswordChangePassworErrorKind.InvalidVerificationCodeError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Invalid verification code.";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Неверный код подтверждения.";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "验证码无效。";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Neplatný overovací kód.";
                        break;
                    default:
                        msg = "Invalid verification code.";
                        break;
                }
                break;
            case RecoverPasswordChangePassworErrorKind.InternalError:
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
            case RecoverPasswordChangePassworErrorKind.PasswordIsEmptyError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Password is empty.";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пароль пуст.";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "密码为空。";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Heslo je prázdne.";
                        break;
                    default:
                        msg = "Password is empty.";
                        break;
                }
                break;
            case RecoverPasswordChangePassworErrorKind.PasswordsDoNotMatchError:
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        msg = "Passwords do not match.";
                        break;
                    case SystemLanguage.Russian:
                        msg = "Пароли не совпадают.";
                        break;
                    case SystemLanguage.Chinese:
                        msg = "密码不匹配。";
                        break;
                    case SystemLanguage.Slovak:
                        msg = "Heslá sa nezhodujú.";
                        break;
                    default:
                        msg = "Passwords do not match.";
                        break;
                }
                break;
            default:
                msg = "Internal error.";
                break;
        }
        return msg;
    }


    private enum FormStateEnum
    {
        Initial,
        VerificationCode,
        PasswordChange,
        Finish
    }
    private FormStateEnum FormState = FormStateEnum.Initial;

    Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeResponse gRecoverPasswordSendVerificationCodeResponse;
    string gVerificationCode;

    private async UniTask OnSendButtonClickAsync_Initial()
    {
        DisableButton(buttonSend);
        ClearErrorText();
        ClearInfoText();

        string passwordOrUsername = passwordOrUsernameTextInput.text;

        string msg;

        Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeResponse response = await Gaos.User.User.RecoverPassword.SendVerificationCode(passwordOrUsername);
        gRecoverPasswordSendVerificationCodeResponse = response;
        if (response == null)
        {
            msg = GetErrorMessage(Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeErrorKind.InternalError);
            SetErrorText(msg);
            EnableButton(buttonSend);
            return;
        }
        if (response.IsError == true)
        {
            msg = GetErrorMessage(response.ErrorKind.Value);
            SetErrorText(msg);
            EnableButton(buttonSend);
            return;
        }

        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                msg = "Verification code was sent to your email.";
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
                msg = "Verification code was sent to your email.";
                break;
        }

        Deactivate(emailOrUserNameInputGameObject);
        Activate(verificationCodeGameInputObject);
        EnableButton(buttonSend);

        FormState = FormStateEnum.VerificationCode;
        SetInfoText(msg);
    }

    private async UniTask OnSendButtonClickAsync_VerificationCode()
    {
        DisableButton(buttonSend);
        ClearErrorText();
        ClearInfoText();

        string verificationCode = verificationCodeTextInput.text;
        string msg;

        Gaos.Routes.Model.UserJson.RecoverPasswordVerifyCodeResponse response = await Gaos.User.User.RecoverPassword.VerifyCode((int)gRecoverPasswordSendVerificationCodeResponse.UserId, verificationCode);
        if (response == null)
        {
            msg = GetErrorMessage(Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeErrorKind.InternalError);
            SetErrorText(msg);
            EnableButton(buttonSend);
            return;
        }
        if (response.IsError == true)
        {
            msg = GetErrorMessage(response.ErrorKind.Value);
            SetErrorText(msg);
            EnableButton(buttonSend);
            return;
        }

        if (!(bool)response.IsVerified)
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    msg = "Verification code is not correct.";
                    break;
                case SystemLanguage.Russian:
                    msg = "Код подтверждения неверен.";
                    break;
                case SystemLanguage.Chinese:
                    msg = "验证码不正确。";
                    break;
                case SystemLanguage.Slovak:
                    msg = "Overovací kód nie je správny.";
                    break;
                default:
                    msg = "Verification code is not correct.";
                    break;
            }
            SetErrorText(msg);
            EnableButton(buttonSend);
            return;
        }

        gVerificationCode = verificationCode;

        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                msg = $"Your username is {response.UserName}. You may change your password now.";
                break;
            case SystemLanguage.Russian:
                msg = $"Ваше имя пользователя - {response.UserName}. Теперь вы можете изменить свой пароль.";
                break;
            case SystemLanguage.Chinese:
                msg = $"您的用户名是{response.UserName}。您现在可以更改密码。";
                break;
            case SystemLanguage.Slovak:
                msg = $"Vaše používateľské meno je {response.UserName}. Teraz môžete zmeniť svoje heslo.";
                break;
            default:
                msg = $"Your username is {response.UserName}. You may change your password now.";
                break;
        }

        Deactivate(verificationCodeGameInputObject);
        Activate(newPasswordInputGeameObject);
        Activate(newPasswordVerifyInputGeameObject);
        EnableButton(buttonSend);

        FormState = FormStateEnum.PasswordChange;
        SetInfoText(msg);
    }

    private async UniTask OnSendButtonClickAsync_PasswordChange()
    {
        DisableButton(buttonSend);
        ClearErrorText();
        ClearInfoText();

        string newPassword = newPasswordTextInput.text;
        string newPasswordVerify = newPasswordVerifyTextInput.text;

        string msg;

        Gaos.Routes.Model.UserJson.RecoverPasswordChangePasswordResponse response = await Gaos.User.User.RecoverPassword.ChangePassword((int)gRecoverPasswordSendVerificationCodeResponse.UserId, newPassword, newPasswordVerify, gVerificationCode);
        if (response == null)
        {
            msg = GetErrorMessage(Gaos.Routes.Model.UserJson.RecoverPasswordSendVerificationCodeErrorKind.InternalError);
            SetErrorText(msg);
            EnableButton(buttonSend);
            return;
        }
        if (response.IsError == true)
        {
            msg = GetErrorMessage(response.ErrorKind.Value);
            SetErrorText(msg);
            EnableButton(buttonSend);
            return;
        }

        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                msg = "Password was changed.";
                break;
            case SystemLanguage.Russian:
                msg = "Пароль был изменен.";
                break;
            case SystemLanguage.Chinese:
                msg = "密码已更改。";
                break;
            case SystemLanguage.Slovak:
                msg = "Heslo bolo zmenené.";
                break;
            default:
                msg = "Password was changed.";
                break;
        }
        EnableButton(buttonSend);

        Deactivate(newPasswordInputGeameObject);
        Deactivate(newPasswordVerifyInputGeameObject);
        Deactivate(buttonSendGameObject);

        FormState = FormStateEnum.Finish;
        SetInfoText(msg);
    }

    private void OnSendButtonClickAsync_Finish()
    {
        ClearErrorText();
        ClearInfoText();

        string msg;

        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                msg = "Password was changed.";
                break;
            case SystemLanguage.Russian:
                msg = "Пароль был изменен.";
                break;
            case SystemLanguage.Chinese:
                msg = "密码已更改。";
                break;
            case SystemLanguage.Slovak:
                msg = "Heslo bolo zmenené.";
                break;
            default:
                msg = "Password was changed.";
                break;
        }

        Deactivate(newPasswordInputGeameObject);
        Deactivate(newPasswordVerifyInputGeameObject);
        Deactivate(buttonSendGameObject);
        FormState = FormStateEnum.Finish;
        SetInfoText(msg);
    }

    private async UniTaskVoid OnSendButtonClickAsync()
    {
        if (FormState == FormStateEnum.Initial)
        {
            await OnSendButtonClickAsync_Initial();
            return;
        }
        if (FormState == FormStateEnum.VerificationCode)
        {
            await OnSendButtonClickAsync_VerificationCode();
            return;
        }
        if (FormState == FormStateEnum.PasswordChange)
        {
            await OnSendButtonClickAsync_PasswordChange();
            return;
        }
        if (FormState == FormStateEnum.Finish)
        {
            OnSendButtonClickAsync_Finish();
            return;
        }
    }


    public void OnSendButtonClick()
    {
        OnSendButtonClickAsync().Forget();
    }


}
