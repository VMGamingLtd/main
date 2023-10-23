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
        public static int ItemCreationID = 0;
        private float remainingQuantity;

        public GameObject[] slotButtons = new GameObject[11];
        public GameObject[] placeholderImages = new GameObject[11];

        public void RecreateItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass,
            string prefabName, int index, float stackLimit, bool equipable, int ID, bool isEquipped, RectTransform rectTransform = null)
        {
            // if the object was in the general inventory list spawn it there
            if (rectTransform == null)
            {
                GameObject newItem = Instantiate(prefab);
                newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
                newItem.name = prefabName;
                newItem.transform.Find("ChildName").name = prefabName;
                newItem.transform.Find("Image").GetComponent<Image>().sprite = AssignSpriteToSlot(prefabName);

                // we need to give a proper highlight object placeholder so when the object will be dragged, visible possibilities will be shown to player
                var dragAndDropComponent = newItem.GetComponent<DragAndDrop>();

                if (itemType == "PLANTS" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[9]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[9]);
                }
                else if (itemType == "ENERGY" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[7]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[7]);
                }
                else if (itemType == "OXYGEN" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[8]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[8]);
                }
                else if (itemType == "LIQUID" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[10]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[10]);
                }
                ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
                newItemData.itemQuantity = quantity;
                newItemData.itemProduct = itemProduct;
                newItemData.itemType = itemType;
                newItemData.itemClass = itemClass;
                newItemData.itemName = prefabName;
                newItemData.index = index;
                newItemData.equipable = equipable;
                newItemData.stackLimit = stackLimit;
                newItemData.ID = ID;
                newItemData.isEquipped = isEquipped;
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
                newItem.transform.localScale = Vector3.one;
            }
            else
            {
                // if the object is  equipped that means it was in the Inventory tab under the icons of equipped objects, we have to assign it there
                GameObject newItem = Instantiate(prefab, rectTransform);
                newItem.name = prefabName;
                newItem.transform.Find("ChildName").name = prefabName;
                newItem.transform.Find("Image").GetComponent<Image>().sprite = AssignSpriteToSlot(prefabName);

                // we need to give a proper highlight object placeholder so when the object will be dragged, visible possibilities will be shown to player
                var dragAndDropComponent = newItem.GetComponent<DragAndDrop>();

                if (itemType == "PLANTS" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[9]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[9]);
                }
                else if (itemType == "ENERGY" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[7]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[7]);
                }
                else if (itemType == "OXYGEN" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[8]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[8]);
                }
                else if (itemType == "LIQUID" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[10]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[10]);
                }
                ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
                newItemData.itemQuantity = quantity;
                newItemData.itemProduct = itemProduct;
                newItemData.itemType = itemType;
                newItemData.itemClass = itemClass;
                newItemData.itemName = prefabName;
                newItemData.index = index;
                newItemData.equipable = equipable;
                newItemData.stackLimit = stackLimit;
                newItemData.ID = ID;
                newItemData.isEquipped = isEquipped;
                inventoryManager.AddToItemArray(itemProduct, newItem);
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();

                // assign the game object into the Inventory UI under the buttons as a child and align the position to be in the middle of the button
                rectTransform.Find("EmptyButton")?.GetComponent<Image>()?.gameObject.SetActive(false);
                newItem.transform.SetParent(rectTransform);
                newItem.transform.SetAsLastSibling();
                newItem.transform.localPosition = Vector3.one;
                newItem.transform.localScale = Vector3.one;
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
                    if (item.name == prefabName && existingItemData.itemQuantity < existingItemData.stackLimit)
                    {
                        // The item already exists, increment the quantity for the rest of the quantity order
                        existingItemData.itemQuantity += quantity;
                        spliRemainingQuantity = false;
                        if (existingItemData.itemQuantity > existingItemData.stackLimit)
                        {
                            spliRemainingQuantity = true;
                            remainingQuantity = existingItemData.itemQuantity - existingItemData.stackLimit;
                            existingItemData.itemQuantity -= remainingQuantity;
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
                        if (item.name == prefabName && existingItemData.itemQuantity < existingItemData.stackLimit)
                        {
                            existingItemData.itemQuantity += remainingQuantity;
                            if (existingItemData.itemQuantity > existingItemData.stackLimit)
                            {
                                float newRemainingQuantity = existingItemData.itemQuantity - existingItemData.stackLimit;
                                SplitItem(newRemainingQuantity, prefab, itemProduct, itemType, itemClass, prefabName, index, stackLimit, equipable);
                                existingItemData.itemQuantity -= remainingQuantity;
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
                newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);

                // ItemTemplate attribute injection
                newItem.name = prefabName;
                newItem.transform.Find("ChildName").name = prefabName;
                newItem.transform.Find("Image").GetComponent<Image>().sprite = AssignSpriteToSlot(prefabName);

                // we need to give a proper highlight object placeholder so when the object will be dragged, visible possibilities will be shown to player
                var dragAndDropComponent = newItem.GetComponent<DragAndDrop>();

                if (itemType == "PLANTS" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[9]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[9]);
                }
                else if (itemType == "ENERGY" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[7]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[7]);
                }
                else if (itemType == "OXYGEN" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[8]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[8]);
                }
                else if (itemType == "LIQUID" && equipable)
                {
                    dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[10]);
                    dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[10]);
                }


                // Get or add the ItemData component to the new item
                ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();
                newItemData.itemQuantity = quantity;
                newItemData.itemProduct = itemProduct;
                newItemData.itemType = itemType;
                newItemData.itemClass = itemClass;
                newItemData.itemName = prefabName;
                newItemData.index = index;
                newItemData.equipable = equipable;
                newItemData.stackLimit = stackLimit;
                newItemData.ID = ItemCreationID++;
                newItemData.isEquipped = false;

                // Add the new item to the itemArrays dictionary
                inventoryManager.AddToItemArray(itemProduct, newItem);

                // Update the CountInventory text
                UpdateItemCountText(newItem, newItemData);
                newItem.AddComponent<ItemUse>();
                newItem.transform.localScale = Vector3.one;
            }
        }
        public void SplitItem(float quantity, GameObject prefab, string itemProduct, string itemType, string itemClass, string prefabName, int index, float stackLimit, bool equipable)
        {
            // Split the item, meaning that it will be duplicated
            GameObject newItem = Instantiate(prefab);
            newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
            newItem.transform.localScale = new Vector3(1f, 1f, 1f);

            // ItemTemplate attribute injection
            newItem.name = prefabName;
            newItem.transform.Find("ChildName").name = prefabName;
            newItem.transform.Find("Image").GetComponent<Image>().sprite = AssignSpriteToSlot(prefabName);

            // we need to give a proper highlight object placeholder so when the object will be dragged, visible possibilities will be shown to player
            var dragAndDropComponent = newItem.GetComponent<DragAndDrop>();

            if (itemType == "PLANTS" && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[9]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[9]);
            }
            else if (itemType == "ENERGY" && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[7]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[7]);
            }
            else if (itemType == "OXYGEN" && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[8]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[8]);
            }
            else if (itemType == "LIQUID" && equipable)
            {
                dragAndDropComponent.highlightObject = ExtendArray(dragAndDropComponent.highlightObject, slotButtons[10]);
                dragAndDropComponent.placeholderObjects = ExtendArray(dragAndDropComponent.placeholderObjects, placeholderImages[10]);
            }

            // Get or add the ItemData component to the new item
            ItemData newItemData = newItem.GetComponent<ItemData>() ?? newItem.AddComponent<ItemData>();

            newItemData.itemQuantity = quantity;
            newItemData.itemProduct = itemProduct;
            newItemData.itemType = itemType;
            newItemData.itemClass = itemClass;
            newItemData.index = index;
            newItemData.itemName = prefabName;
            newItemData.equipable = equipable;
            newItemData.stackLimit = stackLimit;
            newItemData.isEquipped = false;

            newItemData.ID = ItemCreationID++;

            // Add the new item to the itemArrays dictionary
            inventoryManager.AddToItemArray(itemProduct, newItem);

            // Update the CountInventory text
            UpdateItemCountText(newItem, newItemData);
            newItem.AddComponent<ItemUse>();
        }

        private void UpdateItemCountText(GameObject item, ItemData itemData)
        {
            TextMeshProUGUI existingCountText = item.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
            if (existingCountText != null)
            {
                existingCountText.text = itemData.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
            }
        }

        private Sprite AssignSpriteToSlot(string spriteName)
        {
            Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
            return sprite;
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
