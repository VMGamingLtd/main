using UnityEngine;
using UnityEngine.UI;

public class EventIcon : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Transform explorationArea;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private Sprite iconSprite;
    private Camera renderCamera;
    private GameObject iconInstance;

    void Start()
    {
        if (explorationArea != null)
        {
            renderCamera = GameObject.Find("CameraParent/PlanetCamera").GetComponent<Camera>();
            iconInstance = Instantiate(iconPrefab, explorationArea);
            iconInstance.transform.Find("Image/Icon").GetComponent<Image>().sprite = iconSprite;
        }
    }

    void Update()
    {
        if (renderTexture != null && iconInstance != null)
        {
            RenderTexture.active = renderTexture;

            // Capture the pixel position within the RenderTexture
            Vector2 pixelPosition = renderCamera.WorldToScreenPoint(transform.position);

            Vector2 viewportPosition = new(
                pixelPosition.x - 510f,
                pixelPosition.y - 430f
            );

            // Set the position of the icon
            iconInstance.GetComponent<RectTransform>().anchoredPosition = viewportPosition;

            RenderTexture.active = null;
        }
    }
}
