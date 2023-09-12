using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterStatusLoad : MonoBehaviour
{
    public readonly static string CLASS_NAME = typeof(LoginMenuManager).Name;

    public TextMeshProUGUI DisplayedText;

    private enum RegisterStatus
    {
        NotRegistered,
        Registered,
    }

    void OnEnable()
    {
        DisplayedText = GetComponent<TextMeshProUGUI>();
        if (Gaos.Context.Authentication.GetIsGuest())
        {
            DisplayedText.text = GetRegisterStatusText(RegisterStatus.NotRegistered);
        }
        else
        {
            DisplayedText.text = GetRegisterStatusText(RegisterStatus.Registered);
        }
    }

    private string GetRegisterStatusText(RegisterStatus registerStatus)
    {
        const string METHOD_NAME = "GetRegisterStatusText()";
        string txt = "";

        switch(registerStatus)
        {
            case RegisterStatus.NotRegistered:
                switch(Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        txt = "Not registered";
                        break;
                    case SystemLanguage.Russian:
                        txt = "Не зарегистрирован";
                        break;
                    case SystemLanguage.Chinese:
                        txt = "未注册";
                        break;
                    case SystemLanguage.Slovak:
                        txt = "Neregistrovaný";
                        break;
                    default:
                        txt = "Not registered";
                        break;
                }
                break;
            case RegisterStatus.Registered:
                switch(Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        txt = "Registered";
                        break;
                    case SystemLanguage.Russian:
                        txt = "Зарегистрирован";
                        break;
                    case SystemLanguage.Chinese:
                        txt = "挂号的";
                        break;
                    case SystemLanguage.Slovak:
                        txt = "Registrovaný";
                        break;
                    default:
                        txt = "Registered";
                        break;
                }
                break;
            default:
                txt = "Error!";
                Debug.Log($"{CLASS_NAME}:{METHOD_NAME}: ERROR: missing translation for: {registerStatus}");
                break;  

        }

        return txt;
    }

}
