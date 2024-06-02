using UnityEngine;

public class InteractWithRenderTexture : MonoBehaviour
{
    public LayerMask interactionLayer;
    private Camera renderCamera;
    private float distanceFromSurface = 0.1f;
    public GameObject markerPrefab;
    private LineRendererController lineRendererController;
    private GameObject planet;
    public static float currentZoomLevel = 0.5f;

    void Start()
    {
        lineRendererController = GameObject.Find("PlanetParent/StartPlanet/Player").GetComponent<LineRendererController>();
        renderCamera = GameObject.Find("CameraParent/PlanetCamera").GetComponent<Camera>();
        planet = GameObject.Find("PlanetParent/StartPlanet");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (!InteractionManager.IsMoveEnabled)
                return;

            Vector3 mousePosition = Input.mousePosition;

            // Cast a ray from the camera
            Ray ray = renderCamera.ScreenPointToRay(mousePosition);
            float xOffset = currentZoomLevel * -1;
            float yOffset = currentZoomLevel * -1;
            // Apply the offsets to the ray's origin as Overlay canvas always makes an offset to actual viewport
            Vector3 offset = renderCamera.transform.TransformVector(new Vector3(xOffset, yOffset, 0));
            ray.origin += offset;

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactionLayer))
            {
                Vector3 destinationPosition = hit.point + hit.normal * distanceFromSurface;
                GameObject oldMarker = GameObject.Find("PlanetParent/StartPlanet/Marker(Clone)");
                Destroy(oldMarker);

                Instantiate(markerPrefab, destinationPosition, Quaternion.identity, planet.transform);

                StartCoroutine(lineRendererController.UpdateLineRenderer());

                InteractionManager.IsMoveEnabled = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Debug.Log("Raycast did not hit any object on the interaction layer");
            }
        }
    }
}
