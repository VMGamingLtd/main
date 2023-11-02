using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Gaos.Routes.Model.UserJson;
using UnityEngine.UI;

public class LoginMenuManager : MonoBehaviour
{
    public readonly static string CLASS_NAME = typeof(LoginMenuManager).Name;


    public GameObject loginIntroScreenForGuest;
    public GameObject loginIntroScreenForUser;

    public GameObject AccountPanel;
    public GameObject FriendsPanel;
    public GameObject RegisterButton;


    public void OnEnable()
    {
        //showFirstScreen();

    }
    public void ActivateAccountPanel()
    {
        AccountPanel.SetActive(true);
        ActivateFriendsTab();
    }

    public void DeactivateAccountPanel()
    {
        AccountPanel.SetActive(false);
    }

    public void ActivateFriendsTab()
    {
        FriendsPanel.SetActive(true);
        var button = RegisterButton.GetComponent<Button>();
        button.interactable = true;
    }
    public void DeactivateFriendsTab()
    {
        FriendsPanel.SetActive(false);
        var button = RegisterButton.GetComponent<Button>();
        button.interactable = false;
    }


    public void GoToLogin()
    {
        if (Gaos.Context.Authentication.GetIsGuest())
        {
            loginIntroScreenForGuest.SetActive(true);
            //CanvasGroup canvasGroup = newUserScreen.GetComponent<CanvasGroup>();
            //canvasGroup.alpha = 1;
            //canvasGroup.interactable = true;
            //Animation animation = loginIntroScreenForGuest.GetComponent<Animation>();
            //animation.Play("TooltipStart");

        }
        else
        {
            loginIntroScreenForUser.SetActive(true);
            //Animation animation = loginIntroScreenForUser.GetComponent<Animation>();
            //animation.Play("TooltipStart");
        }
        DeactivateFriendsTab();
    }

}
