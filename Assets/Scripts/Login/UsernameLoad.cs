using TMPro;
using UnityEngine;
using System;
using UniRx;
using ModelsRx;

public class UsernameLoad : MonoBehaviour
{
    private TextMeshProUGUI _nameText;
    private IDisposable _disposableUserRx;

    void OnEnable()
    {
        _nameText = GetComponent<TextMeshProUGUI>();
        _nameText.text = ContextRx.UserRx.UserName;
        _disposableUserRx = ContextRx.UserRx.Observable.Where(x => x.EventArgs.PropertyName == "UserName")
            .Subscribe(x => _nameText.text = ((UserRx)x.Sender).UserName);
    }

    void OnDisable()
    {
        _disposableUserRx.Dispose();
    }
}