using UnityEngine;

public class PlanetSelfRotation : MonoBehaviour
{
    private Vector3 rotationSpeed = new(0.1f, 0.1f, 0.1f);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
