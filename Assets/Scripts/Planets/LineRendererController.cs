using System.Collections;
using System.Linq;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private static Vector3 planetCenter;
    private static Vector3 endPos;
    private static Vector3[] pathPoints;
    private Coroutine movementCoroutine;
    private float movementSpeed = 0.1f;

    public IEnumerator UpdateLineRenderer()
    {
        yield return new WaitForSeconds(0.1f);
        lineRenderer.positionCount = 0;
        Vector3 startPos = GameObject.Find("PlanetParent/StartPlanet/Player").transform.position;
        endPos = GameObject.Find("Marker(Clone)").transform.position;
        planetCenter = GameObject.Find("PlanetParent").transform.position;
        pathPoints = CurvedPathGenerator.CalculateCurvedPath(startPos, endPos, planetCenter);
        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(pathPoints);
        if (!Planet0Buildings.PlayerMovement)
        {
            Planet0Buildings.PlayerMovement = true;
            StartMovement();
        }
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
        int currentIndex = 0;

        while (currentIndex < pathPoints.Length)
        {
            Vector3 point = pathPoints[currentIndex];

            while (Vector3.Distance(transform.position, point) > 0.01f)
            {
                // Calculate the direction towards the current point
                Vector3 direction = (point - transform.position).normalized;

                // Move the player along the direction
                transform.position += movementSpeed * Time.deltaTime * direction;

                yield return new WaitForSeconds(0.1f);
            }

            currentIndex++;

            if (currentIndex < pathPoints.Length)
            {
                // Update LineRenderer position count and remove the first point
                lineRenderer.positionCount = pathPoints.Length - currentIndex;
                lineRenderer.SetPositions(pathPoints.Skip(currentIndex).ToArray());
            }
        }
        lineRenderer.positionCount = 0;
        GameObject markerPrefab = GameObject.Find("Marker(Clone)");
        Destroy(markerPrefab);
        pathPoints = null;
        endPos = Vector3.zero;
        Planet0Buildings.PlayerMovement = false;
    }
}
