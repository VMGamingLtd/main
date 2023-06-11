using System.Collections;
using System.Collections.Generic;
using ItemManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoroutineManager : MonoBehaviour
{
    public ItemCreator itemCreator;
    public TranslationManager translationManager;
    public InventoryManager inventoryManager;
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

    public static bool[] AllCoroutineBooleans = new bool[10];
    public static int IndexNumber;

    public Image imageToFill;
    private float targetFillAmountPlanet0bb = 1f;
    private float currentFillAmountPlanet0bb = 0f;

    // after production is finished add +1 to the counter
    public TextMeshProUGUI[] plantsTexts;
    public TextMeshProUGUI[] waterTexts;
    public TextMeshProUGUI[] biofuelTexts;
    public TextMeshProUGUI[] purifiedWaterTexts;
    public TextMeshProUGUI[] batteryTexts;
    public TextMeshProUGUI[] batteryCoreTexts;
    public TextMeshProUGUI[] planetStatsTexts;

    public void OnEnable()
    {
        if (StartNewGame.loadingNewGame == false){
            StartCoroutine(WaitForLoadingBar());
        }
        else if (StartNewGame.loadingNewGame == true) {
            StartCoroutine(ResetNewGame());
        }
    }

    public void StopRunningCoroutine()
    {
        if (CoroutineManager.IndexNumber == 0)
        {
            StartCoroutine("StopCollectPlants");
        }
        else if (CoroutineManager.IndexNumber == 1)
        {
            StartCoroutine("StopCollectWater");
        }
        else if (CoroutineManager.IndexNumber == 2)
        {
            StartCoroutine("StopCollectBiofuel");
        }
        else if (CoroutineManager.IndexNumber == 3)
        {
            StartCoroutine("StopCollectPurifiedWater");
        }
        else if (CoroutineManager.IndexNumber == 4)
        {
            StartCoroutine("StopCreateBattery");
        }
        else if (CoroutineManager.IndexNumber == 5)
        {
            StartCoroutine("StopCreateOxygenTanks");
        }
        else if (CoroutineManager.IndexNumber == 6)
        {
            StartCoroutine("StopCreateBatteryCore");
        }
    }

    IEnumerator WaitForLoadingBar () {
        //translationManager.PushTextsIntoFile();

        bool registereduser = CoroutineManager.registeredUser;
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

    IEnumerator ResetNewGame () {
        textObject = this.loadingBar.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
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
        yield return new WaitForSeconds(1.5f);
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
        for (int i = 0; i < CoroutineManager.AllCoroutineBooleans.Length; i++)
        {
            CoroutineManager.AllCoroutineBooleans[i] = false; // Set each element in the array to false
        }
    }

    public static bool CheckForTrueValues()
    {
        for (int i = 0; i < CoroutineManager.AllCoroutineBooleans.Length; i++)
        {
            if (CoroutineManager.AllCoroutineBooleans[i] == true)
            {
                CoroutineManager.IndexNumber = i;
                return true;
            }
        }
        return false;
    }

    public IEnumerator CollectPlants()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 2f;

        GameObject fillBarObject = RecipeList.transform.Find("FibrousLeavesRecipe(Clone)/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += UnityEngine.Time.deltaTime;
            yield return null;
        }
        imageToFill.fillAmount = 0f;

        itemCreator.CreatePlants(5);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 1);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
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
            Level.AddCurrentResource(ref Level.StatPoints, 5);
            Level.AddCurrentResource(ref Level.SkillPoints, 1);
            statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
            skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
        }

        string currentPlantResource = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC").ToString();

        for (int i = 0; i < plantsTexts.Length; i++)
        {
            plantsTexts[i].text = currentPlantResource;
        }
        StartCoroutine("CollectPlants");
    }

    public IEnumerator StopCollectPlants()
    {
        StopCoroutine("CollectPlants");
        CoroutineManager.AllCoroutineBooleans[0] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectWater()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 2f;

        GameObject fillBarObject = RecipeList.transform.Find("AlienWaterRecipe(Clone)/FillBckg/FillBar").gameObject;
        imageToFill = fillBarObject.GetComponent<Image>();

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFill.fillAmount = currentFillAmountPlanet0bb;
            timer += UnityEngine.Time.deltaTime;
            yield return null;
        }
        itemCreator.CreateWater(10);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 1);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
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
            Level.AddCurrentResource(ref Level.StatPoints, 5);
            Level.AddCurrentResource(ref Level.SkillPoints, 1);
            statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
            skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
        }

        string currentWaterResource = inventoryManager.GetItemQuantity("AlienWater", "BASIC").ToString();

        for (int i = 0; i < waterTexts.Length; i++)
        {
            waterTexts[i].text = currentWaterResource;
        }
        imageToFill.fillAmount = 0f;

        StartCoroutine("CollectWater");
    }

    public IEnumerator StopCollectWater()
    {
        StopCoroutine("CollectWater");
        CoroutineManager.AllCoroutineBooleans[1] = false;
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
            CoroutineManager.AllCoroutineBooleans[2] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BiofuelRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("FibrousLeaves", "BASIC", 20);

            string currentPlantResource = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC").ToString();

            for (int i = 0; i < plantsTexts.Length; i++)
            {
                plantsTexts[i].text = currentPlantResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += UnityEngine.Time.deltaTime;
                yield return null;
            }
            imageToFill.fillAmount = 0f;
            itemCreator.CreateBiofuel(1);

            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 5);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
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
                Level.AddCurrentResource(ref Level.StatPoints, 5);
                Level.AddCurrentResource(ref Level.SkillPoints, 1);
                statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
                skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
            }

            string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

            for (int i = 0; i < biofuelTexts.Length; i++)
            {
                biofuelTexts[i].text = currentBiofuelResource;
            }
            StartCoroutine("CollectBiofuel");

        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[2] = false;
        }
    }

    public IEnumerator StopCollectBiofuel()
    {
        StopCoroutine("CollectBiofuel");
        inventoryManager.AddItemQuantity("FibrousLeaves", "BASIC", 20);

        string currentPlantResource = inventoryManager.GetItemQuantity("FibrousLeaves", "BASIC").ToString();

        for (int i = 0; i < plantsTexts.Length; i++)
        {
            plantsTexts[i].text = currentPlantResource;
        }

        CoroutineManager.AllCoroutineBooleans[2] = false;
        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectPurifiedWater()
    {
        bool isQuantityMet = inventoryManager.CheckItemQuantity("AlienWater", "BASIC", 50);
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 3f;
            CoroutineManager.AllCoroutineBooleans[3] = true;

            GameObject fillBarObject = RecipeList.transform.Find("PurifiedWaterRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("AlienWater", "BASIC", 50);

            string currentWaterResource = inventoryManager.GetItemQuantity("AlienWater", "BASIC").ToString();

            for (int i = 0; i < waterTexts.Length; i++)
            {
                waterTexts[i].text = currentWaterResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += UnityEngine.Time.deltaTime;
                yield return null;
            }
            itemCreator.CreatePurifiedWater(1);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 5);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
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
                Level.AddCurrentResource(ref Level.StatPoints, 5);
                Level.AddCurrentResource(ref Level.SkillPoints, 1);
                statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
                skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
            }

            imageToFill.fillAmount = 0f;

            string currentPurifiedWaterResource = inventoryManager.GetItemQuantity("PurifiedWater", "PROCESSED").ToString();

            for (int i = 0; i < purifiedWaterTexts.Length; i++)
            {
                purifiedWaterTexts[i].text = currentPurifiedWaterResource;
            }

            StartCoroutine("CollectPurifiedWater");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[3] = false;
        }
    }

    public IEnumerator StopCollectPurifiedWater()
    {
        StopCoroutine("CollectPurifiedWater");
        CoroutineManager.AllCoroutineBooleans[3] = false;
        inventoryManager.AddItemQuantity("AlienWater", "BASIC", 50);

        string currentWaterResource = inventoryManager.GetItemQuantity("AlienWater", "BASIC").ToString();

        for (int i = 0; i < waterTexts.Length; i++)
        {
            waterTexts[i].text = currentWaterResource;
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
            CoroutineManager.AllCoroutineBooleans[6] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BatteryCoreRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("Biofuel", "PROCESSED", 4);

            string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

            for (int i = 0; i < biofuelTexts.Length; i++)
            {
                biofuelTexts[i].text = currentBiofuelResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += UnityEngine.Time.deltaTime;
                yield return null;
            }

            itemCreator.CreateBatteryCore(1);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 8);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
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
                Level.AddCurrentResource(ref Level.StatPoints, 5);
                Level.AddCurrentResource(ref Level.SkillPoints, 1);
                statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
                skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
            }

            imageToFill.fillAmount = 0f;

            string currentBatteryCoreResource = inventoryManager.GetItemQuantity("BatteryCore", "REFINED").ToString();

            for (int i = 0; i < batteryCoreTexts.Length; i++)
            {
                batteryCoreTexts[i].text = currentBatteryCoreResource;
            }
            StartCoroutine("CreateBatteryCore");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[6] = false;
        }
    }
    public IEnumerator StopCreateBatteryCore()
    {
        StopCoroutine("CreateBatteryCore");
        CoroutineManager.AllCoroutineBooleans[6] = false;
        inventoryManager.AddItemQuantity("Biofuel", "PROCESSED", 4);

        string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

        for (int i = 0; i < biofuelTexts.Length; i++)
        {
            biofuelTexts[i].text = currentBiofuelResource;
        }


        imageToFill.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateBattery()
    {
        bool isBiofuelQuantityMet = inventoryManager.CheckItemQuantity("Biofuel", "PROCESSED", 1);
        bool isBatteryCoreQuantityMet = inventoryManager.CheckItemQuantity("BatteryCore", "REFINED", 1);
        bool isQuantityMet = isBatteryCoreQuantityMet && isBiofuelQuantityMet;
        if (isQuantityMet)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 8f;
            CoroutineManager.AllCoroutineBooleans[4] = true;

            GameObject fillBarObject = RecipeList.transform.Find("BatteryRecipe(Clone)/FillBckg/FillBar").gameObject;
            imageToFill = fillBarObject.GetComponent<Image>();

            inventoryManager.ReduceItemQuantity("BatteryCore", "REFINED", 1);
            inventoryManager.ReduceItemQuantity("Biofuel", "PROCESSED", 1);

            string currentBatteryCoreResource = inventoryManager.GetItemQuantity("BatteryCore", "REFINED").ToString();
            string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

            for (int i = 0; i < batteryCoreTexts.Length; i++)
            {
                batteryCoreTexts[i].text = currentBatteryCoreResource;
            }
            for (int i = 0; i < biofuelTexts.Length; i++)
            {
                biofuelTexts[i].text = currentBiofuelResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFill.fillAmount = currentFillAmountPlanet0bb;
                timer += UnityEngine.Time.deltaTime;
                yield return null;
            }

            itemCreator.CreateBattery(1);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 10);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();
            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
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
                Level.AddCurrentResource(ref Level.StatPoints, 5);
                Level.AddCurrentResource(ref Level.SkillPoints, 1);
                statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
                skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
            }

            imageToFill.fillAmount = 0f;

            string currentBatteryResource = inventoryManager.GetItemQuantity("Battery", "ASSEMBLED").ToString();

            for (int i = 0; i < batteryTexts.Length; i++)
            {
                batteryTexts[i].text = currentBatteryResource;
            }
            StartCoroutine("CreateBattery");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[4] = false;
        }
    }
    public IEnumerator StopCreateBattery()
    {
        StopCoroutine("CreateBattery");
        CoroutineManager.AllCoroutineBooleans[4] = false;
        inventoryManager.AddItemQuantity("BatteryCore", "REFINED", 1);
        inventoryManager.AddItemQuantity("Biofuel", "PROCESSED", 1);

        string currentBatteryCoreResource = inventoryManager.GetItemQuantity("BatteryCore", "REFINED").ToString();
        string currentBiofuelResource = inventoryManager.GetItemQuantity("Biofuel", "PROCESSED").ToString();

        for (int i = 0; i < batteryCoreTexts.Length; i++)
        {
            batteryCoreTexts[i].text = currentBatteryCoreResource;
        }
        for (int i = 0; i < biofuelTexts.Length; i++)
        {
            biofuelTexts[i].text = currentBiofuelResource;
        }


        imageToFill.fillAmount = 0f;
        yield return null;
    }
}
