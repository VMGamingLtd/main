using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BuildingManagement;

public class DragAndDropBuildings : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Image objectImage;
    private Animation objAnimation;
    private BuildingItemData initialItemData;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        objAnimation = GetComponent<Animation>();
        objectImage = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        objectImage.color = Color.yellow;
        BuildingItemData itemData = eventData.pointerEnter.GetComponent<BuildingItemData>();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (objAnimation != null && objAnimation.isPlaying)
        {

        }
        else
        {
            objectImage.color = Color.black;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Set the sibling index to ensure the dragged object is rendered on top
        rectTransform.SetAsLastSibling();
        BuildingManager.isDraggingBuilding = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (objAnimation != null)
        {
            objAnimation.Play("BuildingSpawn");
        }
        BuildingManager.isDraggingBuilding = false;

    }
}
