using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefreshMaxElectricity : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void OnEnable()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = Planet0Buildings.Planet0MaxElectricity.ToString();
    }

    public void enableRefreshMaxElectricity()
    {
        StartCoroutine(refreshMaxElectricity());
    }

    public IEnumerator refreshMaxElectricity()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            textField.text = Planet0Buildings.Planet0MaxElectricity.ToString();
        }
    }
}

