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

    public void StopRunningCoroutine()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == IndexNumber);
        StartCoroutine("StopRunningCreateRecipe", recipeData);
    }

    IEnumerator WaitForLoadingBar()
    {
        //translationManager.PushTextsIntoFile();

        bool registereduser = registeredUser;
        GlobalCalculator.GameStarted = false;
        yield return new WaitForSeconds(2.0f);

        if (registeredUser == false)
        {
            loginMenu.SetActive(true);
            loadingBar.SetActive(false);
            saveSlots.SetActive(false);
        }
        else
        {
            saveSlots.SetActive(true);
            loadingBar.SetActive(false);
            loginMenu.SetActive(false);
        }
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
        Level.ResetCurrentResource(ref Level.PlayerCurrentExp);
        int result = playerMaxExp * 2;
        Level.AddCurrentResource(ref Level.PlayerMaxExp, result);
        maxExpText.text = result.ToString();
        Level.AddCurrentResource(ref Level.PlayerLevel, 1);
        levelText.text = Level.GetCurrentResource(ref Level.PlayerLevel).ToString();
        Level.AddCurrentResource(ref Level.StatPoints, 1);
        Level.AddCurrentResource(ref Level.SkillPoints, 1);
        statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
        skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
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
                bool isQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[i].name, recipeDataChildList[i].product, recipeDataChildList[i].quantity);
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
            float fillTimePlanet0bb = recipeData.productionTime;
            AllCoroutineBooleans[recipeData.index] = true;
            string searchString = recipeData.recipeName + "/FillBckg/FillBar";
            GameObject fillBarObject = RecipeList.transform.Find(searchString).gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            if (recipeData.childData.Count > 0)
            {
                List<ChildData> recipeDataChildList = recipeData.childData;
                for (int i = 0; i < recipeDataChildList.Count; i++)
                {
                    inventoryManager.ReduceItemQuantity(recipeDataChildList[i].name, recipeDataChildList[i].product, recipeDataChildList[i].quantity);
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


            Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
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
                itemCreator.CreateItem(recipeData.index);
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
