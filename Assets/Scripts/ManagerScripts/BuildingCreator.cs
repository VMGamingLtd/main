using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BuildingManagement
{
    public class BuildingItemData : MonoBehaviour
        {
            public string itemProduct;
            public string itemType;
            public string itemClass;
        }
    public class BuildingCreator : MonoBehaviour
    {
        public GameObject[] buildingPrefabs;
        public BuildingManager buildingManager;
        public void CreatePowerGenerator()
        {
            CreateBuilding(buildingPrefabs[0], "BASIC", "ENERGY", "CLASS-F", buildingPrefabs[0].name);
        }
        public void CreatePlantField()
        {
            CreateBuilding(buildingPrefabs[1], "BASIC", "PLANTS", "CLASS-F", buildingPrefabs[1].name);
        }
        public void CreateWaterWell()
        {
            CreateBuilding(buildingPrefabs[2], "PROCESSED", "LIQUID", "CLASS-F", buildingPrefabs[2].name);
        }

        private void CreateBuilding(GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName)
        {
            // Create the item once and set the initial quantity
            GameObject newItem = Instantiate(prefab);
            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);

            // Get or add the ItemData component to the new item
            BuildingItemData newItemData = newItem.GetComponent<BuildingItemData>();
            if (newItemData == null)
            {
                newItemData = newItem.AddComponent<BuildingItemData>();
            }
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            buildingManager.AddToItemArray(itemProduct, newItem);
        }
    }
}
