using TMPro;
using UnityEngine;

public class RefreshCurrentEXP : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = Player.CurrentExp.ToString();
    }
}
