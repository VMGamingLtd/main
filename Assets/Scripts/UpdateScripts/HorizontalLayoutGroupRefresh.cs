using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalLayoutGroupRefresh : MonoBehaviour
{
    private HorizontalLayoutGroup horizontalLayoutGroup;

    private void OnEnable()
    {
        horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        RefreshComponent();
    }
    private void RefreshComponent()
    {
        StartCoroutine(DelayedRefreshComponent());
    }

    private IEnumerator DelayedRefreshComponent()
    {
        yield return new WaitForSeconds(0.1f);
        horizontalLayoutGroup.enabled = false;
        yield return new WaitForSeconds(0.2f);
        horizontalLayoutGroup.enabled = true;
    }
}
