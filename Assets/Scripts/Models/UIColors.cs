using UnityEngine;

public static class UIColors
{
    // BUTTON COLORS
    public static Color highlightCol = new(1f, 0.83f, 0.3f, 0.6f);  // unique golden frame color
    public static Color fadedCol = new(1f, 1f, 1f, 0.1f);  // near full transparent color for see through images
    public static Color halfFadedCol = new(1f, 1f, 1f, 0.5f);  // half transparent color for see through images
    public static Color invisibleCol = new(0f, 0f, 0f, 0f);  // all values are 0 so nothing is visible

    // TIMEBAR COLORS
    public static Color timebarColorGreen = new(0.254902f, 0.9058824f, 0.1333333f, 0.3137255f);
    public static Color timebarColorRed = new(0.9058824f, 0.1333333f, 0.2196078f, 0.3137255f);
    public static Color timebarColorYellow = new(0.9058824f, 0.8666667f, 0.1333333f, 0.3137255f);

    // STANDARD COLORS
    public static Color blackHalfTransparent = new(0f, 0f, 0f, 0.3764706f);
}
