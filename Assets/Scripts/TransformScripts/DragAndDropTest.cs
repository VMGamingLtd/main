using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDropTest : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject cloneObject;
    private bool isDragging = false;

    public GameObject[] highlightObject;
    public GameObject[] placeholderObjects;
    public GameObject highestObject;
    private GameObject previousHighlightObject;
    private string originalParentName;
    private Transform emptyButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Prevent click-through on UI elements
        eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create a clone object and set up dragging
        originalParentName = transform.parent.name;
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
        foreach (GameObject obj in placeholderObjects)
        {
            obj.SetActive(true);
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
        if (isDragging)
        {
            isDragging = false;

            foreach (GameObject obj in placeholderObjects)
            {
                obj.SetActive(false);
            }

            // Check if the cloneObject is dropped onto any highlight object
            foreach (GameObject obj2 in highlightObject)
            {
                RectTransform rectTransform = obj2.GetComponent<RectTransform>();
                if (rectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, null))
                {
                    if (obj2.name == "OxygenButton")
                    {
                        if (originalParentName == "INVENTORYMANAGER")
                        {
                            EquipmentManager.slotEquipped[8] = false;
                            Transform emptyButton = obj2.transform.Find("EmptyButton");
                            Image emptyButtonImage = emptyButton.GetComponent<Image>();
                            if (emptyButtonImage != null)
                            {
                                emptyButtonImage.enabled = false;
                            }
                            gameObject.transform.SetParent(obj2.transform);
                            gameObject.transform.SetAsLastSibling();

                            RectTransform buttonRectTransform = obj2.GetComponent<RectTransform>();
                            RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
                            objectRectTransform.anchorMin = buttonRectTransform.anchorMin;
                            objectRectTransform.anchorMax = buttonRectTransform.anchorMax;
                            objectRectTransform.anchoredPosition = buttonRectTransform.anchoredPosition;
                            objectRectTransform.sizeDelta = buttonRectTransform.sizeDelta;
                            objectRectTransform.localScale = buttonRectTransform.localScale;
                            objectRectTransform.rotation = buttonRectTransform.rotation;
                            gameObject.transform.localPosition = Vector3.zero;
                        }
                        else if (originalParentName == "OxygenButton")
                        {
                            EquipmentManager.slotEquipped[8] = true;
                        }
                    }
                    else if (obj2.name == "InventoryContent")
                    {
                        if (originalParentName == "OxygenButton")
                        {
                            EquipmentManager.slotEquipped[8] = false;
                            Image emptyButtonImage = emptyButton.GetComponent<Image>();
                            if (emptyButtonImage != null)
                            {
                                emptyButtonImage.enabled = true;
                            }

                            Transform inventoryManagerObj = obj2.transform.Find("List/INVENTORYMANAGER");
                            gameObject.transform.SetParent(inventoryManagerObj);
                            gameObject.transform.SetAsLastSibling();

                            RectTransform buttonRectTransform = obj2.GetComponent<RectTransform>();
                            RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
                            objectRectTransform.anchorMin = buttonRectTransform.anchorMin;
                            objectRectTransform.anchorMax = buttonRectTransform.anchorMax;
                            objectRectTransform.anchoredPosition = buttonRectTransform.anchoredPosition;
                            objectRectTransform.sizeDelta = buttonRectTransform.sizeDelta;
                            objectRectTransform.localScale = buttonRectTransform.localScale;
                            objectRectTransform.rotation = buttonRectTransform.rotation;
                            gameObject.transform.localPosition = Vector3.zero;
                        }
                        else if (originalParentName == "INVENTORYMANAGER")
                        {
                            EquipmentManager.slotEquipped[8] = true;
                        }
                    }
                    DeactivateHighlightObject();
                    break;
                }
            }

            Destroy(cloneObject);
            previousHighlightObject = null;
        }
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
