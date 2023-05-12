using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoriesUnlock : MonoBehaviour
{
    public GameObject Intermediate;
    public GameObject Assembled;


    public void UnlockIntermediateCategory()
    {
        Intermediate.SetActive(true);
    }

    public void UnlockAssembledCategory()
    {
        Assembled.SetActive(true);
    }
}
