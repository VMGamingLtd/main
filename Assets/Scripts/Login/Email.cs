using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Email : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI errorText;
    public static string email;
    public TMP_InputField myInputField;

    void OnEnable () {
        myInputField.Select();
        myInputField.text = "";
        errorText.text = "";
    }

    public void setEmailFromText()
    {
        errorText.text = "";
    }
}
