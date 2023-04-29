using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


public class List : MonoBehaviour
{
    public GameObject StudentList;
    public Transform DateLocation;

    
    public void SortByDate() {
        List<Transform> children = new List<Transform>();
        List<string> dates = new List<string>();

        foreach (Transform child in StudentList.transform) { 
            if  (child.gameObject.activeInHierarchy){
                children.Add(child);
                DateLocation = child.transform.Find("RegistrationDate/Text");
                string rowDate = DateLocation.GetComponent<TextMeshProUGUI>().text;
                dates.Add(rowDate);
                }
        }
        dates.Reverse();
        int RowIndex = 0;
        foreach (var rowDate in dates) {
                RowIndex++;
        }

        foreach (Transform child in StudentList.transform) { 
            if  (child.gameObject.activeInHierarchy){
                DateLocation = child.transform.Find("RegistrationDate/Text");
                transform.SetSiblingIndex(RowIndex);

                }
        }







        dates.Reverse();
        foreach (var rowDate in dates) {
                Debug.Log(rowDate);
        //children.Reverse();
        }
    }
}

