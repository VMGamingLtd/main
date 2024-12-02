using BuildingManagement;
using ItemManagement;
using RecipeManagement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipObjects tooltipObjects;
    private TooltipObjectsSmall smallTooltipObjects;
    private TooltipObjectsItems itemTooltipObjects;
    private TooltipFollowMouse tooltipFollowMouse;
    public bool exitedTooltip;

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
            if (eventData.pointerEnter.transform.parent.TryGetComponent<SuitData>(out var suitData))
            {
                StartCoroutine(DisplayTooltip("SuitTooltip", suitData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<HelmetData>(out var helmetData))
            {
                StartCoroutine(DisplayTooltip("HelmetTooltip", null, helmetData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<ToolData>(out var toolData))
            {
                StartCoroutine(DisplayTooltip("ToolTooltip", null, null, toolData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<BuildingItemData>(out var buildingItemData))
            {
                StartCoroutine(DisplayTooltip("BuildingTooltip", null, null, null, null, buildingItemData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<RecipeItemData>(out var recipeItemData))
            {
                StartCoroutine(DisplayTooltip("ItemRecipeTooltip", null, null, null, null, null, recipeItemData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<EventIconData>(out var eventIconData))
            {
                StartCoroutine(DisplayTooltip("EventIconTooltip", null, null, null, null, null, null, eventIconData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<MeleeWeaponData>(out var meleeWeaponData))
            {
                StartCoroutine(DisplayTooltip("MeleeWeaponTooltip", null, null, null, null, null, null, null, meleeWeaponData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<RangedWeaponData>(out var rangedWeaponData))
            {
                StartCoroutine(DisplayTooltip("RangedWeaponTooltip", null, null, null, null, null, null, null, null, rangedWeaponData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<ShieldData>(out var shieldData))
            {
                StartCoroutine(DisplayTooltip("ShieldTooltip", null, null, null, null, null, null, null, null, null, shieldData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<OffHandData>(out var offHandData))
            {
                StartCoroutine(DisplayTooltip("OffHandTooltip", null, null, null, null, null, null, null, null, null, null, offHandData));
            }
            else if (eventData.pointerEnter.transform.parent.TryGetComponent<ItemData>(out var itemData))
            {
                StartCoroutine(DisplayTooltip("ItemTooltip", null, null, null, itemData));
            }
            else
            {
                StartCoroutine(DisplayTooltip(eventData.pointerEnter.transform.name));

                var parent = eventData.pointerEnter.transform.parent;
                var grandParent = parent.transform.parent;

                if (grandParent.TryGetComponent(out Animation animation))
                {
                    animation.Stop();
                }
            }

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var parent = eventData.pointerEnter.transform.parent;
        var grandParent = parent.transform.parent;

        if (grandParent.TryGetComponent(out Animation animation))
        {
            animation.Play();
        }

        HideAllTooltips();

        if (tooltipFollowMouse != null)
        {
            tooltipFollowMouse.ResetTooltipPosition();
            tooltipFollowMouse.enabled = false;
        }

        exitedTooltip = true;
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

    public IEnumerator DisplayTooltip(string objectName, SuitData suitData = null, HelmetData helmetData = null,
        ToolData toolData = null, ItemData itemData = null, BuildingItemData buildingItemData = null, RecipeItemData recipeItemData = null,
        EventIconData eventIconData = null, MeleeWeaponData meleeWeaponData = null, RangedWeaponData rangedWeaponData = null,
        ShieldData shieldData = null, OffHandData offHandData = null)
    {
        HideAllTooltips();

        GameObject tooltipObject;

        if (Player.InCombat)
        {
            tooltipObject = FindTooltipObject(Constants.StatusEffect);
        }
        else
        {
            tooltipObject = FindTooltipObject(objectName);
        }

        if (tooltipObject != null)
        {          
            if (tooltipObject.transform.parent.TryGetComponent<TooltipFollowMouse>(out tooltipFollowMouse))
            {
                tooltipFollowMouse.enabled = true;
            }

            exitedTooltip = false;
            FadeCanvasGroup(tooltipObject, 0);
            float timer = 0f;
            float totalTime = 0.1f;
            float delay = 0.5f;

            tooltipObject.SetActive(true);

            if (Player.InCombat)
            {
                TranslationManager translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
                Debug.Log(objectName);
                tooltipObject.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(objectName);
            }
            else if (suitData != null)
            {
                SuitDataInjector suitDataInjector = tooltipObject.GetComponent<SuitDataInjector>();
                suitDataInjector.InjectData(suitData);
            }
            else if (helmetData != null)
            {
                HelmetDataInjector helmetDataInjector = tooltipObject.GetComponent<HelmetDataInjector>();
                helmetDataInjector.InjectData(helmetData);
            }
            else if (toolData != null)
            {
                ToolDataInjector toolDataInjector = tooltipObject.GetComponent<ToolDataInjector>();
                toolDataInjector.InjectData(toolData);
            }
            else if (itemData != null)
            {
                ItemDataInjector itemDataInjector = tooltipObject.GetComponent<ItemDataInjector>();
                itemDataInjector.InjectData(itemData);
            }
            else if (buildingItemData != null)
            {
                BuildingDataInjector buildingDataInjector = tooltipObject.GetComponent<BuildingDataInjector>();
                buildingDataInjector.InjectData(buildingItemData);
            }
            else if (recipeItemData != null)
            {
                RecipeDataInjector recipeDataInjector = tooltipObject.GetComponent<RecipeDataInjector>();
                recipeDataInjector.InjectData(recipeItemData);
            }
            else if (eventIconData != null)
            {
                EventIconDataInjector eventIconDataInjector = tooltipObject.GetComponent<EventIconDataInjector>();
                eventIconDataInjector.InjectData(eventIconData);
            }
            else if (meleeWeaponData != null)
            {
                MeleeWeaponDataInjector meleeWeaponDataInjector = tooltipObject.GetComponent<MeleeWeaponDataInjector>();
                meleeWeaponDataInjector.InjectData(meleeWeaponData);
            }
            else if (rangedWeaponData != null)
            {
                RangedWeaponDataInjector rangedWeaponDataInjector = tooltipObject.GetComponent<RangedWeaponDataInjector>();
                rangedWeaponDataInjector.InjectData(rangedWeaponData);
            }
            else if (shieldData != null)
            {
                ShieldDataInjector shieldDataInjector = tooltipObject.GetComponent<ShieldDataInjector>();
                shieldDataInjector.InjectData(shieldData);
            }
            else if (offHandData != null)
            {
                OffHandDataInjector offHandDataInjector = tooltipObject.GetComponent<OffHandDataInjector>();
                offHandDataInjector.InjectData(offHandData);
            }

            yield return new WaitForSeconds(delay);
            if (!exitedTooltip)
            {
                while (timer < totalTime)
                {
                    timer += Time.deltaTime;
                    float normalizedTime = Mathf.Clamp01(timer / totalTime);
                    FadeCanvasGroup(tooltipObject, normalizedTime);
                    yield return null;
                }
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
                tooltipFollowMouse.enabled = false;
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
