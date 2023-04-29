using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowDown : MonoBehaviour
{

    public void Sort()
    {
        var IndexNo = transform.GetSiblingIndex();
        IndexNo++;
        transform.SetSiblingIndex(IndexNo);
    }
}
