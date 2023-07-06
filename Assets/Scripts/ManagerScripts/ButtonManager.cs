using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject BaseButton;
    public static string MenuButtonTypeOn = "";
    void OnEnable()
    {
        if (GoalManager.secondGoal == true)
        {
            BaseButton.SetActive(true);
        }
    }

    public void ChangeMenuButtonType(string NewMenuType)
    {
        MenuButtonTypeOn = NewMenuType;
        return;
    }
}
