using UnityEngine;
using UnityEngine.UI;

public class RefreshEXPbar : MonoBehaviour
{
    private Image ExpBar;

    void Start()
    {
        ExpBar = GetComponent<Image>();
        int playerCurrentExp = Player.GetCurrentResource(ref Player.PlayerCurrentExp);
        int playerMaxExp = Player.GetCurrentResource(ref Player.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;
    }

}
