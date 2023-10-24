using TMPro;
using UnityEngine;

public class UsernameLoad : MonoBehaviour
{

    private TextMeshProUGUI NameText;

    void OnEnable()
    {
        NameText = GetComponent<TextMeshProUGUI>();
        NameText.text = UserName.userName;

        Assets.Scripts.Login.UserChangedEvent.OnEvent += OnUserChanged;
    }

    void OnDisable()
    {
        Assets.Scripts.Login.UserChangedEvent.OnEvent -= OnUserChanged;
    }

    void OnUserChanged(Assets.Scripts.Login.UserChangedEventPayload payload)
    {
        NameText.text = payload.UserName;
    }
}
