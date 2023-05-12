using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefreshEXPPoints : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = EXPPoints.expPoints.ToString();
    }

}
