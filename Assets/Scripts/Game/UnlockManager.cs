using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public GameObject Base;
    public GameObject Explore;


    public void UnlockBaseFeature()
    {
        Base.SetActive(true);
    }

    public void UnlockExploreFeature()
    {
        Explore.SetActive(true);
    }
}
