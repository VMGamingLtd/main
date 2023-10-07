using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public GameObject AccountPanel;
    public GameObject FriendsTab;


    public void ActivateAccountPanel()
    {
        AccountPanel.SetActive(true);
    }

    public void DeactivateAccountPanel()
    {
        AccountPanel.SetActive(false);
    }

    public void ActivateFriendsTab()
    {
        FriendsTab.SetActive(true);
    }
    public void DeactivateFriendsTab()
    {
        FriendsTab.SetActive(false);
    }
}
