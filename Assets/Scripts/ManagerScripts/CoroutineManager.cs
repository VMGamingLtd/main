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
    public ProductionCreator productionCreator;
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
    /*void OnEnable()
    {
        ResetNewGame();
    }*/
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
            StartCoroutine("StopCreateFibrousLeaves");
        }
        else if (IndexNumber == 1)
        {
            StartCoroutine("StopCreateWater");
        }
        else if (IndexNumber == 2)
        {
            StartCoroutine("StopCreateBiofuel");
        }
        else if (IndexNumber == 3)
        {
            StartCoroutine("StopCreateDistilledWater");
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
            StartCoroutine("StopCreateWood");
        }
        else if (IndexNumber == 8)
        {
            StartCoroutine("StopCreateIronOre");
        }
        else if (IndexNumber == 9)
        {
            StartCoroutine("StopCreateCoal");
        }
        else if (IndexNumber == 10)
        {
            StartCoroutine("StopCreateBiofuelGenerator");
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

    public void ReturnMaterials(List<ChildData> recipeDataChildList)
    {
        foreach (ChildData childData in recipeDataChildList)
        {
            itemCreator.CreateItem(childData.index, childData.quantity);
        }
    }
    public IEnumerator CreateFibrousLeaves()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 0);

        float timer = 0f;
        float fillTimePlanet0bb = 2f;

        GameObject fillBarObject = RecipeList.transform.Find("FibrousLeaves/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
        if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(0);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        

        string currentPlantResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

        for (int i = 0; i < FibrousLeavesTexts.Length; i++)
        {
            FibrousLeavesTexts[i].text = currentPlantResource;
        }
        StartCoroutine("CreateFibrousLeaves");
    }

    public IEnumerator StopCreateFibrousLeaves()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateFibrousLeaves");
        AllCoroutineBooleans[0] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateWater()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 1);

        float timer = 0f;
        float fillTimePlanet0bb = 2f;

        GameObject fillBarObject = RecipeList.transform.Find("Water/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
        if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        itemCreator.CreateItem(1);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentWaterResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

        for (int i = 0; i < WaterTexts.Length; i++)
        {
            WaterTexts[i].text = currentWaterResource;
        }
        imageToFill.fillAmount = 0f;

        StartCoroutine("CreateWater");
    }

    public IEnumerator StopCreateWater()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateWater");
        AllCoroutineBooleans[1] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }
    public IEnumerator CreateWood()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 7);

        float timer = 0f;
        float fillTimePlanet0bb = 6f;

        GameObject fillBarObject = RecipeList.transform.Find("Wood/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
        if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(8);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentWoodResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

        for (int i = 0; i < WoodTexts.Length; i++)
        {
            WoodTexts[i].text = currentWoodResource;
        }
        StartCoroutine("CreateWood");
    }
    public IEnumerator StopCreateWood()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateWood");
        AllCoroutineBooleans[7] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateIronOre()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 8);

        float timer = 0f;
        float fillTimePlanet0bb = 6f;

        GameObject fillBarObject = RecipeList.transform.Find("IronOre/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
        if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(9);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentIronOreResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

        for (int i = 0; i < IronOreTexts.Length; i++)
        {
            IronOreTexts[i].text = currentIronOreResource;
        }
        StartCoroutine("CreateIronOre");
    }
    public IEnumerator StopCreateIronOre()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateIronOre");
        AllCoroutineBooleans[8] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateCoal()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 9);

        float timer = 0f;
        float fillTimePlanet0bb = 6f;

        GameObject fillBarObject = RecipeList.transform.Find("Coal/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
        if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreateItem(10);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            LevelUp(playerMaxExp);
        }

        string currentCoalResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

        for (int i = 0; i < CoalTexts.Length; i++)
        {
            CoalTexts[i].text = currentCoalResource;
        }
        StartCoroutine("CreateCoal");
    }
    public IEnumerator StopCreateCoal()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateCoal");
        AllCoroutineBooleans[9] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateIronBeam()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 10);
        List<ChildData> recipeDataChildList = recipeData.childDataList;
        bool isIronOreQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
        bool isCoalQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product, recipeDataChildList[1].quantity);
        bool isQuantityMet = isIronOreQuantityMet && isCoalQuantityMet;
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 8f;
            AllCoroutineBooleans[10] = true;

            GameObject fillBarObject = RecipeList.transform.Find("IronBeam/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
            inventoryManager.ReduceItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product, recipeDataChildList[1].quantity);

            // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
            if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

            string currentIronOreResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();

            for (int i = 0; i < IronOreTexts.Length; i++)
            {
                IronOreTexts[i].text = currentIronOreResource;
            }

            string currentCoalResource = inventoryManager.GetItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product).ToString();

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
            itemCreator.CreateItem(10);

            Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }

            string currentIronBeamResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

            for (int i = 0; i < IronBeamTexts.Length; i++)
            {
                IronBeamTexts[i].text = currentIronBeamResource;
            }
            StartCoroutine("CreateIronBeam");

        }
        else
        {
            productionCreator.DelistManualProduction();
            AllCoroutineBooleans[10] = false;
        }
    }

    public IEnumerator StopCreateIronBeam()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateIronBeam");
        List<ChildData> recipeDataChildList = recipeCreator.recipeDataList.Find(recipe => recipe.index == 10)?.childDataList;
        ReturnMaterials(recipeDataChildList);
        string currentIronOreResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();
        string currentCoalResource = inventoryManager.GetItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product).ToString();

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

    public IEnumerator CreateBiofuel()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 2);
        List<ChildData> recipeDataChildList = recipeData.childDataList;
        bool isQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 3f;
            AllCoroutineBooleans[2] = true;

            GameObject fillBarObject = RecipeList.transform.Find("Biofuel/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
            if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

            inventoryManager.ReduceItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);

            string currentPlantResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();

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

            Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                LevelUp(playerMaxExp);
            }

            string currentBiofuelResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

            for (int i = 0; i < BiofuelTexts.Length; i++)
            {
                BiofuelTexts[i].text = currentBiofuelResource;
            }
            StartCoroutine("CreateBiofuel");

        }
        else
        {
            productionCreator.DelistManualProduction();
            AllCoroutineBooleans[2] = false;
        }
    }

    public IEnumerator StopCreateBiofuel()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateBiofuel");
        List<ChildData> recipeDataChildList = recipeCreator.recipeDataList.Find(recipe => recipe.index == 2)?.childDataList;
        ReturnMaterials(recipeDataChildList);

        string currentPlantResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();

        for (int i = 0; i < FibrousLeavesTexts.Length; i++)
        {
            FibrousLeavesTexts[i].text = currentPlantResource;
        }

        AllCoroutineBooleans[2] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateDistilledWater()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 3);
        List<ChildData> recipeDataChildList = recipeData.childDataList;
        bool isQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 3f;
            AllCoroutineBooleans[3] = true;

            GameObject fillBarObject = RecipeList.transform.Find("DistilledWater/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
            if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

            inventoryManager.ReduceItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);

            string currentWaterResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();

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
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
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

            string currentDistilledWaterResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

            for (int i = 0; i < DistilledWaterTexts.Length; i++)
            {
                DistilledWaterTexts[i].text = currentDistilledWaterResource;
            }

            StartCoroutine("CreateDistilledWater");
        }
        else
        {
            productionCreator.DelistManualProduction();
            AllCoroutineBooleans[3] = false;
        }
    }

    public IEnumerator StopCreateDistilledWater()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateDistilledWater");
        List<ChildData> recipeDataChildList = recipeCreator.recipeDataList.Find(recipe => recipe.index == 3)?.childDataList;
        AllCoroutineBooleans[3] = false;
        ReturnMaterials(recipeDataChildList);

        string currentWaterResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();

        for (int i = 0; i < WaterTexts.Length; i++)
        {
            WaterTexts[i].text = currentWaterResource;
        }

        imageToFill.fillAmount = 0f;
        yield return null;
    }
    public IEnumerator CreateBatteryCore()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 6);
        List<ChildData> recipeDataChildList = recipeData.childDataList;
        bool isQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 6f;
            AllCoroutineBooleans[6] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BatteryCore/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
            if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

            inventoryManager.ReduceItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);

            string currentBiofuelResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();

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
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
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

            string currentBatteryCoreResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();

            for (int i = 0; i < BatteryCoreTexts.Length; i++)
            {
                BatteryCoreTexts[i].text = currentBatteryCoreResource;
            }
            StartCoroutine("CreateBatteryCore");
        }
        else
        {
            productionCreator.DelistManualProduction();
            AllCoroutineBooleans[6] = false;
        }
    }
    public IEnumerator StopCreateBatteryCore()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateBatteryCore");
        List<ChildData> recipeDataChildList = recipeCreator.recipeDataList.Find(recipe => recipe.index == 6)?.childDataList;
        AllCoroutineBooleans[6] = false;
        ReturnMaterials(recipeDataChildList);

        string currentBiofuelResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();

        for (int i = 0; i < BiofuelTexts.Length; i++)
        {
            BiofuelTexts[i].text = currentBiofuelResource;
        }

        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateBattery()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 4);
        List<ChildData> recipeDataChildList = recipeData.childDataList;
        bool isBiofuelQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
        bool isBatteryCoreQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product, recipeDataChildList[1].quantity);
        bool isQuantityMet = isBatteryCoreQuantityMet && isBiofuelQuantityMet;
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 8f;
            AllCoroutineBooleans[4] = true;

            GameObject fillBarObject = RecipeList.transform.Find("Battery/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
            if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

            inventoryManager.ReduceItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
            inventoryManager.ReduceItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product, recipeDataChildList[1].quantity);
            
            string currentBiofuelResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();
            string currentBatteryCoreResource = inventoryManager.GetItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product).ToString();

            for (int i = 0; i < BiofuelTexts.Length; i++)
            {
                BiofuelTexts[i].text = currentBiofuelResource;
            }
            for (int i = 0; i < BatteryCoreTexts.Length; i++)
            {
                BatteryCoreTexts[i].text = currentBatteryCoreResource;
            }
            
            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += Time.deltaTime;
                yield return null;
            }

            itemCreator.CreateItem(4);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
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
            string currentBatteryResource = inventoryManager.GetItemQuantity(recipeData.recipeName, recipeData.recipeProduct).ToString();
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
            productionCreator.DelistManualProduction();
            AllCoroutineBooleans[4] = false;
        }
    }
    public IEnumerator StopCreateBattery()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateBattery");
        List<ChildData> recipeDataChildList = recipeCreator.recipeDataList.Find(recipe => recipe.index == 4)?.childDataList;
        AllCoroutineBooleans[4] = false;
        ReturnMaterials(recipeDataChildList);

        string currentBiofuelResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();
        string currentBatteryCoreResource = inventoryManager.GetItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product).ToString();

        for (int i = 0; i < BiofuelTexts.Length; i++)
        {
            BiofuelTexts[i].text = currentBiofuelResource;
        }
        for (int i = 0; i < BatteryCoreTexts.Length; i++)
        {
            BatteryCoreTexts[i].text = currentBatteryCoreResource;
        }
        
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateBiofuelGenerator()
    {
        RecipeDataJson recipeData = recipeCreator.recipeDataList.Find(recipe => recipe.index == 11);
        List<ChildData> recipeDataChildList = recipeData.childDataList;
        bool isIronBeamQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
        bool isWoodQuantityMet = inventoryManager.CheckItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product, recipeDataChildList[1].quantity);
        bool isQuantityMet = isWoodQuantityMet && isIronBeamQuantityMet;
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 30f;
            AllCoroutineBooleans[11] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BiofuelGenerator/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            // we are enlisting a UI row in production overview tab, but only if it doesn't exist already
            if (ProductionCreator.manualRowID == 0) productionCreator.EnlistManualProduction(recipeData);

            inventoryManager.ReduceItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product, recipeDataChildList[0].quantity);
            inventoryManager.ReduceItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product, recipeDataChildList[1].quantity);

            string currentIronBeamResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();
            string currentWoodResource = inventoryManager.GetItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product).ToString();

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
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, recipeData.experience);
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
            string currentBiofuelGeneratorResource = Planet0Buildings.Planet0BiofuelGenerator.ToString();
            for (int i = 0; i < BiofuelGeneratorTexts.Length; i++)
            {
                BiofuelGeneratorTexts[i].text = currentBiofuelGeneratorResource;
            }
            StartCoroutine("CreateBiofuelGenerator");
        }
        else
        {
            productionCreator.DelistManualProduction();
            AllCoroutineBooleans[11] = false;
        }
    }
    public IEnumerator StopCreateBiofuelGenerator()
    {
        productionCreator.DelistManualProduction();
        StopCoroutine("CreateBiofuelGenerator");
        AllCoroutineBooleans[11] = false;
        List<ChildData> recipeDataChildList = recipeCreator.recipeDataList.Find(recipe => recipe.index == 11)?.childDataList;
        ReturnMaterials(recipeDataChildList);

        string currentIronBeamResource = inventoryManager.GetItemQuantity(recipeDataChildList[0].name, recipeDataChildList[0].product).ToString();
        string currentWoodResource = inventoryManager.GetItemQuantity(recipeDataChildList[1].name, recipeDataChildList[1].product).ToString();

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
