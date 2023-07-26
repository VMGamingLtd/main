using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;
using TMPro;

public class BuildingOptions : MonoBehaviour
{
    private BuildingItemData itemData;
    private BuildingOptionsInterface optionsInterfaceScript;
    private BuildingOptionsWindow buildingOptionsWindow;
    private GameObject refObj;


    // Initialize references in Awake to ensure they are set before other methods are called
    private void Awake()
    {
        buildingOptionsWindow = GameObject.Find("BuildingOptionsWindow").GetComponent<BuildingOptionsWindow>();
        optionsInterfaceScript = buildingOptionsWindow.optionsInterfaceScript;
    }

    public void PassOptionWindowData()
    {
        itemData = GetComponent<BuildingItemData>();
        refObj = transform.gameObject;
        buildingOptionsWindow.buildingOptions.SetActive(false);
        optionsInterfaceScript.StartUpdatingUI(itemData, refObj);
    }
}
