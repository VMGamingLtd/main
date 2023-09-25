using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Globalization;
using Assets.Scripts.ItemFactory;
using System.IO;

namespace ItemManagement
{
    [Serializable]
    public class ItemDataJson
    {
        public int index;
        public float quantity;
        public float stackLimit;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
    }
    public class ItemData : MonoBehaviour
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float itemQuantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
    }
    public class ItemCreator : MonoBehaviour
    {
        public GameObject[] itemPrefabs;
        public ItemFactory itemFactory;

        [Serializable]
        private class ItemDataJsonArray
        {
            public List<ItemDataJson> items;
        }

        private List<ItemDataJson> itemDataList;

        private void Awake()
        {
            string filePath = Path.Combine(Application.dataPath, "Scripts/Models/ItemsList.json");

            if (File.Exists(filePath))
            {
                string jsonText = File.ReadAllText(filePath);
                ItemDataJsonArray itemDataArray = JsonUtility.FromJson<ItemDataJsonArray>(jsonText);
                if (itemDataArray != null)
                {
                    itemDataList = itemDataArray.items;
                }
            }
            else
            {
                Debug.LogError("ItemsList.json not found at: " + filePath);
            }
        }
        public void CreateItem(int itemIndex, float? quantity = null)
        {
            var itemData = itemDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateItem(finalQuantity, itemPrefabs[itemIndex], itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit);
        }
        public void SplitItem(int itemIndex, float quantity)
        {
            var itemData = itemDataList[itemIndex];
            itemFactory.SplitItem(quantity, itemPrefabs[itemIndex], itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit);
        }
    }
}
