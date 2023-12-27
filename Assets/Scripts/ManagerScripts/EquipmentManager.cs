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

    public GameObject HelmetSlot;
    public GameObject SuitSlot;
    public GameObject LeftHandSlot;
    public GameObject BackpackSlot;
    public GameObject RightHandSlot;
    public GameObject EnergySlot;
    public GameObject OxygenSlot;
    public GameObject WaterSlot;
    public GameObject HungerSlot;

    public void InitStartEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            slotEquipped[i] = false;
        }
        RefreshStats();
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

        string[] keys = {
            "FibrousLeaves", "Water", "Biofuel", "DistilledWater", "Battery",
            "OxygenTank", "BatteryCore", "Steam", "IronOre", "Wood",
            "Coal", "IronBeam", "BiofuelGenerator", "IronSheet", "IronRod"
        };

        for (int i = 0; i < keys.Length; i++)
        {
            coroutineManager.UpdateProductionTextsForKey(keys[i], recipeList[i].productionTime / Player.ProductionSpeed);
        }
        for (int i = 0; i < recipeList.Count; i++)
        {
            foreach (var childData in recipeList[i].childDataList)
            {
                float updatedQuantity = childData.quantity / Player.MaterialCost;
                coroutineManager.UpdateMaterialCostTextsForKey(childData.name, updatedQuantity);
            }
        }
        for (int i = 0; i < keys.Length; i++)
        {
            coroutineManager.UpdateProductionOutcomeTextsForKey(keys[i], recipeList[i].outputValue * Player.OutcomeRate);
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
        statsManager.RefreshStats();
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
        statsManager.RefreshStats();
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
        statsManager.RefreshStats();
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
