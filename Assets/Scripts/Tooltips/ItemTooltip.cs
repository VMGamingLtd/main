using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipObjects tooltipObjects;
    private TooltipObjectsSmall smallTooltipObjects;
    private TooltipObjectsItems itemTooltipObjects;
    private TooltipFollowMouse tooltipFollowMouse;

    private void Awake()
    {
        tooltipObjects = GameObject.Find("TooltipCanvas/TooltipObjects").GetComponent<TooltipObjects>();
        smallTooltipObjects = GameObject.Find("TooltipCanvas/TooltipObjectsSmall").GetComponent<TooltipObjectsSmall>();
        itemTooltipObjects = GameObject.Find("TooltipCanvas/TooltipObjectsItems").GetComponent<TooltipObjectsItems>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GlobalCalculator.GameStarted)
        {
            string objectName = eventData.pointerEnter.transform.name;
            StartCoroutine(DisplayTooltip(objectName));
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideAllTooltips();

        /*if (eventData != null)
        {
            string objectName = eventData.pointerEnter.transform.name;
            HideTooltip(objectName);
        }
        else
        {
            HideAllTooltips();
        }*/
    }
    private GameObject FindTooltipObject(string objectName)
    {
        foreach (GameObject tooltipObj in tooltipObjects.tooltipObjects)
        {
            if (tooltipObj.name == objectName)
            {
                return tooltipObj;
            }
        }
        foreach (GameObject tooltipObj in smallTooltipObjects.tooltipObjectsSmall)
        {
            if (tooltipObj.name == objectName)
            {
                return tooltipObj;
            }
        }
        foreach (GameObject tooltipObj in itemTooltipObjects.tooltipObjectItems)
        {
            if (tooltipObj.name == objectName)
            {
                return tooltipObj;
            }
        }

        return null;
    }

    public IEnumerator DisplayTooltip(string objectName)
    {
        HideAllTooltips();
        GameObject tooltipObject = FindTooltipObject(objectName);
        if (tooltipObject != null)
        {
            tooltipFollowMouse = tooltipObject.transform.parent.GetComponent<TooltipFollowMouse>();
            if (tooltipFollowMouse != null)
            {
                tooltipFollowMouse.enabled = true;
            }
            FadeCanvasGroup(tooltipObject, 0);
            float timer = 0f;
            float totalTime = 0.1f;
            tooltipObject.SetActive(true);

            while (timer < totalTime)
            {
                timer += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(timer / totalTime);
                FadeCanvasGroup(tooltipObject, normalizedTime);
                yield return null;
            }
        }
    }

    private void FadeCanvasGroup(GameObject targetObject, float alpha)
    {
        CanvasGroup canvasGroup = targetObject.transform.parent.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    public void HideTooltip(string objectName)
    {
        GameObject tooltipObject = FindTooltipObject(objectName);
        if (tooltipObject != null)
        {
            if (tooltipFollowMouse != null)
            {
                tooltipFollowMouse.enabled = true;
            }
            tooltipObject.SetActive(false);
        }
    }
    public void HideAllTooltips()
    {
        foreach (GameObject tooltipObject in tooltipObjects.tooltipObjects)
        {
            tooltipObject.SetActive(false);
        }
        foreach (GameObject tooltipObject in smallTooltipObjects.tooltipObjectsSmall)
        {
            tooltipObject.SetActive(false);
        }
        foreach (GameObject tooltipObject in itemTooltipObjects.tooltipObjectItems)
        {
            tooltipObject.SetActive(false);
        }
    }
}
