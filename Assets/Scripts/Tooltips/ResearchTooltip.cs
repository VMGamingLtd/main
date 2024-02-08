using RecipeManagement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipObjectsItems itemTooltipObjects;
    private TooltipFollowMouse tooltipFollowMouse;
    private RecipeCreator recipeCreator;
    private TranslationManager translationManager;
    private Transform requirementsStatList;
    private Transform rewardsStatList;
    private GameObject stat;
    public bool exitedTooltip;

    private void Awake()
    {
        itemTooltipObjects = GameObject.Find("TooltipCanvas/TooltipObjectsItems").GetComponent<TooltipObjectsItems>();
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
        recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GlobalCalculator.GameStarted)
        {
            StartCoroutine(DisplayTooltip(eventData.pointerEnter.transform.name));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideAllTooltips();
        exitedTooltip = true;
    }
    private GameObject FindTooltipObject(string objectName)
    {
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
        GameObject tooltipObject = FindTooltipObject("ResearchTooltip");
        if (tooltipObject != null)
        {
            if (tooltipObject.transform.parent.TryGetComponent(out tooltipFollowMouse))
            {
                tooltipFollowMouse.enabled = true;
            }
            Debug.Log("Displaying tooltip");
            exitedTooltip = false;
            FadeCanvasGroup(tooltipObject, 0);
            float timer = 0f;
            float totalTime = 0.1f;
            float delay = 0.5f;
            tooltipObject.SetActive(true);

            if (int.TryParse(objectName, out int objectID))
            {
                var researchData = recipeCreator.researchDataList[objectID];
                Debug.Log(objectID);
                if (researchData != null)
                {
                    tooltipObject.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = researchData.projectName;
                    tooltipObject.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssignSkillSpriteToSlot(researchData.projectName);
                    tooltipObject.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(researchData.projectType);
                    tooltipObject.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(researchData.projectClass);
                    tooltipObject.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(researchData.projectName + "Desc");

                    stat = GameObject.Find("ItemCreatorList/BuildingTooltipStatTemplate");

                    requirementsStatList = tooltipObject.transform.Find("RequirementsList").GetComponent<Transform>();
                    rewardsStatList = tooltipObject.transform.Find("RewardsList").GetComponent<Transform>();

                    if (researchData.hasRequirements)
                    {
                        foreach (ChildData requirementItem in researchData.requirementsList)
                        {
                            CreateRequirementStat(requirementItem.name, requirementItem.quantity.ToString(), requirementItem.name);
                        }
                    }
                    else
                    {
                        tooltipObject.transform.Find("RequirementsTitle").gameObject.SetActive(false);
                    }

                    foreach (ChildData rewardItem in researchData.rewardsList)
                    {
                        CreateRewardStat(rewardItem.name, rewardItem.quantity.ToString(), rewardItem.name);
                    }
                }
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

    private void CreateRequirementStat(string Name, string Value, string SpriteName)
    {
        GameObject newStat = Instantiate(stat, requirementsStatList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Value;
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignBuildingSpriteToSlot(SpriteName);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    private void CreateRewardStat(string Name, string Value, string SpriteName)
    {
        GameObject newStat = Instantiate(stat, rewardsStatList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Value;
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignResourceSpriteToSlot(SpriteName);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    private Sprite AssignBuildingSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("buildingicons", spriteName);
        return sprite;
    }

    private Sprite AssignSkillSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("skillsicons", spriteName);
        return sprite;
    }

    private Sprite AssignResourceSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }

    private void FadeCanvasGroup(GameObject targetObject, float alpha)
    {
        CanvasGroup canvasGroup = targetObject.transform.parent.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
    }

    public void HideAllTooltips()
    {
        foreach (GameObject tooltipObject in itemTooltipObjects.tooltipObjectItems)
        {
            tooltipObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (requirementsStatList != null)
            foreach (Transform child in requirementsStatList)
            {
                Destroy(child.gameObject);
            }

        if (rewardsStatList != null)
            foreach (Transform child2 in rewardsStatList)
            {
                Destroy(child2.gameObject);
            }
    }
}
