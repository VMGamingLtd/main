using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ItemManagement;
using System.Collections;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool isDragging = false;
    private RectTransform rectTransform;
    //private Canvas canvas;
    private Image draggingImage;
    private Vector2 offset;
    private Vector2 fixedSize = new Vector2(64f, 64f);
    private GameObject highlightedObject;
    private GameObject draggingObject;
    public GameObject[] highlightObjects;
    public GameObject[] placeholderObjects;
    public InventoryManager inventoryManager;
    public ItemCreator itemCreator;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Begin dragging
        isDragging = true;

        // Create the dragging image at the mouse cursor location
        draggingImage = CreateDraggingImage(eventData.position);

        foreach (GameObject placeholderObject in placeholderObjects)
        {
            placeholderObject.gameObject.SetActive(true);
        }
        ResetHighlight();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the dragging image position to follow the mouse/finger position
        draggingImage.rectTransform.position = eventData.position;

        // Check if dragging image is within the bounds of any highlight objects
        foreach (GameObject obj in highlightObjects)
        {
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position))
            {
                // Highlight the object
                HighlightObject(obj);
                return;
            }
        }

        // No object is highlighted
        ResetHighlight();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        if (highlightedObject != null)
        {
            // Where are you dragging object onto
            string highlightedObjectName = highlightedObject.transform.name;

            // Check parent object of an object that you are currently dragging
            string draggingObjectImage = draggingImage.GetComponent<Image>().sprite.name;

            if (draggingObjectImage == "OxygenTank" && highlightedObjectName == "InventoryContent")
            {
                Transform emptyButton = draggingImage.transform.parent.Find("EmptyButton");
                if (emptyButton != null)
                {
                    emptyButton.gameObject.SetActive(true);
                }
                Transform fullButton = draggingImage.transform.parent.Find("ButtonIcon");
                if (fullButton != null)
                {
                    fullButton.gameObject.SetActive(false);
                }
                itemCreator.CreateOxygenTanks(1);
                inventoryManager.InventoryCheckUp();
            }
            else
            {
                Debug.Log("Can't check parent object");
            }
        }

        foreach (GameObject placeholderObject in placeholderObjects)
        {
            placeholderObject.gameObject.SetActive(false);
        }

        Destroy(draggingImage.gameObject);
        ResetHighlight();
    }

    public Image CreateDraggingImage(Vector2 position)
    {
        Image originalImage = GetComponent<Image>();

        if (originalImage == null)
        {
            Debug.LogError("No Image component found on the object to drag.");
            return null;
        }

        GameObject draggingObject = new GameObject("DraggingObject");
        RectTransform draggingRectTransform = draggingObject.AddComponent<RectTransform>();
        Image draggingImage = draggingObject.AddComponent<Image>();

        draggingObject.transform.SetParent(transform.parent, false);
        draggingRectTransform.sizeDelta = fixedSize;

        draggingImage.sprite = originalImage.sprite;
        draggingImage.rectTransform.sizeDelta = fixedSize;
        draggingImage.rectTransform.position = position;

        return draggingImage;
    }
    public void HighlightObject(GameObject obj)
    {
        // Search for the child object named "HighlightImage"
        Transform highlightImage = obj.transform.Find("HighlightImage");

        // Enable the highlight image if found
        if (highlightImage != null)
        {
            highlightImage.gameObject.SetActive(true);
        }
        highlightedObject = obj;
    }


    public void ResetHighlight()
    {
        highlightedObject = null;

        foreach (GameObject obj in highlightObjects)
        {
        // Search for the child object named "HighlightImage"
        Transform highlightImage = obj.transform.Find("HighlightImage");

            // Disable the highlight image if found
            if (highlightImage != null)
            {
                highlightImage.gameObject.SetActive(false);
            }
        }
    }
}
