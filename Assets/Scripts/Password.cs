using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Password : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public static string password;
    public TMP_InputField myInputField;

    void OnEnable () {
        myInputField.Select();
        myInputField.text = "";
    }

    public void setPasswordFromText() 
    {
        Password.password = NameText.text;
    }
}
