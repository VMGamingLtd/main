using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAccountManager : MonoBehaviour
{
    public LoginMenuManager loginManager;

    void OnEnable()
    {
        loginManager.ActivateAccountPanel();
    }
}
