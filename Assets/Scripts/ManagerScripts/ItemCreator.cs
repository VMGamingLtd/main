using Assets.Scripts.ItemFactory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemManagement
{
    [Serializable]
    public class ItemDataJson // 1st layer
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
    public class EquipmentDataJson : ItemDataJson // 2nd layer
    {
        public int durability;
        public int maxDurability;
        public int strength;
        public int perception;
        public int intelligence;
        public int agility;
        public int charisma;
        public int willpower;
    }

    [Serializable]
    public class ToolDataJson : EquipmentDataJson // 3rd layer
    {
        public float productionSpeed;
        public float materialCost;
        public float outcomeRate;
    }

    [Serializable]
    public class HelmetDataJson : EquipmentDataJson // 3rd layer
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int gasProtection;
        public int explosionProtection;
        public int shieldPoints;
        public int hitPoints;
        public int visibilityRadius;
        public int explorationRadius;
        public int pickupRadius;

    }

    [Serializable]
    public class SuitDataJson : EquipmentDataJson // 3rd layer
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int gasProtection;
        public int explosionProtection;
        public int shieldPoints;
        public int hitPoints;
        public int energyCapacity;
        public int inventorySlots;
    }

    [Serializable]
    public class ItemData : MonoBehaviour // 1st layer mono behaviour
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float quantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
    }

    [Serializable]
    public class EquipmentData : ItemData // 2nd layer mono behaviour
    {
        public int durability;
        public int maxDurability;
        public int strength;
        public int perception;
        public int intelligence;
        public int agility;
        public int charisma;
        public int willpower;
    }

    [Serializable]
    public class ToolData : EquipmentData
    {
        public float productionSpeed;
        public float materialCost;
        public float outcomeRate;
    }

    [Serializable]
    public class HelmetData : EquipmentData // 3rd layer mono behaviour
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int gasProtection;
        public int explosionProtection;
        public int shieldPoints;
        public int hitPoints;
        public int visibilityRadius;
        public int explorationRadius;
        public int pickupRadius;
    }

    [Serializable]
    public class SuitData : EquipmentData // 3rd layer mono behaviour
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int gasProtection;
        public int explosionProtection;
        public int shieldPoints;
        public int hitPoints;
        public int energyCapacity;
        public int inventorySlots;
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

        [Serializable]
        private class SuitDataJsonArray
        {
            public List<SuitDataJson> items;
        }

        [Serializable]
        private class HelmetDataJsonArray
        {
            public List<HelmetDataJson> items;
        }

        [Serializable]
        private class ToolDataJsonArray
        {
            public List<ToolDataJson> items;
        }

        private List<ItemDataJson> itemDataList;
        private List<SuitDataJson> suitDataList;
        private List<HelmetDataJson> helmetDataList;
        private List<ToolDataJson> toolDataList;

        private void Awake()
        {
            string jsonText = Assets.Scripts.Models.ItemsListJson.json;
            ItemDataJsonArray itemDataArray = JsonUtility.FromJson<ItemDataJsonArray>(jsonText);
            if (itemDataArray != null)
            {
                itemDataList = itemDataArray.items;
            }

            string suitJsonText = Assets.Scripts.Models.SuitListJson.json;
            SuitDataJsonArray suitDataArray = JsonUtility.FromJson<SuitDataJsonArray>(suitJsonText);
            if (suitDataArray != null)
            {
                suitDataList = suitDataArray.items;
            }

            string helmetJsonText = Assets.Scripts.Models.HelmetListJson.json;
            HelmetDataJsonArray helmetDataArray = JsonUtility.FromJson<HelmetDataJsonArray>(helmetJsonText);
            if (suitDataArray != null)
            {
                helmetDataList = helmetDataArray.items;
            }

            string toolJsonText = Assets.Scripts.Models.ToolsListJson.json;
            ToolDataJsonArray toolDataArray = JsonUtility.FromJson<ToolDataJsonArray>(toolJsonText);
            if (toolDataArray != null)
            {
                toolDataList = toolDataArray.items;
            }
        }
        public void Recreateitem(float quantity, string itemProduct, string itemType, string itemClass, string itemName,
            int index, float stackLimit, bool equipable, int ID, bool isEquipped, RectTransform rectTransform = null)
        {
            itemFactory.RecreateItem(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, rectTransform);
        }

        public void RecreateSuit(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit,
            bool equipable, int ID, bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection,
            int shieldPoints, int hitPoints, int energyCapacity, int durability, int maxDurability, int inventorySlots, int strength, int perception, int intelligence,
            int agility, int charisma, int willpower, RectTransform rectTransform = null)
        {
            itemFactory.RecreateSuit(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, physicalProtection,
                fireProtection, coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, energyCapacity, durability, maxDurability, inventorySlots,
                strength, perception, intelligence, agility, charisma, willpower, rectTransform);
        }

        public void RecreateHelmet(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit,
            bool equipable, int ID, bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection,
            int shieldPoints, int hitPoints, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma,
            int willpower, int visibilityRadius, int explorationRadius, int pickupRadius, RectTransform rectTransform = null)
        {
            itemFactory.RecreateHelmet(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, physicalProtection,
                fireProtection, coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, durability, maxDurability,
                strength, perception, intelligence, agility, charisma, willpower, visibilityRadius, explorationRadius, pickupRadius, rectTransform);
        }

        public void RecreateTool(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit,
            bool equipable, int ID, bool isEquipped, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma,
            int willpower, float productionSpeed, float materialCost, float outcomeRate, RectTransform rectTransform = null)
        {
            itemFactory.RecreateTool(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, durability,
                maxDurability, strength, perception, intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate, rectTransform);
        }

        public void CreateHelmet(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = helmetDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateHelmet(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index,
                itemData.stackLimit, itemData.equipable, itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.gasProtection, itemData.explosionProtection,
                itemData.shieldPoints, itemData.hitPoints, itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility,
                itemData.charisma, itemData.willpower, itemData.visibilityRadius, itemData.explorationRadius, itemData.pickupRadius, rectTransform);
        }

        public void CreateSuit(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = suitDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateSuit(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index,
                itemData.stackLimit, itemData.equipable, itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.gasProtection, itemData.explosionProtection,
                itemData.shieldPoints, itemData.hitPoints, itemData.energyCapacity, itemData.durability, itemData.maxDurability, itemData.inventorySlots, itemData.strength,
                itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, rectTransform);
        }

        public void CreateTool(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = toolDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateTool(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, itemData.productionSpeed,
                itemData.materialCost, itemData.outcomeRate, rectTransform);
        }
        public void CreateItem(int itemIndex, float? quantity = null)
        {
            var itemData = itemDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateItem(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable);
        }
        public void SplitTool(int itemIndex, float quantity)
        {
            var itemData = toolDataList[itemIndex];
            itemFactory.SplitTool(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, itemData.productionSpeed,
                itemData.materialCost, itemData.outcomeRate);
        }
        public void SplitHelmet(int itemIndex, float quantity)
        {
            var itemData = helmetDataList[itemIndex];
            itemFactory.SplitHelmet(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.gasProtection, itemData.explosionProtection, itemData.shieldPoints, itemData.hitPoints,
                itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, itemData.visibilityRadius,
                itemData.explorationRadius, itemData.pickupRadius);
        }
        public void SplitSuit(int itemIndex, float quantity)
        {
            var itemData = suitDataList[itemIndex];
            itemFactory.SplitSuit(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.gasProtection, itemData.explosionProtection, itemData.shieldPoints, itemData.hitPoints,
                itemData.energyCapacity, itemData.durability, itemData.maxDurability, itemData.inventorySlots, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility,
                itemData.charisma, itemData.willpower);
        }
        public void SplitItem(int itemIndex, float quantity)
        {
            var itemData = itemDataList[itemIndex];
            itemFactory.SplitItem(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable);
        }
    }
}
