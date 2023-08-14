using UnityEngine;
using UnityEngine.EventSystems;

public static class UIUtils
{
    public static bool IsWithinBounds(PointerEventData eventData, RectTransform bounds)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(bounds, eventData.position, null, out localPoint);

        Rect boundsRect = new Rect(bounds.rect.position, bounds.rect.size);

        return boundsRect.Contains(localPoint);
    }
    public static bool CheckForOverlap(GameObject draggedObject)
    {
        BoxCollider2D draggedCollider = draggedObject.GetComponent<BoxCollider2D>();

        if (draggedCollider == null)
        {
            return false;
        }

        Vector2 colliderSize = new Vector2(100f, 100f);

        return Physics2D.OverlapBox(draggedCollider.bounds.center, colliderSize, 0f) != null;
    }
}
