using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingIncrementor : MonoBehaviour
{
    public TextMeshProUGUI[] buildingCounts;


    public void InitializeBuildingCounts()
    {
        buildingCounts[0].text = Planet0Buildings.Planet0BiofuelGeneratorBlueprint.ToString();
    }
}
