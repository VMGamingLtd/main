using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class GlobalCalculator : MonoBehaviour
{
    public static int days = 0;
    public static int hours = 0;
    public static int minutes = 0;
    public static int seconds = 0;
    private float timer;
    public static bool isPlayerInBiologicalBiome = true;
    public GameObject NoEnergy;
    private bool isEnergyImageVisible = false;
    public EquipmentManager equipmentManager;
    public InventoryManager inventoryManager;

    public SaveManager saveManager;

    // current date display
    public TextMeshProUGUI DateDisplay;
    public RotateImage rotateImage;
    public TextMeshProUGUI[] PlayerNeeds;

    public static bool GameStarted = false;

    private const float delayTime = 1f;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (GlobalCalculator.GameStarted == true)
        {
            timer += Time.deltaTime;
            if (timer >= delayTime)
            {
                timer -= delayTime;
                seconds++;

                if (seconds > 59)
                {
                    seconds = 0;
                    minutes++;
                    saveManager.SaveToJsonFile();
                    StartCoroutine(rotateImage.RotateOverTime(0.5f));
                }
                if (minutes > 59)
                {
                    minutes = 0;
                    hours++;
                }
                if (hours > 24)
                {
                    hours = 0;
                    days++;
                }
                UpdateEverySecond();
            }
        }
    }

    public void UpdateEverySecond()
    {
        DateDisplay.text = $"{GlobalCalculator.days.ToString("00")}:{GlobalCalculator.hours.ToString("00")}:{GlobalCalculator.minutes.ToString("00")}:{GlobalCalculator.seconds.ToString("00")}";

        bool isAirBreathable = IsBreathableAir();
        bool isMinutesBelowThreshold = IsTimeBelowThreshold(PlayerResources.PlayerOxygen, 5);

        if (!isAirBreathable)
        {
            if (PlayerResources.PlayerOxygen != "00:00:00:00")
            {
                PlayerResources.ReduceCurrentResourceTime(ref PlayerResources.PlayerOxygen, 0, 0, 0, 1);
            }
        }

        if (PlayerResources.PlayerHunger != "00:00:00:00")
        {
            PlayerResources.ReduceCurrentResourceTime(ref PlayerResources.PlayerWater, 0, 0, 0, 1);
        }

        if (PlayerResources.PlayerEnergy == "00:00:00:00")
        {
            isEnergyImageVisible = !isEnergyImageVisible; // Toggle the visibility state
            NoEnergy.SetActive(isEnergyImageVisible); // Set the visibility state
            PlayerNeeds[2].text = "ERROR";
            PlayerNeeds[2].color = Color.red;
        }
        else
        {
            isEnergyImageVisible = false;
            NoEnergy.SetActive(false);
            PlayerResources.ReduceCurrentResourceTime(ref PlayerResources.PlayerEnergy, 0, 0, 0, 1);
            if (EquipmentManager.slotEquipped[7] == true)
            {
                equipmentManager.ReduceEnergyTimer();
            }
            PlayerNeeds[2].text = PlayerResources.GetCurrentResourceTime(PlayerResources.PlayerEnergy);
            PlayerNeeds[2].color = IsTimeBelowThreshold(PlayerResources.PlayerEnergy, 5) ? Color.yellow : Color.white;
        }

        if (PlayerResources.PlayerHunger != "00:00:00:00")
        {
            PlayerResources.ReduceCurrentResourceTime(ref PlayerResources.PlayerHunger, 0, 0, 0, 1);
        }

        PlayerNeeds[0].color = isAirBreathable ? Color.green : (isMinutesBelowThreshold ? Color.red : Color.white);
        PlayerNeeds[0].text = isAirBreathable ? "INFINITE" : PlayerResources.GetCurrentResourceTime(PlayerResources.PlayerOxygen);
        PlayerNeeds[1].color = IsTimeBelowThreshold(PlayerResources.PlayerWater, 5) ? Color.yellow : PlayerNeeds[1].color;
        PlayerNeeds[1].text = PlayerResources.GetCurrentResourceTime(PlayerResources.PlayerWater);
        PlayerNeeds[3].color = IsTimeBelowThreshold(PlayerResources.PlayerHunger, 5) ? Color.yellow : PlayerNeeds[3].color;
        PlayerNeeds[3].text = PlayerResources.GetCurrentResourceTime(PlayerResources.PlayerHunger);
    }

    private bool IsTimeBelowThreshold(string time, int minutesThreshold)
    {
        string[] timeParts = time.Split(':');
        int minutes = int.Parse(timeParts[2]);

        return minutes < minutesThreshold;
    }

    public bool IsBreathableAir()
    {
        return GlobalCalculator.isPlayerInBiologicalBiome;
    }
}




