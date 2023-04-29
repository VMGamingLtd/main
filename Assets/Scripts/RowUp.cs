using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowUp : MonoBehaviour
{

    public void Sort()
    {
        var IndexNo = transform.GetSiblingIndex();
        if (IndexNo > 1) {
        IndexNo--;
        transform.SetSiblingIndex(IndexNo);
        }
    }
}
