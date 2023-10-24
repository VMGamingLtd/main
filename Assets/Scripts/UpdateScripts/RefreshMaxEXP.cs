using TMPro;
using UnityEngine;

public class RefreshMaxEXP : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = Level.PlayerMaxExp.ToString();
    }
}
