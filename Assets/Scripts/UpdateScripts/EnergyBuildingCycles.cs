using BuildingManagement;
using Cysharp.Threading.Tasks;
using ItemManagement;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBuildingCycles : MonoBehaviour
{
    private Image fillImg;
    private Image pauseImage;
    private Color pauseImgColor;
    private Color electricImg;
    private TimebarControl timebarControl;
    private InventoryManager inventoryManagerRef;
    private CoroutineManager coroutineManagerRef;
    private ItemCreator itemCreatorRef;
    private EnergyBuildingItemData itemData;
    private new Animation animation;
    public float currentFillAmount;
    private CancellationTokenSource cts = null;
    private CancellationToken cancellationToken;
    private ProductionCreator productionCreator;
    public bool enlistedProduction;

    private void InitializeBuildingData()
    {
        itemData = GetComponent<EnergyBuildingItemData>();
        BuildingManager buildingManager = GetComponentInParent<BuildingManager>();
        inventoryManagerRef = buildingManager.inventoryManagerRef;
        itemCreatorRef = inventoryManagerRef.itemCreator;
        Transform fillImgObj = transform.Find("FillImg");
        fillImg = fillImgObj.GetComponent<Image>();
        coroutineManagerRef = buildingManager.coroutineManagerRef;

        if (productionCreator == null)
            productionCreator = GameObject.Find("PRODUCTIONCREATOR").GetComponent<ProductionCreator>();

        pauseImage = transform.Find("PauseImage").GetComponent<Image>();
        pauseImgColor = pauseImage.color;
        // we have to generate cancellation token for async task in order to stop it later
        cts = new CancellationTokenSource();
        cancellationToken = cts.Token;

    }
    private async UniTask FinalizeBuildingData()
    {
        itemData.actualPowerOutput = itemData.powerOutput;
        currentFillAmount = Mathf.Lerp(0f, 1f, itemData.timer / itemData.totalTime);
        fillImg.fillAmount = currentFillAmount;
        timebarControl.UpdateTimebar(currentFillAmount);
        itemData.timer += Time.deltaTime;
        itemData.efficiency = itemData.efficiencySetting;
        await UniTask.DelayFrame(1);
    }

    private void PauseMode()
    {
        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorYellow);
        productionCreator.ChangeProductionItemBackground(itemData, UIColors.timebarColorYellow);
        itemData.efficiency = 0;
        itemData.actualPowerOutput = 0;
        pauseImgColor = UIColors.fadedCol;
        pauseImage.color = pauseImgColor;
        cts.Cancel();
        return;
    }
    private async UniTask NoElectricityMode()
    {
        itemData.efficiency = 0;
        itemData.actualPowerOutput = 0;
        pauseImgColor = UIColors.invisibleCol;
        electricImg = UIColors.halfFadedCol;
        await NotEnoughMaterials();
        return;
    }
    public async UniTaskVoid Start()
    {
        InitializeBuildingData();
        await UniTask.DelayFrame(10);
        if (itemData.isPaused)
        {
            PauseMode();
        }
        else
        {
            pauseImgColor = UIColors.invisibleCol;
            pauseImage.color = pauseImgColor;
            await StartBuildingCycleEnergy(cancellationToken);
        }

    }

    public async UniTask NotEnoughMaterials()
    {
        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorRed);
        productionCreator.ChangeProductionItemBackground(itemData, UIColors.timebarColorRed);
        itemData = GetComponent<EnergyBuildingItemData>();
        itemData.efficiency = 0;
        itemData.actualPowerOutput = 0;
        animation = GetComponent<Animation>();
        animation.Play("NotEnoughMaterialsBuilding");
        await UniTask.DelayFrame(60);
        animation.Stop();
        await StartBuildingCycleEnergy(cancellationToken);
    }

    public async UniTask StartBuildingCycleEnergy(CancellationToken cancellationToken)
    {
        // the whole UniTask will be killed here when the cts.Cancel() is called
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (!itemData.enlistedProduction && !enlistedProduction)
        {
            GameObject obj = productionCreator.EnlistEnergyBuildingProduction(itemData);
            timebarControl = obj.GetComponent<TimebarControl>();
            itemData.enlistedProduction = true;
        }
        enlistedProduction = true;

        // if the building is paused, then do not continue in production cycle
        if (itemData.isPaused)
        {
            PauseMode();
        }
        else
        {
            pauseImgColor = UIColors.invisibleCol;
            pauseImage.color = pauseImgColor;
        }

        int consumedSlots = itemData.consumedSlotCount;
        float[] getResources = new float[consumedSlots];

        for (int i = 0; i < consumedSlots; i++)
        {
            getResources[i] = inventoryManagerRef.GetItemQuantity(itemData.consumedItems[i].itemName, itemData.consumedItems[i].itemQuality);
        }

        if (itemData.timer == 0)
        {
            bool hasEnoughResources = true;
            for (int i = 0; i < consumedSlots; i++)
            {
                if (getResources[i] < itemData.consumedItems[i].quantity)
                {
                    hasEnoughResources = false;
                    break;
                }
            }

            if (hasEnoughResources)
            {
                for (int i = 0; i < consumedSlots; i++)
                {
                    var item = itemData.consumedItems[i];
                    inventoryManagerRef.ReduceItemQuantity(itemData.consumedItems[i].itemName, itemData.consumedItems[i].itemQuality, itemData.consumedItems[i].quantity);
                    UpdateUITextForConsumable(item.itemName, item.itemQuality);
                }
            }
            else
            {
                await NotEnoughMaterials();
                return;
            }
        }

        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorGreen);
        productionCreator.ChangeProductionItemBackground(itemData, UIColors.blackHalfTransparent);

        while (itemData.timer < itemData.totalTime && itemData.efficiencySetting > 0)
        {
            // If the building process is paused, end the cycle
            if (itemData.isPaused)
            {
                itemData.efficiency = 0;
                itemData.actualPowerOutput = 0;
                pauseImgColor = UIColors.fadedCol;
                pauseImage.color = pauseImgColor;
                cts.Cancel();
                return;
            }
            await FinalizeBuildingData();
        }

        fillImg.fillAmount = 0f;
        itemData.timer = 0f;
        currentFillAmount = 0f;

        if (itemData.efficiency == 0)
        {
            await NotEnoughMaterials();
        }
        else
        {
            await StartBuildingCycleEnergy(cancellationToken);
        }
    }
    private void UpdateUITextForConsumable(string consumableName, string quality)
    {
        if (coroutineManagerRef.resourceTextMap.TryGetValue(consumableName, out TextMeshProUGUI[] currentTextArray))
        {
            string currentResource = inventoryManagerRef.GetItemQuantity(consumableName, quality).ToString("F2", CultureInfo.InvariantCulture);
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                currentTextArray[i].text = currentResource;
            }
        }
        else
        {
            Debug.LogError($"Text array for '{consumableName}' not found.");
        }
    }

}
