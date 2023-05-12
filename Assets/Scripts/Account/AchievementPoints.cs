using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class AchievementPoints
{
    public static int achievementPoints = 0;

    public static void AddPoints(int amount)
    {
        achievementPoints += amount;
    }

    public static int GetPoints()
    {
        return achievementPoints;
    }

    public static void ResetPoints()
    {
        achievementPoints = 0;
    }
}
