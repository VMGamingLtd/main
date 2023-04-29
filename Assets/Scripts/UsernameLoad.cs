using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UsernameLoad : MonoBehaviour
{
    
    private TextMeshProUGUI NameText;

    void OnEnable()
    {
        NameText = GetComponent<TextMeshProUGUI>();
        NameText.text = UserName.userName;
    }
}
