using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRows : MonoBehaviour
{
    public Transform m_ContentContainer;
    public GameObject m_Row;


    public void CreateRow()
    {
        var item_go = Instantiate(m_Row);
        item_go.transform.SetParent(m_ContentContainer);
    }
}
