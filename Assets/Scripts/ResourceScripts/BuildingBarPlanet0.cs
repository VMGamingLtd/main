using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using ItemManagement;

public class BuildingBarPlanet0 : MonoBehaviour {


    public CoroutineManager coroutineManager;
    public Animation errorMessage;
    public InventoryManager inventoryManager;
    private Transform errorObjectTransform;


    public void StartCollectPlants()
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

    }

    public void StartCollectWater()
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
    }

    public void StartCollectBiofuel()
    {
        RefreshResourceMap();
        float plants = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC");
        if (CoroutineManager.AllCoroutineBooleans[2] == true)
        {
            coroutineManager.StartCoroutine("StopCollectBiofuel");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (plants >= 20)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[2] = true;
                coroutineManager.StartCoroutine("CollectBiofuel");
            }
            else
            {
                StartCoroutine(errorCoroutine());
            }
        }
        else if (plants >= 20)
        {
            CoroutineManager.AllCoroutineBooleans[2] = true;
            coroutineManager.StartCoroutine("CollectBiofuel");
        }
        else
        {
            StartCoroutine(errorCoroutine());
        }
    }
    public void StartWaterBottle()
    {
        RefreshResourceMap();
        float water = inventoryManager.GetItemQuantity("Water", "BASIC");
        if (CoroutineManager.AllCoroutineBooleans[3] == true)
        {
            coroutineManager.StartCoroutine("StopCollectDistilledWater");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (water >= 50)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[3] = true;
                coroutineManager.StartCoroutine("CollectDistilledWater");
            }
            else
            {
                StartCoroutine(errorCoroutine());
            }
        }
        else if (water >= 50)
        {
            CoroutineManager.AllCoroutineBooleans[3] = true;
            coroutineManager.StartCoroutine("CollectDistilledWater");
        }
        else
        {
            StartCoroutine(errorCoroutine());
        }
    }
    public void StartCreateBatteryCore()
    {
        RefreshResourceMap();
        float biofuel = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED");
        if (CoroutineManager.AllCoroutineBooleans[6] == true)
        {
            coroutineManager.StartCoroutine("StopCreateBatteryCore");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (biofuel >= 4)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[6] = true;
                coroutineManager.StartCoroutine("CreateBatteryCore");
            }
            else
            {
                StartCoroutine(errorCoroutine());
            }
        }
        else if (biofuel >= 4)
        {
            CoroutineManager.AllCoroutineBooleans[6] = true;
            coroutineManager.StartCoroutine("CreateBatteryCore");
        }
        else
        {
            StartCoroutine(errorCoroutine());
        }
    }

    public void StartCreateBattery()
    {
        RefreshResourceMap();
        float batteryCore = inventoryManager.GetItemQuantity("BatteryCore", "ENHANCED");
        float biofuel = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED");

        if (CoroutineManager.AllCoroutineBooleans[4] == true)
        {
            coroutineManager.StartCoroutine("StopCreateBattery");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (batteryCore >= 1 && biofuel >= 1)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[4] = true;
                coroutineManager.StartCoroutine("CreateBattery");
            }
            else
            {
                StartCoroutine(errorCoroutine());
            }
        }
        else if (batteryCore >= 1 && biofuel >= 1)
        {
            CoroutineManager.AllCoroutineBooleans[4] = true;
            coroutineManager.StartCoroutine("CreateBattery");
        }
        else
        {
            StartCoroutine(errorCoroutine());
        }
    }

    private void OnDisable()
    {
        if (errorMessage.isPlaying)
        {
            Transform errorObjectTransform = transform.Find("ErrorImage");
            if (errorObjectTransform != null)
            {
                errorObjectTransform.gameObject.SetActive(false);
            }
        }
    }

    private void RefreshResourceMap()
    {
        coroutineManager.InitializeResourceMap();
    }

    IEnumerator errorCoroutine()
    {
        errorMessage.Play("ErrorMessage");
        yield return new WaitForSeconds(1.4f);
    }

  }
