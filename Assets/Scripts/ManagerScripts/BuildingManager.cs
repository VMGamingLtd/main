using BuildingManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCycleCount
{
    public List<int> tenCycleCounts = new() { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120 };
    public List<int> thirtyCycleCounts = new() { 30, 60, 90, 120 };
    public List<int> sixHourCounts = new() { 6, 12, 18, 24, 30, 36, 42, 48, 54, 60, 66, 72, 78, 84, 90, 96, 102, 108, 114, 120 };
}
public class BuildingManager : MonoBehaviour
{
    public Dictionary<string, GameObject[]> buildingArrays;
    public BuildingCreator buildingCreator;
    public static string ShowBuildingTypes = "ALL";
    public static bool isDraggingBuilding = false;
    public InventoryManager inventoryManagerRef;
    public CoroutineManager coroutineManagerRef;
    private BuildingCycleCount cycleCount;

    public void PopulateBuildingArrays()
    {
        buildingArrays = new Dictionary<string, GameObject[]>
        {
            { "AGRICULTURE", new GameObject[0] },
            { "PUMPINGFACILITY", new GameObject[0] },
            { "FACTORY", new GameObject[0] },
            { "COMMFACILITY", new GameObject[0] },
            { "STORAGEHOUSE", new GameObject[0] },
            { "NAVALFACILITY", new GameObject[0] },
            { "OXYGENFACILITY", new GameObject[0] },
            { "AVIATIONFACILITY", new GameObject[0] },
            { "HEATINGFACILITY", new GameObject[0] },
            { "COOLINGFACILITY", new GameObject[0] },
            { "POWERPLANT", new GameObject[0] },
            { "OXYGENSTATION", new GameObject[0] },
            { "MININGRIG", new GameObject[0] },
            { "RESEARCH", new GameObject[0] }  // @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ TODO: this was milan fix of crash, fix needs to be consulted with brano
        };
        cycleCount = new BuildingCycleCount();
    }
    void OnEnable()
    {
        ShowItems(ShowBuildingTypes);
    }
    public void UpdateBuildingCyclesForAllBuildings()
    {
        if (buildingArrays != null && buildingArrays.Count > 0)
        {
            foreach (var kvp in buildingArrays)
            {
                GameObject[] itemArray = kvp.Value;

                foreach (GameObject item in itemArray)
                {
                    if (kvp.Key == "POWERPLANT")
                    {
                        EnergyBuildingItemData itemDataEnergy = item.GetComponent<EnergyBuildingItemData>();
                        UpdateEnergyBuildingCycles(itemDataEnergy);
                    }
                    else
                    {
                        BuildingItemData itemData = item.GetComponent<BuildingItemData>();
                        UpdateBuildingCycles(itemData);
                    }

                }
            }
        }
    }
    public void UpdateEnergyBuildingCycles(EnergyBuildingItemData itemData)
    {
        // Shift the elements in the secondCycle array to the right (oldest values will be discarded)
        ShiftAndFillCycle(itemData.powerCycleData.secondCycle, itemData.actualPowerOutput);
        itemData.secondCycleCount++;

        // Add actualPowerOutput to the first element of the 10s cycle array
        if (cycleCount.tenCycleCounts.Contains(itemData.secondCycleCount))
        {
            ShiftAndFillCycle(itemData.powerCycleData.tenSecondCycle, itemData.actualPowerOutput);
        }

        // Add actualPowerOutput to the first element of the 30s cycle array
        if (cycleCount.thirtyCycleCounts.Contains(itemData.secondCycleCount))
        {
            ShiftAndFillCycle(itemData.powerCycleData.thirtySecondCycle, itemData.actualPowerOutput);
        }

        // Add actualPowerOutput to the first element of the 1 minute cycle array
        if (itemData.secondCycleCount == 60 || itemData.secondCycleCount == 120)
        {
            ShiftAndFillCycle(itemData.powerCycleData.minuteCycle, itemData.actualPowerOutput);
            itemData.minuteCycleCount++;
        }

        // Add actualPowerOutput to the first element of the 10 minute cycle array
        if (cycleCount.tenCycleCounts.Contains(itemData.minuteCycleCount))
        {
            ShiftAndFillCycle(itemData.powerCycleData.tenMinuteCycle, itemData.actualPowerOutput);
        }

        // Add actualPowerOutput to the first element of the 30 minute cycle array
        if (cycleCount.thirtyCycleCounts.Contains(itemData.minuteCycleCount))
        {
            ShiftAndFillCycle(itemData.powerCycleData.thirtyMinuteCycle, itemData.actualPowerOutput);
        }

        // Add actualPowerOutput to the first element of the 1 hour cycle array
        if (itemData.minuteCycleCount == 60 || itemData.minuteCycleCount == 120)
        {
            ShiftAndFillCycle(itemData.powerCycleData.hourCycle, itemData.actualPowerOutput);
            itemData.hourCycleCount++;
        }

        // Add actualPowerOutput to the first element of the 6 hour cycle array
        if (cycleCount.sixHourCounts.Contains(itemData.hourCycleCount))
        {
            ShiftAndFillCycle(itemData.powerCycleData.sixHourCycle, itemData.actualPowerOutput);
        }

        if (itemData.secondCycleCount == 120)
        {
            itemData.secondCycleCount = 0;
        }
        if (itemData.minuteCycleCount == 120)
        {
            itemData.minuteCycleCount = 0;
        }
        if (itemData.hourCycleCount == 120)
        {
            itemData.hourCycleCount = 0;
        }
    }
    public void UpdateBuildingCycles(BuildingItemData itemData)
    {
        ShiftAndFillCycle(itemData.powerConsumptionCycleData.secondCycle, itemData.actualPowerConsumption);
        itemData.secondCycleCount++;
    }
    private void ShiftAndFillCycle(float[] cycleArray, float value)
    {
        Array.Copy(cycleArray, 0, cycleArray, 1, cycleArray.Length - 1);
        cycleArray[0] = value;
    }

    public void AddToItemArray(string itemType, GameObject item)
    {
        item.transform.SetParent(transform);
        // Check if the item type already exists in the buildingArrays dictionary
        if (buildingArrays.ContainsKey(itemType))
        {
            // Update the existing item

            // Get the existing array of items for the item type
            GameObject[] itemArray = buildingArrays[itemType];

            // Create a new array with increased length to accommodate the new item
            GameObject[] newArray = new GameObject[itemArray.Length + 1];

            // Copy the existing items to the new array
            itemArray.CopyTo(newArray, 0);

            // Add the new item to the new array
            newArray[newArray.Length - 1] = item;

            // Update the itemArray reference in the dictionary
            buildingArrays[itemType] = newArray;
        }
        else
        {
            // Create a new array with a single item
            GameObject[] itemArray = new GameObject[] { item };

            // Add the item array to the dictionary with the item type as the key
            buildingArrays.Add(itemType, itemArray);
        }
    }

    public void RemoveFromItemArray(string itemType, GameObject item)
    {
        // Check if the item type exists in the buildingArrays dictionary
        if (buildingArrays.ContainsKey(itemType))
        {
            // Get the array of items for the item type
            GameObject[] itemArray = buildingArrays[itemType];

            // Find the index of the item in the array
            int index = Array.IndexOf(itemArray, item);

            if (index >= 0)
            {
                // Create a new array with reduced length
                GameObject[] newArray = new GameObject[itemArray.Length - 1];

                // Copy the items before the removed item
                Array.Copy(itemArray, 0, newArray, 0, index);

                // Copy the items after the removed item
                Array.Copy(itemArray, index + 1, newArray, index, itemArray.Length - index - 1);

                // Update the itemArray reference in the dictionary
                buildingArrays[itemType] = newArray;
            }
        }
    }
    public void ShowFilteredItems()
    {
        ShowItems(ShowBuildingTypes);
    }
    public void ShowItems(string itemType)
    {
        bool showAllTypes = itemType == "ALL";

        foreach (var kvp in buildingArrays)
        {
            string product = kvp.Key;
            GameObject[] itemArray = kvp.Value;

            foreach (GameObject item in itemArray)
            {
                BuildingItemData itemData = item.GetComponent<BuildingItemData>();

                bool showItem = showAllTypes || itemType == itemData.buildingType;

                item.SetActive(showItem);
            }
        }
    }
}
