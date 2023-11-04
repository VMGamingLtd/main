using UnityEngine;

public class InteractWithRenderTexture : MonoBehaviour
{
    public LayerMask interactionLayer;
    private float xOffset = -0.5f;
    private float yOffset = -0.4f;
    private Camera renderCamera;
    private float distanceFromSurface = 0.1f;
    public GameObject markerPrefab;
    private LineRendererController lineRendererController;
    public static float currentZoomLevel = 1f;

    void Start()
    {
        lineRendererController = GameObject.Find("PlanetParent/StartPlanet/Player").GetComponent<LineRendererController>();
        renderCamera = GameObject.Find("CameraParent/PlanetCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 mousePosition = Input.mousePosition;

            // Cast a ray from the camera
            Ray ray = renderCamera.ScreenPointToRay(mousePosition);

            // Apply the offsets to the ray's origin as Overlay canvas always makes an offset to actual viewport
            Vector3 offset = renderCamera.transform.TransformVector(new Vector3(xOffset, yOffset, 0));
            ray.origin += offset;

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactionLayer))
            {
                Vector3 destinationPosition = hit.point + hit.normal * distanceFromSurface;
                GameObject oldMarker = GameObject.Find("Marker(Clone)");
                Destroy(oldMarker);

                Instantiate(markerPrefab, destinationPosition, Quaternion.identity);
                StartCoroutine(lineRendererController.UpdateLineRenderer());

                lineRendererController.StartMovement();
            }
        }
    }
}
