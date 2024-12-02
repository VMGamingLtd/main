using System;
using System.Globalization;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

public static class ColorUtils
{
    public static string ColorToString(Color color)
    {
        return string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}/{3}", color.r, color.g, color.b, color.a);
    }

    public static Color StringToColor(string colorString)
    {
        string[] values = colorString.Split('/');

        if (values.Length != 4)
        {
            Debug.LogError("Invalid color string format");
        }

        float ParseValue(string value)
        {
            float result;

            // Attempt to parse using both formats
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                // Successfully parsed with InvariantCulture
                return result;
            }

            if (float.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out result))
            {
                // Successfully parsed with CurrentCulture
                return result;
            }

            // If parsing fails, attempt to normalize separators
            value = value.Replace(',', '.'); // Replace comma with period
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }

            Debug.LogError($"Unable to parse value: {value}");
            return 0f; // Default to 0 if parsing fails
        }

        try
        {
            float r = ParseValue(values[0]);
            float g = ParseValue(values[1]);
            float b = ParseValue(values[2]);
            float a = ParseValue(values[3]);

            return new Color(r, g, b, a);
        }
        catch (FormatException ex)
        {
            Debug.LogError($"Error parsing color values: {ex.Message}");
            return Color.black;
        }
    }
}
