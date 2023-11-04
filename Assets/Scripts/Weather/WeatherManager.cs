using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherManager : MonoBehaviour
{
    public Image fillImage;
    public static int planet0UV = 5;
    public static string planet0Weather = "Sunny";
    private float maxPlanet0UV = 20f;
    private float fillTime = 2f;
    public static float planet0Temperature = 28;
    public TextMeshProUGUI planet0UVText;
    public TextMeshProUGUI planet0WeatherText;
    public TextMeshProUGUI planet0TemperatureText;
    private TranslationManager translationManager;

    void Awake()
    {
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
        float fillAmount = Mathf.Clamp01(planet0UV / maxPlanet0UV);
        StartCoroutine(FillOverTime(fillAmount));
        UpdateWeather(planet0Temperature, planet0Weather);
    }
    public IEnumerator FillOverTime(float targetFillAmount)
    {
        float initialFillAmount = fillImage.fillAmount;
        float timer = 0f;
        planet0UVText.text = WeatherManager.planet0UV.ToString();

        while (timer < fillTime)
        {
            timer += Time.deltaTime;
            float fillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, timer / fillTime);
            fillImage.fillAmount = fillAmount;
            yield return null;
        }

        fillImage.fillAmount = targetFillAmount;
    }

    public void SetUVAmount()
    {
        if (ButtonManager.MenuButtonTypeOn == "Production")
        {
            float fillAmount = Mathf.Clamp01(planet0UV / maxPlanet0UV);
            StartCoroutine(FillOverTime(fillAmount));
        }
    }
    public void IncreaseUVAmount(int PlanetUV)
    {
        PlanetUV++;
        SetUVAmount();
    }
    public void UpdateWeather(float temperature, string weather)
    {

        planet0Temperature = temperature;
        planet0Weather = weather;
        planet0WeatherText.text = translationManager.Translate(weather);
        planet0TemperatureText.text = GetFormattedTemperatureString(planet0Temperature);

    }
    private string GetFormattedTemperatureString(float temperature)
    {
        RegionInfo regionInfo = new(CultureInfo.CurrentCulture.Name);
        string formatSpecifier = regionInfo.IsMetric ? "F1" : "F0";

        CultureInfo cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

        return temperature.ToString(formatSpecifier, cultureInfo) + (regionInfo.IsMetric ? "°C" : "°F");
    }

}
