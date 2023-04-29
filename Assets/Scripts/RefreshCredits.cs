using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefreshCredits : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void OnEnable()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = Credits.credits.ToString();
        enableRefreshCredits();
    }

    public void enableRefreshCredits()
    {
        StartCoroutine(refreshCredits());
    }

    public IEnumerator refreshCredits()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            textField.text = Credits.credits.ToString();
        }
    }
}

