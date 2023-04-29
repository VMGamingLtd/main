using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static bool achievement1 = false;
    public static bool achievement2 = false;
    public static bool achievement3 = false;

    public static bool[] booleanArray = new bool[] { achievement1, achievement2, achievement3 };

    public static void EnableAchievement(ref bool achievementName, bool state)
    {
        achievementName = state;
    }


    public IEnumerator CheckAchievement()
    {
        yield return null;
        for (int i = 0; i < booleanArray.Length; i++)
        {
            if (booleanArray[0] == true)
            {
                Debug.Log("achievement 1 true");
                StartCoroutine(BuildOxygenGeneratorAchievement());
            }
        }
        
    }



    IEnumerator BuildOxygenGeneratorAchievement()
    {
        yield return null;
        Debug.Log("hahaha");
    }
}
