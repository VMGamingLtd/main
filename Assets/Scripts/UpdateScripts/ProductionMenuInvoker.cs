using UnityEngine;
using UnityEngine.UI;

public class ProductionMenuInvoker : MonoBehaviour
{
    public Button buttonToClick;
    public Button buttonToClick2;
    public Button filterToClick;
    public Button filterToClick2;
    public int StatusNumber;
    public int FilterNumber;

    // Update is called once per frame
    void Awake()
    {
        buttonToClick.onClick.Invoke();
        filterToClick.onClick.Invoke();
        StatusNumber = 0;
        FilterNumber = 0;
    }

    public void SetStatusNumber(int Number)
    {
        StatusNumber = Number;
    }

    public void SetFilterNumber(int Number)
    {
        FilterNumber = Number;
    }

    private void OnEnable()
    {
        if (StatusNumber == 0)
        {
            buttonToClick.onClick.Invoke();
        }
        else if (StatusNumber == 1)
        {
            buttonToClick2.onClick.Invoke();
        }

        if (FilterNumber == 0)
        {
            filterToClick.onClick.Invoke();
        }
        else if (FilterNumber == 1)
        {
            filterToClick2.onClick.Invoke();
        }
    }

}
