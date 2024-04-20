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
        messageObjects = GameObject.Find(Constants.MessageObjects).GetComponent<MessageObjects>();
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
        if (originalParentName == Constants.OxygenButton || originalParentName == Constants.EnergyButton || originalParentName == Constants.HelmetButton ||
        originalParentName == Constants.SuitButton || originalParentName == Constants.ToolButton || originalParentName == Constants.LeftHandButton ||
        originalParentName == Constants.BackpackButton || originalParentName == Constants.RightHandButton || originalParentName == Constants.DrillButton ||
        originalParentName == Constants.WaterButton || originalParentName == Constants.HungerButton)
        {
            emptyButton = transform.parent.Find(Constants.EmptyButton);
        }
        cloneObject = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
        cloneObject.transform.SetParent(highestObject.transform);
        cloneObject.AddComponent<CanvasGroup>();
        cloneObject.GetComponent<CanvasGroup>().blocksRaycasts = false; // Allow raycasts to pass through the clone
        countInventory = cloneObject.GetComponentInChildren<TextMeshProUGUI>();
        countInventory.enabled = false;
        isDragging = true;

        // during drag we want to show player where he can drag the desired object by activating placeholder objects
        if (originalParentName != Constants.InventoryManager)
        {
            foreach (GameObject placeholderObject in placeholderObjects)
            {
                if (placeholderObject.name == Constants.InventoryPlaceholder)
                {
                    placeholderObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (GameObject placeholderObject in placeholderObjects)
            {
                if (placeholderObject.name != Constants.InventoryPlaceholder)
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

        highlightObj.transform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);

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

        highlightObj.transform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);

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

        highlightObj.transform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);

        objectRectTransform.SetParent(highlightRectTransform);
        objectRectTransform.SetAsLastSibling();
        objectRectTransform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Equips the objectName into the defined slotIndex. If the object is some consumable resource, it will have playerResourceValue 
    /// set to some number. This value will be deducted every minute so the higher the number, the faster will the object be depleted when equipped.
    /// This means that higher values means lower quality of consumable item.
    /// </summary>
    /// <param name="highlightObj"></param>
    /// <param name="objectRectTransform"></param>
    /// <param name="highlightRectTransform"></param>
    /// <param name="slotIndex"></param>
    /// <param name="playerResourceName"></param>
    /// <param name="playerResourceValue"></param>
    /// <param name="objectName"></param>
    /// <param name="itemData"></param>
    private void EquipSlot(GameObject highlightObj, RectTransform objectRectTransform, RectTransform highlightRectTransform,
        int slotIndex, string playerResourceName, float playerResourceValue, string objectName, ItemData itemData)
    {
        itemData.isEquipped = true;
        EquipmentManager.slotEquippedName[slotIndex] = objectName;
        EquipmentManager.slotEquipped[slotIndex] = true;

        // Use reflection to set the player resource value
        FieldInfo playerResourceField = typeof(PlayerResources).GetField(playerResourceName);
        playerResourceField?.SetValue(null, playerResourceValue);

        highlightObj.transform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(false);

        objectRectTransform.SetParent(highlightRectTransform);
        objectRectTransform.SetAsLastSibling();
        objectRectTransform.localPosition = Vector3.zero;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        equipmentManager = GameObject.Find(Constants.EquipmentManager).GetComponent<EquipmentManager>();
        audioManager = GameObject.Find(Constants.AudioManager).GetComponent<AudioManager>();
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

                        if (draggedObj.transform.Find(Constants.CountInventory).TryGetComponent<TextMeshProUGUI>(out var newCountText2))
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
                    if (SelectedObj.transform.Find(Constants.CountInventory).TryGetComponent<TextMeshProUGUI>(out var newCountText))
                    {
                        string quantityText = StoreQuantity(itemData2);
                        newCountText.text = quantityText;
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
            if (highlightObj.name == Constants.HelmetButton)
            {
                if (originalParentName == Constants.InventoryManager && draggedObjectName == "CAS-901")
                {
                    HelmetData itemData = draggedObj.GetComponent<HelmetData>();
                    EquipHelmet(highlightObj, objectRectTransform, highlightRectTransform, 0, draggedObjectName, itemData);
                    audioManager.PlayerEquipSlotSound();
                }
            }
            else if (highlightObj.name == Constants.SuitButton)
            {
                if (originalParentName == Constants.InventoryManager && draggedObjectName == "CAS-101")
                {
                    SuitData itemData = draggedObj.GetComponent<SuitData>();
                    EquipSuit(highlightObj, objectRectTransform, highlightRectTransform, 1, draggedObjectName, itemData);
                    audioManager.PlayerEquipSlotSound();
                }
            }
            else if (highlightObj.name == Constants.LeftHandButton)
            {
                if (originalParentName == Constants.InventoryManager && draggedObjectName == "FAB-1")
                {
                    ToolData itemData = draggedObj.GetComponent<ToolData>();
                    EquipTool(highlightObj, objectRectTransform, highlightRectTransform, 2, draggedObjectName, itemData);
                    audioManager.PlayerEquipSlotSound();
                }
            }
            else if (highlightObj.name == Constants.OxygenButton)
            {
                if (originalParentName == Constants.InventoryManager && draggedObjectName == Constants.OxygenTank && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 6, Constants.PlayerOxygen, 0.01f, draggedObjectName, itemData);
                    audioManager.PlayOxygenSlotSound();
                }
                else
                {
                    MessageInput("OxygenTankEquipFail");
                }
            }
            else if (highlightObj.name == Constants.EnergyButton)
            {
                if (originalParentName == Constants.InventoryManager && draggedObjectName == Constants.Battery)
                {
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 5, Constants.PlayerEnergy, 0.001f, draggedObjectName, itemData);
                    audioManager.PlayEnergySlotSound();
                }
            }
            else if (highlightObj.name == Constants.WaterButton)
            {
                if (originalParentName == Constants.InventoryManager && draggedObjectName == Constants.DistilledWater)
                {
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 7, Constants.PlayerWater, 0.001f, draggedObjectName, itemData);
                    audioManager.PlayWaterSlotSound();

                    /// <summary>
                    /// Second goal finished and set to true.
                    /// </summary>
                    /// <value>true</value>
                    if (GoalManager.firstGoal == false)
                    {
                        GoalManager goalManager = GameObject.Find(Constants.GoalManager).GetComponent<GoalManager>();
                        _ = goalManager.SetSecondGoal();
                    }
                }
            }
            else if (highlightObj.name == Constants.HungerButton)
            {
                ItemData itemData = draggedObj.GetComponent<ItemData>();

                if (originalParentName == Constants.InventoryManager && draggedObjectName == Constants.FibrousLeaves)
                {
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 8, Constants.PlayerHunger, 0.001f, draggedObjectName, itemData);
                }
                else if (originalParentName == Constants.InventoryManager && draggedObjectName == Constants.FishMeat)
                {
                    EquipSlot(highlightObj, objectRectTransform, highlightRectTransform, 8, Constants.PlayerHunger, 0.0005f, draggedObjectName, itemData);
                }

                audioManager.PlayHungerSlotSound();
            }

            // *dragging from EQUIPMENT SLOTS back into INVENTORY*
            else if (highlightObj.name == Constants.InventoryContent)
            {
                if (originalParentName == Constants.HelmetButton)
                {
                    EquipmentManager.slotEquipped[0] = false;
                    EquipmentManager.slotEquippedName[0] = "";
                    HelmetData itemData = draggedObj.GetComponent<HelmetData>();
                    equipmentManager.UnequipHelmet(itemData);
                }
                else if (originalParentName == Constants.SuitButton)
                {
                    EquipmentManager.slotEquipped[1] = false;
                    EquipmentManager.slotEquippedName[1] = "";
                    SuitData itemData = draggedObj.GetComponent<SuitData>();
                    equipmentManager.UnequipSuit(itemData);
                }
                else if (originalParentName == Constants.LeftHandButton)
                {
                    EquipmentManager.slotEquipped[2] = false;
                    EquipmentManager.slotEquippedName[2] = "";
                    ToolData itemData = draggedObj.GetComponent<ToolData>();
                    equipmentManager.UnequipTool(itemData);
                }
                else if (originalParentName == Constants.EnergyButton)
                {
                    EquipmentManager.slotEquipped[5] = false;
                    EquipmentManager.slotEquippedName[5] = "";
                    PlayerResources.PlayerEnergy = 0f;
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                    GameObject noEnergyObjects = GameObject.Find(Constants.NoEnergyObjects);
                    if (noEnergyObjects != null)
                    {
                        if (noEnergyObjects.TryGetComponent<ActivateObjects>(out var activateScript))
                        {
                            activateScript.ActivateAllObjects();
                        }
                    }
                }
                else if (originalParentName == Constants.OxygenButton)
                {
                    EquipmentManager.slotEquipped[6] = false;
                    EquipmentManager.slotEquippedName[6] = "";
                    PlayerResources.PlayerOxygen = 0f;
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                }
                else if (originalParentName == Constants.WaterButton)
                {
                    EquipmentManager.slotEquipped[7] = false;
                    EquipmentManager.slotEquippedName[7] = "";
                    PlayerResources.PlayerWater = 0f;
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                }
                else if (originalParentName == Constants.HungerButton)
                {
                    EquipmentManager.slotEquipped[8] = false;
                    EquipmentManager.slotEquippedName[8] = "";
                    PlayerResources.PlayerHunger = 0f;
                    ItemData itemData = draggedObj.GetComponent<ItemData>();
                    itemData.isEquipped = false;
                }
                emptyButton.GetComponent<Image>().gameObject.SetActive(true);
                Transform inventoryManagerObj = highlightObj.transform.Find("List/INVENTORYMANAGER");
                gameObject.transform.SetParent(inventoryManagerObj);
                audioManager.PlayUnequipSlotSound();
            }
            // because any of the above objects may have altered consumption or any important value, that GlobalCalculator holds, it needs to be updated
            globalCalculator = GameObject.Find(Constants.GlobalCalculator).GetComponent<GlobalCalculator>();
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
            Transform previousHighlightImage = previousHighlightObject.transform.Find(Constants.HighlightImage);
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
            if (originalParentName == Constants.InventoryManager && obj.name == Constants.InventoryContent)
            {
                continue; // Skip the iteration if originalParentName is "INVENTORYMANAGER" and obj is "InventoryContent"
            }

            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, null))
            {
                newHighlightObject = obj;
                highlightImage = obj.transform.Find(Constants.HighlightImage);
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
