using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EventIconType
{
    Player,
    FriendlyPlayer,
    EnemyPlayer,
    Fish,
    Plant,
    Mineral,
    FurAnimal,
    MilkAnimal,
    Animal
}

public class EventIcon : MonoBehaviour
{
    private EventObjectContainer eventObjectContainer;
    private RenderTexture renderTexture;
    private Transform explorationArea;
    private GameObject iconPrefab;
    private Camera renderCamera;
    private GameObject iconInstance;
    private GameObject planet;
    private GameObject playerIcon;

    [SerializeField]
    private float sphericalRadius = Player.VisibilityRadius / 20;

    public EventIconType iconType;
    public float CurrentQuantity;
    public float MinQuantityRange;
    public float MaxQuantityRange;
    public float Elevation;
    public Guid RecipeGuid;
    public Guid RecipeGuid2;
    public Guid RecipeGuid3;
    public Guid RecipeGuid4;
    public int RecipeIndex = -1;
    public int RecipeIndex2 = -1;
    public int RecipeIndex3 = -1;
    public int RecipeIndex4 = -1;
    public string RecipeProduct = string.Empty;
    public string RecipeProduct2 = string.Empty;
    public string RecipeProduct3 = string.Empty;
    public string RecipeProduct4 = string.Empty;
    public bool PlayerDistance;

    public void SetPlanet(GameObject planet)
    {
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
        playerIcon = GameObject.Find("PlanetParent/StartPlanet/Player");

        var component = iconInstance.AddComponent<EventIconData>();
        component.Name = transform.name;
        component.Type = iconType;
        component.CurrentQuantity = MaxQuantityRange;
        component.MinQuantityRange = MinQuantityRange;
        component.MaxQuantityRange = MaxQuantityRange;
    }

    void Update()
    {
        if (renderTexture != null && iconInstance != null)
        {
            sphericalRadius = Player.VisibilityRadius / 20;
            float distanceToCamera = Vector3.Distance(transform.position, renderCamera.transform.position);
            float distanceToPlayer = Vector3.Distance(transform.position, playerIcon.transform.position);

            // Check if the sprite is outside the spherical radius
            if (distanceToPlayer > sphericalRadius && transform.name != "Player")
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

            if (PlayerDistance)
            {
                _ = iconInstance.GetComponent<Image>().color = Color.green;
            }
        }
    }

    private void OnDestroy()
    {
        if (planet == null)
        {
            return;
        }
        List<GameObject> eventObjects = planet.GetComponent<Planet>().eventObjects;

        // remove the event object from the list
        eventObjects.Remove(gameObject);

        if (iconInstance != null)
        {
            // destroy the icon
            Destroy(iconInstance);
        }

    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }
}
