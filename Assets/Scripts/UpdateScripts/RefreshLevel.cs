using TMPro;
using UnityEngine;

public class RefreshLevel : MonoBehaviour
{
    private TextMeshProUGUI textField;

    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        textField.text = Player.Level.ToString();
    }
}
