using ItemManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUse : MonoBehaviour, IPointerClickHandler
{
    private GameObject itemObject;
    private GameObject selectedObj;
    private MessageObjects messageObjects;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Splitter.isSplitting && eventData.pointerEnter.transform.parent != null)
        {
            string objName = transform.name.Replace("(Clone)", "");
            itemObject = eventData.pointerEnter.transform.parent.gameObject;
            ItemData itemData = itemObject.GetComponent<ItemData>();
            messageObjects = GameObject.Find("MessageCanvas/MESSAGEOBJECTS").GetComponent<MessageObjects>();
            if (itemData.quantity < 2)
            {
                if (itemData.itemType == "SUIT" || itemData.itemType == "HELMET" || itemData.itemType == "FABRICATOR")
                {
                    messageObjects.DisplayMessage("SplitItemFail");
                }
                else
                {
                    messageObjects.DisplaySplitWindow(itemData, objName);
                }
            }
            else
            {
                messageObjects.DisplaySplitWindow(itemData, objName);
            }
            Splitter.isSplitting = false;
            Splitter.isAwaitingInput = false;
            //StartCoroutine(SingleClickRoutine(itemObject));
        }
    }

    /*private System.Collections.IEnumerator SingleClickRoutine(GameObject itemObject)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (isPointerDown == false)
        {
            if (itemObject != null)
            {
                // Check if the mouse is over a game object with an ItemData component
                ItemData itemData = itemObject.GetComponent<ItemData>();
                if (itemData != null)
                {
                    string itemName = itemObject.name;

                    if (itemName == "DistilledWater(Clone)")
                    {
                        if (GoalManager.secondGoal == false)
                        {
                            unlockManager.StartFeature();
                        }
                        ItemTooltip tooltip = itemObject.GetComponent<ItemTooltip>();
                        tooltip.OnPointerExit(null);
                        addAnimation.Play("AddPlayerNeed");
                        string waterValue = itemData.WaterTimer;
                        string[] timerParts = waterValue.Split(':');
                        int days = int.Parse(timerParts[0]);
                        int hours = int.Parse(timerParts[1]);
                        int minutes = int.Parse(timerParts[2]);
                        int seconds = int.Parse(timerParts[3]);
                        PlayerResources.AddCurrentResourceTime(ref PlayerResources.PlayerWater, days, hours, minutes, seconds);
                        inventoryManager.ReduceItemQuantity("DistilledWater", "PROCESSED", 1);
                    }
                }
            }
        }
    }*/

}
