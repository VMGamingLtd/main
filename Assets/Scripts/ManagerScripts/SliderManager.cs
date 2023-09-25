using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using ItemManagement;

public class SliderManager : MonoBehaviour
{
    public TextMeshProUGUI originalValue;
    public TextMeshProUGUI newValue;
    public TextMeshProUGUI itemName;
    Slider mainSlider;
    private float currentSplitQuantity;
    private int currentObjID;
    private string currentItemProduct;
    private string currentObjName;
    private int objectIndex;

    public void InitializeSlider(ItemData itemData, string objName)
    {
        itemName.text = objName;
        mainSlider = gameObject.GetComponent<Slider>();
        mainSlider.value = 0;
        originalValue.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
        mainSlider.maxValue = itemData.itemQuantity;
        currentObjName = objName;
        currentItemProduct = itemData.itemProduct;
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
