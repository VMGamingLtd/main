using BuildingManagement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingObjectUITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipObjectsItems itemTooltipObjects;
    private TooltipFollowMouse tooltipFollowMouse;
    private BuildingCreator buildingCreator;
    private TranslationManager translationManager;
    private Transform consumeStatList;
    private Transform outputStatList;
    private GameObject stat;
    public bool exitedTooltip;

    private void Awake()
    {
        itemTooltipObjects = GameObject.Find("TooltipCanvas/TooltipObjectsItems").GetComponent<TooltipObjectsItems>();
        translationManager = GameObject.Find("TranslationManager").GetComponent<TranslationManager>();
        buildingCreator = GameObject.Find("BuildingCreatorList").GetComponent<BuildingCreator>();
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
        GameObject tooltipObject = FindTooltipObject("BuildingTooltip");
        if (tooltipObject != null)
        {
            if (tooltipObject.transform.parent.TryGetComponent(out tooltipFollowMouse))
            {
                tooltipFollowMouse.enabled = true;
            }

            exitedTooltip = false;
            FadeCanvasGroup(tooltipObject, 0);
            float timer = 0f;
            float totalTime = 0.1f;
            float delay = 0.5f;
            tooltipObject.SetActive(true);

            if (int.TryParse(objectName, out int objectID))
            {
                var buildingData = buildingCreator.buildingDataList[objectID];

                if (buildingData != null)
                {
                    tooltipObject.transform.Find("Header/Title").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingData.buildingName);
                    tooltipObject.transform.Find("Header/Image/Icon").GetComponent<Image>().sprite = AssignSpriteToSlot(buildingData.buildingName);
                    tooltipObject.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingData.buildingType);
                    tooltipObject.transform.Find("Class").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingData.buildingClass);
                    tooltipObject.transform.Find("Desc").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(buildingData.buildingName + "Desc");
                    tooltipObject.transform.Find("ConsumptionTitle").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Consumption");
                    tooltipObject.transform.Find("OutputTitle").GetComponent<TextMeshProUGUI>().text = translationManager.Translate("Output");

                    stat = GameObject.Find("ItemCreatorList/BuildingTooltipStatTemplate");

                    consumeStatList = tooltipObject.transform.Find("ConsumeStats").GetComponent<Transform>();
                    outputStatList = tooltipObject.transform.Find("OutputStats").GetComponent<Transform>();

                    if (buildingData.powerConsumption > 0)
                    {
                        NumberFormater numberFormater = new();
                        CreateConsumeStat("Electricity", numberFormater.FormatEnergyInThousands(buildingData.powerConsumption), "Electricity");
                    }

                    foreach (SlotItemData consumeItem in buildingData.consumedItems)
                    {
                        CreateConsumeStat(consumeItem.itemName, consumeItem.quantity.ToString(), consumeItem.itemName);
                    }

                    if (buildingData.buildingType == "POWERPLANT")
                    {
                        NumberFormater numberFormater = new();
                        CreateOutputStat("Electricity", numberFormater.FormatEnergyInThousands(buildingData.basePowerOutput), "Electricity");
                    }
                    else if (buildingData.buildingType == "LABORATORY")
                    {
                        CreateOutputStat("ResearchPoint", buildingData.researchPoints.ToString(), "ResearchPoint");
                    }
                    else
                    {
                        foreach (SlotItemData outputItem in buildingData.producedItems)
                        {
                            CreateOutputStat(outputItem.itemName, outputItem.quantity.ToString(), outputItem.itemName);
                        }
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

    private void CreateConsumeStat(string Name, string Value, string SpriteName)
    {
        GameObject newStat = Instantiate(stat, consumeStatList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Value;
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignResourceSpriteToSlot(SpriteName);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    private void CreateOutputStat(string Name, string Value, string SpriteName)
    {
        GameObject newStat = Instantiate(stat, outputStatList);
        newStat.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = translationManager.Translate(Name);
        newStat.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = Value;
        newStat.transform.Find("Icon").GetComponent<Image>().sprite = AssignResourceSpriteToSlot(SpriteName);
        newStat.transform.localPosition = Vector3.one;
        newStat.transform.localScale = Vector3.one;
    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("buildingicons", spriteName);
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
        foreach (GameObject tooltipObject in itemTooltipObjects.tooltipObjectItems)
        {
            tooltipObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (consumeStatList != null)
            foreach (Transform child in consumeStatList)
            {
                Destroy(child.gameObject);
            }

        if (outputStatList != null)
            foreach (Transform child2 in outputStatList)
            {
                Destroy(child2.gameObject);
            }
    }
}
