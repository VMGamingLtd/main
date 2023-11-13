using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static bool IsMoveEnabled = false;
    public Texture2D cursorTexture;

    public void MoveCommand()
    {
        IsMoveEnabled = true;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
