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

        // Check if tooltip is off the right edge of the screen
        if (xPosition + tooltipWidth + horizontalPadding > screenWidth)
        {
            xPosition = screenWidth - tooltipWidth - horizontalPadding;
        }

        // Check if tooltip is off the left edge of the screen
        if (xPosition - horizontalPadding < 0)
        {
            xPosition = horizontalPadding;
        }

        // Check if tooltip is off the top edge of the screen
        if (yPosition + tooltipHeight + verticalPadding > screenHeight)
        {
            yPosition = screenHeight - tooltipHeight - verticalPadding;
        }

        // Check if tooltip is off the bottom edge of the screen
        if (yPosition - verticalPadding < 0)
        {
            yPosition = verticalPadding;
        }

        tooltipRect.localPosition = new Vector2(xPosition, yPosition);
    }
}
