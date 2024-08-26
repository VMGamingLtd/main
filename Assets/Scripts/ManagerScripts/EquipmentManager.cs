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
        CoroutineManager coroutineManager = GameObject.Find(Constants.CoroutineManager).GetComponent<CoroutineManager>();
        RecipeCreator recipeCreator = GameObject.Find(Constants.RecipeCreatorList).GetComponent<RecipeCreator>();
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
            itemData.isEquipped = true;
            Player.PhysicalProtection += itemData.physicalProtection;
            Player.FireProtection += itemData.fireProtection;
            Player.ColdProtection += itemData.coldProtection;
            Player.PoisonProtection += itemData.poisonProtection;
            Player.EnergyProtection += itemData.energyProtection;
            Player.PsiProtection += itemData.psiProtection;
            Player.ShieldPoints += itemData.shieldPoints;
            Player.Armor += itemData.armor;
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
            itemData.isEquipped = true;
            Player.PhysicalProtection += itemData.physicalProtection;
            Player.FireProtection += itemData.fireProtection;
            Player.ColdProtection += itemData.coldProtection;
            Player.PoisonProtection += itemData.poisonProtection;
            Player.EnergyProtection += itemData.energyProtection;
            Player.PsiProtection += itemData.psiProtection;
            Player.ShieldPoints += itemData.shieldPoints;
            Player.Armor += itemData.armor;
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
            itemData.isEquipped = true;
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
        if (itemData.itemType == Constants.Fabricator)
        {
            EnableProduction();
        }
        RefreshStats();
        RefreshRecipeStats();
    }

    public void ResetPlayerStats()
    {
        Player.AttackSpeed = 1;
        Player.HitChance = 0;
        Player.Dodge = 0;
        Player.Resistance = 0;
        Player.CounterChance = 0;
        Player.Penetration = 0;
        Player.PsiDamage = 0;
        Player.MeleePhysicalDamage = 0;
        Player.MeleeFireDamage = 0;
        Player.MeleeColdDamage = 0;
        Player.MeleePoisonDamage = 0;
        Player.MeleeEnergyDamage = 0;
        Player.RangedPhysicalDamage = 0;
        Player.RangedFireDamage = 0;
        Player.RangedColdDamage = 0;
        Player.RangedPoisonDamage = 0;
        Player.RangedEnergyDamage = 0;
        Player.PhysicalProtection = 0;
        Player.FireProtection = 0;
        Player.ColdProtection = 0;
        Player.PoisonProtection = 0;
        Player.EnergyProtection = 0;
        Player.PsiProtection = 0;
        Player.ShieldPoints = 0;
        Player.Armor = 0;
        Player.HitPoints = 10;
        Player.Strength = 0;
        Player.Perception = 0;
        Player.Intelligence = 0;
        Player.Agility = 0;
        Player.Charisma = 0;
        Player.Willpower = 0;
        Player.ExplorationRadius = 0;
        Player.VisibilityRadius = 0;
        Player.PickupRadius = 0;
        Player.EnergyCapacity = 0;
        Player.InventorySlots = 0;
        Player.ProductionSpeed = 0;
        Player.MaterialCost = 0;
        Player.OutcomeRate = 0;
    }

    public void EquipMeleeWeapon(MeleeWeaponData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = true;
            Player.AttackSpeed += itemData.attackSpeed;
            Player.HitChance += itemData.hitChance;
            Player.Dodge += itemData.dodge;
            Player.Resistance += itemData.resistance;
            Player.CounterChance += itemData.counterChance;
            Player.Penetration += itemData.penetration;
            Player.PsiDamage += itemData.psiDamage;
            Player.MeleePhysicalDamage += itemData.meleePhysicalDamage;
            Player.MeleeFireDamage += itemData.meleeFireDamage;
            Player.MeleeColdDamage += itemData.meleeColdDamage;
            Player.MeleePoisonDamage += itemData.meleePoisonDamage;
            Player.MeleeEnergyDamage += itemData.meleeEnergyDamage;
            Player.Strength += itemData.strength;
            Player.Perception += itemData.perception;
            Player.Intelligence += itemData.intelligence;
            Player.Agility += itemData.agility;
            Player.Charisma += itemData.charisma;
            Player.Willpower += itemData.willpower;

            Player.MeleeAttack = Player.MeleePhysicalDamage + Player.MeleeFireDamage + Player.MeleeColdDamage
                + Player.MeleePoisonDamage + Player.MeleeEnergyDamage;

            ItemCreator itemCreator = GameObject.Find(Constants.ItemCreatorList).GetComponent<ItemCreator>();

            foreach (var ability in itemCreator.abilitiesDataList)
            {
                if (ability.abilityWeapon == itemData.weaponType)
                {
                    Player.CombatAbilities.Add(itemCreator.CreateCombatAbility(ability));
                }
            }
        }

        RefreshStats();
    }

    public void EquipRangedWeapon(RangedWeaponData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = true;
            Player.AttackSpeed += itemData.attackSpeed;
            Player.HitChance += itemData.hitChance;
            Player.Dodge += itemData.dodge;
            Player.Resistance += itemData.resistance;
            Player.CounterChance += itemData.counterChance;
            Player.Penetration += itemData.penetration;
            Player.PsiDamage += itemData.psiDamage;
            Player.RangedPhysicalDamage += itemData.rangedPhysicalDamage;
            Player.RangedFireDamage += itemData.rangedFireDamage;
            Player.RangedColdDamage += itemData.rangedColdDamage;
            Player.RangedPoisonDamage += itemData.rangedPoisonDamage;
            Player.RangedEnergyDamage += itemData.rangedEnergyDamage;
            Player.Strength += itemData.strength;
            Player.Perception += itemData.perception;
            Player.Intelligence += itemData.intelligence;
            Player.Agility += itemData.agility;
            Player.Charisma += itemData.charisma;
            Player.Willpower += itemData.willpower;
        }
        RefreshStats();
    }

    public void EquipShield(ShieldData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = true;
            Player.AttackSpeed += itemData.attackSpeed;
            Player.HitChance += itemData.hitChance;
            Player.Dodge += itemData.dodge;
            Player.Resistance += itemData.resistance;
            Player.CounterChance += itemData.counterChance;
            Player.Penetration += itemData.penetration;
            Player.PsiDamage += itemData.psiDamage;
            Player.MeleePhysicalDamage += itemData.meleePhysicalDamage;
            Player.MeleeFireDamage += itemData.meleeFireDamage;
            Player.MeleeColdDamage += itemData.meleeColdDamage;
            Player.MeleePoisonDamage += itemData.meleePoisonDamage;
            Player.MeleeEnergyDamage += itemData.meleeEnergyDamage;
            Player.PhysicalProtection += itemData.physicalProtection;
            Player.FireProtection += itemData.fireProtection;
            Player.ColdProtection += itemData.coldProtection;
            Player.PoisonProtection += itemData.poisonProtection;
            Player.EnergyProtection += itemData.energyProtection;
            Player.PsiProtection += itemData.psiProtection;
            Player.ShieldPoints += itemData.shieldPoints;
            Player.Armor += itemData.armor;
            Player.HitPoints += itemData.hitPoints;
            Player.Strength += itemData.strength;
            Player.Perception += itemData.perception;
            Player.Intelligence += itemData.intelligence;
            Player.Agility += itemData.agility;
            Player.Charisma += itemData.charisma;
            Player.Willpower += itemData.willpower;
        }
        RefreshStats();
    }

    public void EquipOffhand(OffHandData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = true;
            Player.AttackSpeed += itemData.attackSpeed;
            Player.HitChance += itemData.hitChance;
            Player.Dodge += itemData.dodge;
            Player.Resistance += itemData.resistance;
            Player.CounterChance += itemData.counterChance;
            Player.Penetration += itemData.penetration;
            Player.PsiDamage += itemData.psiDamage;
            Player.MeleePhysicalDamage += itemData.meleePhysicalDamage;
            Player.MeleeFireDamage += itemData.meleeFireDamage;
            Player.MeleeColdDamage += itemData.meleeColdDamage;
            Player.MeleePoisonDamage += itemData.meleePoisonDamage;
            Player.MeleeEnergyDamage += itemData.meleeEnergyDamage;
            Player.RangedPhysicalDamage += itemData.rangedPhysicalDamage;
            Player.RangedFireDamage += itemData.rangedFireDamage;
            Player.RangedColdDamage += itemData.rangedColdDamage;
            Player.RangedPoisonDamage += itemData.rangedPoisonDamage;
            Player.RangedEnergyDamage += itemData.rangedEnergyDamage;
            Player.PhysicalProtection += itemData.physicalProtection;
            Player.FireProtection += itemData.fireProtection;
            Player.ColdProtection += itemData.coldProtection;
            Player.PoisonProtection += itemData.poisonProtection;
            Player.EnergyProtection += itemData.energyProtection;
            Player.PsiProtection += itemData.psiProtection;
            Player.ShieldPoints += itemData.shieldPoints;
            Player.Armor += itemData.armor;
            Player.HitPoints += itemData.hitPoints;
            Player.Strength += itemData.strength;
            Player.Perception += itemData.perception;
            Player.Intelligence += itemData.intelligence;
            Player.Agility += itemData.agility;
            Player.Charisma += itemData.charisma;
            Player.Willpower += itemData.willpower;
        }
        RefreshStats();
    }

    public void UnequipHelmet(HelmetData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.PhysicalProtection -= itemData.physicalProtection;
            Player.FireProtection -= itemData.fireProtection;
            Player.ColdProtection -= itemData.coldProtection;
            Player.PoisonProtection -= itemData.poisonProtection;
            Player.EnergyProtection -= itemData.energyProtection;
            Player.PsiProtection -= itemData.psiProtection;
            Player.ShieldPoints -= itemData.shieldPoints;
            Player.Armor -= itemData.armor;
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
            Player.PoisonProtection -= itemData.poisonProtection;
            Player.EnergyProtection -= itemData.energyProtection;
            Player.PsiProtection -= itemData.psiProtection;
            Player.ShieldPoints -= itemData.shieldPoints;
            Player.Armor -= itemData.armor;
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
        if (itemData.itemType == Constants.Fabricator)
        {
            DisableProduction();
        }
        RefreshStats();
        RefreshRecipeStats();
    }

    public void UnequipMeleeWeapon(MeleeWeaponData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.AttackSpeed -= itemData.attackSpeed;
            Player.HitChance -= itemData.hitChance;
            Player.Dodge -= itemData.dodge;
            Player.Resistance -= itemData.resistance;
            Player.CounterChance -= itemData.counterChance;
            Player.Penetration -= itemData.penetration;
            Player.PsiDamage -= itemData.psiDamage;
            Player.MeleePhysicalDamage -= itemData.meleePhysicalDamage;
            Player.MeleeFireDamage -= itemData.meleeFireDamage;
            Player.MeleeColdDamage -= itemData.meleeColdDamage;
            Player.MeleePoisonDamage -= itemData.meleePoisonDamage;
            Player.MeleeEnergyDamage -= itemData.meleeEnergyDamage;
            Player.Strength -= itemData.strength;
            Player.Perception -= itemData.perception;
            Player.Intelligence -= itemData.intelligence;
            Player.Agility -= itemData.agility;
            Player.Charisma -= itemData.charisma;
            Player.Willpower -= itemData.willpower;
        }
        RefreshStats();
    }

    public void UnequipRangedWeapon(RangedWeaponData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.AttackSpeed -= itemData.attackSpeed;
            Player.HitChance -= itemData.hitChance;
            Player.Dodge -= itemData.dodge;
            Player.Resistance -= itemData.resistance;
            Player.CounterChance -= itemData.counterChance;
            Player.Penetration -= itemData.penetration;
            Player.PsiDamage -= itemData.psiDamage;
            Player.RangedPhysicalDamage -= itemData.rangedPhysicalDamage;
            Player.RangedFireDamage -= itemData.rangedFireDamage;
            Player.RangedColdDamage -= itemData.rangedColdDamage;
            Player.RangedPoisonDamage -= itemData.rangedPoisonDamage;
            Player.RangedEnergyDamage -= itemData.rangedEnergyDamage;
            Player.Strength -= itemData.strength;
            Player.Perception -= itemData.perception;
            Player.Intelligence -= itemData.intelligence;
            Player.Agility -= itemData.agility;
            Player.Charisma -= itemData.charisma;
            Player.Willpower -= itemData.willpower;
        }
        RefreshStats();
    }

    public void UnequipShield(ShieldData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.AttackSpeed -= itemData.attackSpeed;
            Player.HitChance -= itemData.hitChance;
            Player.Dodge -= itemData.dodge;
            Player.Resistance -= itemData.resistance;
            Player.CounterChance -= itemData.counterChance;
            Player.Penetration -= itemData.penetration;
            Player.PsiDamage -= itemData.psiDamage;
            Player.MeleePhysicalDamage -= itemData.meleePhysicalDamage;
            Player.MeleeFireDamage -= itemData.meleeFireDamage;
            Player.MeleeColdDamage -= itemData.meleeColdDamage;
            Player.MeleePoisonDamage -= itemData.meleePoisonDamage;
            Player.MeleeEnergyDamage -= itemData.meleeEnergyDamage;
            Player.PhysicalProtection -= itemData.physicalProtection;
            Player.FireProtection -= itemData.fireProtection;
            Player.ColdProtection -= itemData.coldProtection;
            Player.PoisonProtection -= itemData.poisonProtection;
            Player.EnergyProtection -= itemData.energyProtection;
            Player.PsiProtection -= itemData.psiProtection;
            Player.ShieldPoints -= itemData.shieldPoints;
            Player.Armor -= itemData.armor;
            Player.HitPoints -= itemData.hitPoints;
            Player.Strength -= itemData.strength;
            Player.Perception -= itemData.perception;
            Player.Intelligence -= itemData.intelligence;
            Player.Agility -= itemData.agility;
            Player.Charisma -= itemData.charisma;
            Player.Willpower -= itemData.willpower;
        }
        RefreshStats();
    }

    public void UnequipOffhand(OffHandData itemData)
    {
        if (itemData != null)
        {
            itemData.isEquipped = false;
            Player.AttackSpeed -= itemData.attackSpeed;
            Player.HitChance -= itemData.hitChance;
            Player.Dodge -= itemData.dodge;
            Player.Resistance -= itemData.resistance;
            Player.CounterChance -= itemData.counterChance;
            Player.Penetration -= itemData.penetration;
            Player.PsiDamage -= itemData.psiDamage;
            Player.MeleePhysicalDamage -= itemData.meleePhysicalDamage;
            Player.MeleeFireDamage -= itemData.meleeFireDamage;
            Player.MeleeColdDamage -= itemData.meleeColdDamage;
            Player.MeleePoisonDamage -= itemData.meleePoisonDamage;
            Player.MeleeEnergyDamage -= itemData.meleeEnergyDamage;
            Player.RangedPhysicalDamage -= itemData.rangedPhysicalDamage;
            Player.RangedFireDamage -= itemData.rangedFireDamage;
            Player.RangedColdDamage -= itemData.rangedColdDamage;
            Player.RangedPoisonDamage -= itemData.rangedPoisonDamage;
            Player.RangedEnergyDamage -= itemData.rangedEnergyDamage;
            Player.PhysicalProtection -= itemData.physicalProtection;
            Player.FireProtection -= itemData.fireProtection;
            Player.ColdProtection -= itemData.coldProtection;
            Player.PoisonProtection -= itemData.poisonProtection;
            Player.EnergyProtection -= itemData.energyProtection;
            Player.PsiProtection -= itemData.psiProtection;
            Player.ShieldPoints -= itemData.shieldPoints;
            Player.Armor -= itemData.armor;
            Player.HitPoints -= itemData.hitPoints;
            Player.Strength -= itemData.strength;
            Player.Perception -= itemData.perception;
            Player.Intelligence -= itemData.intelligence;
            Player.Agility -= itemData.agility;
            Player.Charisma -= itemData.charisma;
            Player.Willpower -= itemData.willpower;
        }
        RefreshStats();
    }

    public void DeductFromEquip()
    {
        for (int i = 0; i < slotEquipped.Length; i++)
        {
            if (slotEquipped[i])
            {
                if (i == 5)
                {
                    Transform targetChild = EnergySlot.transform.Cast<Transform>().FirstOrDefault(child => child.GetComponent<ItemData>());
                    if (targetChild != null && targetChild.name == Constants.Battery)
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.quantity > PlayerResources.PlayerEnergy)
                        {
                            itemData.quantity -= PlayerResources.PlayerEnergy;
                            if (itemData.transform.Find(Constants.CountInventory).TryGetComponent<TextMeshProUGUI>(out var newCountText))
                            {
                                newCountText.text = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                                coroutineManager.UpdateQuantityTexts(itemData.itemName, itemData.itemProduct);
                            }
                        }
                        else
                        {
                            if (!autoConsumption)
                            {
                                slotEquipped[i] = false;
                                slotEquippedName[i] = "";
                                PlayerResources.PlayerEnergy = 0f;
                                GameObject noEnergyObjects = GameObject.Find(Constants.NoEnergyObjects);
                                if (noEnergyObjects != null)
                                {
                                    if (noEnergyObjects.TryGetComponent<ActivateObjects>(out var activateScript))
                                    {
                                        activateScript.ActivateAllObjects();
                                    }
                                }
                                targetChild.parent.transform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(true);
                                inventoryManager.DestroySpecificItem(targetChild.name, itemData.itemProduct, itemData.ID);
                                Destroy(targetChild.gameObject);
                                globalCalculator.UpdatePlayerConsumption();
                            }
                        }
                    }
                }
                if (i == 6 && GlobalCalculator.isPlayerInBiologicalBiome == false)
                {
                    Transform targetChild = OxygenSlot.transform.Cast<Transform>().FirstOrDefault(child => child.GetComponent<ItemData>());
                    if (targetChild != null && targetChild.name == Constants.OxygenTank)
                    {
                        if (targetChild.TryGetComponent<ItemData>(out var itemData))
                        {

                        }

                    }
                }
                if (i == 7)
                {
                    Transform targetChild = WaterSlot.transform.Cast<Transform>().FirstOrDefault(child => child.GetComponent<ItemData>());
                    if (targetChild != null)
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.quantity > PlayerResources.PlayerWater)
                        {
                            itemData.quantity -= PlayerResources.PlayerWater;
                            if (itemData.transform.Find(Constants.CountInventory).TryGetComponent<TextMeshProUGUI>(out var newCountText))
                            {
                                newCountText.text = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                                coroutineManager.UpdateQuantityTexts(itemData.itemName, itemData.itemProduct);
                            }
                        }
                        else
                        {
                            if (!autoConsumption)
                            {
                                slotEquipped[i] = false;
                                slotEquippedName[i] = "";
                                PlayerResources.PlayerWater = 0f;
                                targetChild.parent.transform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(true);
                                inventoryManager.DestroySpecificItem(targetChild.name, itemData.itemProduct, itemData.ID);
                                Destroy(targetChild.gameObject);
                                globalCalculator.UpdatePlayerConsumption();
                            }
                        }
                    }
                }
                if (i == 8)
                {
                    Transform targetChild = HungerSlot.transform.Cast<Transform>().FirstOrDefault(child => child.GetComponent<ItemData>());
                    if (targetChild != null)
                    {
                        ItemData itemData = targetChild.GetComponent<ItemData>();
                        if (itemData != null && itemData.quantity > PlayerResources.PlayerHunger)
                        {
                            itemData.quantity -= PlayerResources.PlayerHunger;
                            if (itemData.transform.Find(Constants.CountInventory).TryGetComponent<TextMeshProUGUI>(out var newCountText))
                            {
                                newCountText.text = itemData.quantity.ToString("F2", CultureInfo.InvariantCulture);
                                coroutineManager.UpdateQuantityTexts(itemData.itemName, itemData.itemProduct);
                            }
                        }
                        else
                        {
                            if (!autoConsumption)
                            {
                                slotEquipped[i] = false;
                                slotEquippedName[i] = "";
                                PlayerResources.PlayerHunger = 0f;
                                targetChild.parent.transform.Find(Constants.EmptyButton).GetComponent<Image>().gameObject.SetActive(true);
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
