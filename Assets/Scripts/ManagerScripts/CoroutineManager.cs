using Cysharp.Threading.Tasks;
using Gaos.GameData;
using Gaos.Routes.Model.GameDataJson;
using ItemManagement;
using Newtonsoft.Json;
using RecipeManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class CoroutineManager : MonoBehaviour
{
    public ItemCreator itemCreator;
    public RecipeCreator recipeCreator;
    public TranslationManager translationManager;
    public InventoryManager inventoryManager;
    public BuildingIncrementor buildingIncrementor;
    public ProductionCreator productionCreator;
    public StatsManager statsManager;
    public SaveManager saveManager;
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
    private TextMeshProUGUI textObject;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currentExpText;
    public TextMeshProUGUI maxExpText;
    public TextMeshProUGUI skillPointsText;
    public TextMeshProUGUI statPointsText;
    public Image ExpBar;

    // data for server
    public static bool registeredUser = false;
    public static bool manualProduction = false;
    public static bool[] AllCoroutineBooleans = new bool[15];
    public static int IndexNumber;

    public Image imageToFill;
    private float targetFillAmountPlanet0bb = 1f;
    private float currentFillAmountPlanet0bb = 0f;

    // after production is finished add +1 to the counter
    public Dictionary<string, TextMeshProUGUI[]> resourceTextMap;
    public Dictionary<string, TextMeshProUGUI[]> productionTimeTextMap;
    public Dictionary<string, TextMeshProUGUI[]> materialCostTextMap;
    public Dictionary<string, TextMeshProUGUI[]> productionOutcomeTextMap;

    // resource count texts
    public TextMeshProUGUI[] FibrousLeavesTexts;
    public TextMeshProUGUI[] WaterTexts;
    public TextMeshProUGUI[] BiofuelTexts;
    public TextMeshProUGUI[] DistilledWaterTexts;
    public TextMeshProUGUI[] BatteryTexts;
    public TextMeshProUGUI[] BatteryCoreTexts;
    public TextMeshProUGUI[] PlanetStatsTexts;
    public TextMeshProUGUI[] SteamTexts;
    public TextMeshProUGUI[] WoodTexts;
    public TextMeshProUGUI[] IronOreTexts;
    public TextMeshProUGUI[] CoalTexts;
    public TextMeshProUGUI[] IronBeamTexts;
    public TextMeshProUGUI[] BiofuelGeneratorTexts;
    public TextMeshProUGUI[] IronSheetTexts;
    public TextMeshProUGUI[] IronRodTexts;

    // production time texts
    public TextMeshProUGUI[] FibrousLeavesProductionTexts;
    public TextMeshProUGUI[] WaterProductionTexts;
    public TextMeshProUGUI[] BiofuelProductionTexts;
    public TextMeshProUGUI[] DistilledWaterProductionTexts;
    public TextMeshProUGUI[] BatteryProductionTexts;
    public TextMeshProUGUI[] BatteryCoreProductionTexts;
    public TextMeshProUGUI[] OxygenTankProductionTexts;
    public TextMeshProUGUI[] SteamProductionTexts;
    public TextMeshProUGUI[] WoodProductionTexts;
    public TextMeshProUGUI[] IronOreProductionTexts;
    public TextMeshProUGUI[] CoalProductionTexts;
    public TextMeshProUGUI[] IronBeamProductionTexts;
    public TextMeshProUGUI[] BiofuelGeneratorProductionTexts;
    public TextMeshProUGUI[] IronSheetProductionTexts;
    public TextMeshProUGUI[] IronRodProductionTexts;

    // material cost texts
    public TextMeshProUGUI[] FibrousLeavesMaterialCostTexts;
    public TextMeshProUGUI[] WaterMaterialCostTexts;
    public TextMeshProUGUI[] BiofuelMaterialCostTexts;
    public TextMeshProUGUI[] DistilledWaterMaterialCostTexts;
    public TextMeshProUGUI[] BatteryMaterialCostTexts;
    public TextMeshProUGUI[] BatteryCoreMaterialCostTexts;
    public TextMeshProUGUI[] OxygenTankMaterialCostTexts;
    public TextMeshProUGUI[] SteamMaterialCostTexts;
    public TextMeshProUGUI[] WoodMaterialCostTexts;
    public TextMeshProUGUI[] IronOreMaterialCostTexts;
    public TextMeshProUGUI[] CoalMaterialCostTexts;
    public TextMeshProUGUI[] IronBeamMaterialCostTexts;
    public TextMeshProUGUI[] BiofuelGeneratorMaterialCostTexts;
    public TextMeshProUGUI[] IronSheetMaterialCostTexts;
    public TextMeshProUGUI[] IronRodMaterialCostTexts;

    // production outcome texts
    public TextMeshProUGUI[] FibrousLeavesOutcomeTexts;
    public TextMeshProUGUI[] WaterOutcomeTexts;
    public TextMeshProUGUI[] BiofuelOutcomeTexts;
    public TextMeshProUGUI[] DistilledWaterOutcomeTexts;
    public TextMeshProUGUI[] BatteryOutcomeTexts;
    public TextMeshProUGUI[] BatteryCoreOutcomeTexts;
    public TextMeshProUGUI[] OxygenTankOutcomeTexts;
    public TextMeshProUGUI[] SteamOutcomeTexts;
    public TextMeshProUGUI[] WoodOutcomeTexts;
    public TextMeshProUGUI[] IronOreOutcomeTexts;
    public TextMeshProUGUI[] CoalOutcomeTexts;
    public TextMeshProUGUI[] IronBeamOutcomeTexts;
    public TextMeshProUGUI[] BiofuelGeneratorOutcomeTexts;
    public TextMeshProUGUI[] IronSheetOutcomeTexts;
    public TextMeshProUGUI[] IronRodOutcomeTexts;

    private void CheckSlotData()
    {
        StartCoroutine(UserGameDataGet.Get(1, CallSaveSlot1Data));
    }

    private void CallSaveSlot1Data(UserGameDataGetResponse response)
    {
        if (response != null)
        {
            if (response.GameDataJson != null)
            {
                SaveDataModel gameData = JsonConvert.DeserializeObject<SaveDataModel>(response.GameDataJson);
                string saveName = gameData.username;
                int seconds = gameData.seconds;
                int minutes = gameData.minutes;
                int hours = gameData.hours;
                string description = hours.ToString() + "h " + minutes.ToString() + "m " + seconds.ToString() + "s ";


                saveSlot1Title.text = saveName;
                saveSlot1Desc.text = description;

                saveSlots.GetComponent<CanvasGroupFaderIn>().FadeInObject();
                legal.GetComponent<CanvasGroupFaderIn>().FadeInObject();
            }
            saveSlots.GetComponent<CanvasGroupFaderIn>().FadeInObject();
            legal.GetComponent<CanvasGroupFaderIn>().FadeInObject();
        }
        else
        {
            saveSlots.GetComponent<CanvasGroupFaderIn>().FadeInObject();
            legal.GetComponent<CanvasGroupFaderIn>().FadeInObject();
        }
    }
    public void InitializeResourceMap()
    {
        resourceTextMap = new Dictionary<string, TextMeshProUGUI[]>
        {
            { "FibrousLeaves", FibrousLeavesTexts },
            { "Water", WaterTexts },
            { "Biofuel", BiofuelTexts },
            { "DistilledWater", DistilledWaterTexts },
            { "BatteryCore", BatteryCoreTexts },
            { "PlanetStats", PlanetStatsTexts },
            { "Battery", BatteryTexts },
            { "Steam", SteamTexts },
            { "Wood", WoodTexts },
            { "IronOre", IronOreTexts },
            { "Coal", CoalTexts },
            { "IronBeam", IronBeamTexts },
            { "BiofuelGenerator", BiofuelGeneratorTexts },
            { "IronSheet", IronSheetTexts },
            { "IronRod", IronRodTexts }
        };
    }
    public void InitializeProductionTimeMap()
    {
        productionTimeTextMap = new Dictionary<string, TextMeshProUGUI[]>
        {
            { "FibrousLeaves", FibrousLeavesProductionTexts },
            { "Water", WaterProductionTexts },
            { "Biofuel", BiofuelProductionTexts },
            { "DistilledWater", DistilledWaterProductionTexts },
            { "BatteryCore", BatteryCoreProductionTexts },
            { "Battery", BatteryProductionTexts },
            { "OxygenTank", OxygenTankProductionTexts },
            { "Steam", SteamProductionTexts },
            { "Wood", WoodProductionTexts },
            { "IronOre", IronOreProductionTexts },
            { "Coal", CoalProductionTexts },
            { "IronBeam", IronBeamProductionTexts },
            { "BiofuelGenerator", BiofuelGeneratorProductionTexts },
            { "IronSheet", IronSheetProductionTexts },
            { "IronRod", IronRodProductionTexts }
        };
    }

    public void InitializeMaterialCostMap()
    {
        materialCostTextMap = new Dictionary<string, TextMeshProUGUI[]>
        {
            { "FibrousLeaves", FibrousLeavesMaterialCostTexts },
            { "Water", WaterMaterialCostTexts },
            { "Biofuel", BiofuelMaterialCostTexts },
            { "DistilledWater", DistilledWaterMaterialCostTexts },
            { "BatteryCore", BatteryCoreMaterialCostTexts },
            { "Battery", BatteryMaterialCostTexts },
            { "OxygenTank", OxygenTankMaterialCostTexts },
            { "Steam", SteamMaterialCostTexts },
            { "Wood", WoodMaterialCostTexts },
            { "IronOre", IronOreMaterialCostTexts },
            { "Coal", CoalMaterialCostTexts },
            { "IronBeam", IronBeamMaterialCostTexts },
            { "BiofuelGenerator", BiofuelGeneratorMaterialCostTexts },
            { "IronSheet", IronSheetMaterialCostTexts },
            { "IronRod", IronRodMaterialCostTexts }
        };
    }

    public void InitializeProductionOutcomeMap()
    {
        productionOutcomeTextMap = new Dictionary<string, TextMeshProUGUI[]>
        {
            { "FibrousLeaves", FibrousLeavesOutcomeTexts },
            { "Water", WaterOutcomeTexts },
            { "Biofuel", BiofuelOutcomeTexts },
            { "DistilledWater", DistilledWaterOutcomeTexts },
            { "BatteryCore", BatteryCoreOutcomeTexts },
            { "Battery", BatteryOutcomeTexts },
            { "OxygenTank", OxygenTankOutcomeTexts },
            { "Steam", SteamOutcomeTexts },
            { "Wood", WoodOutcomeTexts },
            { "IronOre", IronOreOutcomeTexts },
            { "Coal", CoalOutcomeTexts },
            { "IronBeam", IronBeamOutcomeTexts },
            { "BiofuelGenerator", BiofuelGeneratorOutcomeTexts },
            { "IronSheet", IronSheetOutcomeTexts },
            { "IronRod", IronRodOutcomeTexts }
        };
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
                currentTextArray[i].text = currentResource;
            }
        }
        else
        {
            Debug.LogError($"Text array for '{consumableName}' not found.");
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
        else
        {
            Debug.LogError($"Text array for '{consumableName}' not found.");
        }
    }

    public void UpdateProductionTextsForKey(string key, float value)
    {
        if (productionTimeTextMap.ContainsKey(key))
        {
            TextMeshProUGUI[] productionTexts = productionTimeTextMap[key];

            for (int i = 0; i < productionTexts.Length; i++)
            {
                productionTexts[i].text = value.ToString();
            }
        }
        else
        {
            Debug.LogError($"Text array for '{key}' not found.");
        }
    }

    public void UpdateMaterialCostTextsForKey(string key, float value)
    {
        if (materialCostTextMap.TryGetValue(key, out TextMeshProUGUI[] currentTextArray))
        {
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                currentTextArray[i].text = value.ToString();
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
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                currentTextArray[i].text = value.ToString();
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
            if (!manualProduction) productionCreator.EnlistManualProduction(recipeData);

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
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
            /// First goal finished and switched to true.
            /// </summary>
            /// <value>true</value>
            if (GoalManager.firstGoal == false && recipeData.recipeName == "Battery")
            {
                GoalManager goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
                _ = goalManager.SetSecondGoal();
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
            }
            else
            {
                itemCreator.CreateItem(recipeData.index, recipeData.outputValue * Player.OutcomeRate);
                UpdateQuantityTexts(recipeData.recipeName, recipeData.recipeProduct);
            }
            StartCoroutine("CreateRecipe", recipeData);

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
