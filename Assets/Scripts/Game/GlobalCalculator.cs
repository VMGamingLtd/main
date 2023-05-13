using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class GlobalCalculator : MonoBehaviour
{
    public static int hours = 0;
    public static int minutes = 0;
    public static int seconds = 0;
    private float timer;

    public SaveManager saveManager;

    // current date display
    public TextMeshProUGUI DateDisplay;
    public RotateImage rotateImage;
    public TextMeshProUGUI[] PlayerNeeds;

    public bool GameStarted = false;

    private const float delayTime = 1f;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (GameStarted == true)
        {
            timer += Time.deltaTime;
            if (timer >= delayTime)
            {
                timer -= delayTime;
                seconds++;

                if (seconds > 59)
                {
                    seconds = 1;
                    minutes++;
                    UpdatePlayerNeeds();
                    saveManager.SaveToJsonFile();
                    StartCoroutine(rotateImage.RotateOverTime(0.5f));
                }
                if (minutes > 59)
                {
                    minutes = 1;
                    hours++;
                }
                UpdateEverySecond();
            }
        }
    }

    public void UpdateEverySecond()
    {
        DateDisplay.text = $"{GlobalCalculator.hours.ToString("00")}:{GlobalCalculator.minutes.ToString("00")}:{GlobalCalculator.seconds.ToString("00")}";
    }

    public void UpdatePlayerNeeds()
    {
        int currentOxygen = PlayerResources.ReduceCurrentResource(ref PlayerResources.PlayerOxygen, 3);
        int currentWater = PlayerResources.ReduceCurrentResource(ref PlayerResources.PlayerWater, 3);
        int currentEnergy = PlayerResources.ReduceCurrentResource(ref PlayerResources.PlayerEnergy, 3);
        int currentHunger = PlayerResources.ReduceCurrentResource(ref PlayerResources.PlayerHunger, 3);

        PlayerNeeds[0].text = $"{currentOxygen}%";
        PlayerNeeds[1].text = $"{currentWater}%";
        PlayerNeeds[2].text = $"{currentEnergy}%";
        PlayerNeeds[3].text = $"{currentHunger}%";



        if (currentOxygen < 25)
        {
            PlayerNeeds[0].color = Color.red;
        }
        else if (currentOxygen < 50)
        {
            PlayerNeeds[0].color = Color.yellow;
        }
        else if (currentOxygen < 75)
        {
            PlayerNeeds[0].color = Color.green;
        }
        else if (currentOxygen <= 100)
        {
            PlayerNeeds[0].color = Color.white;
        }
        if (currentWater < 25)
        {
            PlayerNeeds[1].color = Color.red;
        }
        else if (currentWater < 50)
        {
            PlayerNeeds[1].color = Color.yellow;
        }
        else if (currentWater < 75)
        {
            PlayerNeeds[1].color = Color.green;
        }
        else if (currentWater <= 100)
        {
            PlayerNeeds[1].color = Color.white;
        }
        if (currentEnergy < 25)
        {
            PlayerNeeds[2].color = Color.red;
        }
        else if (currentEnergy < 50)
        {
            PlayerNeeds[2].color = Color.yellow;
        }
        else if (currentEnergy < 75)
        {
            PlayerNeeds[2].color = Color.green;
        }
        else if (currentEnergy <= 100)
        {
            PlayerNeeds[2].color = Color.white;
        }
        if (currentHunger < 25)
        {
            PlayerNeeds[3].color = Color.red;
        }
        else if (currentHunger < 50)
        {
            PlayerNeeds[3].color = Color.yellow;
        }
        else if (currentHunger < 75)
        {
            PlayerNeeds[3].color = Color.green;
        }
        else if (currentHunger <= 100)
        {
            PlayerNeeds[3].color = Color.white;
        }
    }
}




