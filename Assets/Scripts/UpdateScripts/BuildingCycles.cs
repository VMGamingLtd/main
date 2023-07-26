using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using BuildingManagement;

public class BuildingCycles : MonoBehaviour
{
    private Image fillImg;
    private GameObject obj;
    private InventoryManager inventoryManagerRef;
    private CoroutineManager coroutineManagerRef;
    private BuildingItemData itemData;
    private new Animation animation;
    public float currentFillAmount;

    private async UniTaskVoid Start()
    {
        obj = transform.gameObject;
        await UniTask.DelayFrame(10);
        if (obj.layer == LayerMask.NameToLayer("Energy"))
        {
            await StartBuildingCycleEnergy();
        }
        else if (obj.layer == LayerMask.NameToLayer("NoConsume"))
        {
            await StartNoConsumeCycle();
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
        if (obj.layer == LayerMask.NameToLayer("Energy"))
        {
            await StartBuildingCycleEnergy();
        }
        else if (obj.layer == LayerMask.NameToLayer("NoConsume"))
        {
            await StartNoConsumeCycle();
        }
    }

    public async UniTask StartBuildingCycleEnergy()
    {
        obj = transform.gameObject;
        itemData = obj.GetComponent<BuildingItemData>();
        BuildingManager buildingManager = GetComponentInParent<BuildingManager>();
        inventoryManagerRef = buildingManager.inventoryManagerRef;

        Transform fillImgObj = obj.transform.Find("FillImg");
        fillImg = fillImgObj.GetComponent<Image>();

        int consumedSlots = itemData.consumedSlotCount;
        int[] getResources = new int[consumedSlots];

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
                var consumedItem = itemData.consumedItems[i];
                inventoryManagerRef.ReduceItemQuantity(consumedItem.itemName, consumedItem.itemQuality, consumedItem.quantity);

                UpdateUITextForConsumable(consumedItem.itemName);
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
            Color pauseImg = obj.transform.Find("PauseImage").GetComponent<Image>().color -= new Color(0f, 0f, 0f, 0.1f);
        }
        else
        {
            currentFillAmount = 0f;
            for (int i = 0; i < itemData.consumedSlotCount; i++)
            {
                var consumedItem = itemData.consumedItems[i];
                inventoryManagerRef.ReduceItemQuantity(consumedItem.itemName, consumedItem.itemQuality, consumedItem.quantity);

                UpdateUITextForConsumable(consumedItem.itemName);
            }
        }

        while (itemData.timer < itemData.totalTime && itemData.efficiencySetting > 0)
        {
            // If the building process is paused, end the cycle
            if (itemData.isPaused)
            {
                itemData.efficiency = 0;
                Color pauseImg = obj.transform.Find("PauseImage").GetComponent<Image>().color += new Color(0f, 0f, 0f, 0.1f);
                return;
            }

            currentFillAmount = Mathf.Lerp(0f, 1f, itemData.timer / itemData.totalTime);
            fillImg.fillAmount = currentFillAmount;
            itemData.timer += Time.deltaTime;
            itemData.efficiency = itemData.efficiencySetting;

            await UniTask.DelayFrame(1);
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
        obj = transform.gameObject;
        itemData = obj.GetComponent<BuildingItemData>();
        BuildingManager buildingManager = GetComponentInParent<BuildingManager>();
        inventoryManagerRef = buildingManager.inventoryManagerRef;

        Transform fillImgObj = obj.transform.Find("FillImg");
        fillImg = fillImgObj.GetComponent<Image>();

        // this step is important during unpause, as it helps the process to continue from where it ended without restart
        if (itemData.isPaused == true)
        {
            itemData.efficiency = itemData.efficiencySetting;
            itemData.isPaused = false;
            Color pauseImg = obj.transform.Find("PauseImage").GetComponent<Image>().color -= new Color(0f, 0f, 0f, 0.1f);
        }
        else
        {
            currentFillAmount = 0f;
            inventoryManagerRef.ReduceItemQuantity(itemData.consumedItems[0].itemName, itemData.consumedItems[0].itemQuality, itemData.consumedItems[0].quantity);
            coroutineManagerRef = buildingManager.coroutineManagerRef;
            string consumable = itemData.consumedItems[0].itemName;
            if (consumable == "Biofuel")
            {
                string currentBiofuelResource = inventoryManagerRef.GetItemQuantity("Biofuel", "PROCESSED").ToString();
                for (int i = 0; i < coroutineManagerRef.biofuelTexts.Length; i++)
                {
                    coroutineManagerRef.biofuelTexts[i].text = currentBiofuelResource;
                }
            }
        }

        while (itemData.timer < itemData.totalTime && itemData.efficiencySetting > 0)
        {
            // If the building process is paused this whole cycle ends here, keeping all values as they are
            if (itemData.isPaused == true)
            {
                itemData.efficiency = 0;
                Color pauseImg = obj.transform.Find("PauseImage").GetComponent<Image>().color += new Color(0f, 0f, 0f, 0.1f);
                return;
            }
            currentFillAmount = Mathf.Lerp(0f, 1f, itemData.timer / itemData.totalTime);
            fillImg.fillAmount = currentFillAmount;
            itemData.timer += Time.deltaTime;
            itemData.efficiency = itemData.efficiencySetting;

            await UniTask.DelayFrame(1);
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
            await StartNoConsumeCycle();
        }
    }

    private void UpdateUITextForConsumable(string consumableName)
    {
        BuildingManager buildingManager = GetComponentInParent<BuildingManager>();
        coroutineManagerRef = buildingManager.coroutineManagerRef;

        if (consumableName == "Water")
        {
            string currentResource = inventoryManagerRef.GetItemQuantity("Water", "BASIC").ToString();
            for (int i = 0; i < coroutineManagerRef.waterTexts.Length; i++)
            {
                coroutineManagerRef.waterTexts[i].text = currentResource;
            }
        }
        else if (consumableName == "Biofuel")
        {
            string currentResource = inventoryManagerRef.GetItemQuantity("Biofuel", "PROCESSED").ToString();
            for (int i = 0; i < coroutineManagerRef.biofuelTexts.Length; i++)
            {
                coroutineManagerRef.biofuelTexts[i].text = currentResource;
            }
        }
    }
}
