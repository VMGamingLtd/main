using UnityEngine;

public class TooltipFollowMouse : MonoBehaviour
{
    public float xOffset = 10f;
    public float yOffset = 10f;
    public RectTransform canvasRect;
    public float horizontalPadding = 10f;
    public float verticalPadding = 10f;
    private Camera mainCamera;

    public RectTransform tooltipRect;
    private float screenWidth;
    private float screenHeight;

    private void Awake()
    {
        tooltipRect = GetComponent<RectTransform>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        mainCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePosition, null, out localPoint);

        float tooltipWidth = tooltipRect.rect.width;
        float tooltipHeight = tooltipRect.rect.height;
        float xPosition = localPoint.x + xOffset;
        float yPosition = localPoint.y + yOffset;
        //Adjust the x position to keep the tooltip within the screen bounds
        if (xPosition + tooltipWidth + horizontalPadding > screenWidth)
        {
            xPosition = screenWidth - tooltipWidth - horizontalPadding;
        }
        else if (xPosition < 0)
        {
            xPosition -= horizontalPadding;
        }

        // Adjust the y position to keep the tooltip within the screen bounds
        if (yPosition + tooltipHeight + verticalPadding > screenHeight)
        {
            yPosition = screenHeight - tooltipHeight - verticalPadding;
        }
        else if (yPosition - verticalPadding < 0)
        {
            yPosition = verticalPadding;
        }

        // Offset the tooltip position by the padding coordinates from the mouse cursor
        xPosition += horizontalPadding;
        yPosition += verticalPadding;

        // Set the tooltip position
        tooltipRect.localPosition = new Vector2(xPosition, yPosition);
    }

    // Moves the tooltip object list away from the camera to avoid window intersection when moving fast in-between multiple objects with mouse cursor
    private void OnDisable()
    {
        tooltipRect.localPosition = new Vector2(-500, -2000);
    }

}
