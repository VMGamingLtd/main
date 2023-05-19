using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class PlayerResources
{
    public static int PlayerOxygen = 100;
    public static int PlayerWater = 15;
    public static int PlayerEnergy = 4;
    public static int PlayerHunger = 23;

    public static int AddCurrentResource(ref int currentResourceSet, int amount)
    {
        currentResourceSet += amount;
        return currentResourceSet;
    }

    public static int ReduceCurrentResource(ref int currentResourceSet, int amount)
    {
        currentResourceSet -= amount;
        if (currentResourceSet < 0)
        {
            currentResourceSet = 0;
        }
        return currentResourceSet;
    }

    public static string GetCurrentResource(ref int currentResourceSet)
    {
        return currentResourceSet.ToString();
    }

    public static string ResetCurrentResource(ref int currentResourceSet)
    {
        currentResourceSet = 0;
        return currentResourceSet.ToString();
    }
}
