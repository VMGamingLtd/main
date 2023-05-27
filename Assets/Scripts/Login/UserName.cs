using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserName : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public static string userName = null;
    public TMP_InputField myInputField;

    void OnEnable()
    {
        myInputField.Select();
        myInputField.text = "";
    }

    public void setUserNameFromText()
    {
        //UserName.userName = NameText.text;
    }
}
