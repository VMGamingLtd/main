using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BuildingManagement;
using TMPro;
using System.Globalization;
using ItemManagement;

public class BuildingCycles : MonoBehaviour
{
    private Image fillImg;
    private Color pauseImg;
    private Color electricImg;
    private GameObject obj;
    private InventoryManager inventoryManagerRef;
    private CoroutineManager coroutineManagerRef;
    private ItemCreator itemCreatorRef;
    private BuildingItemData itemData;
    private new Animation animation;
    public float currentFillAmount;

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
        Color pauseImg = obj.transform.Find("PauseImage").GetComponent<Image>().color;
        if (itemData.powerConsumption > 0)
        {
            Color electricImg = obj.transform.Find("NoElectricityImage").GetComponent<Image>().color;
        }

    }
    private async UniTask FinalizeBuildingData()
    {
        itemData.actualPowerOutput = itemData.powerOutput;
        currentFillAmount = Mathf.Lerp(0f, 1f, itemData.timer / itemData.totalTime);
        fillImg.fillAmount = currentFillAmount;
        itemData.timer += Time.deltaTime;
        itemData.efficiency = itemData.efficiencySetting;
        await UniTask.DelayFrame(2);
    }

    private void PauseMode()
    {
        itemData.efficiency = 0;
        itemData.actualPowerOutput = 0;
        pauseImg = ButtonColors.fadedCol;
        electricImg = ButtonColors.invisibleCol;
        return;
    }
    private async UniTask NoElectricityMode()
    {
        itemData.efficiency = 0;
        itemData.actualPowerOutput = 0;
        pauseImg = ButtonColors.invisibleCol;
        electricImg = ButtonColors.halfFadedCol;
        await NotEnoughMaterials();
        return;
    }
    private async UniTaskVoid Start()
    {
        InitializeBuildingData();
        await UniTask.DelayFrame(10);
        if (obj.CompareTag("Energy"))
        {
            await StartBuildingCycleEnergy();
        }
        else if (obj.CompareTag("NoConsume"))
        {
            await StartNoConsumeCycle();
        }
        else if (obj.CompareTag("Consume"))
        {
            await StartConsumeCycle();
        }
    }

    public async UniTask NotEnoughMaterials()
    {
        itemData = obj.GetComponent<BuildingItemData>();
        itemData.efficiency = 0;
        itemData.actualPowerOutput = 0;
        animation = GetComponent<Animation>();
        animation.Play("NotEnoughMaterialsBuilding");
        await UniTask.DelayFrame(60);
        animation.Stop();
        if (obj.CompareTag("Energy"))
        {
            await StartBuildingCycleEnergy();
        }
        else if (obj.CompareTag("NoConsume"))
        {
            await StartNoConsumeCycle();
        }
        else if (obj.CompareTag("Consume"))
        {
            await StartConsumeCycle();
        }
    }

    public async UniTask StartBuildingCycleEnergy()
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

        // Resume the process if it was paused
        if (itemData.isPaused)
        {
            itemData.efficiency = itemData.efficiencySetting;
            itemData.isPaused = false;
            pauseImg -= new Color(0f, 0f, 0f, 0.1f);
        }
        else
        {
            currentFillAmount = 0f;
        }

        while (itemData.timer < itemData.totalTime && itemData.efficiencySetting > 0)
        {
            // If the building process is paused, end the cycle
            if (itemData.isPaused)
            {
                itemData.efficiency = 0;
                itemData.actualPowerOutput = 0;
                pauseImg += new Color(0f, 0f, 0f, 0.1f);
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
            await StartBuildingCycleEnergy();
        }
    }
    public async UniTask StartNoConsumeCycle()
    {
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
            await StartNoConsumeCycle();
        }
    }
    public async UniTask StartConsumeCycle()
    {
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
            await StartConsumeCycle();
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
