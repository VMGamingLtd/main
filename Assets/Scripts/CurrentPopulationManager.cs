using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentPopulationManager
{
    public static int Planet0CurrentPopulation = 0;
    public static int Planet0PopulationIncrementor = 0;
    public static int Planet1CurrentPopulation = 0;
    public static int Planet2CurrentPopulation = 0;

    public static string AddCurrentPopulation(ref int currentPopulationSet, int amount)
    {
        currentPopulationSet += amount;
        return currentPopulationSet.ToString();
    }

    public static string ReduceCurrentPopulation(ref int currentPopulationSet, int amount)
    {
        currentPopulationSet -= amount;
        return currentPopulationSet.ToString();
    }

    public static string ResetCurrentPopulation(ref int currentPopulationSet)
    {
        currentPopulationSet = 0;
        return currentPopulationSet.ToString();
    }
}