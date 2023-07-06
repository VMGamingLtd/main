using UnityEngine;
using UnityEngine.UI;

public class ScrollViewZoom : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float zoomSpeed = 0.1f;
    public float moveSpeed = 10f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    private bool isDragging = false;


    private float initialDistance;
    private Vector3 initialScale;
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
    private void SetInitialPosition()
{
    RectTransform contentRect = scrollRect.content.GetComponent<RectTransform>();
    RectTransform rootRect = scrollRect.content.Find("Root")?.GetComponent<RectTransform>();

    if (contentRect != null && rootRect != null)
    {
        Vector2 contentSize = contentRect.sizeDelta;
        Vector2 rootPosition = rootRect.anchoredPosition;
        Vector2 viewportSize = ((RectTransform)scrollRect.viewport.transform).sizeDelta;

        Vector2 contentCenter = (contentSize - viewportSize) * 0.5f;
        initialPosition = -rootPosition - contentCenter;

        scrollRect.content.anchoredPosition = initialPosition;
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
                    // Move the content based on touch delta
                    Vector2 touchDelta = touch.deltaPosition;
                    Vector2 scrollDelta = touchDelta * moveSpeed * Time.deltaTime;
                    scrollRect.content.anchoredPosition += scrollDelta;
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
                // Move the content based on mouse delta
                Vector2 mouseDelta = ((Vector2)Input.mousePosition - initialPosition) * moveSpeed * Time.deltaTime;
                scrollRect.content.anchoredPosition += mouseDelta;
                initialPosition = Input.mousePosition;
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

        if (scrollInput != 0f)
        {
            float zoomAmount = scrollInput * zoomSpeed;

            Vector2 newScale = (Vector2)scrollRect.content.localScale + new Vector2(zoomAmount, zoomAmount);
            newScale = Vector2.Max(newScale, new Vector2(minZoom, minZoom));
            newScale = Vector2.Min(newScale, new Vector2(maxZoom, maxZoom));

            Vector2 positionAdjustment = Vector2.Scale((Vector2)scrollRect.content.sizeDelta, (newScale - (Vector2)scrollRect.content.localScale) * 0.5f);

            scrollRect.content.localScale = newScale;
            scrollRect.content.anchoredPosition -= positionAdjustment;
        }
    }
}
