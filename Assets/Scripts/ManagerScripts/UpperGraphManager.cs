using UnityEngine;
using UnityEngine.UI;
using BuildingManagement;

public class UpperGraphManager : MonoBehaviour
{
    public GameObject graphObjectPrefab;
    public Transform graphObjectList;
    private GameObject[] graphObjects = new GameObject[120];

    private void CreateOrUpdateGraphObject(int index, float fillAmount)
{
    if (graphObjects[index] == null)
    {
        GameObject graphObject = Instantiate(graphObjectPrefab, graphObjectList);
        graphObject.transform.position = Vector3.zero;

        Image imageComponent = graphObject.GetComponent<Image>();
        imageComponent.fillAmount = fillAmount;

        graphObjects[index] = graphObject;
    }
    else
    {
        Image imageComponent = graphObjects[index].GetComponent<Image>();
        imageComponent.fillAmount = fillAmount;
    }
}
    public void FillGraph(float[] cycleList)
    {
        if (BuildingIntervalTypes.BuildingIntervalTypeChanged)
        {
            ClearGraphObjects();
            BuildingIntervalTypes.BuildingIntervalTypeChanged = false;
        }
        for (int i = 0; i < cycleList.Length; i++)
        {
            float fillAmount = cycleList[i] / 10000.0f;
            CreateOrUpdateGraphObject(i, fillAmount);
        }

    }

    private void ClearGraphObjects()
    {
        foreach (GameObject graphObject in graphObjects)
        {
            if (graphObject != null)
            {
                Destroy(graphObject);
            }
        }
    }
}
