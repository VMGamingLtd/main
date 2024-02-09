using BuildingManagement;
using Cysharp.Threading.Tasks;
using ItemManagement;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCycles : MonoBehaviour
{
    private Image fillImg;
    private Color pauseImgColor;
    private Image pauseImage;
    private Color electricImgColor;
    private Image electricImage;
    private GameObject obj;
    private TimebarControl timebarControl;
    private InventoryManager inventoryManagerRef;
    private CoroutineManager coroutineManagerRef;
    private ItemCreator itemCreatorRef;
    private BuildingItemData itemData;
    private new Animation animation;
    public float currentFillAmount;
    private CancellationTokenSource cts = null;
    private CancellationToken cancellationToken;
    private ProductionCreator productionCreator;
    public bool enlistedProduction;

    private void InitializeBuildingData()
    {
        obj = transform.gameObject;
        itemData = obj.GetComponent<BuildingItemData>();
        BuildingManager buildingManager = GetComponentInParent<BuildingManager>();
        inventoryManagerRef = buildingManager.inventoryManagerRef;
        itemCreatorRef = inventoryManagerRef.itemCreator;
        Transform fillImgObj = obj.transform.Find("FillImg");
        fillImg = fillImgObj.GetComponent<Image>();
        coroutineManagerRef = buildingManager.coroutineManagerRef;

        if (productionCreator == null)
            productionCreator = GameObject.Find("PRODUCTIONCREATOR").GetComponent<ProductionCreator>();

        pauseImage = obj.transform.Find("PauseImage").GetComponent<Image>();
        pauseImgColor = pauseImage.color;
        if (itemData.powerConsumption > 0)
        {
            electricImage = transform.Find("NoElectricityImage").GetComponent<Image>();
            electricImgColor = electricImage.color;
        }
        // we have to generate cancellation token for async task in order to stop it later
        cts = new CancellationTokenSource();
        cancellationToken = cts.Token;
    }

    private async UniTask FinalizeBuildingData()
    {
        currentFillAmount = Mathf.Lerp(0f, 1f, itemData.timer / itemData.totalTime);
        fillImg.fillAmount = currentFillAmount;
        itemData.timer += Time.deltaTime;
        timebarControl.UpdateTimebar(currentFillAmount);
        itemData.efficiency = itemData.efficiencySetting;
        await UniTask.DelayFrame(1);
    }

    private void PauseMode()
    {
        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(null, itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorYellow);
        productionCreator.ChangeProductionItemBackground(UIColors.timebarColorYellow, null, itemData);
        itemData.efficiency = 0;
        pauseImgColor = UIColors.fadedCol;
        pauseImage.color = pauseImgColor;
        electricImgColor = UIColors.invisibleCol;
        electricImage.color = electricImgColor;
        cts.Cancel();
        return;
    }

    private async UniTask NoElectricityMode()
    {
        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(null, itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorYellow);
        productionCreator.ChangeProductionItemBackground(UIColors.timebarColorYellow, null, itemData);
        itemData.efficiency = 0;
        pauseImgColor = UIColors.invisibleCol;
        pauseImage.color = pauseImgColor;
        electricImgColor = UIColors.halfFadedCol;
        electricImage.color = electricImgColor;
        await NotEnoughMaterials();
        return;
    }

    public async UniTaskVoid Start()
    {
        InitializeBuildingData();
        await UniTask.DelayFrame(10);
        if (obj.CompareTag("NoConsume"))
        {
            await StartNoConsumeCycle(cancellationToken);
        }
        else if (obj.CompareTag("Consume"))
        {
            await StartConsumeCycle(cancellationToken);
        }
        else if (obj.CompareTag("Research"))
        {
            await StartResearchCycle(cancellationToken);
        }
    }

    public async UniTask NotEnoughMaterials()
    {
        itemData = obj.GetComponent<BuildingItemData>();
        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(null, itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorRed);
        productionCreator.ChangeProductionItemBackground(UIColors.timebarColorRed, null, itemData);
        itemData.efficiency = 0;
        animation = GetComponent<Animation>();
        animation.Play("NotEnoughMaterialsBuilding");
        await UniTask.DelayFrame(60);
        animation.Stop();
        if (obj.CompareTag("NoConsume"))
        {
            await StartNoConsumeCycle(cancellationToken);
        }
        else if (obj.CompareTag("Consume"))
        {
            await StartConsumeCycle(cancellationToken);
        }
        else if (obj.CompareTag("Research"))
        {
            await StartResearchCycle(cancellationToken);
        }
    }

    public async UniTask StartNoConsumeCycle(CancellationToken cancellationToken)
    {
        // the whole UniTask will be killed here when the cts.Cancel() is called
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (!itemData.enlistedProduction && !enlistedProduction)
        {
            GameObject obj = productionCreator.EnlistBuildingProduction(itemData);
            timebarControl = obj.GetComponent<TimebarControl>();
            itemData.enlistedProduction = true;
        }
        enlistedProduction = true;

        // this step is important during unpause, as it helps the process to continue from where it ended without restart
        if (itemData.isPaused)
        {
            PauseMode();
        }

        if (Planet0Buildings.Planet0CurrentElectricity < itemData.powerConsumption && itemData.powerConsumption > 0)
        {
            await NoElectricityMode();
        }

        pauseImgColor = UIColors.invisibleCol;
        electricImgColor = UIColors.invisibleCol;

        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(null, itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorGreen);
        productionCreator.ChangeProductionItemBackground(UIColors.blackHalfTransparent, null, itemData);

        while (itemData.timer < itemData.totalTime && itemData.efficiencySetting > 0)
        {
            // If the building process is paused this whole cycle ends here, keeping all values as they are
            if (itemData.isPaused)
            {
                PauseMode();
            }
            // if during the production the electricity goes off or decreases to demanded number, whole process will stop
            if (Planet0Buildings.Planet0CurrentElectricity < itemData.powerConsumption && itemData.powerConsumption > 0)
            {
                await NoElectricityMode();
            }
            await FinalizeBuildingData();
        }

        // at the end of production cycle, check what should be produced and in what quantity and add it to player's inventory
        if (!itemData.isPaused || itemData.efficiency > 0)
        {
            for (int i = 0; i < itemData.producedSlotCount; i++)
            {
                var item = itemData.producedItems[i];
                itemCreatorRef.CreateItem(itemData.producedItems[i].index, itemData.producedItems[i].quantity);
                UpdateUITextForConsumable(item.itemName, item.itemQuality);
            }

            fillImg.fillAmount = 0f;
            itemData.timer = 0f;
            currentFillAmount = 0f;
        }

        if (itemData.efficiency == 0)
        {
            await NotEnoughMaterials();
        }
        else
        {
            await StartNoConsumeCycle(cancellationToken);
        }
    }
    public async UniTask StartConsumeCycle(CancellationToken cancellationToken)
    {
        // the whole UniTask will be killed here when the cts.Cancel() is called
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (!itemData.enlistedProduction && !enlistedProduction)
        {
            GameObject obj = productionCreator.EnlistBuildingProduction(itemData);
            timebarControl = obj.GetComponent<TimebarControl>();
            itemData.enlistedProduction = true;
        }
        enlistedProduction = true;

        // this step is important during unpause, as it helps the process to continue from where it ended without restart
        if (itemData.isPaused)
        {
            PauseMode();
        }
        // checking if player has enough energy
        if (Planet0Buildings.Planet0CurrentElectricity < itemData.powerConsumption)
        {
            await NoElectricityMode();
        }
        // checking if player has enough of all consumed resources based on BuildingItemData
        int consumedSlots = itemData.consumedSlotCount;
        float[] getResources = new float[consumedSlots];

        for (int i = 0; i < consumedSlots; i++)
        {
            getResources[i] = inventoryManagerRef.GetItemQuantity(itemData.consumedItems[i].itemName, itemData.consumedItems[i].itemQuality);
        }

        bool hasEnoughResources = true;
        for (int i = 0; i < consumedSlots; i++)
        {
            if (getResources[i] < itemData.consumedItems[i].quantity)
            {
                hasEnoughResources = false;
                break;
            }
        }
        // if player has enough resources, then decrease the ones described in BuildingItemData of each building
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

        if (timebarControl == null)
        {
            timebarControl = productionCreator.LinkTimebarToBuildingData(null, itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorGreen);
        productionCreator.ChangeProductionItemBackground(UIColors.blackHalfTransparent, null, itemData);

        // if all above it approved, then starts the building cycle
        while (itemData.timer < itemData.totalTime && itemData.efficiencySetting > 0)
        {
            // If the building process is paused this whole cycle ends here, keeping all values as they are
            if (itemData.isPaused)
            {
                PauseMode();
            }
            // if during the production the electricity goes off or decreases to demanded number, whole process will stop
            if (Planet0Buildings.Planet0CurrentElectricity < itemData.powerConsumption)
            {
                await NoElectricityMode();
            }
            await FinalizeBuildingData();
        }

        // at the end of production cycle, check what should be produced and in what quantity and add it to player's inventory
        if (!itemData.isPaused || itemData.efficiency > 0)
        {
            for (int i = 0; i < itemData.producedSlotCount; i++)
            {
                var item = itemData.producedItems[i];
                itemCreatorRef.CreateItem(itemData.producedItems[i].index, itemData.producedItems[i].quantity);
                UpdateUITextForConsumable(item.itemName, item.itemQuality);
            }

            fillImg.fillAmount = 0f;
            itemData.timer = 0f;
            currentFillAmount = 0f;
        }


        if (itemData.efficiency == 0)
        {
            await NotEnoughMaterials();
        }
        else
        {
            await StartConsumeCycle(cancellationToken);
        }
    }

    public async UniTask StartResearchCycle(CancellationToken cancellationToken)
    {
        // the whole UniTask will be killed here when the cts.Cancel() is called
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (!itemData.enlistedProduction && !enlistedProduction)
        {
            GameObject obj = productionCreator.EnlistBuildingProduction(itemData);
            timebarControl = obj.GetComponent<TimebarControl>();
            itemData.enlistedProduction = true;
        }
        enlistedProduction = true;

        // this step is important during unpause, as it helps the process to continue from where it ended without restart
        if (itemData.isPaused)
        {
            PauseMode();
        }
        // checking if player has enough energy
        if (Planet0Buildings.Planet0CurrentElectricity < itemData.powerConsumption)
        {
            await NoElectricityMode();
        }

        // checking if player has enough of all consumed resources based on BuildingItemData
        if (itemData.consumedSlotCount > 0)
        {
            int consumedSlots = itemData.consumedSlotCount;
            float[] getResources = new float[consumedSlots];

            for (int i = 0; i < consumedSlots; i++)
            {
                getResources[i] = inventoryManagerRef.GetItemQuantity(itemData.consumedItems[i].itemName, itemData.consumedItems[i].itemQuality);
            }

            bool hasEnoughResources = true;
            for (int i = 0; i < consumedSlots; i++)
            {
                if (getResources[i] < itemData.consumedItems[i].quantity)
                {
                    hasEnoughResources = false;
                    break;
                }
            }
            // if player has enough resources, then decrease the ones described in BuildingItemData of each building
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
            timebarControl = productionCreator.LinkTimebarToBuildingData(null, itemData);
        }
        timebarControl.ChangeTimeBarColor(UIColors.timebarColorGreen);
        productionCreator.ChangeProductionItemBackground(UIColors.blackHalfTransparent, null, itemData);

        // if all above it approved, then starts the building cycle
        while (itemData.timer < itemData.totalTime && itemData.efficiencySetting > 0)
        {
            // If the building process is paused this whole cycle ends here, keeping all values as they are
            if (itemData.isPaused)
            {
                PauseMode();
            }
            // if during the production the electricity goes off or decreases to demanded number, whole process will stop
            if (Planet0Buildings.Planet0CurrentElectricity < itemData.powerConsumption)
            {
                await NoElectricityMode();
            }
            await FinalizeBuildingData();
        }

        // at the end of production cycle, check what should be produced and in what quantity and add it to player's inventory
        if (!itemData.isPaused || itemData.efficiency > 0)
        {
            if (itemData.buildingName == "ResearchDevice")
            {
                Player.ResearchPoints++;
            }
            coroutineManagerRef.researchPointsText.text = Player.ResearchPoints.ToString();
            fillImg.fillAmount = 0f;
            itemData.timer = 0f;
            currentFillAmount = 0f;
        }


        if (itemData.efficiency == 0)
        {
            await NotEnoughMaterials();
        }
        else
        {
            await StartResearchCycle(cancellationToken);
        }
    }

    private void UpdateUITextForConsumable(string consumableName, string quality)
    {
        if (coroutineManagerRef.resourceTextMap.TryGetValue(consumableName, out TextMeshProUGUI[] currentTextArray))
        {
            string currentResource = inventoryManagerRef.GetItemQuantity(consumableName, quality).ToString("F2", CultureInfo.InvariantCulture);
            for (int i = 0; i < currentTextArray.Length; i++)
            {
                if (currentResource.EndsWith(".00")) currentResource = currentResource[..^3];
                currentTextArray[i].text = currentResource;
            }
        }
        else
        {
            Debug.LogError($"Text array for '{consumableName}' not found.");
        }
    }

}
