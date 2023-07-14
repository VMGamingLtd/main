using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefreshCurrentElectricity : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void OnEnable()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = Planet0Buildings.Planet0CurrentElectricity.ToString();
    }

    public void enableRefreshCurrentElectricity()
    {
        StartCoroutine(refreshCurrentElectricity());
    }

    public IEnumerator refreshCurrentElectricity()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            textField.text = Planet0Buildings.Planet0CurrentElectricity.ToString();
        }
    }
}

