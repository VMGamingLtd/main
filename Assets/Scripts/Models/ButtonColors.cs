using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ButtonColors
{
    public static Color highlightCol = new Color(1f, 0.83f, 0.3f, 0.6f);  // unique golden frame color
    public static Color fadedCol = new Color(1f, 1f, 1f, 0.1f);  // near full transparent color for see through images
    public static Color halfFadedCol = new Color(1f, 1f, 1f, 0.5f);  // half transparent color for see through images
    public static Color invisibleCol = new Color(0f, 0f, 0f, 0f);  // all values are 0 so nothing is visible
}
