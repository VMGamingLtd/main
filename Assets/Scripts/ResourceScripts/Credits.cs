using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class Credits
{
    public static float credits = 0;

    public static void AddCredits(float amount)
    {
        credits += amount;
    }

    public static float GetCredits()
    {
        return credits;
    }

    public static void ResetCredits()
    {
        credits = 0;
    }

}
