using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerReference : MonoBehaviour
{
    public UIManager UIManagerAsset;
    [SerializeField] Image PrimaryColorButtonValue;
    [SerializeField] Image SecondaryColorButtonValue;
    [SerializeField] Image BackgroundColorButtonValue;
    [SerializeField] Image NegativeColorButtonValue;
    [SerializeField] Image SecondaryBackgroundColorButtonValue;

    public void SetPrimaryColor(Color color)
    {
        UIManagerAsset.primaryColor = color;
    }

    public void SetSecondaryColor(Color color)
    {
        UIManagerAsset.secondaryColor = color;
    }

    public void SetBackgroundColor(Color color)
    {
        UIManagerAsset.backgroundColor = color;
    }

    public void SetSecondaryBackgroundColor(Color color)
    {
        UIManagerAsset.primaryReversed = color;
    }

    public void SetNegativeColor(Color color)
    {
        UIManagerAsset.negativeColor = color;
    }

    public void ResetPrimaryColor()
    {
        UIManagerAsset.primaryColor = UIColors.PrimaryColor;
        PrimaryColorButtonValue.color = UIColors.PrimaryColor;
    }

    public void ResetSecondaryColor()
    {
        UIManagerAsset.secondaryColor = UIColors.SecondaryColor;
        SecondaryColorButtonValue.color = UIColors.SecondaryColor;
    }

    public void ResetBackgroundColor()
    {
        UIManagerAsset.backgroundColor = UIColors.BackgroundColor;
        BackgroundColorButtonValue.color = UIColors.BackgroundColor;
    }

    public void ResetSecondaryBackgroundColor()
    {
        UIManagerAsset.primaryReversed = UIColors.PrimaryReversed;
        SecondaryBackgroundColorButtonValue.color = UIColors.PrimaryReversed;
    }

    public void ResetNegativeColor()
    {
        UIManagerAsset.negativeColor = UIColors.NegativeColor;
        NegativeColorButtonValue.color = UIColors.NegativeColor;
    }

    public void ResetFactoryColors()
    {
        UIManagerAsset.primaryColor = UIColors.PrimaryColor;
        UIManagerAsset.secondaryColor = UIColors.SecondaryColor;
        UIManagerAsset.primaryReversed = UIColors.PrimaryReversed;
        UIManagerAsset.negativeColor = UIColors.NegativeColor;
        UIManagerAsset.backgroundColor = UIColors.BackgroundColor;
    }
}
