using UnityEngine;
using UnityEngine.UI;

public class RefreshEXPbar : MonoBehaviour
{
    private Image ExpBar;

    void Start()
    {
        ExpBar = GetComponent<Image>();
        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;
    }

}
