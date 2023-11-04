using UnityEngine;

public static class CurvedPathGenerator
{
    private static int numberOfPoints = 40;

    public static Vector3[] CalculateCurvedPath(Vector3 startPos, Vector3 endPos, Vector3 planetCenter)
    {
        Vector3[] pathPoints = new Vector3[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1);
            Vector3 pointOnCurve = CalculatePointOnCurve(startPos, endPos, planetCenter, t);
            pathPoints[i] = pointOnCurve;
        }
        return pathPoints;
    }

    private static Vector3 CalculatePointOnCurve(Vector3 startPos, Vector3 endPos, Vector3 planetCenter, float t)
    {
        // Calculate the curve point
        Vector3 curvePoint = Vector3.Lerp(startPos, endPos, t);

        // Calculate the direction from the planet center to the curve point
        Vector3 direction = curvePoint - planetCenter;

        // Calculate the curve height offset
        float yOffset = Mathf.Sin(t * Mathf.PI);

        // Adjust the curve point based on the spherical surface
        curvePoint = planetCenter + direction.normalized * planetCenter.magnitude / 1.37f;

        return curvePoint;
    }
}
