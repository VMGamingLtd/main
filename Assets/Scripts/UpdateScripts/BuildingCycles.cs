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

    private async UniTaskVoid Start()
    {
        await StartBuildingCycle();
    }

    private async UniTask StartBuildingCycle()
    {
        obj = transform.gameObject;
        BuildingItemData itemData = obj.GetComponent<BuildingItemData>();
        Transform fillImgObj = obj.transform.Find("FillImg");
        fillImg = fillImgObj.GetComponent<Image>();
        float timer = 0f;
        float currentFillAmount = 0f;
        float totalTime = itemData.consumedTime;

        while (timer < totalTime)
        {
            currentFillAmount = Mathf.Lerp(0f, 1f, timer / totalTime);
            fillImg.fillAmount = currentFillAmount;
            timer += Time.deltaTime;

            await UniTask.DelayFrame(1);
        }
        fillImg.fillAmount = 0f;

        BuildingManager buildingManager = GetComponentInParent<BuildingManager>();
        inventoryManagerRef = buildingManager.inventoryManagerRef;
        inventoryManagerRef.ReduceItemQuantity(itemData.consumedItem, itemData.consumedItemQuality, itemData.consumedQuantity);

        coroutineManagerRef = buildingManager.coroutineManagerRef;
        string consumable = itemData.consumedItem;
        if (consumable == "Biofuel")
        {
            string currentBiofuelResource = inventoryManagerRef.GetItemQuantity("Biofuel", "PROCESSED").ToString();
            for (int i = 0; i < coroutineManagerRef.biofuelTexts.Length; i++)
            {
                coroutineManagerRef.biofuelTexts[i].text = currentBiofuelResource;
            }
        }



    }
}
