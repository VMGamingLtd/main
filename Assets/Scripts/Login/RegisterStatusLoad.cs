using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterStatusLoad : MonoBehaviour
{
    public TextMeshProUGUI DisplayedText;

    void OnEnable()
    {
        DisplayedText = GetComponent<TextMeshProUGUI>();
        if (Gaos.Context.Authentication.GetIsGuest())
        {
            DisplayedText.text = "Not-registeredx";
        }
        else
        {
            DisplayedText.text = "Registered";
        }
    }

}
