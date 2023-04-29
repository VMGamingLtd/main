using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameHUB : MonoBehaviour
{
    public static int year;
    public static int month;
    public static int day;
    private float timer;
    public TextMeshProUGUI DateDisplay;

    private const float DAY_LENGTH = 1f; // One second equals one day

    void Start()
    {
        year = 2124;
        month = 8;
        day = 12;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= DAY_LENGTH)
        {
            timer -= DAY_LENGTH;
            day++;

            if (day > 30)
            {
                day = 1;
                month++;

                if (month > 12)
                {
                    month = 1;
                    year++;
                }
            }
            UpdateDate();
        }
    }

    public void UpdateDate()
    {
        DateDisplay.text = $"{GameHUB.day.ToString()}/{GameHUB.month.ToString()}/{GameHUB.year.ToString()}";
    }

    public void UpdatePopulation()
    {
        int populationPlanet0Check = CurrentPopulationManager.Planet0CurrentPopulation;

    }
}




