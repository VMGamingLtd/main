using UnityEngine;
using UnityEngine.UI;

public class TimebarControl : MonoBehaviour
{
    public Image Timebar;

    public void UpdateTimebar(float currentAmount)
    {
        Timebar.fillAmount = currentAmount;
    }

    public void ChangeTimeBarColor(Color color)
    {
        Timebar.color = color;
    }
}
