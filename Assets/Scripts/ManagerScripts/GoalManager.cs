using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public GameObject[] Goals;
    public static bool firstGoal = false;
    public static bool secondGoal = false;
    public static bool thirdGoal = false;

    public void ChangeGoal(string GoalName)
    {
        for (int i = 0; i < Goals.Length; i++)
        {
            if (Goals[i].name == GoalName)
            {
                Goals[i].SetActive(true);
            }
            else
            {
                Goals[i].SetActive(false);
            }
        }
    }
}
