using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using ItemManagement;

public class BuildingBarPlanet0 : MonoBehaviour {


    public CoroutineManager coroutineManager;
    public Animation errorMessage;
    public InventoryManager inventoryManager;

    public void StartCollectPlants()
    {
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
        int plants = inventoryManager.GetItemQuantity("Plants", "RAW");
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
        int water = inventoryManager.GetItemQuantity("Water", "RAW");
        if (CoroutineManager.AllCoroutineBooleans[3] == true)
        {
            coroutineManager.StartCoroutine("StopCollectWaterBottle");
        }
        else if (CoroutineManager.CheckForTrueValues())
        {
            if (water >= 50)
            {
                coroutineManager.StopRunningCoroutine();
                CoroutineManager.AllCoroutineBooleans[3] = true;
                coroutineManager.StartCoroutine("CollectWaterBottle");
            }
            else
            {
                StartCoroutine(errorCoroutine());
            }
        }
        else if (water >= 50)
        {
            CoroutineManager.AllCoroutineBooleans[3] = true;
            coroutineManager.StartCoroutine("CollectWaterBottle");
        }
        else
        {
            StartCoroutine(errorCoroutine());
        }
    }

    public void StartCreateBattery()
    {
        int biofuel = inventoryManager.GetItemQuantity("Biofuel", "INTERMEDIATE");

        if (biofuel >= 3)
        {
            coroutineManager.StartCoroutine("CreateBattery");
        }
        else
        {
            StartCoroutine(errorCoroutine());
        }
    }

    IEnumerator errorCoroutine()
    {
        errorMessage.Play("ErrorMessage");
        yield return new WaitForSeconds(1.4f);
    }

  }
