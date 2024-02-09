using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject BaseButton;
    public GameObject ResearchButton;
    private Animator animator;
    public static string MenuButtonTypeOn = "";


    public void BaseButtonAnimationOn()
    {
        if (BaseButton.activeSelf)
        {
            animator = BaseButton.GetComponent<Animator>();
            animator.Play("Normal to Pressed");
        }
    }

    public void ResearchButtonAnimationOn()
    {
        if (ResearchButton.activeSelf)
        {
            animator = ResearchButton.GetComponent<Animator>();
            animator.Play("Normal to Pressed");
        }
    }

    public void AllButtonAnimationOff()
    {
        if (BaseButton.activeSelf)
        {
            animator = BaseButton.GetComponent<Animator>();
            animator.Play("Pressed to Normal");
        }
        if (ResearchButton.activeSelf)
        {
            animator = ResearchButton.GetComponent<Animator>();
            animator.Play("Pressed to Normal");
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

    public void ResearchButtonAnimationOff()
    {
        if (ResearchButton.activeSelf)
        {
            animator = ResearchButton.GetComponent<Animator>();
            animator.Play("Pressed to Normal");
        }
    }

    public void ChangeMenuButtonType(string NewMenuType)
    {
        MenuButtonTypeOn = NewMenuType;
        return;
    }

    public void UnlockBaseButton()
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

    public void UnlockResearchButton()
    {
        if (GoalManager.thirdGoal == true)
        {
            ResearchButton.SetActive(true);
        }
        else
        {
            ResearchButton.SetActive(false);
        }
    }
}
