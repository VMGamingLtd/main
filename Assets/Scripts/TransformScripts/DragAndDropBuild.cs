using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DragAndDropBuild : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject NewBuilding;
    public GameObject BuildingManagerArea;
    public GameObject BuildingArea;
    private GameObject draggedObject;
    private RectTransform buildingAreaRectTransform;
    private Image buildingAreaImage;

    private void Awake()
    {
        buildingAreaRectTransform = BuildingArea.GetComponent<RectTransform>();
        buildingAreaImage = BuildingArea.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Instantiate the new building at the mouse cursor position within its parent
        draggedObject = Instantiate(NewBuilding, BuildingArea.transform);
        draggedObject.transform.localScale = BuildingManagerArea.transform.localScale;
        draggedObject.transform.position = eventData.position;
        buildingAreaImage.enabled = true;
        BuildingManager.isDraggingBuilding = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the dragged object based on the mouse cursor
        draggedObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the dragged object is within the boundaries of the building area
        if (IsWithinBounds(eventData, buildingAreaRectTransform))
        {
            draggedObject.transform.SetParent(BuildingManagerArea.transform, true);
            Animation animation = draggedObject.GetComponent<Animation>();
            if (animation != null)
            {
                animation.Play("BuildingSpawn");
            }
            StartCoroutine(AttachDragAndDropBuildingsWithDelay());
        }
        else
        {
            Destroy(draggedObject);
        }
        buildingAreaImage.enabled = false;
        BuildingManager.isDraggingBuilding = false;
    }
    private bool IsWithinBounds(PointerEventData eventData, RectTransform bounds)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bounds, eventData.position, null, out localPoint);

        Rect boundsRect = new Rect(bounds.rect.position, bounds.rect.size);

        return boundsRect.Contains(localPoint);
    }
    private IEnumerator AttachDragAndDropBuildingsWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        draggedObject.AddComponent<DragAndDropBuildings>();
    }
}
