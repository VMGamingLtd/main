using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoroutineManager : MonoBehaviour
{
    public GlobalCalculator globalCalculator;
    public GameObject loadingBar;
    public GameObject saveSlots;
    public GameObject loginMenu;
    public GameObject newGamePopup;
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


    // Planet 0 building bars
    public Image imageToFillCollectPlants;
    public Image imageToFillCollectWater;
    public Image imageToFillCollectBiofuel;
    public Image imageToFillCollectWaterBottle;
    public Image imageToFillCreateBattery;

    private float targetFillAmountPlanet0bb = 1f;
    private float currentFillAmountPlanet0bb = 0f;

    // after production is finished add +1 to the counter
    public TextMeshProUGUI[] plantsTexts;
    public TextMeshProUGUI[] waterTexts;
    public TextMeshProUGUI[] biofuelTexts;
    public TextMeshProUGUI[] waterBottleTexts;
    public TextMeshProUGUI[] batteryTexts;
    public TextMeshProUGUI[] planetStatsTexts;



    public CategoriesUnlock categoriesUnlock;

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
            StartCoroutine("StopCollectWaterBottle");
        }
        else if (CoroutineManager.IndexNumber == 4)
        {
            StartCoroutine("StopCreateBattery");
        }
    }

    IEnumerator WaitForLoadingBar () {

        bool registereduser = CoroutineManager.registeredUser;
        globalCalculator.GameStarted = false;
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
        textObject.text = "INITIALIZING GAME...";
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

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFillCollectPlants.fillAmount = currentFillAmountPlanet0bb;
            timer += UnityEngine.Time.deltaTime;
            yield return null;
        }
        imageToFillCollectPlants.fillAmount = 0f;

        PlayerResources.AddCurrentResource(ref PlayerResources.Plants, 5);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 1);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            ExpBar.fillAmount = 0f;
            currentExpText.text = "0";
            Level.ResetCurrentResource(ref Level.PlayerCurrentExp);
            int result = playerMaxExp * 2;
            Level.AddCurrentResource(ref Level.PlayerMaxExp, result);
            Level.AddCurrentResource(ref Level.PlayerLevel, 1);
            levelText.text = Level.GetCurrentResource(ref Level.PlayerLevel).ToString();
            Level.AddCurrentResource(ref Level.StatPoints, 5);
            Level.AddCurrentResource(ref Level.SkillPoints, 1);
            statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
            skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
        }

        string currentPlantResource = PlayerResources.GetCurrentResource(ref PlayerResources.Plants);

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
        imageToFillCollectPlants.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectWater()
    {
        float timer = 0f;
        float fillTimePlanet0bb = 2f;

        while (timer < fillTimePlanet0bb)
        {
            currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
            imageToFillCollectWater.fillAmount = currentFillAmountPlanet0bb;
            timer += UnityEngine.Time.deltaTime;
            yield return null;
        }

        PlayerResources.AddCurrentResource(ref PlayerResources.Water, 10);
        Level.AddCurrentResource(ref Level.PlayerCurrentExp, 1);
        currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

        int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
        int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
        float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
        ExpBar.fillAmount = fillAmount;

        if (playerCurrentExp >= playerMaxExp)
        {
            ExpBar.fillAmount = 0f;
            currentExpText.text = "0";
            Level.ResetCurrentResource(ref Level.PlayerCurrentExp);
            int result = playerMaxExp * 2;
            Level.AddCurrentResource(ref Level.PlayerMaxExp, result);
            Level.AddCurrentResource(ref Level.PlayerLevel, 1);
            levelText.text = Level.GetCurrentResource(ref Level.PlayerLevel).ToString();
            Level.AddCurrentResource(ref Level.StatPoints, 5);
            Level.AddCurrentResource(ref Level.SkillPoints, 1);
            statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
            skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
        }

        string currentWaterResource = PlayerResources.GetCurrentResource(ref PlayerResources.Water);

        for (int i = 0; i < waterTexts.Length; i++)
        {
            waterTexts[i].text = currentWaterResource;
        }
        imageToFillCollectWater.fillAmount = 0f;

        StartCoroutine("CollectWater");
    }

    public IEnumerator StopCollectWater()
    {
        StopCoroutine("CollectWater");
        CoroutineManager.AllCoroutineBooleans[1] = false;
        imageToFillCollectWater.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectBiofuel()
    {
        int plants = PlayerResources.Plants;

        if (plants >= 20)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 3f;
            CoroutineManager.AllCoroutineBooleans[2] = true;

            PlayerResources.ReduceCurrentResource(ref PlayerResources.Plants, 20);

            string currentPlantResource = PlayerResources.GetCurrentResource(ref PlayerResources.Plants);

            for (int i = 0; i < plantsTexts.Length; i++)
            {
                plantsTexts[i].text = currentPlantResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFillCollectBiofuel.fillAmount = currentFillAmountPlanet0bb;
                timer += UnityEngine.Time.deltaTime;
                yield return null;
            }
            imageToFillCollectBiofuel.fillAmount = 0f;

            PlayerResources.AddCurrentResource(ref PlayerResources.Biofuel, 1);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 5);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                ExpBar.fillAmount = 0f;
                currentExpText.text = "0";
                Level.ResetCurrentResource(ref Level.PlayerCurrentExp);
                int result = playerMaxExp * 2;
                Level.AddCurrentResource(ref Level.PlayerMaxExp, result);
                Level.AddCurrentResource(ref Level.PlayerLevel, 1);
                levelText.text = Level.GetCurrentResource(ref Level.PlayerLevel).ToString();
                Level.AddCurrentResource(ref Level.StatPoints, 5);
                Level.AddCurrentResource(ref Level.SkillPoints, 1);
                statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
                skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
            }

            string currentBiofuelResource = PlayerResources.GetCurrentResource(ref PlayerResources.Biofuel);

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
        PlayerResources.AddCurrentResource(ref PlayerResources.Plants, 20);

        string currentPlantResource = PlayerResources.GetCurrentResource(ref PlayerResources.Plants);

        for (int i = 0; i < plantsTexts.Length; i++)
        {
            plantsTexts[i].text = currentPlantResource;
        }

        CoroutineManager.AllCoroutineBooleans[2] = false;
        imageToFillCollectBiofuel.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CollectWaterBottle()
    {
        int water = PlayerResources.Water;

        if (water >= 50)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 3f;
            CoroutineManager.AllCoroutineBooleans[3] = true;

            PlayerResources.ReduceCurrentResource(ref PlayerResources.Water, 50);

            string currentWaterResource = PlayerResources.GetCurrentResource(ref PlayerResources.Water);

            for (int i = 0; i < waterTexts.Length; i++)
            {
                waterTexts[i].text = currentWaterResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFillCollectWaterBottle.fillAmount = currentFillAmountPlanet0bb;
                timer += UnityEngine.Time.deltaTime;
                yield return null;
            }

            PlayerResources.AddCurrentResource(ref PlayerResources.WaterBottle, 1);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 5);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                ExpBar.fillAmount = 0f;
                currentExpText.text = "0";
                Level.ResetCurrentResource(ref Level.PlayerCurrentExp);
                int result = playerMaxExp * 2;
                Level.AddCurrentResource(ref Level.PlayerMaxExp, result);
                Level.AddCurrentResource(ref Level.PlayerLevel, 1);
                levelText.text = Level.GetCurrentResource(ref Level.PlayerLevel).ToString();
                Level.AddCurrentResource(ref Level.StatPoints, 5);
                Level.AddCurrentResource(ref Level.SkillPoints, 1);
                statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
                skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
            }

            imageToFillCollectWaterBottle.fillAmount = 0f;

            string currentWaterBottleResource = PlayerResources.GetCurrentResource(ref PlayerResources.WaterBottle);

            for (int i = 0; i < waterBottleTexts.Length; i++)
            {
                waterBottleTexts[i].text = currentWaterBottleResource;
            }

            StartCoroutine("CollectWaterBottle");
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[3] = false;
        }
    }

    public IEnumerator StopCollectWaterBottle()
    {
        StopCoroutine("CollectWaterBottle");
        CoroutineManager.AllCoroutineBooleans[3] = true;
        PlayerResources.AddCurrentResource(ref PlayerResources.Water, 50);

        string currentWaterResource = PlayerResources.GetCurrentResource(ref PlayerResources.Water);

        for (int i = 0; i < waterTexts.Length; i++)
        {
            waterTexts[i].text = currentWaterResource;
        }

        imageToFillCollectWaterBottle.fillAmount = 0f;
        yield return null;
    }

    public IEnumerator CreateBattery()
    {
        int biofuel = PlayerResources.Biofuel;

        if (biofuel >= 10)
        {
            float timer = 0f;
            float fillTimePlanet0bb = 5f;
            CoroutineManager.AllCoroutineBooleans[4] = true;

            PlayerResources.ReduceCurrentResource(ref PlayerResources.Biofuel, 10);

            string currentBiofuelResource = PlayerResources.GetCurrentResource(ref PlayerResources.Biofuel);

            for (int i = 0; i < biofuelTexts.Length; i++)
            {
                biofuelTexts[i].text = currentBiofuelResource;
            }

            while (timer < fillTimePlanet0bb)
            {
                currentFillAmountPlanet0bb = Mathf.Lerp(0f, targetFillAmountPlanet0bb, timer / fillTimePlanet0bb);
                imageToFillCreateBattery.fillAmount = currentFillAmountPlanet0bb;
                timer += UnityEngine.Time.deltaTime;
                yield return null;
            }

            PlayerResources.AddCurrentResource(ref PlayerResources.Battery, 1);
            Level.AddCurrentResource(ref Level.PlayerCurrentExp, 10);
            currentExpText.text = Level.GetCurrentResource(ref Level.PlayerCurrentExp).ToString();

            int playerCurrentExp = Level.GetCurrentResource(ref Level.PlayerCurrentExp);
            int playerMaxExp = Level.GetCurrentResource(ref Level.PlayerMaxExp);
            float fillAmount = (float)playerCurrentExp / (float)playerMaxExp;
            ExpBar.fillAmount = fillAmount;

            if (playerCurrentExp >= playerMaxExp)
            {
                ExpBar.fillAmount = 0f;
                currentExpText.text = "0";
                Level.ResetCurrentResource(ref Level.PlayerCurrentExp);
                int result = playerMaxExp * 2;
                Level.AddCurrentResource(ref Level.PlayerMaxExp, result);
                Level.AddCurrentResource(ref Level.PlayerLevel, 1);
                levelText.text = Level.GetCurrentResource(ref Level.PlayerLevel).ToString();
                Level.AddCurrentResource(ref Level.StatPoints, 5);
                Level.AddCurrentResource(ref Level.SkillPoints, 1);
                statPointsText.text = Level.GetCurrentResource(ref Level.StatPoints).ToString();
                skillPointsText.text = Level.GetCurrentResource(ref Level.SkillPoints).ToString();
            }

            imageToFillCreateBattery.fillAmount = 0f;

            string currentBatteryResource = PlayerResources.GetCurrentResource(ref PlayerResources.Battery);

            for (int i = 0; i < batteryTexts.Length; i++)
            {
                batteryTexts[i].text = currentBatteryResource;
            }
        }
        else
        {
            CoroutineManager.AllCoroutineBooleans[4] = false;
        }
    }
    public IEnumerator StopCreateBattery()
    {
        StopCoroutine("CreateBattery");
        CoroutineManager.AllCoroutineBooleans[4] = true;
        PlayerResources.AddCurrentResource(ref PlayerResources.Biofuel, 10);

        string currentBiofuelResource = PlayerResources.GetCurrentResource(ref PlayerResources.Biofuel);

        for (int i = 0; i < biofuelTexts.Length; i++)
        {
            biofuelTexts[i].text = currentBiofuelResource;
        }


        imageToFillCreateBattery.fillAmount = 0f;
        yield return null;
    }
}
