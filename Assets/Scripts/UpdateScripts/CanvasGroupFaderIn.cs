using System.Collections;
using UnityEngine;

public class CanvasGroupFaderIn : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float startAlpha = 0f;
    private float endAlpha = 1f;
    public float totalDuration = 1.0f;
    private float remainingTime = 0f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = startAlpha;
    }
    public void FadeInObject()
    {
        StartCoroutine(FadeCanvasGroup());
    }

    private IEnumerator FadeCanvasGroup()
    {
        while (remainingTime < totalDuration)
        {
            remainingTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(remainingTime / totalDuration);
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            canvasGroup.alpha = currentAlpha;
            yield return null;
        }
        canvasGroup.interactable = true;
    }
}
