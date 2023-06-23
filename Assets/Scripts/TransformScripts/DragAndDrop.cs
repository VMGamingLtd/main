using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject cloneObject;
    private bool isDragging = false;

    public GameObject[] highlightObject;
    public GameObject[] placeholderObjects;
    public GameObject highestObject;
    private GameObject previousHighlightObject;
    private string originalParentName;
    private string draggedObjectName;
    private Transform emptyButton;
    public EquipmentManager equipmentManager;
    public MessageObjects messageObjects;
    public AudioSource audioSource;
    public AudioClip audioClip;


    public void OnPointerDown(PointerEventData eventData)
    {
        eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParentName = transform.parent.name;
        draggedObjectName = gameObject.name;
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        isDragging = false;

        foreach (GameObject obj in placeholderObjects)
        {
            obj.SetActive(false);
        }

        GameObject highlightObj = GetDroppedHighlightObject(eventData);
        if (highlightObj != null)
        {
            RectTransform highlightRectTransform = highlightObj.GetComponent<RectTransform>();
            RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();

            if (highlightObj.name == "OxygenButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "OxygenTank(Clone)" && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    EquipmentManager.slotEquipped[8] = true;
                    Transform emptyButton = highlightObj.transform.Find("EmptyButton");
                    Image emptyButtonImage = emptyButton?.GetComponent<Image>();
                    if (emptyButtonImage != null)
                    {
                        emptyButtonImage.enabled = false;
                    }
                    objectRectTransform.SetParent(highlightRectTransform);
                    objectRectTransform.SetAsLastSibling();
                    objectRectTransform.localPosition = Vector3.zero;
                    equipmentManager.CheckForEquip();
                }
                else
                {
                    messageObjects.DisplayMessage("OxygenTankEquipFail");
                }
            }
            else if (highlightObj.name == "EnergyButton")
            {
                if (originalParentName == "INVENTORYMANAGER" && draggedObjectName == "Battery(Clone)")
                {
                    EquipmentManager.slotEquipped[7] = true;
                    Transform emptyButton = highlightObj.transform.Find("EmptyButton");
                    Image emptyButtonImage = emptyButton?.GetComponent<Image>();
                    if (emptyButtonImage != null)
                    {
                        emptyButtonImage.enabled = false;
                    }
                    objectRectTransform.SetParent(highlightRectTransform);
                    objectRectTransform.SetAsLastSibling();
                    objectRectTransform.localPosition = Vector3.zero;
                    equipmentManager.CheckForEquip();
                    if (audioClip != null && audioSource != null)
                    {
                        Debug.Log("bingo");
                        audioSource.PlayOneShot(audioClip);
                    }
                }
                else
                {
                    messageObjects.DisplayMessage("OxygenTankEquipFail");
                }
            }
            else if (highlightObj.name == "InventoryContent")
            {
                if (originalParentName == "EnergyButton")
                {
                    EquipmentManager.slotEquipped[7] = false;
                    Image emptyButtonImage = emptyButton?.GetComponent<Image>();
                    if (emptyButtonImage != null)
                    {
                        emptyButtonImage.enabled = true;
                    }
                    Transform inventoryManagerObj = highlightObj.transform.Find("List/INVENTORYMANAGER");
                    GameObject noEnergyObjects = GameObject.Find("NoEnergyObjects");
                    gameObject.transform.SetParent(inventoryManagerObj);
                    PlayerResources.PlayerEnergy = "00:00:00:00";
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
                    EquipmentManager.slotEquipped[8] = false;
                    Image emptyButtonImage = emptyButton?.GetComponent<Image>();
                    if (emptyButtonImage != null)
                    {
                        emptyButtonImage.enabled = true;
                    }
                    Transform inventoryManagerObj = highlightObj.transform.Find("List/INVENTORYMANAGER");
                    gameObject.transform.SetParent(inventoryManagerObj);
                }
            }

            DeactivateHighlightObject();
        }

        Destroy(cloneObject);
        previousHighlightObject = null;
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
