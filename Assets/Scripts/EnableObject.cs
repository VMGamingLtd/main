using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : MonoBehaviour
{
    public GameObject myObject;

    public void EnabledObject()
    {
        myObject.SetActive (true);
    }

    public void DisabledObject()
    {
        myObject.SetActive (false);
    }
}
