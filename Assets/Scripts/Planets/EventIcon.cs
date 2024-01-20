using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventIcon : MonoBehaviour
{
    private EventObjectContainer eventObjectContainer;
    private RenderTexture renderTexture;
    private Transform explorationArea;
    private GameObject iconPrefab;
    private Camera renderCamera;
    private GameObject iconInstance;
    [SerializeField]
    private float sphericalRadius = 3.6f;

    private GameObject planet;
    
    public void setPlanet(GameObject planet)
    {
        Debug.Log($@"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ cp 5200: EventIcon.setPlanet() {planet}");
        this.planet = planet;
    }
    
    void Start()
    {
        eventObjectContainer = GameObject.Find("PlanetParent").GetComponent<EventObjectContainer>();
        explorationArea = eventObjectContainer.ExplorationAreaRef;
        iconPrefab = eventObjectContainer.EventObject;
        renderCamera = GameObject.Find("CameraParent/PlanetCamera").GetComponent<Camera>();
        iconInstance = Instantiate(iconPrefab, explorationArea);
        renderTexture = renderCamera.targetTexture;
        Sprite sprite = AssignSpriteToSlot(transform.name);
        iconInstance.transform.Find("Image/Icon").GetComponent<Image>().sprite = sprite;
        iconInstance.name = transform.name;
    }

    void Update()
    {
        if (renderTexture != null && iconInstance != null)
        {
            float distanceToCamera = Vector3.Distance(transform.position, renderCamera.transform.position);

            // Check if the sprite is outside the spherical radius
            if (distanceToCamera > sphericalRadius && transform.name != "Player")
            {
                iconInstance.SetActive(false);
            }
            else if (distanceToCamera > sphericalRadius && transform.name == "Player")
            {
                iconInstance.SetActive(true);
                _ = iconInstance.GetComponent<Image>().color = Color.red;
                _ = iconInstance.GetComponent<Button>().interactable = false;
                Vector2 pixelPosition = renderCamera.WorldToScreenPoint(transform.position);
                Vector2 viewportPosition = new(
                    pixelPosition.x - 510f,
                    pixelPosition.y - 430f
                );
                iconInstance.GetComponent<RectTransform>().anchoredPosition = viewportPosition;
            }
            else
            {
                // Sprite is within the desired range, make sure it's active
                iconInstance.SetActive(true);
                _ = iconInstance.GetComponent<Image>().color = Color.white;
                _ = iconInstance.GetComponent<Button>().interactable = true;
                Vector2 pixelPosition = renderCamera.WorldToScreenPoint(transform.position);
                Vector2 viewportPosition = new(
                    pixelPosition.x - 510f,
                    pixelPosition.y - 430f
                );
                iconInstance.GetComponent<RectTransform>().anchoredPosition = viewportPosition;
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log($@"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 4000: EventIcon.OnDestroy() {transform.name}");
        if (planet == null)
        {
            return;
        }
        Debug.Log($@"@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 4010");

        List<GameObject> eventObjects = planet.GetComponent<Planet>().eventObjects;
        
        // remove the event object from the list
        eventObjects.Remove(gameObject);
    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }
}
