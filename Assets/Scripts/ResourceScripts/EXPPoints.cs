using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class EXPPoints
{
    public static int expPoints = 0;

    public static void AddPoints(int amount)
    {
        expPoints += amount;
    }

    public static int GetPoints()
    {
        return expPoints;
    }

    public static void ResetPoints()
    {
        expPoints = 0;
    }
}
