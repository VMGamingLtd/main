using UnityEngine;
using UnityEngine.UIElements;

public class TooltipFollowMouse : MonoBehaviour
{
    public float xOffset = 0f;
    public float yOffset = 0f;
    private RectTransform canvasRect;
    private RectTransform tooltipRect;

    private void Awake()
    {
        tooltipRect = GetComponent<RectTransform>();
        canvasRect = GameObject.Find("TooltipCanvas").GetComponent<RectTransform>();
        tooltipRect = transform.GetComponent<RectTransform>();
    }
    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePosition, null, out localPoint);

        float xPosition = localPoint.x + xOffset;
        float yPosition = localPoint.y + yOffset;

        float yAdjustment = SmoothYAdjustment(yPosition);


        if (xPosition > 600f)
        {
            xPosition -= tooltipRect.rect.width / 1.8f + xOffset;
        }


        yPosition += yAdjustment;


        // Set the tooltip position
        tooltipRect.localPosition = new Vector2(xPosition, yPosition);
    }
    private float SmoothXAdjustment(float xPosition)
    {
        float startX = -600f;
        float endX = 900f;
        float startAdjustmentX = 100f;
        float endAdjustmentX = -200f;

        float smoothAdjustmentX = Mathf.Lerp(startAdjustmentX, endAdjustmentX, Mathf.InverseLerp(startX, endX, xPosition));

        return smoothAdjustmentX;
    }
    private float SmoothYAdjustment(float yPosition)
    {
        // Define the start and end yPosition values and corresponding adjustment values
        float startY = 500f;
        float endY = -500f;
        float startAdjustmentY = -100f;
        float endAdjustmentY = 170f;

        // Use Mathf.Lerp to calculate the smooth adjustment between start and end values
        float smoothAdjustmentY = Mathf.Lerp(startAdjustmentY, endAdjustmentY, Mathf.InverseLerp(startY, endY, yPosition));

        return smoothAdjustmentY;
    }

    // Moves the tooltip object list away from the camera to avoid window intersection when moving fast in-between multiple objects with mouse cursor
    private void OnDisable()
    {
        tooltipRect.localPosition = new Vector2(-500, -2000);
    }

}
