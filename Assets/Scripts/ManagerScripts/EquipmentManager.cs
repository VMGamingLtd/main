using ItemManagement;
using RecipeManagement;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    public static bool[] slotEquipped = new bool[9];
    public static string[] slotEquippedName = new string[9];
    public static bool autoConsumption;
    public GlobalCalculator globalCalculator;
    public InventoryManager inventoryManager;
    public StatsManager statsManager;
    public CoroutineManager coroutineManager;

    public GameObject HelmetSlot;
    public GameObject SuitSlot;
    public GameObject LeftHandSlot;
    public GameObject BackpackSlot;
    public GameObject RightHandSlot;
    public GameObject EnergySlot;
    public GameObject OxygenSlot;
    public GameObject WaterSlot;
    public GameObject HungerSlot;
    public GameObject DisabledProduction;

    public void InitStartEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            slotEquipped[i] = false;
        }
        RefreshStats();
    }
    public void EnableProduction()
    {
        Player.CanProduce = true;
        DisabledProduction.SetActive(false);
    }
    public void DisableProduction()
    {
        Player.CanProduce = false;
        DisabledProduction.SetActive(true);
    }

    public void RefreshRecipeStats()
    {
        CoroutineManager coroutineManager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        RecipeCreator recipeCreator = GameObject.Find("RecipeCreatorList").GetComponent<RecipeCreator>();
        var recipeList = recipeCreator.recipeDataList;

        // the dictionary maps have to be refreshed each time you work with them as they are not using static arrays
        coroutineManager.InitializeProductionTimeMap();
        coroutineManager.InitializeMaterialCostMap();
        coroutineManager.InitializeProductionOutcomeMap();

        for (int i = 0; i < CoroutineManager.RecipeKeys.Length; i++)
        {
            float newProductionTime = recipeList[i].productionTime / Player.ProductionSpeed;
            if (Player.ProductionSpeed <= 0) newProductionTime = 0;
            coroutineManager.UpdateProductionTextsForKey(CoroutineManager.RecipeKeys[i], newProductionTime);
        }
        for (int i = 0; i < recipeList.Count; i++)
        {
            foreach (var childData in recipeList[i].childDataList)
            {
                float updatedQuantity = childData.quantity / Player.MaterialCost;
                if (Player.MaterialCost <= 0) updatedQuantity = 0;
                coroutineManager.UpdateMaterialCostTextsForKey(childData.name);
            }
        }
        for (int i = 0; i < CoroutineManager.RecipeKeys.Length; i++)
        {
            float newOutcomeRate = recipeList[i].outputValue * Player.OutcomeRate;
            if (Player.OutcomeRate <= 0) newOutcomeRate = 0;
            coroutineManager.UpdateProductionOutcomeTextsForKey(CoroutineManager.RecipeKeys[i], newOutcomeRate);
        }
    }

    public void RefreshStats()
    {
        statsManager.RefreshStats();
    }
    public void EquipHelmet(HelmetData itemData)
    {
        if (itemData != null)
        {
            Player.PhysicalProtection += itemData.physicalProtection;
            Player.FireProtection += itemData.fireProtection;
            Player.ColdProtection += itemData.coldProtection;
            Player.GasProtection += itemData.gasProtection;
            Player.ExplosionProtection += itemData.explosionProtection;
            Player.ShieldPoints += itemData.shieldPoints;
            Player.HitPoints += itemData.hitPoints;
            Player.Strength += itemData.strength;
            Player.Perception += itemData.perception;
            Player.Intelligence += itemData.intelligence;
            Player.Agility += itemData.agility;
            Player.Charisma += itemData.charisma;
            Player.Willpower += itemData.willpower;
            Player.ExplorationRadius += itemData.explorationRadius;
            Player.VisibilityRadius += itemData.visibilityRadius;
            Player.PickupRadius += itemData.pickupRadius;
        }
        RefreshStats();
    }

    public void EquipSuit(SuitData itemData)
    {
        if (itemData != null)
        {
            Player.PhysicalProtection += itemData.physicalProtection;
            Player.FireProtection += itemData.fireProtection;
            Player.ColdProtection += itemData.coldProtection;
            Player.GasProtection += itemData.gasProtection;
            Player.ExplosionProtection += itemData.explosionProtection;
            Player.ShieldPoints += itemData.shieldPoints;
            Player.HitPoints += itemData.hitPoints;
            Player.EnergyCapacity += itemData.energyCapacity;
            Player.InventorySlots += itemData.inventorySlots;
            Player.Strength += itemData.strength;
            Player.Perception += itemData.perception;
            Player.Intelligence += itemData.intelligence;
            Player.Agility += itemData.agility;
            Player.Charisma += itemData.charisma;
            Player.Willpower += itemData.willpower;
        }
        RefreshStats();
    }
    public void EquipTool(ToolData itemData)
    {
        if (itemData != null)
        {
            Player.Strength += itemData.strength;
            Player.Perception += itemData.perception;
            Player.Intelligence += itemData.intelligence;
            Player.Agility += itemData.agility;
            Player.Charisma += itemData.charisma;
            Player.Willpower += itemData.willpower;
            Player.ProductionSpeed += itemData.productionSpeed;
            Player.MaterialCost += itemData.materialCost;
            Player.OutcomeRate += itemData.outcomeRate;
        }
        if (itemData.itemType == "FABRICATOR")
        {
            EnableProduction();
        }
        RefreshStats();
        RefreshRecipeStats();
    }

    public void UnequipHelmet(HelmetData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.PhysicalProtection -= itemData.physicalProtection;
            Player.FireProtection -= itemData.fireProtection;
            Player.ColdProtection -= itemData.coldProtection;
            Player.GasProtection -= itemData.gasProtection;
            Player.ExplosionProtection -= itemData.explosionProtection;
            Player.ShieldPoints -= itemData.shieldPoints;
            Player.HitPoints -= itemData.hitPoints;
            Player.Strength -= itemData.strength;
            Player.Perception -= itemData.perception;
            Player.Intelligence -= itemData.intelligence;
            Player.Agility -= itemData.agility;
            Player.Charisma -= itemData.charisma;
            Player.Willpower -= itemData.willpower;
            Player.ExplorationRadius -= itemData.explorationRadius;
            Player.VisibilityRadius -= itemData.visibilityRadius;
            Player.PickupRadius -= itemData.pickupRadius;
        }
        RefreshStats();
    }

    public void UnequipSuit(SuitData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.PhysicalProtection -= itemData.physicalProtection;
            Player.FireProtection -= itemData.fireProtection;
            Player.ColdProtection -= itemData.coldProtection;
            Player.GasProtection -= itemData.gasProtection;
            Player.ExplosionProtection -= itemData.explosionProtection;
            Player.ShieldPoints -= itemData.shieldPoints;
            Player.HitPoints -= itemData.hitPoints;
            Player.EnergyCapacity -= itemData.energyCapacity;
            Player.InventorySlots -= itemData.inventorySlots;
            Player.Strength -= itemData.strength;
            Player.Perception -= itemData.perception;
            Player.Intelligence -= itemData.intelligence;
            Player.Agility -= itemData.agility;
            Player.Charisma -= itemData.charisma;
            Player.Willpower -= itemData.willpower;
        }
        RefreshStats();
    }

    public void UnequipTool(ToolData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.Strength -= itemData.strength;
            Player.Perception -= itemData.perception;
            Player.Intelligence -= itemData.intelligence;
            Player.Agility -= itemData.agility;
            Player.Charisma -= itemData.charisma;
            Player.Willpower -= itemData.willpower;
            Player.ProductionSpeed -= itemData.productionSpeed;
            Player.MaterialCost -= itemData.materialCost;
            Player.OutcomeRate -= itemData.outcomeRate;
        }
        if (itemData.itemType == "FABRICATOR")
        {
            DisableProduction();
        }
        RefreshStats();
        RefreshRecipeStats();
    }

    public void DeductFromEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            if (slotEquipped[i])
            {
                if (i == 5)
                {
                    Transform targetChild = EnergySlot.transform.Cast<Transform>().FirstOrDefault(child => child.name.Contains("Battery"));
                    if (targetChild != null && targetChild.name == "Battery")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.quantity > PlayerResources.PlayerEnergy)
                        {
                            itemData.quantity -= PlayerResources.PlayerEnergy;
                            TextMeshProUGUI newCountTextEnergy = itemData.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                            if (newCountTextEnergy != null)
                            {
                                newCountTextEnergy.text = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                                coroutineManager.UpdateQuantityTexts("Battery", "ASSEMBLED");
                            }
                        }
                        else
                        {
                            if (!autoConsumption)
                            {
                                slotEquipped[5] = false;
                                slotEquippedName[5] = "";
                                PlayerResources.PlayerEnergy = 0f;
                                GameObject noEnergyObjects = GameObject.Find("NoEnergyObjects");
                                if (noEnergyObjects != null)
                                {
                                    ActivateObjects activateScript = noEnergyObjects.GetComponent<ActivateObjects>();
                                    activateScript?.ActivateAllObjects();
                                }
                                targetChild.parent.transform.Find("EmptyButton").GetComponent<Image>().gameObject.SetActive(true);
                                inventoryManager.DestroySpecificItem(targetChild.name, itemData.itemProduct, itemData.ID);
                                Destroy(targetChild.gameObject);
                                globalCalculator.UpdatePlayerConsumption();
                            }
                        }
                    }
                }
                if (i == 6 && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    Transform targetChild = OxygenSlot.transform.Cast<Transform>().FirstOrDefault(child => child.name.Contains("OxygenTank"));
                    if (targetChild != null && targetChild.name == "OxygenTank")
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null)
                        {

                        }

                    }
                }
                if (i == 7)
                {
                    Transform targetChild = WaterSlot.transform.Cast<Transform>().FirstOrDefault(child => child.name.Contains("DistilledWater"));
                    if (targetChild != null)
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.quantity > PlayerResources.PlayerWater)
                        {
                            itemData.quantity -= PlayerResources.PlayerWater;
                            TextMeshProUGUI newCountTextWater = itemData.transform.Find("CountInventory")?.GetComponent<TextMeshProUGUI>();
                            if (newCountTextWater != null)
                            {
                                newCountTextWater.text = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                                coroutineManager.UpdateQuantityTexts("DistilledWater", "PROCESSED");
                            }
                        }
                        else
                        {
                            if (!autoConsumption)
                            {
                                slotEquipped[7] = false;
                                slotEquippedName[7] = "";
                                PlayerResources.PlayerWater = 0f;
                                targetChild.parent.transform.Find("EmptyButton").GetComponent<Image>().gameObject.SetActive(true);
                                inventoryManager.DestroySpecificItem(targetChild.name, itemData.itemProduct, itemData.ID);
                                Destroy(targetChild.gameObject);
                                globalCalculator.UpdatePlayerConsumption();
                            }
                        }
                    }
                }
            }
        }
    }
}
