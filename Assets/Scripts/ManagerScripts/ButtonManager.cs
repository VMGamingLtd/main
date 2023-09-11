using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject BaseButton;
    private Animator animator;
    public static string MenuButtonTypeOn = "";
    void OnEnable()
    {
        if (GoalManager.secondGoal == true)
        {
            BaseButton.SetActive(true);
        }
        else
        {
            BaseButton.SetActive(false);
        }
    }

    public void BaseButtonAnimationOn()
    {
        if (BaseButton.activeSelf)
        {
            animator = BaseButton.GetComponent<Animator>();
            animator.Play("Normal to Pressed");
        }
    }
    public void BaseButtonAnimationOff()
    {
        if (BaseButton.activeSelf)
        {
            animator = BaseButton.GetComponent<Animator>();
            animator.Play("Pressed to Normal");
        }
    }
    public void ChangeMenuButtonType(string NewMenuType)
    {
        MenuButtonTypeOn = NewMenuType;
        return;
    }

}
