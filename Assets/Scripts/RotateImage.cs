using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RotateImage : MonoBehaviour {
    private float rotationSpeed = 360f; // speed of rotation in degrees per second
    private Color targetColor = Color.yellow;
    public TextMeshProUGUI planet0Index;


    public IEnumerator RotateOverTime(float duration) {
        float currentTime = 0f;
        Planet0Buildings.CalculatePlanet0Index();
        Image image = GetComponent<Image>();
        Color startColor = image.color;
        image.color = targetColor;
        while (currentTime < duration) {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
        planet0Index.text = $"{Planet0Buildings.Planet0Index.ToString()}%";
        image.color = startColor;
    }

    void Start() {
        StartCoroutine(RotateOverTime(0.5f));
    }
}
