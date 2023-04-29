using UnityEngine;
using UnityEngine.UI;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 1f; // Speed of rotation in degrees per second

    private RectTransform rectTransform;
    private float currentRotation = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        currentRotation += rotationSpeed * UnityEngine.Time.deltaTime;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }
}