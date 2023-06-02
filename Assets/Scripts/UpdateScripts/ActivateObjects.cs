using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjects : MonoBehaviour
{
    public GameObject[] NoEnergyObjects;
    public void ActivateAllObjects()
    {
        foreach (GameObject obj in NoEnergyObjects)
        {
            obj.SetActive(true);
        }
    }
}
