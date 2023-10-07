using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using ItemManagement;

public class BuildingBarPlanet0 : MonoBehaviour {


    public CoroutineManager coroutineManager;
    public InventoryManager inventoryManager;


    public bool CreateFibrousLeaves()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[0] == true)
        {
            coroutineManager.StartCoroutine("StopCreateFibrousLeaves");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[0] = true;
            coroutineManager.StartCoroutine("CreateFibrousLeaves");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[0] = true;
            coroutineManager.StartCoroutine("CreateFibrousLeaves");
        }
        return true;
    }

    public bool CreateWater()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[1] == true)
        {
            coroutineManager.StartCoroutine("StopCreateWater");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[1] = true;
            coroutineManager.StartCoroutine("CreateWater");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[1] = true;
            coroutineManager.StartCoroutine("CreateWater");
        }
        return true;
    }

    public bool CreateBiofuel()
    {
        RefreshResourceMap();
        float plants = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC");
        if (CoroutineManager.AllCoroutineBooleans[2] == true)
        {
            coroutineManager.StartCoroutine("StopCreateBiofuel");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (plants >= 20)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[2] = true;
                coroutineManager.StartCoroutine("CreateBiofuel");
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
            coroutineManager.StartCoroutine("CreateBiofuel");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CreateDistilledWater()
    {
        RefreshResourceMap();
        float water = inventoryManager.GetItemQuantity("Water", "BASIC");
        if (CoroutineManager.AllCoroutineBooleans[3] == true)
        {
            coroutineManager.StartCoroutine("StopCreateDistilledWater");
            return true;
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (water >= 50)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[3] = true;
                coroutineManager.StartCoroutine("CreateDistilledWater");
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
            coroutineManager.StartCoroutine("CreateDistilledWater");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CreateBatteryCore()
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

    public bool CreateBattery()
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
    public bool CreateWood()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[7] == true)
        {
            coroutineManager.StartCoroutine("StopCreateWood");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[7] = true;
            coroutineManager.StartCoroutine("CreateWood");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[7] = true;
            coroutineManager.StartCoroutine("CreateWood");
        }
        return true;
    }
    public bool CreateIronOre()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[8] == true)
        {
            coroutineManager.StartCoroutine("StopCreateIronOre");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[8] = true;
            coroutineManager.StartCoroutine("CreateIronOre");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[8] = true;
            coroutineManager.StartCoroutine("CreateIronOre");
        }
        return true;
    }

    public bool CreateCoal()
    {
        RefreshResourceMap();
        if (CoroutineManager.AllCoroutineBooleans[9] == true)
        {
            coroutineManager.StartCoroutine("StopCreateCoal");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            coroutineManager.StopRunningCoroutine();
            CoroutineManager.AllCoroutineBooleans[9] = true;
            coroutineManager.StartCoroutine("CreateCoal");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[9] = true;
            coroutineManager.StartCoroutine("CreateCoal");
        }
        return true;
    }

    public bool CreateIronBeam()
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
    public bool CreateBiofuelGenerator()
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
