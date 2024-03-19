using ItemManagement;
using System.Globalization;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private TextMeshProUGUI countInventory;

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
            selectedObjName = SelectedObj.name.Replace("(Clone)", "");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.endingDrag = true;
        draggedObj = transform.gameObject;
        originalParentName = transform.parent.name;
        draggedObjectName = gameObject.name.Replace("(Clone)", "");

        // first we check what are we dragging 
        if (originalParentName == "OxygenButton" || originalParentName == "EnergyButton" || originalParentName == "HelmetButton" ||
        originalParentName == "SuitButton" || originalParentName == "ToolButton" || originalParentName == "LeftHandButton" ||
        originalParentName == "BackpackButton" || originalParentName == "RightHandButton" || originalParentName == "DrillButton" ||
        originalParentName == "EnergyButton" || originalParentName == "WaterButton")
        {
            emptyButton = transform.parent.Find("EmptyButton");
        }
        cloneObject = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
        cloneObject.transform.SetParent(highestObject.transform);
        cloneObject.AddComponent<CanvasGroup>();
        cloneObject.GetComponent<CanvasGroup>().blocksRaycasts = false; // Allow raycasts to pass through the clone
        countInventory = cloneObject.GetComponentInChildren<TextMeshProUGUI>();
        countInventory.enabled = false;
        isDragging = true;

        // during drag we want to show player where he can drag the desired object by activating placeholder objects
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
    private void EquipHelmet(GameObject highlightObj, RectTransform objectRectTransform, RectTransform highlightRectTransform,
        int slotIndex, string objectName, HelmetData itemData)
    {
        itemData.isEquipped = true;
        EquipmentManager.slotEquippedName[slotIndex] = objectName;
        EquipmentManager.slotEquipped[slotIndex] = true;
        equipmentManager.EquipHelmet(itemData);

        highlightObj.transform.Find("EmptyButton")?.GetComponent<Image>()?.gameObject.SetActive(false);

        objectRectTransform.SetParent(highlightRectTransform);
        objectRectTransform.SetAsLastSibling();
        objectRectTransform.localPosition = Vector3.zero;
    }
    private void EquipSuit(GameObject highlightObj, RectTransform objectRectTransform, RectTransform highlightRectTransform,
        int slotIndex, string objectName, SuitData itemData)
    {
        itemData.isEquipped = true;
        EquipmentManager.slotEquippedName[slotIndex] = objectName;
        EquipmentManager.slotEquipped[slotIndex] = true;
        equipmentManager.EquipSuit(itemData);

        highlightObj.transform.Find("EmptyButton")?.GetComponent<Image>()?.gameObject.SetActive(false);

        objectRectTransform.SetParent(highlightRectTransform);
        objectRectTransform.SetAsLastSibling();
        objectRectTransform.localPosition = Vector3.zero;
    }
    private void EquipTool(GameObject highlightObj, RectTransform objectRectTransform, RectTransform highlightRectTransform,
        int slotIndex, string objectName, ToolData itemData)
    {
        itemData.isEquipped = true;
        EquipmentManager.slotEquippedName[slotIndex] = objectName;
        EquipmentManager.slotEquipped[slotIndex] = true;
        equipmentManager.EquipTool(itemData);

        highlightObj.transform.Find("EmptyButton")?.GetComponent<Image>()?.gameObject.SetActive(false);

        objectRectTransform.SetParent(highlightRectTransform);
        objectRectTransform.SetAsLastSibling();
        objectRectTransform.localPosition = Vector3.zero;
    }
    private void EquipSlot(GameObject highlightObj, RectTransform objectRectTransform, RectTransform highlightRectTransform,
        int slotIndex, string playerResourceName, float playerResourceValue, string objectName, ItemData itemData)
    {
        itemData.isEquipped = true;
        EquipmentManager.slotEquippedName[slotIndex] = objectName;
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

        // we deactivate all placeholder objects since there is nowhere to drag anymore
        foreach (GameObject obj in placeholderObjects)
        {
            obj.SetActive(false);
        }
        GameObject highlightObj = GetDroppedHighlightObject(eventData);

        // *dragging the same object onto each other (stacking them together with max limit 100)
        if (selectedObjName == draggedObjectName)
        {
            int draggedObjID = 0;
            float draggedObjQuantity = 0f;
            int selectedObjID = 0;
            float selectedObjQuantity = 0f;
            float selectedObjStackLimit = 0f;
            if (draggedObj.TryGetComponent<ItemData>(out var itemData1))
            {
                draggedObjID = itemData1.ID;
                draggedObjQuantity = itemData1.quantity;
            }
            if (SelectedObj.TryGetComponent<ItemData>(out var itemData2))
            {
                selectedObjID = itemData2.ID;
                selectedObjQuantity = itemData2.quantity;
                selectedObjStackLimit = itemData2.stackLimit;
            }
            if (draggedObjID != selectedObjID)
            {
                if (selectedObjQuantity < selectedObjStackLimit)
                {
                    float totalStackQuantity = selectedObjQuantity + draggedObjQuantity;
                    if (totalStackQuantity > selectedObjStackLimit)
                    {
                        float remainingStackQuantity = totalStackQuantity - selectedObjStackLimit;
                        itemData1.quantity = remainingStackQuantity;

                        TextMeshProUGUI newCountText2 = draggedObj.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                        if (newCountText2 != null)
                        {
                            string quantityText = StoreQuantity(itemData1);
                            newCountText2.text = quantityText;
                        }
                        itemData2.quantity = selectedObjStackLimit;
                    }
                    else
                    {
                        InventoryManager inventoryManager = draggedObj.transform.parent.GetComponent<InventoryManager>();
                        inventoryManager.DestroySpecificItem(draggedObjectName, itemData1.itemProduct, itemData1.ID);
                        itemData2.quantity += draggedObjQuantity;
                    }
                    TextMeshProUGUI newCountText = SelectedObj.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                    if (newCountText != null)
                    {
                        string quantityText = StoreQuantity(itemData2);
                        newCountText.text = quantityText;
                        //newCountText.text = itemData2.quantity.ToString("F2", CultureInfo.InvariantCulture);
                    }
                }
            }
            Destroy(cloneObject);
            previousHighlightObject = null;
            InventoryManager.endingDrag = false;
            selectedObjName = null;
            SelectedObj = null;
            return;
        }
        // if player is dragging it into one of the desired destinations then we do something, otherwise reset to normal
        if (highlightObj != null)
        {
            RectTransform highlightRectTransform = highlightObj.GetComponent<RectTransform>();
            RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();

            // *dragging from INVENTORY to EQUIPMENT SLOTS*
            if (highlightObj.name == "HelmetButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "CAS-901")
                {
                    HelmetData itemData = draggedObj.GetComponent<HelmetData>();
                    EquipHelmet(highlightObj, objectRectTransform, highlightRectTransform, 0, draggedObjectName, itemData);
                    audioManager.PlayerEquipSlotSound();
                }
            }
            else if (highlightObj.name == "SuitButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "CAS-101")
                {
                    SuitData itemData = draggedObj.GetComponent<SuitData>();
                    EquipSuit(highlightObj, objectRectTransform, highlightRectTransform, 1, draggedObjectName, itemData);
                    audioManager.PlayerEquipSlotSound();
                }
            }
            else if (highlightObj.name == "LeftHandButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "FAB-1")
                {
                    ToolData itemData = draggedObj.GetComponent<ToolData>();
                    EquipTool(highlightObj, objectRectTransform, highlightRectTransform, 2, draggedObjectName, itemData);
                    audioManager.PlayerEquipSlotSound();
                }
            }
            else if (highlightObj.name == "OxygenButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "OxygenTank" && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 6, "PlayerOxygen", 0.05f, draggedObjectName, itemData);
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
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 5, "PlayerEnergy", 0.001f, draggedObjectName, itemData);
                    audioManager.PlayEnergySlotSound();
                }
            }
            else if (highlightObj.name == "WaterButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "DistilledWater")
                {
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 7, "PlayerWater", 0.001f, draggedObjectName, itemData);
                    audioManager.PlayWaterSlotSound();

                    /// <summary>
                    /// Second goal finished and set to true.
                    /// </summary>
                    /// <value>true</value>
                    if (GoalManager.firstGoal == false)
                    {
                        GoalManager goalManager = GameObject.Find("GOALMANAGER").GetComponent<GoalManager>();
                        _ = goalManager.SetSecondGoal();
                    }
                }
            }
            else if (highlightObj.name == "HungerButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "FibrousLeaves")
                {
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 8, "PlayerHunger", 0.2f, draggedObjectName, itemData);
                    audioManager.PlayHungerSlotSound();
                }
            }

            // *dragging from EQUIPMENT SLOTS back into INVENTORY*
            else if (highlightObj.name == "InventoryContent")
            {
                if (originalParentName == "HelmetButton")
                {
                    EquipmentManager.slotEquipped[0] = false;
                    EquipmentManager.slotEquippedName[0] = "";
                    HelmetData itemData = draggedObj.GetComponent<HelmetData>();
                    equipmentManager.UnequipHelmet(itemData);
                }
                else if (originalParentName == "SuitButton")
                {
                    EquipmentManager.slotEquipped[1] = false;
                    EquipmentManager.slotEquippedName[1] = "";
                    SuitData itemData = draggedObj.GetComponent<SuitData>();
                    equipmentManager.UnequipSuit(itemData);
                }
                else if (originalParentName == "LeftHandButton")
                {
                    EquipmentManager.slotEquipped[2] = false;
                    EquipmentManager.slotEquippedName[2] = "";
                    ToolData itemData = draggedObj.GetComponent<ToolData>();
                    equipmentManager.UnequipTool(itemData);
                }
                else if (originalParentName == "EnergyButton")
                {
                    EquipmentManager.slotEquipped[5] = false;
                    EquipmentManager.slotEquippedName[5] = "";
                    PlayerResources.PlayerEnergy = 0f;
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                    GameObject noEnergyObjects = GameObject.Find("NoEnergyObjects");
                    if (noEnergyObjects != null)
                    {
                        if (noEnergyObjects.TryGetComponent<ActivateObjects>(out var activateScript))
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
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                }
                else if (originalParentName == "WaterButton")
                {
                    EquipmentManager.slotEquipped[7] = false;
                    EquipmentManager.slotEquippedName[7] = "";
                    PlayerResources.PlayerWater = 0f;
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                }
                else if (originalParentName == "HungerButton")
                {
                    EquipmentManager.slotEquipped[8] = false;
                    EquipmentManager.slotEquippedName[8] = "";
                    PlayerResources.PlayerHunger = 0f;
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                }
                emptyButton?.GetComponent<Image>()?.gameObject.SetActive(true);
                Transform inventoryManagerObj = highlightObj.transform.Find("List/INVENTORYMANAGER");
                gameObject.transform.SetParent(inventoryManagerObj);
                audioManager.PlayUnequipSlotSound();
            }
            // because any of the above objects may have altered consumption or any important value, that GlobalCalculator holds, it needs to be updated
            globalCalculator = GameObject.Find("GlobalCalculator").GetComponent<GlobalCalculator>();
            globalCalculator.UpdatePlayerConsumption();
            DeactivateHighlightObject();
        }
        globalCalculator.RecalculateInventorySlots();
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

    private string StoreQuantity(ItemData itemData)
    {
        string quantityText = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);

        if (quantityText.EndsWith(".00"))
        {
            quantityText = quantityText[..^3];
        }
        return quantityText;
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
