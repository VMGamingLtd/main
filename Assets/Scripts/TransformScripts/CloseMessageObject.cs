using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMessageObject : MonoBehaviour
{
    private GameObject myObject;
    public void CloseMessage()
    {
        myObject = transform.parent.gameObject;
        myObject.SetActive(false);
    }
}
