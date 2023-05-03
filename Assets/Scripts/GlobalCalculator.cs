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
                    saveManager.SaveToJsonFile();
                    StartCoroutine(rotateImage.RotateOverTime(0.5f));
                }
                if (minutes > 60)
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

}




