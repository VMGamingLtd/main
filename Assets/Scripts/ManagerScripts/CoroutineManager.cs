using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ItemManagement;
using RecipeManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CoroutineManager : MonoBehaviour
{
    public ItemCreator itemCreator;
    public RecipeCreator recipeCreator;
    public TranslationManager translationManager;
    public InventoryManager inventoryManager;
    public BuildingIncrementor buildingIncrementor;
    public GameObject RecipeList;
    public GameObject loadingBar;
    public GameObject saveSlots;
    public GameObject loginMenu;
    public GameObject newGamePopup;
    public GameObject levelUpObject;
    private TextMeshProUGUI textObject;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI currentExpText;
    public TextMeshProUGUI maxExpText;
    public TextMeshProUGUI skillPointsText;
    public TextMeshProUGUI statPointsText;
    public Image ExpBar;

    // data for server
    public static bool registeredUser = false;
    public static bool[] AllCoroutineBooleans = new bool[12];
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

    // OnEnable is only for testing purposes! Has to be deleted in official launch!
    void OnEnable()
    {
        ResetNewGame();
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
            { "Battery", BatteryTexts },
            { "Steam", SteamTexts },
            { "Wood", WoodTexts },
            { "IronOre", IronOreTexts },
            { "Coal", CoalTexts },
            { "IronBeam", IronBeamTexts },
            { "BiofuelGenerator", BiofuelGeneratorTexts }
        };
    }

    public void StopRunningCoroutine()
    {
        if (IndexNumber == 0)
        {
            StartCoroutine("StopCollectPlants");
        }
        else if (IndexNumber == 1)
        {
            StartCoroutine("StopCollectWater");
        }
        else if (IndexNumber == 2)
        {
            StartCoroutine("StopCollectBiofuel");
        }
        else if (IndexNumber == 3)
        {
            StartCoroutine("StopCollectDistilledWater");
        }
        else if (IndexNumber == 4)
        {
            StartCoroutine("StopCreateBattery");
        }
        else if (IndexNumber == 5)
        {
            StartCoroutine("StopCreateOxygenTanks");
        }
        else if (IndexNumber == 6)
        {
            StartCoroutine("StopCreateBatteryCore");
        }
        else if (IndexNumber == 7)
        {
            StartCoroutine("StopCollectWood");
        }
        else if (IndexNumber == 8)
        {
            StartCoroutine("StopCollectIronOre");
        }
        else if (IndexNumber == 9)
        {
            StartCoroutine("StopCollectCoal");
        }
        else if (IndexNumber == 10)
        {
            StartCoroutine("StopBiofuelGenerator");
        }
    }

    IEnumerator WaitForLoadingBar()
    {
        //translationManager.PushTextsIntoFile();

        bool registereduser = registeredUser;
        GlobalCalculator.GameStarted = false;
        yield return new WaitForSeconds(2.0f);

        if (registeredUser == false)
        {
            loginMenu.SetActive (true);
            loadingBar.SetActive (false);
            saveSlots.SetActive(false);
        }
        else
        {
            saveSlots.SetActive(true);
            loadingBar.SetActive(false);
            loginMenu.SetActive(false);
        }
    }

    public async UniTask ResetNewGame()
    {
        textObject = loadingBar.transform.GetComponentInChildren<TextMeshProUGUI>();
        SystemLanguage systemLanguage = Application.systemLanguage;
        switch (systemLanguage)
        {
            case SystemLanguage.Russian:
                textObject.text = "ИНИЦИАЛИЗАЦИЯ ИГРЫ...";
                break;
            case SystemLanguage.Chinese:
                textObject.text = "游戏初始化中...";
                break;
            case SystemLanguage.Slovak:
                textObject.text = "INICIALIZÁCIA HRY...";
                break;
            default: // English by default
                textObject.text = "INITIALIZING GAME...";
                break;
        }
        newGamePopup.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        newGamePopup.GetComponent<Michsky.UI.Shift.ModalWindowManager>().ModalWindowIn();
        loadingBar.SetActive (false);
    }
    public IEnumerator LoadSaveSlots()
    {
        saveSlots.SetActive(true);
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

    public IEnumerator CreateFibrousLeaves()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 2f;

        GameObject fillBarObject = RecipeList.transform.Find("FibrousLeaves/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += UnityEngine.Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(0);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 1);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentPlantResource = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC").ToString();

        for (int i = 0; i < FibrousLeavesTexts.Length; i++)
        {
            FibrousLeavesTexts[i].text = currentPlantResource;
        }
        StartCoroutine("CreateFibrousLeaves");
    }

    public IEnumerator StopCreateFibrousLeaves()
    {
        StopCoroutine("CreateFibrousLeaves");
        AllCoroutineBooleans[0] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectWater()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 2f;

        GameObject fillBarObject = RecipeList.transform.Find("WaterRecipe(Clone)/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        itemCreator.CreateItem(1);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 1);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentWaterResource = inventoryManager.GetItemQuantity("Water", "BASIC").ToString();

        for (int i = 0; i < WaterTexts.Length; i++)
        {
            WaterTexts[i].text = currentWaterResource;
        }
        imageToFill.fillAmount = 0f;

        StartCoroutine("CollectWater");
    }

    public IEnumerator StopCollectWater()
    {
        StopCoroutine("CollectWater");
        AllCoroutineBooleans[1] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }
    public IEnumerator CollectWood()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 6f;

        GameObject fillBarObject = RecipeList.transform.Find("WoodRecipe(Clone)/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(8);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 2);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentWoodResource = inventoryManager.GetItemQuantity("Wood", "BASIC").ToString();

        for (int i = 0; i < WoodTexts.Length; i++)
        {
            WoodTexts[i].text = currentWoodResource;
        }
        StartCoroutine("CollectWood");
    }
    public IEnumerator StopCollectWood()
    {
        StopCoroutine("CollectWood");
        AllCoroutineBooleans[7] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectIronOre()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 6f;

        GameObject fillBarObject = RecipeList.transform.Find("IronOreRecipe(Clone)/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(9);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 2);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentIronOreResource = inventoryManager.GetItemQuantity("IronOre", "BASIC").ToString();

        for (int i = 0; i < IronOreTexts.Length; i++)
        {
            IronOreTexts[i].text = currentIronOreResource;
        }
        StartCoroutine("CollectIronOre");
    }
    public IEnumerator StopCollectIronOre()
    {
        StopCoroutine("CollectIronOre");
        AllCoroutineBooleans[8] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectCoal()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 6f;

        GameObject fillBarObject = RecipeList.transform.Find("CoalRecipe(Clone)/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(10);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 2);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentCoalResource = inventoryManager.GetItemQuantity("Coal", "BASIC").ToString();

        for (int i = 0; i < CoalTexts.Length; i++)
        {
            CoalTexts[i].text = currentCoalResource;
        }
        StartCoroutine("CollectCoal");
    }
    public IEnumerator StopCollectCoal()
    {
        StopCoroutine("CollectCoal");
        AllCoroutineBooleans[9] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateIronBeam()
    {
        bool isIronOreQuantityMet = inventoryManager.CheckItemQuantity("IronOre", "BASIC", 6);
        bool isCoalQuantityMet = inventoryManager.CheckItemQuantity("Coal", "BASIC", 2);
        bool isQuantityMet = isIronOreQuantityMet && isCoalQuantityMet;
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 8f;
            AllCoroutineBooleans[10] = true;

            GameObject fillBarObject = RecipeList.transform.Find("IronBeamRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("IronOre", "BASIC", 6);
            inventoryManager.ReduceItemQuantity("Coal", "BASIC", 2);

            string currentIronOreResource = inventoryManager.GetItemQuantity("IronOre", "BASIC").ToString();

            for (int i = 0; i < IronOreTexts.Length; i++)
            {
                IronOreTexts[i].text = currentIronOreResource;
            }

            string currentCoalResource = inventoryManager.GetItemQuantity("Coal", "BASIC").ToString();

            for (int i = 0; i < CoalTexts.Length; i++)
            {
                CoalTexts[i].text = currentCoalResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += Time.deltaTime;
                yield return null;
            }
            imageToFill.fillAmount = 0f;
            itemCreator.CreateItem(11);

            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 4);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }

            string currentIronBeamResource = inventoryManager.GetItemQuantity("IronBeam", "PROCESSED").ToString();

            for (int i = 0; i < IronBeamTexts.Length; i++)
            {
                IronBeamTexts[i].text = currentIronBeamResource;
            }
            StartCoroutine("CreateIronBeam");

        }
        else
        {
            AllCoroutineBooleans[10] = false;
        }
    }

    public IEnumerator StopCreateIronBeam()
    {
        StopCoroutine("CreateIronBeam");
        itemCreator.CreateItem(9, 6);
        itemCreator.CreateItem(10, 2);

        string currentIronOreResource = inventoryManager.GetItemQuantity("IronOre", "BASIC").ToString();
        string currentCoalResource = inventoryManager.GetItemQuantity("Coal", "BASIC").ToString();

        for (int i = 0; i < IronOreTexts.Length; i++)
        {
            IronOreTexts[i].text = currentIronOreResource;
        }
        for (int i = 0; i < CoalTexts.Length; i++)
        {
            CoalTexts[i].text = currentCoalResource;
        }

        AllCoroutineBooleans[10] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectBiofuel()
    {
        bool isQuantityMet = inventoryManager.CheckItemQuantity("FibrousLeaves", "BASIC", 20);
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 3f;
            AllCoroutineBooleans[2] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BiofuelRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("FibrousLeaves", "BASIC", 20);

            string currentPlantResource = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC").ToString();

            for (int i = 0; i < FibrousLeavesTexts.Length; i++)
            {
                FibrousLeavesTexts[i].text = currentPlantResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += Time.deltaTime;
                yield return null;
            }
            imageToFill.fillAmount = 0f;
            itemCreator.CreateItem(2);

            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 2);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }

            string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

            for (int i = 0; i < BiofuelTexts.Length; i++)
            {
                BiofuelTexts[i].text = currentBiofuelResource;
            }
            StartCoroutine("CollectBiofuel");

        }
        else
        {
            AllCoroutineBooleans[2] = false;
        }
    }

    public IEnumerator StopCollectBiofuel()
    {
        StopCoroutine("CollectBiofuel");
        itemCreator.CreateItem(0, 20);

        string currentPlantResource = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC").ToString();

        for (int i = 0; i < FibrousLeavesTexts.Length; i++)
        {
            FibrousLeavesTexts[i].text = currentPlantResource;
        }

        AllCoroutineBooleans[2] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectDistilledWater()
    {
        bool isQuantityMet = inventoryManager.CheckItemQuantity("Water", "BASIC", 50);
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 3f;
            AllCoroutineBooleans[3] = true;

            GameObject fillBarObject = RecipeList.transform.Find("DistilledWaterRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("Water", "BASIC", 50);

            string currentWaterResource = inventoryManager.GetItemQuantity("Water", "BASIC").ToString();

            for (int i = 0; i < WaterTexts.Length; i++)
            {
                WaterTexts[i].text = currentWaterResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += Time.deltaTime;
                yield return null;
            }
            itemCreator.CreateItem(3);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 2);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }

            imageToFill.fillAmount = 0f;

            string currentDistilledWaterResource = inventoryManager.GetItemQuantity("DistilledWater", "PROCESSED").ToString();

            for (int i = 0; i < DistilledWaterTexts.Length; i++)
            {
                DistilledWaterTexts[i].text = currentDistilledWaterResource;
            }

            StartCoroutine("CollectDistilledWater");
        }
        else
        {
            AllCoroutineBooleans[3] = false;
        }
    }

    public IEnumerator StopCollectDistilledWater()
    {
        StopCoroutine("CollectDistilledWater");
        AllCoroutineBooleans[3] = false;
        itemCreator.CreateItem(1, 50);

        string currentWaterResource = inventoryManager.GetItemQuantity("Water", "BASIC").ToString();

        for (int i = 0; i < WaterTexts.Length; i++)
        {
            WaterTexts[i].text = currentWaterResource;
        }

        imageToFill.fillAmount = 0f;
        yield return null;
    }
    public IEnumerator CreateBatteryCore()
    {
        bool isQuantityMet = inventoryManager.CheckItemQuantity("Biofuel", "PROCESSED", 4);
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 6f;
            AllCoroutineBooleans[6] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BatteryCoreRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("Biofuel", "PROCESSED", 4);

            string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

            for (int i = 0; i < BiofuelTexts.Length; i++)
            {
                BiofuelTexts[i].text = currentBiofuelResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += Time.deltaTime;
                yield return null;
            }

            itemCreator.CreateItem(6);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 3);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }

            imageToFill.fillAmount = 0f;

            string currentBatteryCoreResource = inventoryManager.GetItemQuantity("BatteryCore", "ENHANCED").ToString();

            for (int i = 0; i < BatteryCoreTexts.Length; i++)
            {
                BatteryCoreTexts[i].text = currentBatteryCoreResource;
            }
            StartCoroutine("CreateBatteryCore");
        }
        else
        {
            AllCoroutineBooleans[6] = false;
        }
    }
    public IEnumerator StopCreateBatteryCore()
    {
        StopCoroutine("CreateBatteryCore");
        AllCoroutineBooleans[6] = false;
        itemCreator.CreateItem(2, 4);

        string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

        for (int i = 0; i < BiofuelTexts.Length; i++)
        {
            BiofuelTexts[i].text = currentBiofuelResource;
        }


        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateBattery()
    {
        bool isBiofuelQuantityMet = inventoryManager.CheckItemQuantity("Biofuel", "PROCESSED", 1);
        bool isBatteryCoreQuantityMet = inventoryManager.CheckItemQuantity("BatteryCore", "ENHANCED", 1);
        bool isQuantityMet = isBatteryCoreQuantityMet && isBiofuelQuantityMet;
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 8f;
            AllCoroutineBooleans[4] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BatteryRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("BatteryCore", "ENHANCED", 1);
            inventoryManager.ReduceItemQuantity("Biofuel", "PROCESSED", 1);

            string currentBatteryCoreResource = inventoryManager.GetItemQuantity("BatteryCore", "ENHANCED").ToString();
            string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

            for (int i = 0; i < BatteryCoreTexts.Length; i++)
            {
                BatteryCoreTexts[i].text = currentBatteryCoreResource;
            }
            for (int i = 0; i < BiofuelTexts.Length; i++)
            {
                BiofuelTexts[i].text = currentBiofuelResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += Time.deltaTime;
                yield return null;
            }

            itemCreator.CreateItem(4);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 4);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }
            imageToFill.fillAmount = 0f;
            string currentBatteryResource = inventoryManager.GetItemQuantity("Battery", "ASSEMBLED").ToString();
            for (int i = 0; i < BatteryTexts.Length; i++)
            {
                BatteryTexts[i].text = currentBatteryResource;
            }
            StartCoroutine("CreateBattery");

            /// <summary>
            /// First goal finished and switched to true.
            /// </summary>
            /// <value>true</value>
            if (GoalManager.firstGoal == false)
            {
                GoalManager goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
                _ = goalManager.SetSecondGoal();
            }
        }
        else
        {
            AllCoroutineBooleans[4] = false;
        }
    }
    public IEnumerator StopCreateBattery()
    {
        StopCoroutine("CreateBattery");
        AllCoroutineBooleans[4] = false;
        itemCreator.CreateItem(6, 1);
        itemCreator.CreateItem(2, 1);

        string currentBatteryCoreResource = inventoryManager.GetItemQuantity("BatteryCore", "ENHANCED").ToString();
        string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

        for (int i = 0; i < BatteryCoreTexts.Length; i++)
        {
            BatteryCoreTexts[i].text = currentBatteryCoreResource;
        }
        for (int i = 0; i < BiofuelTexts.Length; i++)
        {
            BiofuelTexts[i].text = currentBiofuelResource;
        }
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateBiofuelGeneratorBlueprint()
    {
        bool isIronBeamQuantityMet = inventoryManager.CheckItemQuantity("IronBeam", "PROCESSED", 4);
        bool isWoodQuantityMet = inventoryManager.CheckItemQuantity("Wood", "BASIC", 4);
        bool isQuantityMet = isWoodQuantityMet && isIronBeamQuantityMet;
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 30f;
            AllCoroutineBooleans[11] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BiofuelGeneratorBlueprint(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("IronBeam", "PROCESSED", 4);
            inventoryManager.ReduceItemQuantity("Wood", "BASIC", 4);

            string currentIronBeamResource = inventoryManager.GetItemQuantity("IronBeam", "PROCESSED").ToString();
            string currentWoodResource = inventoryManager.GetItemQuantity("Wood", "BASIC").ToString();

            for (int i = 0; i < IronBeamTexts.Length; i++)
            {
                IronBeamTexts[i].text = currentIronBeamResource;
            }
            for (int i = 0; i < WoodTexts.Length; i++)
            {
                WoodTexts[i].text = currentWoodResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += Time.deltaTime;
                yield return null;
            }

            Planet0Buildings.Planet0BiofuelGenerator++;
            buildingIncrementor.buildingCounts[0].text = Planet0Buildings.Planet0BiofuelGenerator.ToString();
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 16);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }
            imageToFill.fillAmount = 0f;
            string currentBiofuelGeneratorResource = inventoryManager.GetItemQuantity("BiofuelGenerator", "BUILDINGS").ToString();
            for (int i = 0; i < BiofuelGeneratorTexts.Length; i++)
            {
                BiofuelGeneratorTexts[i].text = currentBiofuelGeneratorResource;
            }
            StartCoroutine("CreateBiofuelGeneratorBlueprint");

            if (GoalManager.firstGoal == false)
            {
                GoalManager goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
                _ = goalManager.SetFourthGoal();
            }
        }
        else
        {
            AllCoroutineBooleans[11] = false;
        }
    }
    public IEnumerator StopCreateBiofuelGeneratorBlueprint()
    {
        StopCoroutine("CreateBiofuelGeneratorBlueprint");
        AllCoroutineBooleans[11] = false;
        itemCreator.CreateItem(11, 4);
        itemCreator.CreateItem(8, 4);

        string currentIronBeamResource = inventoryManager.GetItemQuantity("IronBeam", "PROCESSED").ToString();
        string currentWoodResource = inventoryManager.GetItemQuantity("Wood", "BASIC").ToString();

        for (int i = 0; i < IronBeamTexts.Length; i++)
        {
            IronBeamTexts[i].text = currentIronBeamResource;
        }
        for (int i = 0; i < WoodTexts.Length; i++)
        {
             WoodTexts[i].text = currentWoodResource;
        }
        imageToFill.fillAmount = 0f;
        yield return null;
    }

}
