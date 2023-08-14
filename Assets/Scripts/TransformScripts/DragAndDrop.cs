using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;
using ItemManagement;
using TMPro;
using System.Globalization;
using UnityEditor.Build;

public class DragAndDrop : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject cloneObject;
    private static GameObject SelectedObj;
    private GameObject draggedObj;
    private bool isDragging = false;
    public GameObject[] highlightObject;
    public GameObject[] placeholderObjects;
    public GameObject highestObject;
    private GameObject previousHighlightObject;
    private string originalParentName;
    private string draggedObjectName;
    private static string selectedObjName;
    private Transform emptyButton;
    private EquipmentManager equipmentManager;
    private MessageObjects messageObjects;
    private AudioManager audioManager;
    private GlobalCalculator globalCalculator;

    public void MessageInput(string messageText)
    {
        messageObjects = GameObject.Find("MessageCanvas/MESSAGEOBJECTS").GetComponent<MessageObjects>();
        messageObjects.DisplayMessage(messageText);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null && InventoryManager.endingDrag == true)
        {
            SelectedObj = eventData.pointerEnter.transform.parent.gameObject;
            selectedObjName = SelectedObj.name.Replace("(Clone)","");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.endingDrag = true;
        draggedObj = transform.gameObject;
        originalParentName = transform.parent.name;
        draggedObjectName = gameObject.name.Replace("(Clone)","");
        if (originalParentName == "OxygenButton" || originalParentName == "EnergyButton" || originalParentName == "HelmetButton" ||
        originalParentName == "SuitButton" || originalParentName == "ToolButton" || originalParentName == "LeftHandButton" ||
        originalParentName == "BackpackButton" || originalParentName == "RightHandButton" || originalParentName == "DrillButton" ||
        originalParentName == "EnergyButton")
        {
            emptyButton = transform.parent.Find("EmptyButton");
        }
        cloneObject = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
        cloneObject.transform.SetParent(highestObject.transform);
        cloneObject.AddComponent<CanvasGroup>();
        cloneObject.GetComponent<CanvasGroup>().blocksRaycasts = false; // Allow raycasts to pass through the clone

        isDragging = true;
        if (originalParentName != "INVENTORYMANAGER")
        {
            foreach (GameObject placeholderObject in placeholderObjects)
            {
                if (placeholderObject.name == "InventoryPlaceholder")
                {
                    placeholderObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (GameObject placeholderObject in placeholderObjects)
            {
                if (placeholderObject.name != "InventoryPlaceholder")
                {
                    placeholderObject.SetActive(true);
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            cloneObject.transform.position = eventData.position;
            CheckHighlightObject(eventData);
        }
    }
    private void EquipSlot(GameObject highlightObj, RectTransform objectRectTransform, RectTransform highlightRectTransform, int slotIndex, string playerResourceName, float playerResourceValue)
    {
        EquipmentManager.slotEquippedName[slotIndex] = "";
        EquipmentManager.slotEquipped[slotIndex] = true;

        // Use reflection to set the player resource value
        FieldInfo playerResourceField = typeof(PlayerResources).GetField(playerResourceName);
        playerResourceField?.SetValue(null, playerResourceValue);

        highlightObj.transform.Find("EmptyButton")?.GetComponent<Image>()?.gameObject.SetActive(false);

        objectRectTransform.SetParent(highlightRectTransform);
        objectRectTransform.SetAsLastSibling();
        objectRectTransform.localPosition = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        equipmentManager = GameObject.Find("EquipmentManager").GetComponent<EquipmentManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (!isDragging)
            return;

        isDragging = false;

        foreach (GameObject obj in placeholderObjects)
        {
            obj.SetActive(false);
        }
        GameObject highlightObj = GetDroppedHighlightObject(eventData);

        // *dragging the same object onto each other (stacking them together with max limit 9999)
        if (selectedObjName == draggedObjectName)
        {
            int draggedObjID = 0;
            float draggedObjQuantity = 0f;
            int selectedObjID = 0;
            float selectedObjQuantity = 0f;
            ItemData itemData1 = draggedObj.GetComponent<ItemData>();
            if (itemData1 != null)
            {
                draggedObjID = itemData1.ID;
                draggedObjQuantity = itemData1.itemQuantity;
            }
            ItemData itemData2 = SelectedObj.GetComponent<ItemData>();
            if (itemData2 != null)
            {
                selectedObjID = itemData2.ID;
                selectedObjQuantity = itemData2.itemQuantity;
            }
            if (draggedObjID != selectedObjID)
            {
                if (selectedObjQuantity > StackLimits.LowStackLimit)
                {
                    float remainingStackQuantity = selectedObjQuantity - StackLimits.LargeStackLimit;
                    itemData1.itemQuantity = remainingStackQuantity;
                    TextMeshProUGUI newCountText2 = draggedObj.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                    if (newCountText2 != null)
                    {
                        newCountText2.text = itemData1.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    InventoryManager inventoryManager = draggedObj.transform.parent.GetComponent<InventoryManager>();
                    inventoryManager.DestroySpecificItem(draggedObjectName, itemData1.itemProduct, itemData1.ID);
                }
                itemData2.itemQuantity += draggedObjQuantity;
                TextMeshProUGUI newCountText = SelectedObj.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                if (newCountText != null)
                {
                    newCountText.text = itemData2.itemQuantity.ToString("F2", CultureInfo.InvariantCulture);
                }
            }
            Destroy(cloneObject);
            previousHighlightObject = null;
            InventoryManager.endingDrag = false;
            selectedObjName = null;
            SelectedObj = null;
            return;
        }

        if (highlightObj != null)
        {
            RectTransform highlightRectTransform = highlightObj.GetComponent<RectTransform>();
            RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();

            // *dragging from INVENTORY to EQUIPMENT SLOTS*
            if (highlightObj.name == "OxygenButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "OxygenTank" && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 6, "PlayerOxygen", 0.05f);
                    audioManager.PlayOxygenSlotSound();
                }
                else
                {
                    MessageInput("OxygenTankEquipFail");
                }
            }
            else if (highlightObj.name == "EnergyButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "Battery")
                {
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 5, "PlayerEnergy", 0.2f);
                    audioManager.PlayEnergySlotSound();
                }
            }
            else if (highlightObj.name == "WaterButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "DistilledWater")
                {
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 7, "PlayerWater", 0.1f);
                    audioManager.PlayWaterSlotSound();
                }
            }
            else if (highlightObj.name == "HungerButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "FibrousLeaves")
                {
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 8, "PlayerHunger", 0.2f);
                    audioManager.PlayHungerSlotSound();
                }
            }

            // *dragging from EQUIPMENT SLOTS back into INVENTORY*
            else if (highlightObj.name == "InventoryContent")
            {
                if (originalParentName == "EnergyButton")
                {
                    EquipmentManager.slotEquipped[5] = false;
                    EquipmentManager.slotEquippedName[5] = "";
                    PlayerResources.PlayerEnergy = 0f;
                    GameObject noEnergyObjects = GameObject.Find("NoEnergyObjects");
                    if (noEnergyObjects != null)
                    {
                        ActivateObjects activateScript = noEnergyObjects.GetComponent<ActivateObjects>();
                        if (activateScript != null)
                        {
                            activateScript.ActivateAllObjects();
                        }
                    }
                }
                else if (originalParentName == "OxygenButton")
                {
                    EquipmentManager.slotEquipped[6] = false;
                    EquipmentManager.slotEquippedName[6] = "";
                    PlayerResources.PlayerOxygen = 0f;
                }
                else if (originalParentName == "WaterButton")
                {
                    EquipmentManager.slotEquipped[7] = false;
                    EquipmentManager.slotEquippedName[7] = "";
                    PlayerResources.PlayerWater = 0f;
                }
                else if (originalParentName == "HungerButton")
                {
                    EquipmentManager.slotEquipped[8] = false;
                    EquipmentManager.slotEquippedName[8] = "";
                    PlayerResources.PlayerHunger = 0f;
                }
                emptyButton?.GetComponent<Image>()?.gameObject.SetActive(true);
                Transform inventoryManagerObj = highlightObj.transform.Find("List/INVENTORYMANAGER");
                gameObject.transform.SetParent(inventoryManagerObj);
                audioManager.PlayUnequipSlotSound();
            }
            globalCalculator = GameObject.Find("GlobalCalculator").GetComponent<GlobalCalculator>();
            globalCalculator.UpdatePlayerConsumption();
            DeactivateHighlightObject();
        }

        Destroy(cloneObject);
        previousHighlightObject = null;
        InventoryManager.endingDrag = false;
        selectedObjName = null;
    }

    private GameObject GetDroppedHighlightObject(PointerEventData eventData)
    {
        foreach (GameObject obj2 in highlightObject)
        {
            RectTransform rectTransform = obj2.GetComponent<RectTransform>();
            if (rectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, null))
            {
                return obj2;
            }
        }

        return null;
    }


    private void DeactivateHighlightObject()
    {
        if (previousHighlightObject != null)
        {
            Transform previousHighlightImage = previousHighlightObject.transform.Find("HighlightImage");
            if (previousHighlightImage != null)
            {
                previousHighlightImage.gameObject.SetActive(false);
            }
        }
    }
    private void CheckHighlightObject(PointerEventData eventData)
    {
        GameObject newHighlightObject = null;
        Transform highlightImage = null;

        foreach (GameObject obj in highlightObject)
        {
            if (originalParentName == "INVENTORYMANAGER" && obj.name == "InventoryContent")
            {
                continue; // Skip the iteration if originalParentName is "INVENTORYMANAGER" and obj is "InventoryContent"
            }

            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, null))
            {
                newHighlightObject = obj;
                highlightImage = obj.transform.Find("HighlightImage");
                break;
            }
        }

        if (highlightImage != null)
        {
            highlightImage.gameObject.SetActive(true);
        }

        // Remove highlight effect from the previous highlightObject (if any)
        if (newHighlightObject != previousHighlightObject)
        {
            DeactivateHighlightObject();
            previousHighlightObject = newHighlightObject;
        }
    }
}
