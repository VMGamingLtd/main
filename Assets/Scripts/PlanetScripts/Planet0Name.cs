using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Planet0Name : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public static string planet0Name = "Kelia I";
    public TMP_InputField myInputField;

    void OnEnable()
    {
        myInputField.Select();
        myInputField.text = "";
        NameText.text = Planet0Name.planet0Name;
    }

    public void setPlanetName()
    {
        Planet0Name.planet0Name = NameText.text;
    }
}
