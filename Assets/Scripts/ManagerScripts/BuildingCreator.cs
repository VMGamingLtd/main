using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BuildingManagement
{
    public class BuildingItemData : MonoBehaviour
        {
            public string itemType;
        }
    public class BuildingCreator : MonoBehaviour
    {
        public GameObject[] buildingPrefabs;
        public GameObject newItemParent;
        public BuildingManager buildingManager;
        public void CreateSteamGenerator()
        {
            CreateBuilding(buildingPrefabs[0], "ENERGY", buildingPrefabs[0].name, newItemParent);
        }
        public void CreatePlantField()
        {
            CreateBuilding(buildingPrefabs[1], "PLANTS", buildingPrefabs[1].name, newItemParent);
        }
        public void CreateWaterWell()
        {
            CreateBuilding(buildingPrefabs[2], "LIQUID", buildingPrefabs[2].name, newItemParent);
        }

        private void CreateBuilding(GameObject prefab, string itemType, string prefabName, GameObject parentObject)
        {
            GameObject newItem = Instantiate(prefab, parentObject.transform.position, parentObject.transform.rotation);


            // ItemData component to the new item
            BuildingItemData newItemData = newItem.GetComponent<BuildingItemData>();
            if (newItemData == null)
            {
                newItemData = newItem.AddComponent<BuildingItemData>();
            }
            newItemData.itemType = itemType;
            buildingManager.AddToItemArray(itemType, newItem);
        }
    }
}
