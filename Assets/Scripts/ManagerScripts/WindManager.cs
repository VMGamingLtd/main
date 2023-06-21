using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WindManager : MonoBehaviour
{
    public GameObject windObjectPrefab;
    public Transform windObjectsParent;
    public TextMeshProUGUI windSpeedText;
    public TextMeshProUGUI windStatus;

    public float minWindSpeed = 0.1f;
    public float maxWindSpeed = 10f;
    private int windDataIndex = 15;
    private Color lightBlueColor = new Color(1f, 0.33f, 0.86f, 1f);
    private Color originalColor = new Color(0.7f, 0.7f, 0.7f, 1f);
    public float updateTime = 300f; // 5 minutes in seconds
    public static string Planet0WindStatus = "Calm";

    private GameObject[] windObjects;

    private void Start()
    {
        windObjects = new GameObject[30];
        PopulateWindObjects();
        InvokeRepeating("UpdateWind", 0f, updateTime);
        if (Planet0WindStatus == "Calm")
        {
            GenerateCalmWeather();
        }
        else if (Planet0WindStatus == "Wind")
        {
            GenerateWindyWeather();
        }
        else if (Planet0WindStatus == "Storm")
        {
            GenerateStormWeather();
        }
        else if (Planet0WindStatus == "Typhoon")
        {
            GenerateTyphoonWeather();
        }
        else if (Planet0WindStatus == "Hurricane")
        {
            GenerateHurricaneWeather();
        }
        else if (Planet0WindStatus == "Tornado")
        {
            GenerateTornadoWeather();
        }
    }

    private void PopulateWindObjects()
    {
        for (int i = 0; i < windObjects.Length; i++)
        {
            GameObject windObject = Instantiate(windObjectPrefab, windObjectsParent);
            Image imageComponent = windObject.GetComponent<Image>();

            float deviationPercentage = Random.Range(-0.2f, 0.2f);
            float windSpeed = Random.Range(minWindSpeed, maxWindSpeed);
            float deviation = windSpeed * deviationPercentage;
            float finalWindSpeed = windSpeed + deviation;

            imageComponent.fillAmount = finalWindSpeed / maxWindSpeed;

            if (i >= 12 && i <= 19)
            {
                float alpha = Mathf.Lerp(1f, 0.4f, Mathf.Abs(windDataIndex - i) / 1.5f);
                imageComponent.color = new Color(lightBlueColor.r, lightBlueColor.g, lightBlueColor.b, alpha);
            }

            windObjects[i] = windObject;

            if (i == windDataIndex)
            {
                windDataIndex = i;
            }
        }
    }

    private void UpdateWind()
    {
        // Generate a new random wind speed
        float newWindSpeed = Random.Range(minWindSpeed, maxWindSpeed);
        // Destroy the windObject at index 0
        if (windObjects[0] != null)
        {
            Destroy(windObjects[0]);
        }

        // Shift the windObjects array elements to the left
        for (int i = 0; i < windObjects.Length - 1; i++)
        {
            windObjects[i] = windObjects[i + 1];

            // Update the color of windObjects 13 to 18
            Image imageComponent = windObjects[i].GetComponent<Image>();
            if (i == 11)
            {
                imageComponent.color = originalColor;
            }
            if (i >= 13 && i <= 18)
            {
                float alpha = Mathf.Lerp(1f, 0.4f, Mathf.Abs(windDataIndex - i) / 2f);
                imageComponent.color = new Color(lightBlueColor.r, lightBlueColor.g, lightBlueColor.b, alpha);
            }
        }

        // Create a new windObject at index 29
        GameObject newWindObject = Instantiate(windObjectPrefab, windObjectsParent);
        Image newImageComponent = newWindObject.GetComponent<Image>();
        newImageComponent.fillAmount = newWindSpeed / maxWindSpeed;

        // Add the new windObject at index 29
        windObjects[windObjects.Length - 1] = newWindObject;

        // Update the wind speed text
        float windImageAmount = windObjects[15].GetComponent<Image>().fillAmount;
        float newWindStatus = windImageAmount * maxWindSpeed;
        windSpeedText.text = newWindStatus.ToString("F2");

    }

    private string TranslateWindStatus(string language, string windStatus)
    {
        switch (language)
        {
            case "Russian":
                return TranslateToRussian(windStatus);
            case "Chinese":
                return TranslateToChinese(windStatus);
            case "Slovak":
                return TranslateToSlovak(windStatus);
            default:
                return windStatus;
        }
    }

    private string TranslateToRussian(string windStatus)
    {
        switch (windStatus)
        {
            case "Calm":
                return "Спокойно";
            case "Windy":
                return "Ветрено";
            case "Storm":
                return "Шторм";
            case "Typhoon":
                return "Тайфун";
            case "Hurricane":
                return "Ураган";
            case "Tornado":
                return "Торнадо";
            default:
                return windStatus;
        }
    }

    private string TranslateToChinese(string windStatus)
    {
        switch (windStatus)
        {
            case "Calm":
                return "平静";
            case "Windy":
                return "有风";
            case "Storm":
                return "暴风雨";
            case "Typhoon":
                return "台风";
            case "Hurricane":
                return "飓风";
            case "Tornado":
                return "龙卷风";
            default:
                return windStatus;
        }
    }

    private string TranslateToSlovak(string windStatus)
    {
        switch (windStatus)
        {
            case "Calm":
                return "Pokojné";
            case "Windy":
                return "Veterno";
            case "Storm":
                return "Búrka";
            case "Typhoon":
                return "Tajfún";
            case "Hurricane":
                return "Hurikán";
            case "Tornado":
                return "Tornádo";
            default:
                return windStatus;
        }
    }

    private void UpdateWindStatusText(string language)
    {
        windStatus.text = TranslateWindStatus(language, Planet0WindStatus);
    }

    public void GenerateCalmWeather()
    {
        Planet0WindStatus = "Calm";
        minWindSpeed = 1f;
        maxWindSpeed = 8f;
        UpdateWindStatusText(Application.systemLanguage.ToString());
    }

    public void GenerateWindyWeather()
    {
        Planet0WindStatus = "Windy";
        minWindSpeed = 9;
        maxWindSpeed = 25f;
        UpdateWindStatusText(Application.systemLanguage.ToString());
    }

    public void GenerateStormWeather()
    {
        Planet0WindStatus = "Storm";
        minWindSpeed = 25;
        maxWindSpeed = 50f;
        UpdateWindStatusText(Application.systemLanguage.ToString());
    }

    public void GenerateTyphoonWeather()
    {
        Planet0WindStatus = "Typhoon";
        minWindSpeed = 51;
        maxWindSpeed = 100f;
        UpdateWindStatusText(Application.systemLanguage.ToString());
    }

    public void GenerateHurricaneWeather()
    {
        Planet0WindStatus = "Hurricane";
        minWindSpeed = 101f;
        maxWindSpeed = 200f;
        UpdateWindStatusText(Application.systemLanguage.ToString());
    }

    public void GenerateTornadoWeather()
    {
        Planet0WindStatus = "Tornado";
        minWindSpeed = 201f;
        maxWindSpeed = 300f;
        UpdateWindStatusText(Application.systemLanguage.ToString());
    }
}
