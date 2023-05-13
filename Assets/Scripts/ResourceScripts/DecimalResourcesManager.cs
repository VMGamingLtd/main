using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DecimalResourceManager
{
    // planet natural resources
    public static float Planet0AtmosphereResource = 90;
    public static float Planet0FloraResource = 47;
    public static float Planet0WaterResource = 51;


    // population resources
    public static float Planet0Oxygen = 0;
    public static float Planet0OxygenIncrementor = 0;
    public static float Planet0Water = 0;
    public static float Planet0WaterIncrementor = 0;
    public static float Planet0Energy = 0;
    public static float Planet0EnergyIncrementor = 0;

    public static string AddDecimalResource(ref float resourceName, float amount)
    {
        resourceName += amount;
        return resourceName.ToString();
    }

    public static string ReduceDecimalResource(ref float resourceName, float amount)
    {
        resourceName -= amount;
        return resourceName.ToString();
    }

    public static string ResetDecimalResource(ref float resourceName)
    {
        resourceName = 0;
        return resourceName.ToString();
    }
}