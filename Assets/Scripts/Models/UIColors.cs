using UnityEngine;

public static class UIColors
{
    // BUTTON COLORS
    public static Color highlightCol = new(1f, 0.83f, 0.3f, 0.6f);  // unique golden frame color
    public static Color fadedCol = new(1f, 1f, 1f, 0.1f);  // near full transparent color for see through images
    public static Color quarterFadedCol = new(1f, 1f, 1f, 0.2f);  // half transparent color for see through images
    public static Color halfFadedCol = new(1f, 1f, 1f, 0.5f);  // half transparent color for see through images
    public static Color invisibleCol = new(0f, 0f, 0f, 0f);  // all values are 0 so nothing is visible
    public static Color quarterGreenCol = new(0.2549f, 0.9058f, 0.1333f, 0.2f);

    // TIMEBAR COLORS
    public static Color timebarColorGreen = new(0.254902f, 0.9058824f, 0.1333333f, 0.3137255f);
    public static Color timebarColorRed = new(0.9058824f, 0.1333333f, 0.2196078f, 0.3137255f);
    public static Color timebarColorYellow = new(0.9058824f, 0.8666667f, 0.1333333f, 0.3137255f);

    // STANDARD COLORS
    public static Color blackHalfTransparent = new(0f, 0f, 0f, 0.3764706f);
    public static Color NeonBlueOriginalFull = new(0.3882353f, 0.9254902f, 1f, 1f);
    public static Color NeonBlueOriginalHalf = new(0.3882353f, 0.9254902f, 1f, 0.5f);
    public static Color NeonRedFull = new(1f, 0f, 0f, 1f);
    public static Color NeonRedHalf = new(1f, 0f, 0f, 0.5f);
    public static Color NeonRedInvisible = new(1f, 0f, 0f, 0f);
    public static Color YellowInvisible = new(1f, 0.8431373f, 0f, 0f);
}
