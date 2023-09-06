using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using ItemManagement;

public class BuildingBarPlanet0 : MonoBehaviour {


    public CoroutineManager coroutineManager;
    public InventoryManager inventoryManager;


    public bool StartCollectPlants()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[0] == true)
        {
            coroutineManager.StartCoroutine("StopCollectPlants");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[0] = true;
            coroutineManager.StartCoroutine("CollectPlants");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[0] = true;
            coroutineManager.StartCoroutine("CollectPlants");
        }
        return true;
    }

    public bool StartCollectWater()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[1] == true)
        {
            coroutineManager.StartCoroutine("StopCollectWater");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[1] = true;
            coroutineManager.StartCoroutine("CollectWater");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[1] = true;
            coroutineManager.StartCoroutine("CollectWater");
        }
        return true;
    }

    public bool StartCollectBiofuel()
    {
        RefreshResourceMap();
        float plants = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC");
        if (CoroutineManager.AllCoroutineBooleans[2] == true)
        {
            coroutineManager.StartCoroutine("StopCollectBiofuel");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (plants >= 20)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[2] = true;
                coroutineManager.StartCoroutine("CollectBiofuel");
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (plants >= 20)
        {
            CoroutineManager.AllCoroutineBooleans[2] = true;
            coroutineManager.StartCoroutine("CollectBiofuel");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool StartWaterBottle()
    {
        RefreshResourceMap();
        float water = inventoryManager.GetItemQuantity("Water", "BASIC");
        if (CoroutineManager.AllCoroutineBooleans[3] == true)
        {
            coroutineManager.StartCoroutine("StopCollectDistilledWater");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (water >= 50)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[3] = true;
                coroutineManager.StartCoroutine("CollectDistilledWater");
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (water >= 50)
        {
            CoroutineManager.AllCoroutineBooleans[3] = true;
            coroutineManager.StartCoroutine("CollectDistilledWater");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool StartCreateBatteryCore()
    {
        RefreshResourceMap();
        float biofuel = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED");
        if (CoroutineManager.AllCoroutineBooleans[6] == true)
        {
            coroutineManager.StartCoroutine("StopCreateBatteryCore");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (biofuel >= 4)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[6] = true;
                coroutineManager.StartCoroutine("CreateBatteryCore");
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (biofuel >= 4)
        {
            CoroutineManager.AllCoroutineBooleans[6] = true;
            coroutineManager.StartCoroutine("CreateBatteryCore");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool StartCreateBattery()
    {
        RefreshResourceMap();
        float batteryCore = inventoryManager.GetItemQuantity("BatteryCore", "ENHANCED");
        float biofuel = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED");

        if (CoroutineManager.AllCoroutineBooleans[4] == true)
        {
            coroutineManager.StartCoroutine("StopCreateBattery");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (batteryCore >= 1 && biofuel >= 1)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[4] = true;
                coroutineManager.StartCoroutine("CreateBattery");
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (batteryCore >= 1 && biofuel >= 1)
        {
            CoroutineManager.AllCoroutineBooleans[4] = true;
            coroutineManager.StartCoroutine("CreateBattery");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool StartCollectWood()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[7] == true)
        {
            coroutineManager.StartCoroutine("StopCollectWood");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[7] = true;
            coroutineManager.StartCoroutine("CollectWood");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[7] = true;
            coroutineManager.StartCoroutine("CollectWood");
        }
        return true;
    }
    public bool StartCollectIronOre()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[8] == true)
        {
            coroutineManager.StartCoroutine("StopCollectIronOre");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[8] = true;
            coroutineManager.StartCoroutine("CollectIronOre");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[8] = true;
            coroutineManager.StartCoroutine("CollectIronOre");
        }
        return true;
    }

    public bool StartCollectCoal()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[9] == true)
        {
            coroutineManager.StartCoroutine("StopCollectCoal");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[9] = true;
            coroutineManager.StartCoroutine("CollectCoal");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[9] = true;
            coroutineManager.StartCoroutine("CollectCoal");
        }
        return true;
    }

    public bool StartCreateIronBeam()
    {
        RefreshResourceMap();
        float ironOre = inventoryManager.GetItemQuantity("IronOre", "BASIC");
        float coal = inventoryManager.GetItemQuantity("Coal", "BASIC");

        if (CoroutineManager.AllCoroutineBooleans[10] == true)
        {
            coroutineManager.StartCoroutine("StopCreateIronBeam");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (ironOre >= 6 && coal >= 2)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[10] = true;
                coroutineManager.StartCoroutine("CreateIronBeam");
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (ironOre >= 6 && coal >= 2)
        {
            CoroutineManager.AllCoroutineBooleans[10] = true;
            coroutineManager.StartCoroutine("CreateIronBeam");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool StartCreateBiofuelGenerator()
    {
        RefreshResourceMap();
        float ironBeam = inventoryManager.GetItemQuantity("IronBeam", "PROCESSED");
        float wood = inventoryManager.GetItemQuantity("Wood", "BASIC");

        if (CoroutineManager.AllCoroutineBooleans[11] == true)
        {
            coroutineManager.StartCoroutine("StopCreateBiofuelGenerator");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (ironBeam >= 4 && wood >= 4)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[11] = true;
                coroutineManager.StartCoroutine("CreateBiofuelGenerator");
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (ironBeam >= 4 && wood >= 4)
        {
            CoroutineManager.AllCoroutineBooleans[11] = true;
            coroutineManager.StartCoroutine("CreateBiofuelGenerator");
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RefreshResourceMap()
    {
        coroutineManager.InitializeResourceMap();
    }

  }
