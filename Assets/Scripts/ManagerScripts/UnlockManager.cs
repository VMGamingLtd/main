using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public GoalManager goalManager;
    public GameObject goalGenerator;

    public void StartFeature()
    {
        StartCoroutine(UnlockBaseFeature());
    }

    public IEnumerator UnlockBaseFeature()
    {
        Animation animation = goalGenerator.GetComponent<Animation>();
        if (animation != null)
        {
            animation.Play("Success");
            yield return new WaitForSeconds(1f);
            goalManager.ChangeGoal("BuildBase");
            animation.Play("Idle");
        }
    }
}
