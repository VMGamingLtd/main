using UnityEngine;
using UnityEngine.EventSystems;
using ItemManagement;
using TMPro;

public class ItemUse : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject itemObject;
    public Animation addAnimation;
    private bool isPointerDown = false;
    public InventoryManager inventoryManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            itemObject = eventData.pointerEnter.gameObject.transform.parent.gameObject;
            isPointerDown = true;
            StartCoroutine(SingleClickRoutine(itemObject));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isPointerDown = false;
        }
    }

    private System.Collections.IEnumerator SingleClickRoutine(GameObject itemObject)
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

                    if (itemName == "PurifiedWater(Clone)")
                    {
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
                        inventoryManager.ReduceItemQuantity("PurifiedWater", "PROCESSED", 1);
                    }
                }
            }
        }
    }

}
