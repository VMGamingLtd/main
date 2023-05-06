using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Level
{
    public static int PlayerLevel = 1;
    public static int PlayerCurrentExp = 1;
    public static int PlayerMaxExp = 20;
    public static int SkillPoints = 0;
    public static int StatPoints = 0;

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

    public static int GetCurrentResource(ref int currentResourceSet)
    {
        return currentResourceSet;
    }

    public static int ResetCurrentResource(ref int currentResourceSet)
    {
        currentResourceSet = 0;
        return currentResourceSet;
    }
}
