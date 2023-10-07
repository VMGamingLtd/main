using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class DragAndDropBuild : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject objectTemplate;
    private GameObject BuildingManagerArea;
    private GameObject BuildingArea;
    private GameObject draggedObject;
    private bool isOverlapping;
    private RectTransform buildingAreaRectTransform;
    private Image buildingAreaImage;
    private BuildingCreator buildingCreator;
    private BuildingOptionsInterface optionsInterfaceScript;
    private BuildingOptionsWindow buildingOptionsWindow;
    private string currentCount;
    private string objName;

    private void Awake()
    {
        Transform parentTransform = transform.parent;
        Transform grandparentTransform = parentTransform.parent;
        Transform greatGreatGrandparent = grandparentTransform.parent;
        BuildingArea = greatGreatGrandparent.Find("BuildingArea").gameObject;
        BuildingManagerArea = greatGreatGrandparent.Find("BuildingArea/Viewport/BUILDINGMANAGER").gameObject;
        buildingAreaRectTransform = BuildingArea.GetComponent<RectTransform>();
        buildingAreaImage = BuildingArea.GetComponent<Image>();
        buildingCreator = GameObject.Find("BuildingCreatorList").GetComponent<BuildingCreator>();
        objectTemplate = GameObject.Find("BuildingCreatorList/BuildingTemplate");
        //dragImage = GameObject.Find("BuildingIcon");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentCount = transform.Find("Count").GetComponent<TextMeshProUGUI>().text;

        if (currentCount != "0")
        {
            objName = transform.name;
            draggedObject = Instantiate(objectTemplate, BuildingArea.transform);
            draggedObject.transform.localScale = BuildingManagerArea.transform.localScale;
            draggedObject.transform.position = eventData.position;
            draggedObject.transform.Find("Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(objName);
            buildingAreaImage.enabled = true;
            BuildingManager.isDraggingBuilding = true;
            isOverlapping = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentCount != "0")
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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentCount != "0")
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
        
    }
    public IEnumerator AttachDragAndDropBuildingsWithDelay(GameObject draggedObject)
    {
        yield return new WaitForSeconds(0.5f);
        draggedObject.AddComponent<DragAndDropBuildings>();

        // if the Building options window is open, then it will be closed
        buildingOptionsWindow = GameObject.Find("BuildingOptionsWindow").GetComponent<BuildingOptionsWindow>();
        optionsInterfaceScript = buildingOptionsWindow.optionsInterfaceScript;
        buildingOptionsWindow.buildingOptions.SetActive(false);

        switch (objName)
        {
            case "BiofuelGenerator":
                buildingCreator.CreateBuilding(draggedObject, 0);
                break;
            case "WaterPump":
                buildingCreator.CreateBuilding(draggedObject, 1);
                break;
            case "PlantField":
                buildingCreator.CreateBuilding(draggedObject, 2);
                break;
            case "Boiler":
                buildingCreator.CreateBuilding(draggedObject, 3);
                break;
            case "SteamGenerator":
                buildingCreator.CreateBuilding(draggedObject, 4);
                break;
            case "Furnace":
                buildingCreator.CreateBuilding(draggedObject, 5);
                break;
            default:
                Debug.LogWarning("Unknown building type: " + objName);
                break;
        }
    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("buildingicons", spriteName);
        return sprite;
    }
}
