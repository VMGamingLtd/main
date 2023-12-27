using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TranslationManager translationManager;
    public TrackManager trackManager;

    private LineRendererController lineRendererController;

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
        lineRendererController = GameObject.Find("PlanetParent/StartPlanet/Player").GetComponent<LineRendererController>();
    }

    /// <summary>
    /// Global time calculation of the whole project
    /// </summary>
    /// <result> seconds </result>
    void Update()
    {
        if (GameStarted == true)
        {
            timer += Time.deltaTime;
            if (timer >= delayTime)
            {
                timer -= delayTime;
                seconds++;

                if (seconds == 0 || seconds == 5 || seconds == 10 || seconds == 15 || seconds == 20 || seconds == 25 || seconds == 30 || seconds == 35 ||
                    seconds == 40 || seconds == 45 || seconds == 50 || seconds == 55)
                {
                    saveManager.TestSaveGameDataOnServer();
                }

                if (seconds > 59)
                {
                    seconds = 0;
                    minutes++;
                    UpdatePlayerConsumption();
                    //weatherManager.SetUVAmount();
                    //StartCoroutine(rotateImage.RotateOverTime(0.5f));
                    //daylightSemicircle.RunAllCoroutines();
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
    }
    public void RecalculateInventorySlots()
    {
        inventoryManager.CalculateInventorySlots();
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
            PlayerNeeds[1].text = translationManager.Translate("ERROR");
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
            PlayerNeeds[2].text = translationManager.Translate("ERROR");
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
            PlayerNeeds[3].text = translationManager.Translate("ERROR");
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
            PlayerNeeds[4].text = translationManager.Translate("ERROR");
        }
        else
        {
            NoFood.SetActive(false);
            PlayerNeeds[4].text = PlayerResources.PlayerHunger.ToString() + "/m";
        }
    }
    public void UpdateEverySecond()
    {
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

        if (PlayerResources.PlayerMovement)
        {
            if (PlayerResources.PlayerCurrentTravelProgress == 100)
            {
                PlayerResources.PlayerMovement = false;
                trackManager.ResetAllData();
                trackManager.Loader.SetActive(false);
                return;
            }
            trackManager.Loader.SetActive(true);
            trackManager.UpdateTravelSpeed(PlayerResources.PlayerMovementSpeed);
            trackManager.CurrentProgress.text = PlayerResources.PlayerCurrentTravelProgress.ToString("F2") + "%";
            trackManager.UpdateArrivalTime(PlayerResources.PlayerRemainingTravelTime);
            trackManager.UpdateDistance(PlayerResources.PlayerRemainingDistance);


        }
    }

    public bool IsBreathableAir()
    {
        return isPlayerInBiologicalBiome;
    }
}




