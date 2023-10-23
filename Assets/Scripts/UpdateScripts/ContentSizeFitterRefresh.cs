using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeFitterRefresh : MonoBehaviour
{
    private ContentSizeFitter contentSizeFitter;

    private void OnEnable()
    {
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        RefreshContentSize();
    }
    private void RefreshContentSize()
    {
        StartCoroutine(DelayedRefreshContentSize());
    }

    private IEnumerator DelayedRefreshContentSize()
    {
        yield return new WaitForSeconds(0.1f);
        contentSizeFitter.enabled = false;
        yield return new WaitForSeconds(0.1f);
        contentSizeFitter.enabled = true;
    }
}
