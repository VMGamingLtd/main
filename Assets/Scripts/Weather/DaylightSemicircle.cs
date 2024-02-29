using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DaylightSemicircle : MonoBehaviour
{
    public RectTransform sunIcon;
    public RectTransform[] points;
    public Image semicircleImage;
    public string sunriseTime;
    public string sunsetTime;
    public float updateInterval = 1f;

    private void Start()
    {
        {
            StartCoroutine(UpdateSunIconPosition());
            StartCoroutine(UpdateFillAmount());
        }
    }
    public void RunAllCoroutines()
    {
        if (ButtonManager.MenuButtonTypeOn == "Production")
        {
            StartCoroutine(UpdateSunIconPosition());
            StartCoroutine(UpdateFillAmount());
        }
    }

    private IEnumerator UpdateSunIconPosition()
    {
        while (true)
        {
            DateTime sunrise = DateTime.ParseExact(sunriseTime, "h:mm tt", null);
            DateTime sunset = DateTime.ParseExact(sunsetTime, "h:mm tt", null);
            DateTime currentTime = DateTime.Now;

            float startTime = sunrise.Hour * 60f + sunrise.Minute;
            float endTime = sunset.Hour * 60f + sunset.Minute;
            float currentTimeValue = currentTime.Hour * 60f + currentTime.Minute;

            float normalizedTime = Mathf.InverseLerp(startTime, endTime, currentTimeValue);
            int pointIndex = Mathf.FloorToInt(normalizedTime * (points.Length - 1));

            // Get the positions of the two nearest points
            RectTransform pointA = points[pointIndex];
            RectTransform pointB = pointIndex < points.Length - 1 ? points[pointIndex + 1] : points[pointIndex];

            // Calculate the interpolated position based on the percentage
            float percentage = (normalizedTime - (float)pointIndex / (points.Length - 1)) * (points.Length - 1);
            Vector2 position = Vector2.Lerp(pointA.anchoredPosition, pointB.anchoredPosition, percentage);

            sunIcon.anchoredPosition = position;

            yield return new WaitForSeconds(updateInterval);
        }
    }

    private IEnumerator UpdateFillAmount()
    {
        while (true)
        {
            DateTime sunrise = DateTime.ParseExact(sunriseTime, "h:mm tt", null);
            DateTime sunset = DateTime.ParseExact(sunsetTime, "h:mm tt", null);
            DateTime currentTime = DateTime.Now;

            if (currentTime < sunrise || currentTime > sunset)
            {
                semicircleImage.fillAmount = 0f;
            }
            else
            {
                float duration = (float)(sunset - sunrise).TotalSeconds;
                float elapsed = (float)(currentTime - sunrise).TotalSeconds;
                float fillAmount = elapsed / duration;
                semicircleImage.fillAmount = fillAmount;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
