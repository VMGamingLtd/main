using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] Image PrimaryColorButtonValue;
    [SerializeField] Image SecondaryColorButtonValue;
    [SerializeField] Image BackgroundColorButtonValue;
    [SerializeField] Image SecondaryBackgroundColorButtonValue;
    [SerializeField] Image NegativeColorButtonValue;

    void OnEnable()
    {
        UIManagerReference uIManagerRef = GameObject.Find("UIMANAGER").GetComponent<UIManagerReference>();

        PrimaryColorButtonValue.color = uIManagerRef.UIManagerAsset.primaryColor;
        SecondaryColorButtonValue.color = uIManagerRef.UIManagerAsset.secondaryColor;
        BackgroundColorButtonValue.color = uIManagerRef.UIManagerAsset.backgroundColor;
        SecondaryBackgroundColorButtonValue.color = uIManagerRef.UIManagerAsset.primaryReversed;
        NegativeColorButtonValue.color = uIManagerRef.UIManagerAsset.negativeColor;
    }
}
