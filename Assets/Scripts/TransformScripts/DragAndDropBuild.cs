using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using BuildingManagement;

public class DragAndDropBuild : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject NewBuilding;
    public GameObject BuildingManagerArea;
    private GameObject BuildingArea;
    private GameObject draggedObject;
    private bool isOverlapping;
    private RectTransform buildingAreaRectTransform;
    private Image buildingAreaImage;
    private BuildingCreator buildingCreator;
    private BuildingOptionsInterface optionsInterfaceScript;
    private BuildingOptionsWindow buildingOptionsWindow;

    private void Awake()
    {
        Transform parentTransform = transform.parent;
        Transform grandparentTransform = parentTransform.parent;
        BuildingArea = grandparentTransform.Find("BuildingArea").gameObject;
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
        isOverlapping = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Image objImg = draggedObject.GetComponent<Image>();

        if (UIUtils.CheckForOverlap(draggedObject))
        {
            objImg.color = Color.red;
            isOverlapping = true;
        }
        else
        {
            objImg.color = Color.black;
            isOverlapping = false;
        }
        draggedObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isOverlapping)
        {
            // Check if the dragged object is within the boundaries of the building area
            if (UIUtils.IsWithinBounds(eventData, buildingAreaRectTransform))
            {
                draggedObject.transform.SetParent(BuildingManagerArea.transform, true);
                Animation animation = draggedObject.GetComponent<Animation>();
                BoxCollider2D draggedCollider = draggedObject.GetComponent<BoxCollider2D>();
                draggedCollider.enabled = true;
                if (animation != null)
                {
                    animation.Play("BuildingSpawn");
                }
                StartCoroutine(AttachDragAndDropBuildingsWithDelay(draggedObject));
            }
            else
            {
                Destroy(draggedObject);
            }
            buildingAreaImage.enabled = false;
            BuildingManager.isDraggingBuilding = false;
        }
        else
        {
            Destroy(draggedObject);
            buildingAreaImage.enabled = false;
            BuildingManager.isDraggingBuilding = false;
        }
    }
    public IEnumerator AttachDragAndDropBuildingsWithDelay(GameObject draggedObject)
    {
        yield return new WaitForSeconds(0.5f);
        draggedObject.AddComponent<DragAndDropBuildings>();

        // if the Building options window is open, then it will be closed
        buildingOptionsWindow = GameObject.Find("BuildingOptionsWindow").GetComponent<BuildingOptionsWindow>();
        optionsInterfaceScript = buildingOptionsWindow.optionsInterfaceScript;
        buildingOptionsWindow.buildingOptions.SetActive(false);

        // add the object into buildingArray and with BuildingDataItem component
        buildingCreator = GameObject.Find("BuildingCreatorList").GetComponent<BuildingCreator>();
        string objectName = draggedObject.transform.name.Replace("(Clone)", "");
        switch (objectName)
        {
            case "WaterPump":
                buildingCreator.CreateBuilding(draggedObject, "PUMPINGFACILITY");
                break;
            case "PlantField":
                buildingCreator.CreateBuilding(draggedObject, "AGRICULTURE");
                break;
            case "Boiler":
                buildingCreator.CreateBuilding(draggedObject, "HEATINGFACILITY");
                break;
            case "SteamGenerator":
                buildingCreator.CreateBuilding(draggedObject, "POWERPLANT");
                break;
            case "BiofuelGenerator":
                buildingCreator.CreateBuilding(draggedObject, "POWERPLANT");
                break;
            case "Furnace":
                buildingCreator.CreateBuilding(draggedObject, "FACTORY");
                break;
            default:
                Debug.LogWarning("Unknown building type: " + objectName);
                break;
        }
    }
}
