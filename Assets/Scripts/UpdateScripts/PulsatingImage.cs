using UnityEngine;
using UnityEngine.UI;

public class PulsatingImage : MonoBehaviour
{
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    public float pulseSpeed = 1f;
    private GameObject targetObject;

    private Image image;
    private Color originalColor;
    private float timer = 0f;

    private void Start()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        targetObject = gameObject;
    }

    private void Update()
    {
        if (PlayerResources.PlayerEnergy == "00:00:00:00")
        {
            if (targetObject.activeSelf)
            {
                return;
            }
            else
            {
                timer += Time.deltaTime * pulseSpeed;
                float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.Sin(timer));
                Color currentColor = originalColor;
                currentColor.a = currentAlpha;
                image.color = currentColor;
            }
        }
        else
        {
            targetObject.SetActive(false);
        }
    }
}
