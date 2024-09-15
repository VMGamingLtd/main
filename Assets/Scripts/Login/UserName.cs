using TMPro;
using UnityEngine;

public class UserName : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public static string userName = null;
    public static string country = null;
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