using System.Collections;
using System.Linq;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private static Vector3 planetCenter;
    private static Vector3 startPos;
    private static Vector3 endPos;
    public static Vector3[] pathPoints;
    private Coroutine movementCoroutine;
    private bool shouldStopCoroutine;

    public IEnumerator UpdateLineRenderer()
    {
        yield return new WaitForSeconds(0.1f);
        lineRenderer.positionCount = 0;
        startPos = GameObject.Find("PlanetParent/StartPlanet/Player").transform.position;
        endPos = GameObject.Find("PlanetParent/StartPlanet/Marker(Clone)").transform.position;
        planetCenter = GameObject.Find("PlanetParent").transform.position;
        pathPoints = CurvedPathGenerator.CalculateCurvedPath(startPos, endPos, planetCenter);
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
        if (!PlayerResources.PlayerMovement) PlayerResources.PlayerMovement = true;
        PlayerResources.PlayerCurrentTravelProgress = 0;
        StartMovement();
    }
    public void StartMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }

        movementCoroutine = StartCoroutine(MovePlayerTowardsEndPos());
    }

    private IEnumerator MovePlayerTowardsEndPos()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = startPos;
        PlayerResources.PlayerPixelDistance = CalculateTotalDistance();
        PlayerResources.PlayerTotalDistance = PlayerResources.PlayerPixelDistance * 10;
        PlayerResources.PlayerDistancePerLinePoint = PlayerResources.PlayerPixelDistance / 40;
        PlayerResources.PlayerRemainingTravelTime = PlayerResources.PlayerTotalDistance / PlayerResources.PlayerMovementSpeed * 60 * 60;
        float travelTimePerPoint = PlayerResources.PlayerRemainingTravelTime / 40;
        float distanceCovered = 0f;
        int currentIndex = 0;
        shouldStopCoroutine = false;

        while (currentIndex < pathPoints.Length && !shouldStopCoroutine)
        {
            Vector3 point = pathPoints[currentIndex];
            float distanceToPoint = Vector3.Distance(transform.position, point);

            while (distanceToPoint > 0.005f && !shouldStopCoroutine)
            {
                Vector3 direction = (point - transform.position).normalized;
                float distanceToMove = PlayerResources.PlayerDistancePerLinePoint / travelTimePerPoint;
                transform.position += direction * distanceToMove * 1.1f;
                distanceCovered += distanceToMove;

                PlayerResources.PlayerRemainingDistance = PlayerResources.PlayerTotalDistance - (distanceCovered * 10);
                PlayerResources.PlayerRemainingTravelTime--;
                PlayerResources.PlayerCurrentTravelProgress = (distanceCovered * 10 / PlayerResources.PlayerTotalDistance) * 100f;



                yield return new WaitForSeconds(1f);
                distanceToPoint = Vector3.Distance(transform.position, point);
                UpdateLine();
                if (PlayerResources.PlayerRemainingTravelTime < 2f)
                {
                    transform.position = endPos;
                    shouldStopCoroutine = true;
                }
            }

            currentIndex++;
            if (currentIndex < pathPoints.Length)
            {
                lineRenderer.positionCount = pathPoints.Length - currentIndex;
                lineRenderer.SetPositions(pathPoints.Skip(currentIndex).ToArray());
            }
        }
        lineRenderer.positionCount = 0;
        GameObject markerPrefab = GameObject.Find("PlanetParent/StartPlanet/Marker(Clone)");
        Destroy(markerPrefab);
        pathPoints = null;
        endPos = Vector3.zero;
        PlayerResources.PlayerRemainingTravelTime = 0;
        PlayerResources.PlayerCurrentTravelProgress = 100;
    }

    private float CalculateTotalDistance()
    {
        float totalDistance = 0f;

        // Calculate the total distance by summing up the distances between consecutive points
        for (int i = 0; i < pathPoints.Length - 1; i++)
        {
            totalDistance += Vector3.Distance(pathPoints[i], pathPoints[i + 1]);
        }

        return totalDistance;
    }


    private void UpdateLine()
    {
        lineRenderer.positionCount = 0;
        Vector3 startPos = GameObject.Find("PlanetParent/StartPlanet/Player").transform.position;
        endPos = GameObject.Find("PlanetParent/StartPlanet/Marker(Clone)").transform.position;
        planetCenter = GameObject.Find("PlanetParent").transform.position;
        pathPoints = CurvedPathGenerator.CalculateCurvedPath(startPos, endPos, planetCenter);
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
    }
}
