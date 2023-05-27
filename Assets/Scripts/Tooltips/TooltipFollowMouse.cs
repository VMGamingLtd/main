using UnityEngine;

public class TooltipFollowMouse : MonoBehaviour
{
    public float xOffset = 10f;
    public float yOffset = 10f;
    public RectTransform canvasRect;
    public float horizontalPadding = 10f;
    public float verticalPadding = 10f;

    public RectTransform tooltipRect;
    private float screenWidth;
    private float screenHeight;

    private void Awake()
    {
        tooltipRect = GetComponent<RectTransform>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
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

        // Adjust the x position to keep the tooltip within the screen bounds
        if (xPosition + tooltipWidth + horizontalPadding > screenWidth)
        {
            xPosition = screenWidth - tooltipWidth - horizontalPadding;
        }
        else if (xPosition - horizontalPadding < -300)
        {
            xPosition = horizontalPadding;
        }

        // Adjust the y position to keep the tooltip within the screen bounds
        if (yPosition + tooltipHeight + verticalPadding > screenHeight)
        {
            yPosition = screenHeight - tooltipHeight - verticalPadding;
        }
        else if (yPosition - verticalPadding < -300)
        {
            yPosition = verticalPadding;
        }

        // Offset the tooltip position by the padding coordinates from the mouse cursor
        xPosition += horizontalPadding;
        yPosition += verticalPadding;

        // Set the tooltip position
        tooltipRect.localPosition = new Vector2(xPosition, yPosition);
    }

}
