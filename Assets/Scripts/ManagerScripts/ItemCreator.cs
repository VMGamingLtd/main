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
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
    }
    public class ItemData : MonoBehaviour
    {
        public int ID;
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
        public TextAsset itemDataJson;

        [Serializable]
        private class ItemDataJsonArray
        {
            public List<ItemDataJson> items;
        }

        private List<ItemDataJson> itemDataList;

        private void Awake()
        {
            string filePath = Path.Combine(Application.dataPath, "Scripts/Models/AvailableItems.json");

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
                Debug.LogError("AvailableItems.json not found at: " + filePath);
            }
        }
        public void CreateItem(int itemIndex, float? quantity = null)
        {
            var itemData = itemDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateItem(finalQuantity, itemPrefabs[itemIndex], itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName);
        }
        public void CreateBiofuel(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[2], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[2].name);
        }
        public void CreateDistilledWater(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[3], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[3].name);
        }
        public void CreateBattery(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[4], "ASSEMBLED", "ENERGY", "CLASS-F", itemPrefabs[4].name);
        }
        public void CreateOxygenTanks(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[5], "ASSEMBLED", "OXYGEN", "CLASS-F", itemPrefabs[5].name);
        }
        public void CreateBatteryCore(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[6], "ENHANCED", "ENERGY", "CLASS-F", itemPrefabs[6].name);
        }
        public void CreateSteam(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[7], "BASIC", "GAS", "CLASS-F", itemPrefabs[7].name);
        }
        public void CreateWood(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[8], "BASIC", "PLANTS", "CLASS-F", itemPrefabs[8].name);
        }
        public void CreateIronOre(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[9], "BASIC", "MINERALS", "CLASS-F", itemPrefabs[9].name);
        }
        public void CreateCoal(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[10], "BASIC", "MINERALS", "CLASS-F", itemPrefabs[10].name);
        }
        public void CreateIronBeam(float quantity)
        {
            itemFactory.CreateItem(quantity, itemPrefabs[11], "PROCESSED", "METALS", "CLASS-F", itemPrefabs[11].name);
        }


        public void SplitPlants(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[0], "BASIC", "PLANTS", "CLASS-F", itemPrefabs[0].name);
        }
        public void SplitWater(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[1], "BASIC", "LIQUID", "CLASS-F", itemPrefabs[1].name);
        }
        public void SplitBiofuel(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[2], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[2].name);
        }
        public void SplitDistilledWater(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[3], "PROCESSED", "LIQUID", "CLASS-F", itemPrefabs[3].name);
        }
        public void SplitBattery(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[4], "ASSEMBLED", "ENERGY", "CLASS-F", itemPrefabs[4].name);
        }
        public void SplitOxygenTanks(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[5], "ASSEMBLED", "OXYGEN", "CLASS-F", itemPrefabs[5].name);
        }
        public void SplitBatteryCore(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[6], "ENHANCED", "ENERGY", "CLASS-F", itemPrefabs[6].name);
        }
        public void SplitSteam(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[7], "BASIC", "GAS", "CLASS-F", itemPrefabs[7].name);
        }
        public void SplitWood(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[8], "BASIC", "PLANTS", "CLASS-F", itemPrefabs[8].name);
        }
        public void SplitIronOre(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[9], "BASIC", "MINERALS", "CLASS-F", itemPrefabs[9].name);
        }
        public void SplitCoal(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[10], "BASIC", "MINERALS", "CLASS-F", itemPrefabs[10].name);
        }
        public void SplitIronBeam(float quantity)
        {
            itemFactory.SplitItem(quantity, itemPrefabs[11], "PROCESSED", "METALS", "CLASS-F", itemPrefabs[11].name);
        }
    }
}
