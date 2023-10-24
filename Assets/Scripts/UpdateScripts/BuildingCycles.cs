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
    private InventoryManager inventoryManagerRef;
    private CoroutineManager coroutineManagerRef;
    private ItemCreator itemCreatorRef;
    private BuildingItemData itemData;
    private new Animation animation;
    public float currentFillAmount;
    private CancellationTokenSource cts = null;
    private CancellationToken cancellationToken;

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
        itemData.efficiency = itemData.efficiencySetting;
        await UniTask.DelayFrame(2);
    }

    private void PauseMode()
    {
        itemData.efficiency = 0;
        pauseImgColor = ButtonColors.fadedCol;
        pauseImage.color = pauseImgColor;
        electricImgColor = ButtonColors.invisibleCol;
        electricImage.color = electricImgColor;
        cts.Cancel();
        return;
    }
    private async UniTask NoElectricityMode()
    {
        itemData.efficiency = 0;
        pauseImgColor = ButtonColors.invisibleCol;
        pauseImage.color = pauseImgColor;
        electricImgColor = ButtonColors.halfFadedCol;
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
    }

    public async UniTask NotEnoughMaterials()
    {
        itemData = obj.GetComponent<BuildingItemData>();
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
    }

    public async UniTask StartNoConsumeCycle(CancellationToken cancellationToken)
    {
        // the whole UniTask will be killed here when the cts.Cancel() is called
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }
        // this step is important during unpause, as it helps the process to continue from where it ended without restart
        if (itemData.isPaused)
        {
            PauseMode();
        }

        if (Planet0Buildings.Planet0CurrentElectricity < itemData.powerConsumption && itemData.powerConsumption > 0)
        {
            await NoElectricityMode();
        }

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
                //inventoryManagerRef.AddItemQuantity(itemData.producedItems[i].itemName, itemData.producedItems[i].itemQuality, itemData.producedItems[i].quantity, itemData.producedItems[i].index);
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
                //inventoryManagerRef.AddItemQuantity(itemData.producedItems[i].itemName, itemData.producedItems[i].itemQuality, itemData.producedItems[i].quantity, itemData.producedItems[i].index);
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
