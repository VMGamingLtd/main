using UnityEngine;
using UnityEngine.UI;

public class ScrollViewZoom : MonoBehaviour
{
    public RectTransform controlBounds;
    public ScrollRect scrollRect;
    public float zoomSpeed = 0.1f;
    public float moveSpeed = 10f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    private bool isDragging = false;
    private Vector2 initialPosition;

    private void Update()
    {
        if (!BuildingManager.isDraggingBuilding)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                HandleMobileInput();
            }
            else
            {
                HandlePCInput();
            }
        }
    }

    private void HandleMobileInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (!isDragging && Vector2.Distance(touch.position, initialPosition) > 10f)
                {
                    isDragging = true;
                }

                if (isDragging)
                {
                    if (IsInsideContentArea(initialPosition))
                    {
                        // Move the content based on touch delta
                        Vector2 touchDelta = touch.deltaPosition;
                        Vector2 scrollDelta = moveSpeed * Time.deltaTime * touchDelta;
                        scrollRect.content.anchoredPosition += scrollDelta;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }

        HandleZoomInput();
    }

    private void HandlePCInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (!isDragging && Vector2.Distance(Input.mousePosition, initialPosition) > 10f)
            {
                isDragging = true;
            }

            if (isDragging)
            {
                if (IsInsideContentArea(initialPosition))
                {
                    // Move the content based on mouse delta
                    Vector2 mouseDelta = ((Vector2)Input.mousePosition - initialPosition) * moveSpeed * Time.deltaTime;
                    scrollRect.content.anchoredPosition += mouseDelta;
                    initialPosition = Input.mousePosition;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        HandleZoomInput();
    }

    private void HandleZoomInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f && IsInsideContentArea(Input.mousePosition))
        {
            float zoomAmount = scrollInput * zoomSpeed;

            Vector3 newScale = scrollRect.content.localScale + new Vector3(zoomAmount, zoomAmount, 1);
            newScale = Vector3.Max(newScale, new Vector3(minZoom, minZoom, 1));
            newScale = Vector3.Min(newScale, new Vector3(maxZoom, maxZoom, 1));

            Vector3 positionAdjustment = Vector3.Scale(scrollRect.content.sizeDelta, (newScale - scrollRect.content.localScale) * 0.5f);

            scrollRect.content.localScale = newScale;
            scrollRect.content.anchoredPosition3D -= positionAdjustment;
        }
    }

    private bool IsInsideContentArea(Vector2 screenPosition)
    {
        if (controlBounds.TryGetComponent<RectTransform>(out var contentRect))
        {
            return RectTransformUtility.RectangleContainsScreenPoint(contentRect, screenPosition);
        }
        return false;
    }
}
