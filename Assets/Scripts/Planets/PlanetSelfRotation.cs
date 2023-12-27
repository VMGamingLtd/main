using UnityEngine;

public class PlanetSelfRotation : MonoBehaviour
{
    private Vector3 rotationSpeed = new(0.05f, 0.05f, 0.05f);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
