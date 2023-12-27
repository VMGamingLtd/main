using ItemManagement;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public TextMeshProUGUI originalValue;
    public TextMeshProUGUI newValue;
    public TextMeshProUGUI itemName;
    Slider mainSlider;
    private float currentSplitQuantity;
    private int currentObjID;
    private string currentItemProduct;
    private string currentItemType;
    private string currentObjName;
    private int objectIndex;

    public void InitializeSlider(ItemData itemData, string objName)
    {
        itemName.text = objName;
        mainSlider = gameObject.GetComponent<Slider>();
        mainSlider.value = 0;
        if (itemData.itemType == "SUIT" || itemData.itemType == "HELMET" || itemData.itemType == "FABRICATOR")
        {
            mainSlider.wholeNumbers = true;
        }
        else
        {
            mainSlider.wholeNumbers = false;
        }
        originalValue.text = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);
        if (mainSlider.wholeNumbers)
        {
            mainSlider.maxValue = itemData.quantity - 1f;
        }
        else
        {
            mainSlider.maxValue = itemData.quantity - 0.01f;
        }
        currentObjName = objName;
        currentItemProduct = itemData.itemProduct;
        currentItemType = itemData.itemType;
        currentObjID = itemData.ID;
        objectIndex = itemData.index;
        Time.timeScale = 0f;
    }
    public void OnDisable()
    {
        Time.timeScale = 1f;
    }
    public float GetCurrentSplitQuantity()
    {
        currentSplitQuantity = mainSlider.value;
        return currentSplitQuantity;
    }

    public string GetCurrentObjName()
    {
        return currentObjName;
    }
    public string GetCurrentItemProduct()
    {
        return currentItemProduct;
    }
    public string GetCurrentItemType()
    {
        return currentItemType;
    }
    public int GetCurrentObjID()
    {
        return currentObjID;
    }
    public int GetCurrentObjIndex()
    {
        return objectIndex;
    }

    void Update()
    {
        newValue.text = mainSlider.value.ToString("F2", CultureInfo.InvariantCulture);
    }
}
