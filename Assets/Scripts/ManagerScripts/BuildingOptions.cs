using BuildingManagement;
using UnityEngine;

public class BuildingOptions : MonoBehaviour
{
    private BuildingItemData itemData;
    private EnergyBuildingItemData itemDataEnergy;
    private ResearchBuildingItemData itemDataResearch;
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
        if (!BuildingManager.isDraggingBuilding)
        {
            itemData = GetComponent<BuildingItemData>();
            itemDataEnergy = GetComponent<EnergyBuildingItemData>();
            itemDataResearch = GetComponent<ResearchBuildingItemData>();
            refObj = transform.gameObject;
            buildingOptionsWindow.buildingOptions.SetActive(false);
            if (itemData != null)
            {
                optionsInterfaceScript.StartUpdatingUI(itemData, refObj);
            }
            else if (itemDataResearch != null)
            {
                optionsInterfaceScript.StartUpdatingResearchUI(itemDataResearch, refObj);
            }
            else
            {
                optionsInterfaceScript.StartUpdatingEnergyUI(itemDataEnergy, refObj);
            }
        }
    }
}
