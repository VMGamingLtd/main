using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class GlobalCalculator : MonoBehaviour
{
    private float timer;
    public static int days = 0;
    public static int hours = 0;
    public static int minutes = 0;
    public static int seconds = 0;

    public static float EnergyTimer;
    public Image EnergyFill;

    public static bool isPlayerInBiologicalBiome = true;

    public GameObject NoEnergy;
    public GameObject NoWater;
    public GameObject NoOxygen;
    public GameObject NoFood;

    public EquipmentManager equipmentManager;
    public InventoryManager inventoryManager;
    public BuildingManager buildingManager;

    public SaveManager saveManager;
    public WeatherManager weatherManager;
    public DaylightSemicircle daylightSemicircle;

    // current date display
    public TextMeshProUGUI DateDisplay;
    public RotateImage rotateImage;
    public TextMeshProUGUI[] PlayerNeeds;

    public static bool GameStarted = false;

    private const float delayTime = 1f;
    public TextMeshProUGUI timeText;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (GameStarted == true)
        {
            timer += Time.deltaTime;
            if (timer >= delayTime)
            {
                timer -= delayTime;
                seconds++;

                if (seconds == 10 || seconds == 20 || seconds == 30 || seconds == 40 || seconds  == 50 )
                {
                    saveManager.TestSaveGameDataOnServer();
                }

                if (seconds > 59)
                {
                    seconds = 0;
                    minutes++;
                    UpdatePlayerConsumption();
                    saveManager.TestSaveGameDataOnServer();
                    weatherManager.SetUVAmount();
                    StartCoroutine(rotateImage.RotateOverTime(0.5f));
                    daylightSemicircle.RunAllCoroutines();
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
                buildingManager.UpdateBuildingCyclesForAllBuildings();
            }
        }
    }
    public void DeductPlayerConsumption()
    {
        equipmentManager.DeductFromEquip();
        /*
        // Oxygen need deduction based on what player has equipped and if player is in biological
        bool isAirBreathable = IsBreathableAir();
        if (!isAirBreathable)
        {
            if (EquipmentManager.slotEquippedName[6] == "OxygenTank")
            {
                float resouceQuantity1 = inventoryManager.GetItemQuantity("OxygenTank", "ASSEMBLED");
                if (resouceQuantity1 > PlayerResources.PlayerOxygen)
                {
                    inventoryManager.ReduceItemQuantity("OxygenTank", "ASSEMBLED", PlayerResources.PlayerOxygen);
                }
            }
        }

        // Energy need deduction based on what player has equipped
        if (EquipmentManager.slotEquippedName[5] == "Battery")
        {

            float resouceQuantity1 = inventoryManager.GetItemQuantity("Battery", "ASSEMBLED");
            if (resouceQuantity1 > PlayerResources.PlayerEnergy)
            {
                inventoryManager.ReduceItemQuantity("Battery", "ASSEMBLED", PlayerResources.PlayerEnergy);
            }
        }


        // Water need deduction based on what player has equipped
        if (EquipmentManager.slotEquippedName[7] == "DistilledWater")
        {
            float resouceQuantity2 = inventoryManager.GetItemQuantity("DistilledWater", "PROCESSED");
            if (resouceQuantity2 > PlayerResources.PlayerWater)
            {
                inventoryManager.ReduceItemQuantity("DistilledWater", "PROCESSED", PlayerResources.PlayerWater);
            }
        }

        // Hunger need deduction based on what player has equipped
        if (EquipmentManager.slotEquippedName[8] == "FibrousLeaves")
        {
            float resouceQuantity3 = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC");
            if (resouceQuantity3 > PlayerResources.PlayerHunger)
            {
                inventoryManager.ReduceItemQuantity("FibrousLeaves", "BASIC", PlayerResources.PlayerHunger);
            }
        }*/
    }
    public void UpdatePlayerConsumption()
    {
        bool isAirBreathable = IsBreathableAir();
        // oxygen need check
        if (isAirBreathable)
        {
            PlayerNeeds[1].text = "-";
            NoOxygen.SetActive(false);
        }
        else if (EquipmentManager.slotEquipped[6] == false)
        {
            NoOxygen.SetActive(true);
            PlayerNeeds[1].text = TranslateErrorMsg();
        }
        else
        {
            NoOxygen.SetActive(false);
            PlayerNeeds[1].text = PlayerResources.PlayerOxygen.ToString() + "/m";
        }

        // water need check
        if (EquipmentManager.slotEquipped[7] == false)
        {
            NoWater.SetActive(true);
            PlayerNeeds[2].text = TranslateErrorMsg();
        }
        else
        {
            NoWater.SetActive(false);
            PlayerNeeds[2].text = PlayerResources.PlayerWater.ToString() + "/m";
        }

        // power need check
        if (EquipmentManager.slotEquipped[5] == false)
        {
            NoEnergy.SetActive(true);
            PlayerNeeds[3].text = TranslateErrorMsg();
        }
        else
        {
            NoEnergy.SetActive(false);
            PlayerNeeds[3].text = PlayerResources.PlayerEnergy.ToString("F3", CultureInfo.InvariantCulture) + "/m";
        }

        // hunger need check
        if (EquipmentManager.slotEquipped[8] == false)
        {
            NoFood.SetActive(true);
            PlayerNeeds[4].text = TranslateErrorMsg();
        }
        else
        {
            NoFood.SetActive(false);
            PlayerNeeds[4].text = PlayerResources.PlayerHunger.ToString() + "/m";
        }
    }
    public string TranslateErrorMsg()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            return "ERROR";
        }
        else if (Application.systemLanguage == SystemLanguage.Russian)
        {
            return "Ошибка";
        }
        else if (Application.systemLanguage == SystemLanguage.Chinese)
        {
            return "错误";
        }
        else if (Application.systemLanguage == SystemLanguage.Slovak)
        {
            return "Chyba";
        }
        else
        {
            return "ERROR";
        }
    }
    public void UpdateEverySecond()
    {
        //DateDisplay.text = $"{GlobalCalculator.days.ToString("00")}:{GlobalCalculator.hours.ToString("00")}:{GlobalCalculator.minutes.ToString("00")}:{GlobalCalculator.seconds.ToString("00")}";
        DateTime currentTime = DateTime.Now;
        string timeString = currentTime.ToString("HH:mm:ss");
        timeText.text = timeString;
        bool isAirBreathable = IsBreathableAir();

        /*if (EquipmentManager.slotEquipped[5] == true)
        {
            if (EnergyTimer >= 1)
            {
                EnergyTimer = 0;
            }
            else
            {
                EnergyTimer += 0.0166f;
            }
            EnergyFill.fillAmount = EnergyTimer;
        }*/
        if (EquipmentManager.slotEquipped[5] == true || (EquipmentManager.slotEquipped[6] == true) || (EquipmentManager.slotEquipped[7] == true) || (EquipmentManager.slotEquipped[8] == true))
        {
            DeductPlayerConsumption();
        }

    }

    public bool IsBreathableAir()
    {
        return isPlayerInBiologicalBiome;
    }
}




