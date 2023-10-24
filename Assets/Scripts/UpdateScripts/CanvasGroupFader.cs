using System.Collections;
using UnityEngine;

public class CanvasGroupFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float startAlpha = 1f;
    private float endAlpha = 0f;
    private float totalDuration = 1.5f;
    private float remainingTime = 0f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = startAlpha;
    }
    void OnEnable()
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
        gameObject.SetActive(false);
    }
}
