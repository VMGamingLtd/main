using UnityEngine;

public class PlanetControl : MonoBehaviour
{
    public Transform customPivot;
    public Camera mainCamera;
    private Vector3 lastTouchPos;
    private Vector3 initialTouchPos;
    private Vector3 rotationOffset;
    private bool isDragging;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float rotationDamping = 0.95f; // Adjust the damping factor
    public float dragThreshold = 2.0f;
    public float zoomSpeed = 1.0f;
    private float minZoomSize = 3.0f;
    private float maxZoomSize = 8.0f;

    void Start()
    {
        // Store the initial rotation
        initialRotation = customPivot.localRotation;
        targetRotation = customPivot.localRotation;
    }

    void Update()
    {
        // Rotation
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            initialTouchPos = GetTouchPosition();
            isDragging = false;
        }

        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            Vector3 currentTouchPos = GetTouchPosition();
            Vector3 touchDelta = currentTouchPos - initialTouchPos;

            if (!isDragging && touchDelta.magnitude >= dragThreshold)
            {
                isDragging = true;
            }

            if (isDragging)
            {
                rotationOffset = new Vector3(-touchDelta.y, touchDelta.x, 0) * 0.01f;
                targetRotation *= Quaternion.Euler(rotationOffset);
            }
        }

        // Apply damping to rotation
        customPivot.localRotation = Quaternion.Slerp(customPivot.localRotation, targetRotation, 1 - rotationDamping);

        // Zoom
        float zoomInput = Input.GetAxis("Mouse ScrollWheel") + GetPinchZoomInput();

        if (zoomInput != 0)
        {
            float newSize = mainCamera.orthographicSize - zoomInput * zoomSpeed;
            newSize = Mathf.Clamp(newSize, minZoomSize, maxZoomSize);
            mainCamera.orthographicSize = newSize;
            InteractWithRenderTexture.currentZoomLevel = newSize / 10;
        }

        lastTouchPos = GetTouchPosition();
    }

    Vector3 GetTouchPosition()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;

        return Input.mousePosition;
    }

    float GetPinchZoomInput()
    {
        if (Input.touchCount >= 2)
        {
            // Calculate the distance between the two touches
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector2 prevTouch1Pos = touch1.position - touch1.deltaPosition;
            Vector2 prevTouch2Pos = touch2.position - touch2.deltaPosition;
            float prevTouchDeltaMag = (prevTouch1Pos - prevTouch2Pos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            // Calculate the pinch zoom input
            return touchDeltaMag - prevTouchDeltaMag;
        }

        return 0;
    }
}
