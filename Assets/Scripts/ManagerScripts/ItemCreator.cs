using Assets.Scripts.ItemFactory;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        public bool equipable;
    }
    [Serializable]
    public class ItemData : MonoBehaviour
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float itemQuantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
    }
    public class ItemCreator : MonoBehaviour
    {
        public GameObject itemTemplate;
        public ItemFactory itemFactory;

        [Serializable]
        private class ItemDataJsonArray
        {
            public List<ItemDataJson> items;
        }

        private List<ItemDataJson> itemDataList;

        private void Awake()
        {
            string jsonText = Assets.Scripts.Models.ItemsListJson.json;
            ItemDataJsonArray itemDataArray = JsonUtility.FromJson<ItemDataJsonArray>(jsonText);
            if (itemDataArray != null)
            {
                itemDataList = itemDataArray.items;
            }
        }
        public void Recreateitem(float quantity, string itemProduct, string itemType, string itemClass, string itemName,
            int index, float stackLimit, bool equipable, int ID, bool isEquipped, RectTransform rectTransform = null)
        {
            itemFactory.RecreateItem(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, rectTransform);
        }
        public void CreateItem(int itemIndex, float? quantity = null)
        {
            var itemData = itemDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateItem(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable);
        }
        public void SplitItem(int itemIndex, float quantity)
        {
            var itemData = itemDataList[itemIndex];
            itemFactory.SplitItem(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable);
        }
    }
}
