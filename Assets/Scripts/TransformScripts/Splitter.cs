using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using ItemManagement;

public class Splitter : MonoBehaviour
{
    public static bool isSplitting = false;
    public Texture2D cursorImage;
    public static bool isAwaitingInput = false;
    public Transform clickDetectionArea;

    // Implement the cursor loading logic
    private void InitializeCursor()
    {
        Vector2 cursorSize = Vector2.one;
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
    }

    public async void InitializeSplitMode()
    {
        isSplitting = true;
        isAwaitingInput = true;
        InitializeCursor();
        while (isAwaitingInput)
        {
            await UniTask.DelayFrame(10);
            // Continue waiting for input
        }

        ResetCursorAndSplitting();
    }

    public void ResetCursorAndSplitting()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        isSplitting = false;
    }
}
