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
    }

    void Update()
    {
        if (renderTexture != null && iconInstance != null)
        {
            Vector2 pixelPosition = renderCamera.WorldToScreenPoint(transform.position);

            Vector2 viewportPosition = new(
                pixelPosition.x - 510f,
                pixelPosition.y - 430f
            );

            iconInstance.GetComponent<RectTransform>().anchoredPosition = viewportPosition;
        }
    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }
}
