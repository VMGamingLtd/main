using ItemManagement;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ItemFactory
{
    public class ItemFactory : MonoBehaviour
    {
        public InventoryManager inventoryManager;
        public EquipmentManager equipmentManager;
        public static int ItemCreationID = 0;
        private float remainingQuantity;

        public GameObject[] slotButtons = new GameObject[9];
        public GameObject[] placeholderImages = new GameObject[9];

        public void RecreateItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass,
            string prefabName, int index, float stackLimit, bool equipable, int ID, bool isEquipped, RectTransform rectTransform = null)
        {
            // if the object was in the general inventory list spawn it there
            if (rectTransform == null)
            {
                GameObject newItem = Instantiate(prefab);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
                FillGeneralItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
                newItem.transform.localScale = Vector3.one;
            }
            else
            {
                // if the object is  equipped that means it was in the Inventory tab under the icons of equipped objects, we have to assign it there
                GameObject newItem = Instantiate(prefab, rectTransform);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
                FillGeneralItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();

                // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                rectTransform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);
                AlignObject(newItem, rectTransform);
            }
        }
        public void CreateItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable)
        {
            bool itemFound = false;
            bool spliRemainingQuantity = true;
            if (inventoryManager.itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray) && itemArray.Length > 0)
            {
                foreach (GameObject item in itemArray)
                {
                    ItemData existingItemData = item.GetComponent<ItemData>();
                    if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                    {
                        // The item already exists, increment the quantity for the rest of the quantity order
                        existingItemData.quantity += quantity;
                        spliRemainingQuantity = false;
                        if (existingItemData.quantity > existingItemData.stackLimit)
                        {
                            spliRemainingQuantity = true;
                            remainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                            existingItemData.quantity -= remainingQuantity;
                        }
                        UpdateItemCountText(item, existingItemData);
                        itemFound = true;
                        break;
                    }
                }
                if (spliRemainingQuantity)
                {
                    foreach (GameObject item in itemArray)
                    {
                        ItemData existingItemData = item.GetComponent<ItemData>();
                        if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                        {
                            existingItemData.quantity += remainingQuantity;
                            if (existingItemData.quantity > existingItemData.stackLimit)
                            {
                                float newRemainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                                SplitItem(newRemainingQuantity, prefab, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable);
                                existingItemData.quantity -= remainingQuantity;
                            }
                            UpdateItemCountText(item, existingItemData);
                            break;
                        }
                    }
                }
            }
            if (!itemFound)
            {
                // Create the item once and set the initial quantity
                GameObject newItem = Instantiate(prefab);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
                FillGeneralItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
                newItem.transform.localScale = Vector3.one;
            }
        }

        public void RecreateSuit(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int ID, bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints,
            int energyCapacity, int durability, int maxDurability, int inventorySlots, int strength, int perception, int intelligence, int agility, int charisma, int willpower,
            RectTransform rectTransform = null)
        {
            // if the object was in the general inventory list spawn it there
            if (rectTransform == null)
            {
                GameObject newItem = Instantiate(prefab);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                SuitData newItemData = newItem.GetComponent<SuitData>() ?? newItem.AddComponent<SuitData>();
                FillSuitItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, physicalProtection, fireProtection,
                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, energyCapacity, durability, maxDurability, inventorySlots, strength, perception,
                    intelligence, agility, charisma, willpower, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
                newItem.transform.localScale = Vector3.one;
            }
            else
            {
                // if the object is  equipped that means it was in the Inventory tab under the icons of equipped objects, we have to assign it there
                GameObject newItem = Instantiate(prefab, rectTransform);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                SuitData newItemData = newItem.GetComponent<SuitData>() ?? newItem.AddComponent<SuitData>();
                FillSuitItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, physicalProtection, fireProtection,
                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, energyCapacity, durability, maxDurability, inventorySlots, strength, perception,
                    intelligence, agility, charisma, willpower, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();

                // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                rectTransform.Find(Constants.EmptyButton)?.GetComponent<Image>()?.gameObject.SetActive(false);
                AlignObject(newItem, rectTransform);
            }
        }

        public void RecreateHelmet(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int ID, bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints, int durability,
            int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, int visibilityRadius, int explorationRadius, int pickupRadius,
            RectTransform rectTransform = null)
        {
            // if the object was in the general inventory list spawn it there
            if (rectTransform == null)
            {
                GameObject newItem = Instantiate(prefab);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                HelmetData newItemData = newItem.GetComponent<HelmetData>() ?? newItem.AddComponent<HelmetData>();
                FillHelmetItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, physicalProtection, fireProtection,
                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, durability, maxDurability, strength, perception, intelligence, agility,
                    charisma, willpower, visibilityRadius, explorationRadius, pickupRadius, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
            }
            else
            {
                // if the object is  equipped that means it was in the Inventory tab under the icons of equipped objects, we have to assign it there
                GameObject newItem = Instantiate(prefab, rectTransform);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                HelmetData newItemData = newItem.GetComponent<HelmetData>() ?? newItem.AddComponent<HelmetData>();
                FillHelmetItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, physicalProtection, fireProtection,
                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, durability, maxDurability, strength, perception, intelligence, agility,
                    charisma, willpower, visibilityRadius, explorationRadius, pickupRadius, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();

                // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                rectTransform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);
                AlignObject(newItem, rectTransform);
            }
        }

        public void RecreateTool(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int ID, bool isEquipped, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, float productionSpeed,
            float materialCost, float outcomeRate, RectTransform rectTransform = null)
        {
            // if the object was in the general inventory list spawn it there
            if (rectTransform == null)
            {
                GameObject newItem = Instantiate(prefab);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                ToolData newItemData = newItem.GetComponent<ToolData>() ?? newItem.AddComponent<ToolData>();
                FillToolData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, durability, maxDurability, strength, perception,
                        intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
            }
            else
            {
                // if the object is  equipped that means it was in the Inventory tab under the icons of equipped objects, we have to assign it there
                GameObject newItem = Instantiate(prefab, rectTransform);
                InitiatePrefab(newItem, prefabName, itemType, equipable);
                ToolData newItemData = newItem.GetComponent<ToolData>() ?? newItem.AddComponent<ToolData>();
                FillToolData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, isEquipped, durability, maxDurability, strength, perception,
                        intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate, ID);
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();

                // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                rectTransform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);
                AlignObject(newItem, rectTransform);
            }
        }

        public void CreateSuit(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints, int energyCapacity,
            int durability, int maxDurability, int inventorySlots, int strength, int perception, int intelligence, int agility, int charisma, int willpower, RectTransform rectTransform = null)
        {
            bool itemFound = false;
            bool spliRemainingQuantity = true;
            if (inventoryManager.itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray) && itemArray.Length > 0)
            {
                foreach (GameObject item in itemArray)
                {
                    SuitData existingItemData = item.GetComponent<SuitData>();
                    if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                    {
                        // The item already exists, increment the quantity for the rest of the quantity order
                        existingItemData.quantity += quantity;
                        spliRemainingQuantity = false;
                        if (existingItemData.quantity > existingItemData.stackLimit)
                        {
                            spliRemainingQuantity = true;
                            remainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                            existingItemData.quantity -= remainingQuantity;
                        }
                        UpdateItemCountText(item, existingItemData);
                        itemFound = true;
                        break;
                    }
                }
                if (spliRemainingQuantity)
                {
                    foreach (GameObject item in itemArray)
                    {
                        SuitData existingItemData = item.GetComponent<SuitData>();
                        if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                        {
                            existingItemData.quantity += remainingQuantity;
                            if (existingItemData.quantity > existingItemData.stackLimit)
                            {
                                float newRemainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                                SplitSuit(newRemainingQuantity, prefab, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, physicalProtection, fireProtection,
                                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, energyCapacity, durability, maxDurability, inventorySlots, strength,
                                    perception, intelligence, agility, charisma, willpower);
                                existingItemData.quantity -= remainingQuantity;
                            }
                            UpdateItemCountText(item, null, existingItemData);
                            break;
                        }
                    }
                }
            }
            if (!itemFound)
            {
                if (rectTransform != null)
                {
                    // CREATING AND ALSO EQUIPPING THE ITEM RIGHT AWAY (Start game situation)
                    GameObject newItem = Instantiate(prefab, rectTransform);
                    InitiatePrefab(newItem, prefabName, itemType, equipable);
                    SuitData newItemData = newItem.GetComponent<SuitData>() ?? newItem.AddComponent<SuitData>();
                    FillSuitItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, true, physicalProtection, fireProtection,
                        coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, energyCapacity, durability, maxDurability, inventorySlots, strength, perception,
                        intelligence, agility, charisma, willpower, null);
                    inventoryManager.AddToItemArray(itemProduct, newItem);
                    UpdateItemCountText(newItem, newItemData);
                    newItem.AddComponent<ItemUse>();

                    // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                    rectTransform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);
                    AlignObject(newItem, rectTransform);
                    equipmentManager.EquipSuit(newItemData);
                }
                else
                {
                    // Create the item once and set the initial quantity
                    GameObject newItem = Instantiate(prefab);
                    InitiatePrefab(newItem, prefabName, itemType, equipable);
                    SuitData newItemData = newItem.GetComponent<SuitData>() ?? newItem.AddComponent<SuitData>();
                    FillSuitItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false, physicalProtection, fireProtection,
                        coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, energyCapacity, durability, maxDurability, inventorySlots, strength, perception,
                        intelligence, agility, charisma, willpower);

                    // Add the new item to the itemArrays dictionary
                    inventoryManager.AddToItemArray(itemProduct, newItem);

                    // Update the CountInventory text
                    UpdateItemCountText(newItem, null, newItemData);
                    newItem.AddComponent<ItemUse>();
                    newItem.transform.localScale = Vector3.one;
                }

            }
        }

        public void CreateHelmet(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints, int durability, int maxDurability,
            int strength, int perception, int intelligence, int agility, int charisma, int willpower, int visibilityRadius, int explorationRadius, int pickupRadius, RectTransform rectTransform = null)
        {
            bool itemFound = false;
            bool spliRemainingQuantity = true;
            if (inventoryManager.itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray) && itemArray.Length > 0)
            {
                foreach (GameObject item in itemArray)
                {
                    HelmetData existingItemData = item.GetComponent<HelmetData>();
                    if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                    {
                        // The item already exists, increment the quantity for the rest of the quantity order
                        existingItemData.quantity += quantity;
                        spliRemainingQuantity = false;
                        if (existingItemData.quantity > existingItemData.stackLimit)
                        {
                            spliRemainingQuantity = true;
                            remainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                            existingItemData.quantity -= remainingQuantity;
                        }
                        UpdateItemCountText(item, existingItemData);
                        itemFound = true;
                        break;
                    }
                }
                if (spliRemainingQuantity)
                {
                    foreach (GameObject item in itemArray)
                    {
                        HelmetData existingItemData = item.GetComponent<HelmetData>();
                        if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                        {
                            existingItemData.quantity += remainingQuantity;
                            if (existingItemData.quantity > existingItemData.stackLimit)
                            {
                                float newRemainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                                SplitHelmet(newRemainingQuantity, prefab, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, physicalProtection, fireProtection,
                                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, durability, maxDurability, strength, perception, intelligence, agility,
                                    charisma, willpower, visibilityRadius, explorationRadius, pickupRadius);
                                existingItemData.quantity -= remainingQuantity;
                            }
                            UpdateItemCountText(item, null, null, existingItemData);
                            break;
                        }
                    }
                }
            }
            if (!itemFound)
            {
                if (rectTransform != null)
                {
                    // CREATING AND ALSO EQUIPPING THE ITEM RIGHT AWAY (Start game situation)
                    GameObject newItem = Instantiate(prefab, rectTransform);
                    InitiatePrefab(newItem, prefabName, itemType, equipable);
                    HelmetData newItemData = newItem.GetComponent<HelmetData>() ?? newItem.AddComponent<HelmetData>();
                    FillHelmetItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, true, physicalProtection, fireProtection,
                        coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, durability, maxDurability, strength, perception, intelligence, agility,
                        charisma, willpower, visibilityRadius, explorationRadius, pickupRadius, null);
                    inventoryManager.AddToItemArray(itemProduct, newItem);
                    UpdateItemCountText(newItem, newItemData);
                    newItem.AddComponent<ItemUse>();

                    // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                    rectTransform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);
                    AlignObject(newItem, rectTransform);
                    equipmentManager.EquipHelmet(newItemData);
                }
                else
                {
                    // Create the item once and set the initial quantity
                    GameObject newItem = Instantiate(prefab);
                    InitiatePrefab(newItem, prefabName, itemType, equipable);
                    HelmetData newItemData = newItem.GetComponent<HelmetData>() ?? newItem.AddComponent<HelmetData>();
                    FillHelmetItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false, physicalProtection, fireProtection,
                        coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, durability, maxDurability, strength, perception, intelligence, agility,
                        charisma, willpower, visibilityRadius, explorationRadius, pickupRadius);

                    // Add the new item to the itemArrays dictionary
                    inventoryManager.AddToItemArray(itemProduct, newItem);

                    // Update the CountInventory text
                    UpdateItemCountText(newItem, null, null, newItemData);
                    newItem.AddComponent<ItemUse>();
                    newItem.transform.localScale = Vector3.one;
                }
            }
        }

        public void CreateTool(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, float productionSpeed, float materialCost,
            float outcomeRate, RectTransform rectTransform = null)
        {
            bool itemFound = false;
            bool spliRemainingQuantity = true;
            if (inventoryManager.itemArrays.TryGetValue(itemProduct, out GameObject[] itemArray) && itemArray.Length > 0)
            {
                foreach (GameObject item in itemArray)
                {
                    ToolData existingItemData = item.GetComponent<ToolData>();
                    if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                    {
                        // The item already exists, increment the quantity for the rest of the quantity order
                        existingItemData.quantity += quantity;
                        spliRemainingQuantity = false;
                        if (existingItemData.quantity > existingItemData.stackLimit)
                        {
                            spliRemainingQuantity = true;
                            remainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                            existingItemData.quantity -= remainingQuantity;
                        }
                        UpdateItemCountText(item, existingItemData);
                        itemFound = true;
                        break;
                    }
                }
                if (spliRemainingQuantity)
                {
                    foreach (GameObject item in itemArray)
                    {
                        ToolData existingItemData = item.GetComponent<ToolData>();
                        if (item.name == prefabName && existingItemData.quantity < existingItemData.stackLimit)
                        {
                            existingItemData.quantity += remainingQuantity;
                            if (existingItemData.quantity > existingItemData.stackLimit)
                            {
                                float newRemainingQuantity = existingItemData.quantity - existingItemData.stackLimit;
                                SplitTool(newRemainingQuantity, prefab, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, durability, maxDurability, strength,
                                    perception, intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate);
                                existingItemData.quantity -= remainingQuantity;
                            }
                            UpdateItemCountText(item, null, null, null, existingItemData);
                            break;
                        }
                    }
                }
            }
            if (!itemFound)
            {
                if (rectTransform != null)
                {
                    // CREATING AND ALSO EQUIPPING THE ITEM RIGHT AWAY (Start game situation)
                    GameObject newItem = Instantiate(prefab, rectTransform);
                    InitiatePrefab(newItem, prefabName, itemType, equipable);
                    ToolData newItemData = newItem.GetComponent<ToolData>() ?? newItem.AddComponent<ToolData>();
                    FillToolData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, true, durability, maxDurability, strength, perception,
                        intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate);
                    inventoryManager.AddToItemArray(itemProduct, newItem);
                    UpdateItemCountText(newItem, newItemData);
                    newItem.AddComponent<ItemUse>();

                    // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                    rectTransform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);
                    AlignObject(newItem, rectTransform);
                    equipmentManager.EquipTool(newItemData);
                }
                else
                {
                    // Create the item once and set the initial quantity
                    GameObject newItem = Instantiate(prefab);
                    InitiatePrefab(newItem, prefabName, itemType, equipable);
                    ToolData newItemData = newItem.GetComponent<ToolData>() ?? newItem.AddComponent<ToolData>();
                    FillToolData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false, durability, maxDurability, strength, perception,
                        intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate);

                    // Add the new item to the itemArrays dictionary
                    inventoryManager.AddToItemArray(itemProduct, newItem);

                    // Update the CountInventory text
                    UpdateItemCountText(newItem, null, null, null, newItemData);
                    newItem.AddComponent<ItemUse>();
                    newItem.transform.localScale = Vector3.one;
                }

            }
        }

        public void SplitItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable)
        {
            // Split the item, meaning that it will be duplicated
            GameObject newItem = Instantiate(prefab);
            InitiatePrefab(newItem, prefabName, itemType, equipable);
            ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
            FillGeneralItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false);

            // Add the new item to the itemArrays dictionary
            inventoryManager.AddToItemArray(itemProduct, newItem);

            // Update the CountInventory text
            UpdateItemCountText(newItem, newItemData);
            newItem.AddComponent<ItemUse>();
        }

        public void SplitSuit(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints,
            int energyCapacity, int durability, int maxDurability, int inventorySlots, int strength, int perception, int intelligence, int agility, int charisma, int willpower)
        {
            // Split the item, meaning that it will be duplicated
            GameObject newItem = Instantiate(prefab);
            InitiatePrefab(newItem, prefabName, itemType, equipable);
            SuitData newItemData = newItem.GetComponent<SuitData>() ?? newItem.AddComponent<SuitData>();
            FillSuitItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false, physicalProtection, fireProtection,
                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, energyCapacity, durability, maxDurability, inventorySlots, strength, perception,
                    intelligence, agility, charisma, willpower);

            inventoryManager.AddToItemArray(itemProduct, newItem);
            UpdateItemCountText(newItem, null, newItemData);
            newItem.AddComponent<ItemUse>();
        }
        public void SplitHelmet(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints, int durability, int maxDurability,
            int strength, int perception, int intelligence, int agility, int charisma, int willpower, int visibilityRadius, int explorationRadius, int pickupRadius)
        {
            // Split the item, meaning that it will be duplicated
            GameObject newItem = Instantiate(prefab);
            InitiatePrefab(newItem, prefabName, itemType, equipable);
            HelmetData newItemData = newItem.GetComponent<HelmetData>() ?? newItem.AddComponent<HelmetData>();
            FillHelmetItemData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false, physicalProtection, fireProtection,
                    coldProtection, gasProtection, explosionProtection, shieldPoints, hitPoints, durability, maxDurability, strength, perception, intelligence, agility,
                    charisma, willpower, visibilityRadius, explorationRadius, pickupRadius);

            inventoryManager.AddToItemArray(itemProduct, newItem);
            UpdateItemCountText(newItem, null, null, newItemData);
            newItem.AddComponent<ItemUse>();
        }
        public void SplitTool(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, float productionSpeed, float materialCost,
            float outcomeRate)
        {
            // Split the item, meaning that it will be duplicated
            GameObject newItem = Instantiate(prefab);
            InitiatePrefab(newItem, prefabName, itemType, equipable);
            ToolData newItemData = newItem.GetComponent<ToolData>() ?? newItem.AddComponent<ToolData>();
            FillToolData(newItemData, quantity, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable, false, durability, maxDurability, strength, perception,
                        intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate);

            inventoryManager.AddToItemArray(itemProduct, newItem);
            UpdateItemCountText(newItem, null, null, null, newItemData);
            newItem.AddComponent<ItemUse>();
        }
        private void UpdateItemCountText(GameObject item, ItemData itemData = null, SuitData suitData = null, HelmetData helmetData = null, ToolData toolData = null)
        {
            TextMeshProUGUI existingCountText = item.transform.Find(Constants.CountInventory)?.GetComponent<TextMeshProUGUI>();
            if (existingCountText != null)
            {
                string quantityText = "";

                if (itemData != null)
                {
                    quantityText = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                }
                else if (suitData != null)
                {
                    quantityText = suitData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                }
                else if (helmetData != null)
                {
                    quantityText = helmetData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                }
                else if (toolData != null)
                {
                    quantityText = toolData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                }

                if (quantityText.EndsWith(".00"))
                {
                    quantityText = quantityText[..^3];
                }

                existingCountText.text = quantityText;
            }
        }
        private void InitiatePrefab(GameObject newItem, string prefabName, string itemType, bool equipable)
        {
            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
            newItem.transform.localScale = new Vector3(1f, 1f, 1f);
            newItem.name = prefabName;
            newItem.transform.Find("ChildName").name = prefabName;
            if (itemType == Constants.Suit || itemType == Constants.Helmet || itemType == Constants.Fabricator)
            {
                newItem.transform.Find("Image").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(prefabName);
            }
            else
            {
                newItem.transform.Find("Image").GetComponent<Image>().sprite = AssignSpriteToSlot(prefabName);
            }
            var dragAndDropComponent = newItem.GetComponent<DragAndDrop>();
            CheckItemTypes(newItem, itemType, equipable, dragAndDropComponent);
        }
        private Sprite AssignSpriteToSlot(string spriteName)
        {
            Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>(Constants.ResourceIcons, spriteName);
            return sprite;
        }

        private Sprite AssignEquipmentSpriteToSlot(string spriteName)
        {
            Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>(Constants.EquipmentIcons, spriteName);
            return sprite;
        }

        private void AlignObject(GameObject newItem, RectTransform rectTransform)
        {
            newItem.transform.SetParent(rectTransform);
            newItem.transform.SetAsLastSibling();
            newItem.transform.localPosition = Vector3.one;
            newItem.transform.localScale = Vector3.one;
        }

        private void FillGeneralItemData(ItemData newItemData, float quantity, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable, bool isEquipped, int? ID = null)
        {
            newItemData.quantity = quantity;
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            newItemData.index = index;
            newItemData.itemName = prefabName;
            newItemData.equipable = equipable;
            newItemData.stackLimit = stackLimit;
            newItemData.isEquipped = isEquipped;
            newItemData.ID = ID ?? ItemCreationID++;
        }
        private void FillSuitItemData(SuitData newItemData, float quantity, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints, int energyCapacity,
            int durability, int maxDurability, int inventorySlots, int strength, int perception, int intelligence, int agility, int charisma, int willpower, int? ID = null)
        {
            newItemData.quantity = quantity;
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            newItemData.index = index;
            newItemData.itemName = prefabName;
            newItemData.equipable = equipable;
            newItemData.stackLimit = stackLimit;
            newItemData.isEquipped = isEquipped;
            newItemData.physicalProtection = physicalProtection;
            newItemData.fireProtection = fireProtection;
            newItemData.coldProtection = coldProtection;
            newItemData.gasProtection = gasProtection;
            newItemData.explosionProtection = explosionProtection;
            newItemData.shieldPoints = shieldPoints;
            newItemData.hitPoints = hitPoints;
            newItemData.energyCapacity = energyCapacity;
            newItemData.durability = durability;
            newItemData.maxDurability = maxDurability;
            newItemData.inventorySlots = inventorySlots;
            newItemData.strength = strength;
            newItemData.perception = perception;
            newItemData.intelligence = intelligence;
            newItemData.agility = agility;
            newItemData.charisma = charisma;
            newItemData.willpower = willpower;
            newItemData.ID = ID ?? ItemCreationID++;
        }

        private void FillHelmetItemData(HelmetData newItemData, float quantity, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int gasProtection, int explosionProtection, int shieldPoints, int hitPoints, int durability,
            int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, int visibilityRadius, int explorationRadius, int pickupRadius, int? ID = null)
        {
            newItemData.quantity = quantity;
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            newItemData.index = index;
            newItemData.itemName = prefabName;
            newItemData.equipable = equipable;
            newItemData.stackLimit = stackLimit;
            newItemData.isEquipped = isEquipped;
            newItemData.physicalProtection = physicalProtection;
            newItemData.fireProtection = fireProtection;
            newItemData.coldProtection = coldProtection;
            newItemData.gasProtection = gasProtection;
            newItemData.explosionProtection = explosionProtection;
            newItemData.shieldPoints = shieldPoints;
            newItemData.hitPoints = hitPoints;
            newItemData.durability = durability;
            newItemData.maxDurability = maxDurability;
            newItemData.strength = strength;
            newItemData.perception = perception;
            newItemData.intelligence = intelligence;
            newItemData.agility = agility;
            newItemData.charisma = charisma;
            newItemData.willpower = willpower;
            newItemData.visibilityRadius = visibilityRadius;
            newItemData.explorationRadius = explorationRadius;
            newItemData.pickupRadius = pickupRadius;
            newItemData.ID = ID ?? ItemCreationID++;
        }

        private void FillToolData(ToolData newItemData, float quantity, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable,
            bool isEquipped, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, float productionSpeed, float materialCost,
            float outcomeRate, int? ID = null)
        {
            newItemData.quantity = quantity;
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            newItemData.index = index;
            newItemData.itemName = prefabName;
            newItemData.equipable = equipable;
            newItemData.stackLimit = stackLimit;
            newItemData.isEquipped = isEquipped;
            newItemData.durability = durability;
            newItemData.maxDurability = maxDurability;
            newItemData.strength = strength;
            newItemData.perception = perception;
            newItemData.intelligence = intelligence;
            newItemData.agility = agility;
            newItemData.charisma = charisma;
            newItemData.willpower = willpower;
            newItemData.productionSpeed = productionSpeed;
            newItemData.materialCost = materialCost;
            newItemData.outcomeRate = outcomeRate;
            newItemData.ID = ID ?? ItemCreationID++;
        }


        private void CheckItemTypes(GameObject newItem, string itemType, bool equipable, DragAndDrop dragAndDropComponent)
        {
            if (equipable)
            {
                newItem.transform.Find("SlotIcon").GetComponent<Image>().enabled = true;
            }

            if (itemType == Constants.Plants && equipable || itemType == Constants.Meat && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[7]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[7]);
                newItem.transform.Find("SlotIcon").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(Constants.EmptyFoodSlot);
            }
            else if (itemType == Constants.Energy && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[5]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[5]);
                newItem.transform.Find("SlotIcon").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(Constants.EmptyEnergySlot);
            }
            else if (itemType == Constants.Oxygen && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[6]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[6]);
                newItem.transform.Find("SlotIcon").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(Constants.EmptyOxygenSlot);
            }
            else if (itemType == Constants.Liquid && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[8]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[8]);
                newItem.transform.Find("SlotIcon").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(Constants.EmptyWaterSlot);
            }
            else if (itemType == Constants.Suit && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[1]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[1]);
                newItem.transform.Find("SlotIcon").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(Constants.EmptySuitSlot);
            }
            else if (itemType == Constants.Helmet && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[0]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[0]);
                newItem.transform.Find("SlotIcon").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(Constants.EmptyHelmetSlot);
            }
            else if (itemType == Constants.Fabricator && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[2]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[2]);
                newItem.transform.Find("SlotIcon").GetComponent<Image>().sprite = AssignEquipmentSpriteToSlot(Constants.EmptyLeftHandSlot);
            }
        }
        private GameObject[] ExtendArray(GameObject[] oldArray, GameObject newElement)
        {
            int oldLength = oldArray.Length;
            GameObject[] newArray = new GameObject[oldLength + 1];

            for (int i = 0; i < oldLength; i++)
            {
                newArray[i] = oldArray[i];
            }

            newArray[oldLength] = newElement;

            return newArray;
        }
    }
}
