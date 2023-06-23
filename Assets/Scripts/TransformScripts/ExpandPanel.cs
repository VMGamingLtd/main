using UnityEngine;
using System.Collections;

public class ExpandPanel : MonoBehaviour
{
    public RectTransform rectTransform;
    public float targetWidth = 0f;
    public float expandDuration = 0.5f;
    public float currentWidth;
    public GameObject[] objectsToDeactivate;
    public GameObject[] objectsToActivate;

    public void StartExpand()
    {
        StartCoroutine(ExpandWidthCoroutine());
    }

    public IEnumerator ExpandWidthCoroutine()
    {
        float timer = 0f;

        if (objectsToDeactivate != null)
        {
            foreach (GameObject obj in objectsToDeactivate)
            {
                obj.SetActive(false);
            }
        }

        while (timer < expandDuration)
        {
            // Calculate the new width based on the interpolation
            float newWidth = Mathf.Lerp(currentWidth, targetWidth, timer / expandDuration);

            // Set the new width for the RectTransform
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

            // Update the timer
            timer += Time.deltaTime;
            yield return null;
        }
        // Set the final width
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);

        if (objectsToActivate != null)
        {
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }
    }
}
