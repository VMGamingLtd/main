using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Email : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public static string email;
    public TMP_InputField myInputField;

    void OnEnable () {
        myInputField.Select();
        myInputField.text = "";
    }

    public void setEmailFromText() 
    {
        Email.email = NameText.text;
    }
}
