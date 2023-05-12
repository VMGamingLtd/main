using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    private int totalSeconds;
    public GameObject OxygenTank;
    public InventoryManager inventoryManager;

    public void CreateOxygenTankItem()
    {
        GameObject newItem = Instantiate(OxygenTank);
        newItem.transform.position = new Vector3(newItem.transform.position.x, newItem.transform.position.y, 0f);
        inventoryManager.AddToUtilityItems(newItem);
    }
}
