using Cysharp.Threading.Tasks;
using ItemManagement;
using RecipeManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineManager : MonoBehaviour
{
    // Referrences to managers
    public ItemCreator itemCreator;
    public RecipeCreator recipeCreator;
    public TranslationManager translationManager;
    public InventoryManager inventoryManager;
    public BuildingIncrementor buildingIncrementor;
    public ProductionCreator productionCreator;
    public StatsManager statsManager;
    public SaveManager saveManager;
    public Planet planet;

    public GameObject RecipeList;
    public GameObject loadingBar;
    public GameObject saveSlots;
    public TextMeshProUGUI saveSlot1Title;
    public TextMeshProUGUI saveSlot1Desc;
    public TextMeshProUGUI saveSlot2Title;
    public TextMeshProUGUI saveSlot2Desc;
    public TextMeshProUGUI saveSlot3Title;
    public TextMeshProUGUI saveSlot3Desc;
    public TextMeshProUGUI saveSlot4Title;
    public TextMeshProUGUI saveSlot4Desc;
    public GameObject loginMenu;
    public GameObject newGamePopup;
    public GameObject levelUpObject;
    public GameObject legal;
    public GameObject Slot1DeleteButton;
    public GameObject Slot2DeleteButton;
    public GameObject Slot3DeleteButton;
    public GameObject Slot4DeleteButton;

    public TimebarControl timebarControl;

    private TextMeshProUGUI textObject;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currentExpText;
    public TextMeshProUGUI maxExpText;
    public TextMeshProUGUI skillPointsText;
    public TextMeshProUGUI statPointsText;
    public TextMeshProUGUI researchPointsText;
    public Image ExpBar;

    // data for server
    public static bool registeredUser = false;
    public static bool manualProduction = false;
    public static bool autoProduction = false;
    public static bool[] AllCoroutineBooleans = new bool[36];
    public static int IndexNumber;

    public static int CurrentGameSlotId;

    public static string[] RecipeKeys =
    {
         "FibrousLeaves", "Water", "Biofuel", "DistilledWater", "Battery",
         "OxygenTank", "BatteryCore", "Steam", "IronOre", "Wood",
         "Coal", "IronBeam", "BiofuelGenerator", "IronSheet", "IronRod",
         "LatexFoam", "ProteinBeans", "BiomassLeaves", "BiomassWood", "ProteinPowder",
         "BioOil", "IronTube", "WaterPump", "FibrousPlantField", "ResearchDevice",
         "FishMeat", "AnimalMeat", "AnimalSkin", "Milk", "SilicaSand", "Limestone",
         "Clay", "Stone", "CopperOre", "SilverOre", "GoldOre", "Sulfur", "Saltpeter"
    };

    public Image imageToFill;
    private float targetFillAmountPlanet0bb = 1f;
    private float currentFillAmountPlanet0bb = 0f;

    // after production is finished add +1 to the counter
    public Dictionary<string, TextMeshProUGUI[]> resourceTextMap;
    public Dictionary<string, TextMeshProUGUI[]> productionTimeTextMap;
    public Dictionary<string, TextMeshProUGUI[]> materialCostTextMap;
    public Dictionary<string, TextMeshProUGUI[]> productionOutcomeTextMap;

    // resource count texts
    public TextMeshProUGUI[] FibrousLeavesTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] DistilledWaterTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] OxygenTankTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryCoreTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SteamTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WoodTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronOreTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CoalTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronBeamTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelGeneratorTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronSheetTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronRodTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LatexFoamTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinBeansTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassLeavesTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassWoodTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinPowderTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BioOilTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronTubeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterPumpTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FibrousPlantFieldTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ResearchDeviceTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FishMeatTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalMeatTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalSkinTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] MilkTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilicaSandTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ClayTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LimestoneTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] StoneTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CopperOreTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilverOreTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] GoldOreTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SulfurTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SaltpeterTexts = new TextMeshProUGUI[0];

    // production time texts
    public TextMeshProUGUI[] FibrousLeavesProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] DistilledWaterProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] OxygenTankProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryCoreProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SteamProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WoodProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronOreProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CoalProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronBeamProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelGeneratorProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronSheetProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronRodProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LatexFoamProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinBeansProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassLeavesProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassWoodProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinPowderProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BioOilProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronTubeProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterPumpProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FibrousPlantFieldProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ResearchDeviceProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FishMeatProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalMeatProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalSkinProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] MilkProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilicaSandProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LimestoneProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ClayProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] StoneProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CopperOreProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilverOreProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] GoldOreProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SulfurProductionTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SaltpeterProductionTexts = new TextMeshProUGUI[0];

    // material cost texts
    public TextMeshProUGUI[] FibrousLeavesMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] DistilledWaterMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] OxygenTankMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryCoreMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SteamMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WoodMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronOreMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CoalMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronBeamMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelGeneratorMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronSheetMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronRodMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LatexFoamMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinBeansMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassLeavesMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassWoodMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinPowderMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BioOilMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronTubeMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterPumpMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FibrousPlantFieldMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ResearchDeviceMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FishMeatMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalMeatMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalSkinMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] MilkMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilicaSandMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LimestoneMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ClayMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] StoneMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CopperOreMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilverOreMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] GoldOreMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SulfurMaterialCostTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SaltpeterMaterialCostTexts = new TextMeshProUGUI[0];

    // production outcome texts
    public TextMeshProUGUI[] FibrousLeavesOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] DistilledWaterOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] OxygenTankOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BatteryCoreOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SteamOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WoodOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronOreOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CoalOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronBeamOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiofuelGeneratorOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronSheetOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronRodOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LatexFoamOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinBeansOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassLeavesOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BiomassWoodOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ProteinPowderOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] BioOilOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] IronTubeOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] WaterPumpOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FibrousPlantFieldOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ResearchDeviceOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] FishMeatOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalMeatOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] AnimalSkinOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] MilkOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilicaSandOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] LimestoneOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] ClayOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] StoneOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] CopperOreOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SilverOreOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] GoldOreOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SulfurOutcomeTexts = new TextMeshProUGUI[0];
    public TextMeshProUGUI[] SaltpeterOutcomeTexts = new TextMeshProUGUI[0];

    void Start()
    {
        InitializeResourceMap();
        InitializeProductionOutcomeMap();
        InitializeProductionTimeMap();
        InitializeMaterialCostMap();
    }

    private void CheckSlotData()
    {
        SetSlotButtons();
    }

    public void DeleteSlotButton(int slotNumber)
    {
        if (GameObject.Find("MessageCanvas/MESSAGEOBJECTS").TryGetComponent<MessageObjects>(out var messageObjects))
        {
            messageObjects.DisplayMessage("DeleteSlotWindow", slotNumber);
        }
    }

    public void SetSlotButon(Gaos.Routes.Model.DeviceJson.DeviceRegisterResponseUserSlot slotData)
    {
        string description = slotData.Hours.ToString() + "h " + slotData.Minutes.ToString() + "m " + slotData.Seconds.ToString() + "s ";
        TextMeshProUGUI saveSlotTitle = null;
        TextMeshProUGUI saveSlotDesc = null;

        switch (slotData.SlotId)
        {
            case 1:
                saveSlotTitle = saveSlot1Title;
                saveSlotDesc = saveSlot1Desc;
                Slot1DeleteButton.SetActive(true);
                break;
            case 2:
                saveSlotTitle = saveSlot2Title;
                saveSlotDesc = saveSlot2Desc;
                Slot2DeleteButton.SetActive(true);
                break;
            case 3:
                saveSlotTitle = saveSlot3Title;
                saveSlotDesc = saveSlot3Desc;
                Slot3DeleteButton.SetActive(true);
                break;
            case 4:
                saveSlotTitle = saveSlot4Title;
                saveSlotDesc = saveSlot4Desc;
                Slot4DeleteButton.SetActive(true);
                break;
            default:
                Debug.LogWarning($"Invalid slot id: ${slotData.SlotId}, user: ${slotData.UserName}");
                break;
        }

        //saveSlotTitle.text = slotData.UserName;
        saveSlotTitle.text = Gaos.Context.Authentication.GetUserName();
        saveSlotDesc.text = description;
        UserName.userName = slotData.UserName;

        saveSlots.GetComponent<CanvasGroupFaderIn>().FadeInObject();
        legal.GetComponent<CanvasGroupFaderIn>().FadeInObject();
    }

    private void SetSlotButtons()
    {
        // Get all active slots from context
        Gaos.Routes.Model.DeviceJson.DeviceRegisterResponseUserSlot[] userSlots = Gaos.Context.Authentication.GetUserSlots();
        for (int i = 0; i < userSlots.Length; i++)
        {
            SetSlotButon(userSlots[i]);
        }
        saveSlots.GetComponent<CanvasGroupFaderIn>().FadeInObject();
        legal.GetComponent<CanvasGroupFaderIn>().FadeInObject();
    }

    public void InitializeResourceMap()
    {
        resourceTextMap = new Dictionary<string, TextMeshProUGUI[]>();

        foreach (string key in RecipeKeys)
        {
            FieldInfo field = GetType().GetField(key + "Texts");
            if (field != null)
            {
                TextMeshProUGUI[] texts = (TextMeshProUGUI[])field.GetValue(this);
                resourceTextMap.Add(key, texts);
            }
            else
            {
                Debug.LogError("Field not found: " + key + "Texts");
            }
        }
    }
    public void InitializeProductionTimeMap()
    {
        productionTimeTextMap = new Dictionary<string, TextMeshProUGUI[]>();

        foreach (string key in RecipeKeys)
        {
            FieldInfo field = GetType().GetField(key + "ProductionTexts");

            if (field != null)
            {
                TextMeshProUGUI[] texts = (TextMeshProUGUI[])field.GetValue(this);
                productionTimeTextMap.Add(key, texts);
            }
            else
            {
                Debug.LogError("Field not found: " + key + "ProductionTexts");
            }
        }
    }

    public void InitializeMaterialCostMap()
    {
        materialCostTextMap = new Dictionary<string, TextMeshProUGUI[]>();

        foreach (string key in RecipeKeys)
        {
            FieldInfo field = GetType().GetField(key + "MaterialCostTexts");

            if (field != null)
            {
                TextMeshProUGUI[] texts = (TextMeshProUGUI[])field.GetValue(this);
                materialCostTextMap.Add(key, texts);
            }
            else
            {
                Debug.LogError("Field not found: " + key + "MaterialCostTexts");
            }
        }
    }

    public void InitializeProductionOutcomeMap()
    {
        productionOutcomeTextMap = new Dictionary<string, TextMeshProUGUI[]>();

        foreach (string key in RecipeKeys)
        {
            FieldInfo field = GetType().GetField(key + "OutcomeTexts");

            if (field != null)
            {
                TextMeshProUGUI[] texts = (TextMeshProUGUI[])field.GetValue(this);
                productionOutcomeTextMap.Add(key, texts);
            }
            else
            {
                Debug.LogError("Field not found: " + key + "OutcomeTexts");
            }
        }
    }

    public void StopRunningCoroutine()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == IndexNumber);
        StartCoroutine("StopRunningCreateRecipe", recipeData);
    }

    /// <summary>
    /// Uses 'resourceTextMap' dictionary strings to update all respective bound text fields in UI to the current value of the item
    /// </summary>
    /// <param name="consumableName"></param>
    /// <param name="quality"></param>
    public void UpdateQuantityTexts(string consumableName, string quality)
    {
        if (resourceTextMap.TryGetValue(consumableName, out TextMeshProUGUI[] currentTextArray))
        {
            string currentResource = inventoryManager.GetItemQuantity(consumableName, quality).ToString("F2", CultureInfo.InvariantCulture);
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                if (currentResource.EndsWith(".00")) currentResource = currentResource[..^3];
                currentTextArray[i].text = currentResource;
            }
        }
    }
    public void UpdateBuildingText(string consumableName, string resourceQuantity)
    {
        if (resourceTextMap.TryGetValue(consumableName, out TextMeshProUGUI[] currentTextArray))
        {
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                currentTextArray[i].text = resourceQuantity;
            }
        }
    }

    public void UpdateProductionTextsForKey(string key, float value)
    {
        if (productionTimeTextMap.TryGetValue(key, out TextMeshProUGUI[] currentTextArray))
        {
            string currentResource = value.ToString();
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                if (currentResource.EndsWith(".00")) currentResource = currentResource[..^3];
                currentTextArray[i].text = currentResource + " s";
            }
        }
        else
        {
            Debug.LogError($"Text array for '{key}' not found.");
        }
    }

    public void UpdateMaterialCostTextsForKey(string key)
    {
        if (materialCostTextMap.TryGetValue(key, out TextMeshProUGUI[] currentTextArray))
        {
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                string currentResource = currentTextArray[i].text;
                if (float.TryParse(currentResource, out float updatedQuantity))
                {
                    updatedQuantity /= Player.MaterialCost;
                    if (Player.MaterialCost <= 0) updatedQuantity = 0;
                }
                currentResource = updatedQuantity.ToString();

                if (currentResource.EndsWith(".00")) currentResource = currentResource[..^3];
                currentTextArray[i].text = currentResource;
            }
        }
        else
        {
            Debug.LogError($"Text array for '{key}' not found.");
        }
    }

    public void UpdateProductionOutcomeTextsForKey(string key, float value)
    {
        if (productionOutcomeTextMap.TryGetValue(key, out TextMeshProUGUI[] currentTextArray))
        {
            string currentResource = value.ToString();
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                if (currentResource.EndsWith(".00")) currentResource = currentResource[..^3];
                currentTextArray[i].text = currentResource;
            }
        }
        else
        {
            Debug.LogError($"Text array for '{key}' not found.");
        }
    }

    public async UniTask ResetNewGame()
    {
        textObject = loadingBar.transform.GetComponentInChildren<TextMeshProUGUI>();
        textObject.text = translationManager.Translate("InitializingGame");
        newGamePopup.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        newGamePopup.GetComponent<Michsky.UI.Shift.ModalWindowManager>().ModalWindowIn();
        loadingBar.SetActive(false);
    }

    public IEnumerator LoadSaveSlots()
    {
        CheckSlotData();
        loadingBar.SetActive(false);
        loginMenu.SetActive(false);
        yield return null;
    }

    public static void ResetAllCoroutineBooleans()
    {
        for (int i = 0; i < AllCoroutineBooleans.Length; i++)
        {
            AllCoroutineBooleans[i] = false; // Set each element in the array to false
        }
    }

    public static bool CheckForTrueValues()
    {
        for (int i = 0; i < AllCoroutineBooleans.Length; i++)
        {
            if (AllCoroutineBooleans[i] == true)
            {
                IndexNumber = i;
                return true;
            }
        }
        return false;
    }
    public void LevelUp(int playerMaxExp)
    {
        levelUpObject.SetActive(true);
        ExpBar.fillAmount = 0f;
        currentExpText.text = "0";
        Player.ResetCurrentResource(ref Player.PlayerCurrentExp);
        int result = playerMaxExp * 2;
        Player.AddCurrentResource(ref Player.PlayerMaxExp, result);
        maxExpText.text = result.ToString();
        Player.AddCurrentResource(ref Player.PlayerLevel, 1);
        levelText.text = Player.GetCurrentResource(ref Player.PlayerLevel).ToString();
        Player.AddCurrentResource(ref Player.StatPoints, 1);
        Player.AddCurrentResource(ref Player.SkillPoints, 1);
        statPointsText.text = Player.GetCurrentResource(ref Player.StatPoints).ToString();
        skillPointsText.text = Player.GetCurrentResource(ref Player.SkillPoints).ToString();
        statsManager.ShowLevelUpButtons(true);
    }

    public void ReturnMaterials(List<ChildData> recipeDataChildList)
    {
        foreach (ChildData childData in recipeDataChildList)
        {
            itemCreator.CreateItem(childData.index, childData.quantity);
        }
    }
    public IEnumerator CreateRecipe(RecipeItemData recipeData)
    {
        bool allItemsMet = true;
        if (recipeData.childData.Count > 0)
        {
            List<ChildData> recipeDataChildList = recipeData.childData;
            for (int i = 0; i < recipeDataChildList.Count; i++)
            {
                bool isQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[i].name, recipeDataChildList[i].product, recipeDataChildList[i].quantity / Player.MaterialCost);
                if (!isQuantityMet)
                {
                    allItemsMet = false;
                    break;
                }
            }
        }
        if (allItemsMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = recipeData.productionTime / Player.ProductionSpeed;
            AllCoroutineBooleans[recipeData.index] = true;
            string searchString = recipeData.recipeName + "/FillBckg/FillBar";
            GameObject fillBarObject = RecipeList.transform.Find(searchString).gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            if (recipeData.childData.Count > 0)
            {
                List<ChildData> recipeDataChildList = recipeData.childData;
                for (int i = 0; i < recipeDataChildList.Count; i++)
                {
                    inventoryManager.ReduceItemQuantity(recipeDataChildList[i].name, recipeDataChildList[i].product, recipeDataChildList[i].quantity / Player.MaterialCost);
                    UpdateQuantityTexts(recipeDataChildList[i].name, recipeDataChildList[i].product);
                }
            }

            // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
            if (!manualProduction)
            {
                GameObject obj = productionCreator.EnlistManualProduction(recipeData);
                timebarControl = obj.GetComponent<TimebarControl>();
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                if (timebarControl != null) timebarControl.UpdateTimebar(currentFillAmountPlanet0bb);
                timer += Time.deltaTime;
                yield return null;
            }
            imageToFill.fillAmount = 0f;


            Player.AddCurrentResource(ref Player.PlayerCurrentExp, recipeData.experience);
            currentExpText.text = Player.GetCurrentResource(ref Player.PlayerCurrentExp).ToString();
            int playerCurrentExp = Player.GetCurrentResource(ref Player.PlayerCurrentExp);
            int playerMaxExp = Player.GetCurrentResource(ref Player.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            /// <summary>
            /// Second goal finished and switched to true.
            /// </summary>
            /// <value>true</value>
            if (GoalManager.secondGoal == false && recipeData.recipeName == "Battery")
            {
                GoalManager goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
                _ = goalManager.SetThirdGoal();
            }

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }

            if (recipeData.recipeProduct == "BUILDINGS")
            {
                if (recipeData.recipeName == "BiofuelGenerator")
                {
                    Planet0Buildings.Planet0BiofuelGeneratorBlueprint++;
                    buildingIncrementor.buildingCounts[0].text = Planet0Buildings.Planet0BiofuelGeneratorBlueprint.ToString();
                    UpdateBuildingText(recipeData.recipeName, Planet0Buildings.Planet0BiofuelGeneratorBlueprint.ToString());
                }
                else if (recipeData.recipeName == "WaterPump")
                {
                    Planet0Buildings.Planet0WaterPumpBlueprint++;
                    buildingIncrementor.buildingCounts[1].text = Planet0Buildings.Planet0WaterPumpBlueprint.ToString();
                    UpdateBuildingText(recipeData.recipeName, Planet0Buildings.Planet0WaterPumpBlueprint.ToString());
                }
                else if (recipeData.recipeName == "FibrousPlantField")
                {
                    Planet0Buildings.Planet0FibrousPlantFieldBlueprint++;
                    buildingIncrementor.buildingCounts[2].text = Planet0Buildings.Planet0FibrousPlantFieldBlueprint.ToString();
                    UpdateBuildingText(recipeData.recipeName, Planet0Buildings.Planet0FibrousPlantFieldBlueprint.ToString());
                }
                else if (recipeData.recipeName == "ResearchDevice")
                {
                    Planet0Buildings.Planet0ResearchDeviceBlueprint++;
                    buildingIncrementor.buildingCounts[6].text = Planet0Buildings.Planet0ResearchDeviceBlueprint.ToString();
                    UpdateBuildingText(recipeData.recipeName, Planet0Buildings.Planet0ResearchDeviceBlueprint.ToString());
                }
                buildingIncrementor.InitializeBuildingCounts();
            }
            else
            {
                itemCreator.CreateItem(recipeData.index, recipeData.outputValue * Player.OutcomeRate);

                if (recipeData.currentQuantity > 0)
                {
                    recipeData.currentQuantity -= recipeData.outputValue;

                    if (recipeData.currentQuantity < 0) { recipeData.currentQuantity = 0; }

                    foreach (var eventIcon in planet.eventObjects)
                    {
                        var component = eventIcon.GetComponent<EventIcon>();

                        if (component.RecipeGuid == recipeData.guid || component.RecipeGuid2 == recipeData.guid ||
                            component.RecipeGuid3 == recipeData.guid || component.RecipeGuid4 == recipeData.guid)
                        {
                            component.CurrentQuantity = recipeData.currentQuantity;
                            component.Component.CurrentQuantity = recipeData.currentQuantity;
                            break;
                        }
                    }
                }

                UpdateQuantityTexts(recipeData.recipeName, recipeData.recipeProduct);
            }

            if (!autoProduction && recipeCreator.queueRecipes.Count == 0)
            {
                StartCoroutine("CreateRecipe", recipeData);
            }
            else
            {
                StartCoroutine("StopCreateRecipe", recipeData);
            }

        }
        else
        {
            productionCreator.DelistManualProduction();
            AllCoroutineBooleans[recipeData.index] = false;
        }
    }
    public IEnumerator StopCreateRecipe(RecipeItemData recipeData)
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateRecipe");
        if (recipeData.childData.Count > 0)
        {
            List<ChildData> recipeDataChildList = recipeData.childData;

            // we don't return materials back if the product was the last auto produced item
            // because it can't be cancelled anyway
            if (!autoProduction)
            {
                ReturnMaterials(recipeDataChildList);
            }

            for (int i = 0; i < recipeDataChildList.Count; i++)
            {
                UpdateQuantityTexts(recipeDataChildList[i].name, recipeDataChildList[i].product);
            }
        }
        AllCoroutineBooleans[recipeData.index] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }
    public IEnumerator StopRunningCreateRecipe(RecipeDataJson recipeData)
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateRecipe");
        if (recipeData.childDataList.Count > 0)
        {
            List<ChildData> recipeDataChildList = recipeData.childDataList;
            ReturnMaterials(recipeDataChildList);
            for (int i = 0; i < recipeDataChildList.Count; i++)
            {
                UpdateQuantityTexts(recipeDataChildList[i].name, recipeDataChildList[i].product);
            }
        }
        AllCoroutineBooleans[recipeData.index] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }
}
