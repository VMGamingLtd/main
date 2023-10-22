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

        Assets.Scripts.Login.UserCahngedEvent.OnEvent += OnUserChanged;
    }

    void OnDisable()
    {
        Assets.Scripts.Login.UserCahngedEvent.OnEvent -= OnUserChanged;
    }

    void OnUserChanged(Assets.Scripts.Login.UserChangedEventPayload payload)
    {
        NameText.text = payload.UserName;
    }
}
