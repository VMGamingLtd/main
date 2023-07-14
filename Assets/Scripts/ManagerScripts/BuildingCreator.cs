using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BuildingManagement
{
    public class BuildingItemData : MonoBehaviour
    {
        public string itemType;
        public Vector3 buildingPosition;
        public string consumedItem;
        public string consumedItemQuality;
        public int consumedQuantity;
        public float consumedTime;
    }
    public class BuildingCreator : MonoBehaviour
    {
        public GameObject[] buildingPrefabs;
        public GameObject newItemParent;
        public BuildingManager buildingManager;
        private BuildingItemData itemData;

        public void CreateBuilding(GameObject draggedObject, string itemType)
        {
            itemData = draggedObject.AddComponent<BuildingItemData>();
            itemData.itemType = itemType;
            itemData.buildingPosition = draggedObject.GetComponent<RectTransform>().anchoredPosition3D;

            if (draggedObject.name == "BiofuelGenerator(Clone)")
            {
                itemData.consumedItem = "Biofuel";
                itemData.consumedItemQuality = "PROCESSED";
                itemData.consumedQuantity = 1;
                itemData.consumedTime = 10;
            }

            // Add the item to the building manager's item array
            buildingManager.AddToItemArray(itemType, draggedObject);

            // Adding this class will start corotuine on the building object itself
            draggedObject.AddComponent<BuildingCycles>();
        }
    }
}
