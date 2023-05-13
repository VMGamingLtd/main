using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScaling : MonoBehaviour
{
    // Draws a window you can resize between 80px and 200px height
    // Just click the box inside the window and move your mouse
    Rect windowRect = new Rect(10, 10, 100, 100);
    bool scaling = false;

    void OnGUI()
    {
        windowRect = GUILayout.Window(0, windowRect, ScalingWindow, "resizeable",
            GUILayout.MinHeight(80), GUILayout.MaxHeight(200));
    }

    void ScalingWindow(int windowID)
    {
        GUILayout.Box("", GUILayout.Width(20), GUILayout.Height(20));
        if (Event.current.type == EventType.MouseUp)
        {
            scaling = false;
        }
        else if (Event.current.type == EventType.MouseDown &&
                 GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
        {
            scaling = true;
        }

        if (scaling)
        {
            windowRect = new Rect(windowRect.x, windowRect.y,
                windowRect.width + Event.current.delta.x, windowRect.height + Event.current.delta.y);
        }
    }
}