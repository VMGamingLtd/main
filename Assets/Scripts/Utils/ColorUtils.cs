using UnityEngine;

public static class ColorUtils
{
    public static string ColorToString(Color color)
    {
        return $"{color.r}/{color.g}/{color.b}/{color.a}";
    }

    public static Color StringToColor(string colorString)
    {
        string[] values = colorString.Split('/');

        if (values.Length != 4)
        {
            Debug.LogError("Invalid color string format");
        }

        float r = float.Parse(values[0]);
        float g = float.Parse(values[1]);
        float b = float.Parse(values[2]);
        float a = float.Parse(values[3]);

        return new Color(r, g, b, a);
    }
}
