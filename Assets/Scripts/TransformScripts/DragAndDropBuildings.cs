using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropBuildings : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Image objectImage;
    private Animation objAnimation;
    private GameObject draggedObject;
    private GameObject BuildingArea;
    private RectTransform buildingAreaRectTransform;
    private Vector3 initialPosition;
    private bool isOverlapping;
    private BoxCollider2D draggedCollider;

    private void Awake()
    {
        Transform parentTransform = transform.parent;
        Transform grandparentTransform = parentTransform.parent;
        BuildingArea = grandparentTransform.gameObject;
        rectTransform = GetComponent<RectTransform>();
        objAnimation = GetComponent<Animation>();
        objectImage = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        objectImage.color = UIColors.highlightCol;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        objectImage.color = Color.black;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Set the sibling index to ensure the dragged object is rendered on top
        initialPosition = rectTransform.position;
        rectTransform.SetAsLastSibling();
        BuildingManager.isDraggingBuilding = true;
        draggedObject = transform.gameObject;
        draggedCollider = draggedObject.GetComponent<BoxCollider2D>();
        draggedCollider.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UIUtils.CheckForOverlap(draggedObject))
        {
            objectImage.color = Color.red;
            isOverlapping = true;
        }
        else
        {
            objectImage.color = Color.black;
            isOverlapping = false;
        }
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isOverlapping)
        {
            buildingAreaRectTransform = BuildingArea.GetComponent<RectTransform>();
            if (UIUtils.IsWithinBounds(eventData, buildingAreaRectTransform))
            {
                BoxCollider2D draggedCollider = draggedObject.GetComponent<BoxCollider2D>();
                draggedCollider.enabled = true;
            }
            else
            {
                draggedObject.transform.position = initialPosition;
            }
        }
        else
        {
            draggedObject.transform.position = initialPosition;
        }
        BuildingManager.isDraggingBuilding = false;
        draggedCollider.enabled = true;
    }
}
