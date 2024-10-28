using Assets.Scripts.ItemFactory;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Enumerations;

namespace ItemManagement
{
    [Serializable]
    public class ItemDataJson // 1st layer
    {
        public int ID;
        public int index;
        public float quantity;
        public float stackLimit;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
    }

    [Serializable]
    public class EquipmentDataJson : ItemDataJson // 2nd layer
    {
        public int durability;
        public int maxDurability;
        public int strength;
        public int perception;
        public int intelligence;
        public int agility;
        public int charisma;
        public int willpower;
    }

    [Serializable]
    public class ArmorDataJson : EquipmentDataJson // 3rd layer
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int poisonProtection;
        public int energyProtection;
        public int psiProtection;
        public int shieldPoints;
        public int armor;
        public int hitPoints;
    }

    [Serializable]
    public class ToolDataJson : EquipmentDataJson // 3rd layer
    {
        public float productionSpeed;
        public float materialCost;
        public float outcomeRate;
    }

    [Serializable]
    public class WeaponDataJson : EquipmentDataJson // 3rd layer
    {
        public string weaponType;
        public float attackSpeed;
        public int hitChance;
        public int dodge;
        public int resistance;
        public int counterChance;
        public int penetration;
        public int psiDamage;
    }

    [Serializable]
    public class HelmetDataJson : ArmorDataJson // 4th layer
    {
        public int visibilityRadius;
        public int explorationRadius;
        public int pickupRadius;

    }

    [Serializable]
    public class SuitDataJson : ArmorDataJson // 4th layer
    {
        public int energyCapacity;
        public int inventorySlots;
    }

    [Serializable]
    public class MeleeWeaponDataJson : WeaponDataJson // 4th layer
    {
        public int meleePhysicalDamage;
        public int meleeFireDamage;
        public int meleeColdDamage;
        public int meleePoisonDamage;
        public int meleeEnergyDamage;
    }

    [Serializable]
    public class RangedWeaponDataJson : WeaponDataJson // 4th layer
    {
        public int rangedPhysicalDamage;
        public int rangedFireDamage;
        public int rangedColdDamage;
        public int rangedPoisonDamage;
        public int rangedEnergyDamage;
    }

    [Serializable]
    public class ShieldDataJson : MeleeWeaponDataJson // 4th layer
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int poisonProtection;
        public int energyProtection;
        public int psiProtection;
        public int shieldPoints;
        public int armor;
        public int hitPoints;
    }

    [Serializable]
    public class OffHandDataJson : ShieldDataJson // 5th layer
    {
        public int rangedPhysicalDamage;
        public int rangedFireDamage;
        public int rangedColdDamage;
        public int rangedPoisonDamage;
        public int rangedEnergyDamage;
    }

    [Serializable]
    public class ItemData : MonoBehaviour // 1st layer mono behaviour
    {
        public int ID;
        public int index;
        public float stackLimit;
        public float quantity;
        public string itemProduct;
        public string itemType;
        public string itemClass;
        public string itemName;
        public bool equipable;
        public bool isEquipped;
    }

    [Serializable]
    public class EquipmentData : ItemData // 2nd layer mono behaviour
    {
        public int durability;
        public int maxDurability;
        public int strength;
        public int perception;
        public int intelligence;
        public int agility;
        public int charisma;
        public int willpower;

        public virtual void Repair()
        {
            durability = maxDurability;
        }
    }

    [Serializable]
    public class ArmorData : EquipmentData // 3rd layer mono behaviour
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int poisonProtection;
        public int energyProtection;
        public int psiProtection;
        public int shieldPoints;
        public int armor;
        public int hitPoints;
    }

    [Serializable]
    public class WeaponData : EquipmentData // 3rd layer mono behaviour
    {
        public string weaponType;
        public float attackSpeed;
        public int hitChance;
        public int dodge;
        public int resistance;
        public int counterChance;
        public int penetration;
        public int psiDamage;
    }

    [Serializable]
    public class ToolData : EquipmentData // 3rd layer mono behaviour
    {
        public float productionSpeed;
        public float materialCost;
        public float outcomeRate;
    }

    [Serializable]
    public class HelmetData : ArmorData // 4th layer mono behaviour
    {
        public int visibilityRadius;
        public int explorationRadius;
        public int pickupRadius;
    }

    [Serializable]
    public class SuitData : ArmorData // 4th layer mono behaviour
    {
        public int energyCapacity;
        public int inventorySlots;
    }

    [Serializable]
    public class MeleeWeaponData : WeaponData // 4th layer mono behaviour
    {
        public int meleePhysicalDamage;
        public int meleeFireDamage;
        public int meleeColdDamage;
        public int meleePoisonDamage;
        public int meleeEnergyDamage;
    }

    [Serializable]
    public class RangedWeaponData : WeaponData // 4th layer mono behaviour
    {
        public int rangedPhysicalDamage;
        public int rangedFireDamage;
        public int rangedColdDamage;
        public int rangedPoisonDamage;
        public int rangedEnergyDamage;
    }

    [Serializable]
    public class ShieldData : MeleeWeaponData // 4th layer mono behaviour
    {
        public int physicalProtection;
        public int fireProtection;
        public int coldProtection;
        public int poisonProtection;
        public int energyProtection;
        public int psiProtection;
        public int shieldPoints;
        public int armor;
        public int hitPoints;
    }

    [Serializable]
    public class OffHandData : ShieldData // 5th layer mono behaviour
    {
        public int rangedPhysicalDamage;
        public int rangedFireDamage;
        public int rangedColdDamage;
        public int rangedPoisonDamage;
        public int rangedEnergyDamage;
    }

    [Serializable]
    public class CombatAbilityJson
    {
        public int index;
        public string abilityName;
        public string abilityType;
        public string abilityWeapon;
        public bool isMovingAbility;
        public bool isFrontLineAoe;
        public bool isBackLineAoe;
        public float meleeDamageScale;
        public float rangedDamageScale;
        public float psiDamageScale;
        public float scaleMultiplication;
        public int cooldown;
        [JsonConverter(typeof(StringEnumConverter))]
        public AbilityTrigger abilityTrigger;
        public List<StatusEffect> negativeEffectsList;
        public List<StatusEffect> positiveEffectsList;
        public List<AbilityPrefab> abilityPrefabsList;
    }

    public class ItemCreator : MonoBehaviour
    {
        public GameObject itemTemplate;
        public ItemFactory itemFactory;

        [Serializable]
        private class ItemDataJsonArray
        {
            public List<ItemDataJson> items;
        }

        [Serializable]
        private class SuitDataJsonArray
        {
            public List<SuitDataJson> items;
        }

        [Serializable]
        private class HelmetDataJsonArray
        {
            public List<HelmetDataJson> items;
        }

        [Serializable]
        private class ToolDataJsonArray
        {
            public List<ToolDataJson> items;
        }

        [Serializable]
        private class MeleeWeaponDataJsonArray
        {
            public List<MeleeWeaponDataJson> items;
        }

        [Serializable]
        private class RangedWeaponDataJsonArray
        {
            public List<RangedWeaponDataJson> items;
        }

        [Serializable]
        private class ShieldDataJsonArray
        {
            public List<ShieldDataJson> items;
        }

        [Serializable]
        private class OffHandDataJsonArray
        {
            public List<OffHandDataJson> items;
        }

        [Serializable]
        private class CombatAbilitiesJsonArray
        {
            public List<CombatAbilityJson> abilities;
        }

        private List<ItemDataJson> itemDataList;
        private List<SuitDataJson> suitDataList;
        private List<HelmetDataJson> helmetDataList;
        private List<ToolDataJson> toolDataList;
        private List<MeleeWeaponDataJson> meleeWeaponDataList;
        private List<RangedWeaponDataJson> rangedWeaponDataList;
        private List<ShieldDataJson> shieldDataList;
        private List<OffHandDataJson> offHandDataList;
        public List<CombatAbilityJson> abilitiesDataList;

        private void Awake()
        {
            string jsonText = Assets.Scripts.Models.ItemsListJson.json;
            ItemDataJsonArray itemDataArray = JsonUtility.FromJson<ItemDataJsonArray>(jsonText);
            if (itemDataArray != null)
            {
                itemDataList = itemDataArray.items;
            }

            string suitJsonText = Assets.Scripts.Models.SuitListJson.json;
            SuitDataJsonArray suitDataArray = JsonUtility.FromJson<SuitDataJsonArray>(suitJsonText);
            if (suitDataArray != null)
            {
                suitDataList = suitDataArray.items;
            }

            string helmetJsonText = Assets.Scripts.Models.HelmetListJson.json;
            HelmetDataJsonArray helmetDataArray = JsonUtility.FromJson<HelmetDataJsonArray>(helmetJsonText);
            if (suitDataArray != null)
            {
                helmetDataList = helmetDataArray.items;
            }

            string toolJsonText = Assets.Scripts.Models.ToolsListJson.json;
            ToolDataJsonArray toolDataArray = JsonUtility.FromJson<ToolDataJsonArray>(toolJsonText);
            if (toolDataArray != null)
            {
                toolDataList = toolDataArray.items;
            }

            string meleeWeaponJsonText = Assets.Scripts.Models.MeleeWeaponsListJson.json;
            MeleeWeaponDataJsonArray meleeWeaponDataArray = JsonUtility.FromJson<MeleeWeaponDataJsonArray>(meleeWeaponJsonText);
            if (meleeWeaponDataArray != null)
            {
                meleeWeaponDataList = meleeWeaponDataArray.items;
            }

            string rangedWeaponJsonText = Assets.Scripts.Models.RangedWeaponsListJson.json;
            RangedWeaponDataJsonArray rangedWeaponDataArray = JsonUtility.FromJson<RangedWeaponDataJsonArray>(rangedWeaponJsonText);
            if (rangedWeaponDataArray != null)
            {
                rangedWeaponDataList = rangedWeaponDataArray.items;
            }

            string shieldJsonText = Assets.Scripts.Models.ShieldsListJson.json;
            ShieldDataJsonArray shieldDataArray = JsonUtility.FromJson<ShieldDataJsonArray>(shieldJsonText);
            if (shieldDataArray != null)
            {
                shieldDataList = shieldDataArray.items;
            }

            string offHandJsonText = Assets.Scripts.Models.OffHandsListJson.json;
            OffHandDataJsonArray offHandDataArray = JsonUtility.FromJson<OffHandDataJsonArray>(offHandJsonText);
            if (offHandDataArray != null)
            {
                offHandDataList = offHandDataArray.items;
            }

            string abilitiesDataJsonText = Assets.Scripts.Models.AbilitiesListJson.json;
            CombatAbilitiesJsonArray combatAbilitiesDataArray = JsonUtility.FromJson<CombatAbilitiesJsonArray>(abilitiesDataJsonText);
            if (combatAbilitiesDataArray != null)
            {
                abilitiesDataList = combatAbilitiesDataArray.abilities;
            }
        }
        public CombatAbility CreateCombatAbility(CombatAbilityJson abilityData)
        {
            return new CombatAbility(abilityData.index, abilityData.abilityName, abilityData.abilityType, abilityData.abilityWeapon, abilityData.isFrontLineAoe, abilityData.isBackLineAoe,
                abilityData.meleeDamageScale, abilityData.rangedDamageScale, abilityData.psiDamageScale, abilityData.scaleMultiplication, abilityData.cooldown, abilityData.isMovingAbility,
                abilityData.abilityTrigger, abilityData.positiveEffectsList, abilityData.negativeEffectsList, abilityData.abilityPrefabsList);
        }

        public void Recreateitem(float quantity, string itemProduct, string itemType, string itemClass, string itemName,
            int index, float stackLimit, bool equipable, int ID, bool isEquipped, RectTransform rectTransform = null)
        {
            itemFactory.RecreateItem(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, rectTransform);
        }

        public void RecreateSuit(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit,
            bool equipable, int ID, bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int poisonProtection, int explosionProtection, int psiProtection,
            int shieldPoints, int armor, int hitPoints, int energyCapacity, int durability, int maxDurability, int inventorySlots, int strength, int perception, int intelligence,
            int agility, int charisma, int willpower, RectTransform rectTransform = null)
        {
            itemFactory.RecreateSuit(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, physicalProtection,
                fireProtection, coldProtection, poisonProtection, explosionProtection, psiProtection, shieldPoints, armor, hitPoints, energyCapacity, durability, maxDurability, inventorySlots,
                strength, perception, intelligence, agility, charisma, willpower, rectTransform);
        }

        public void RecreateHelmet(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit,
            bool equipable, int ID, bool isEquipped, int physicalProtection, int fireProtection, int coldProtection, int poisonProtection, int explosionProtection, int psiProtection,
            int shieldPoints, int armor, int hitPoints, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma,
            int willpower, int visibilityRadius, int explorationRadius, int pickupRadius, RectTransform rectTransform = null)
        {
            itemFactory.RecreateHelmet(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, physicalProtection,
                fireProtection, coldProtection, poisonProtection, explosionProtection, psiProtection, shieldPoints, armor, hitPoints, durability, maxDurability,
                strength, perception, intelligence, agility, charisma, willpower, visibilityRadius, explorationRadius, pickupRadius, rectTransform);
        }

        public void RecreateMeleeWeapon(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit, bool equipable, int ID, bool isEquipped,
            float attackSpeed, int hitChance, int dodge, int resistance, int counterChance, int penetration, int psiDamage, int meleePhysicalDamage, int meleeFireDamage, int meleeColdDamage, int meleePoisonDamage,
            int meleeEnergyDamage, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, string weaponType, RectTransform rectTransform = null)
        {
            itemFactory.RecreateMeleeWeapon(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, attackSpeed, hitChance, dodge,
                resistance, counterChance, penetration, psiDamage, meleePhysicalDamage, meleeFireDamage, meleeColdDamage, meleePoisonDamage, meleeEnergyDamage, durability, maxDurability, strength,
                perception, intelligence, agility, charisma, willpower, weaponType, rectTransform);
        }

        public void RecreateRangedWeapon(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit, bool equipable, int ID, bool isEquipped,
            float attackSpeed, int hitChance, int dodge, int resistance, int counterChance, int penetration, int psiDamage, int rangedPhysicalDamage, int rangedFireDamage, int rangedColdDamage, int rangedPoisonDamage,
            int rangedEnergyDamage, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, string weaponType, RectTransform rectTransform = null)
        {
            itemFactory.RecreateRangedWeapon(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, attackSpeed, hitChance, dodge,
                resistance, counterChance, penetration, psiDamage, rangedPhysicalDamage, rangedFireDamage, rangedColdDamage, rangedPoisonDamage, rangedEnergyDamage, durability, maxDurability, strength,
                perception, intelligence, agility, charisma, willpower, weaponType, rectTransform);
        }

        public void RecreateShield(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit, bool equipable, int ID, bool isEquipped,
            float attackSpeed, int hitChance, int dodge, int resistance, int counterChance, int penetration, int psiDamage, int meleePhysicalDamage, int meleeFireDamage, int meleeColdDamage, int meleePoisonDamage,
            int meleeEnergyDamage, int physicalProtection, int fireProtection, int coldProtection, int poisonProtection, int energyProtection, int psiProtection, int shieldPoints, int armor, int hitPoints,
            int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma, int willpower, string weaponType, RectTransform rectTransform = null)
        {
            itemFactory.RecreateShield(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, attackSpeed, hitChance, dodge, resistance,
                counterChance, penetration, psiDamage, meleePhysicalDamage, meleeFireDamage, meleeColdDamage, meleePoisonDamage, meleeEnergyDamage, physicalProtection, fireProtection, coldProtection,
                poisonProtection, energyProtection, psiProtection, shieldPoints, armor, hitPoints, durability, maxDurability, strength, perception, intelligence, agility, charisma, willpower, weaponType,
                rectTransform);
        }

        public void RecreateOffhand(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit, bool equipable, int ID, bool isEquipped,
            float attackSpeed, int hitChance, int dodge, int resistance, int counterChance, int penetration, int psiDamage, int meleePhysicalDamage, int meleeFireDamage, int meleeColdDamage, int meleePoisonDamage,
            int meleeEnergyDamage, int rangedPhysicalDamage, int rangedFireDamage, int rangedColdDamage, int rangedPoisonDamage, int rangedEnergyDamage, int physicalProtection, int fireProtection, int coldProtection,
            int poisonProtection, int energyProtection, int psiProtection, int shieldPoints, int armor, int hitPoints, int durability, int maxDurability, int strength, int perception, int intelligence, int agility,
            int charisma, int willpower, string weaponType, RectTransform rectTransform = null)
        {
            itemFactory.RecreateOffhand(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, attackSpeed, hitChance, dodge, resistance,
                counterChance, penetration, psiDamage, meleePhysicalDamage, meleeFireDamage, meleeColdDamage, meleePoisonDamage, meleeEnergyDamage, rangedPhysicalDamage, rangedFireDamage, rangedColdDamage,
                rangedPoisonDamage, rangedEnergyDamage, physicalProtection, fireProtection, coldProtection, poisonProtection, energyProtection, psiProtection, shieldPoints, armor, hitPoints, durability,
                maxDurability, strength, perception, intelligence, agility, charisma, willpower, weaponType, rectTransform);
        }

        public void RecreateTool(float quantity, string itemProduct, string itemType, string itemClass, string itemName, int index, float stackLimit,
            bool equipable, int ID, bool isEquipped, int durability, int maxDurability, int strength, int perception, int intelligence, int agility, int charisma,
            int willpower, float productionSpeed, float materialCost, float outcomeRate, RectTransform rectTransform = null)
        {
            itemFactory.RecreateTool(quantity, itemTemplate, itemProduct, itemType, itemClass, itemName, index, stackLimit, equipable, ID, isEquipped, durability,
                maxDurability, strength, perception, intelligence, agility, charisma, willpower, productionSpeed, materialCost, outcomeRate, rectTransform);
        }

        public void CreateHelmet(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = helmetDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateHelmet(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index,
                itemData.stackLimit, itemData.equipable, itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.poisonProtection, itemData.energyProtection, itemData.psiProtection,
                itemData.shieldPoints, itemData.armor, itemData.hitPoints, itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility,
                itemData.charisma, itemData.willpower, itemData.visibilityRadius, itemData.explorationRadius, itemData.pickupRadius, rectTransform);
        }

        public void CreateSuit(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = suitDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateSuit(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index,
                itemData.stackLimit, itemData.equipable, itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.poisonProtection, itemData.energyProtection, itemData.psiProtection,
                itemData.shieldPoints, itemData.armor, itemData.hitPoints, itemData.energyCapacity, itemData.durability, itemData.maxDurability, itemData.inventorySlots, itemData.strength,
                itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, rectTransform);
        }

        public void CreateMeleeWeapon(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = meleeWeaponDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateMeleeWeapon(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.attackSpeed, itemData.hitChance, itemData.dodge, itemData.resistance, itemData.counterChance, itemData.penetration, itemData.psiDamage, itemData.meleePhysicalDamage, itemData.meleeFireDamage,
                itemData.meleeColdDamage, itemData.meleePoisonDamage, itemData.meleeEnergyDamage, itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence,
                itemData.agility, itemData.charisma, itemData.willpower, itemData.weaponType, rectTransform);
        }

        public void CreateRangedWeapon(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = rangedWeaponDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateRangedWeapon(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.attackSpeed, itemData.hitChance, itemData.dodge, itemData.resistance, itemData.counterChance, itemData.penetration, itemData.psiDamage, itemData.rangedPhysicalDamage, itemData.rangedFireDamage,
                itemData.rangedColdDamage, itemData.rangedPoisonDamage, itemData.rangedEnergyDamage, itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence,
                itemData.agility, itemData.charisma, itemData.willpower, itemData.weaponType, rectTransform);
        }

        public void CreateShield(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = shieldDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateShield(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.attackSpeed, itemData.hitChance, itemData.dodge, itemData.resistance, itemData.counterChance, itemData.penetration, itemData.psiDamage, itemData.meleePhysicalDamage, itemData.meleeFireDamage,
                itemData.meleeColdDamage, itemData.meleePoisonDamage, itemData.meleeEnergyDamage, itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.poisonProtection, itemData.energyProtection,
                itemData.psiProtection, itemData.shieldPoints, itemData.armor, itemData.hitPoints, itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence,
                itemData.agility, itemData.charisma, itemData.willpower, itemData.weaponType, rectTransform);
        }

        public void CreateOffhand(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = offHandDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateOffhand(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.attackSpeed, itemData.hitChance, itemData.dodge, itemData.resistance, itemData.counterChance, itemData.penetration, itemData.psiDamage, itemData.meleePhysicalDamage, itemData.meleeFireDamage,
                itemData.meleeColdDamage, itemData.meleePoisonDamage, itemData.meleeEnergyDamage, itemData.rangedPhysicalDamage, itemData.rangedFireDamage, itemData.rangedColdDamage, itemData.rangedPoisonDamage,
                itemData.rangedEnergyDamage, itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.poisonProtection, itemData.energyProtection, itemData.psiProtection,
                itemData.shieldPoints, itemData.armor, itemData.hitPoints, itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma,
                itemData.willpower, itemData.weaponType, rectTransform);
        }

        public void CreateTool(int itemIndex, float? quantity = null, RectTransform rectTransform = null)
        {
            var itemData = toolDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateTool(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, itemData.productionSpeed,
                itemData.materialCost, itemData.outcomeRate, rectTransform);
        }
        public void CreateItem(int itemIndex, float? quantity = null)
        {
            var itemData = itemDataList[itemIndex];
            float finalQuantity = quantity ?? itemData.quantity;
            itemFactory.CreateItem(finalQuantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable);
        }
        public void SplitTool(int itemIndex, float quantity)
        {
            var itemData = toolDataList[itemIndex];
            itemFactory.SplitTool(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, itemData.productionSpeed,
                itemData.materialCost, itemData.outcomeRate);
        }
        public void SplitHelmet(int itemIndex, float quantity)
        {
            var itemData = helmetDataList[itemIndex];
            itemFactory.SplitHelmet(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.poisonProtection, itemData.energyProtection, itemData.psiProtection, itemData.shieldPoints, itemData.armor, itemData.hitPoints,
                itemData.durability, itemData.maxDurability, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility, itemData.charisma, itemData.willpower, itemData.visibilityRadius,
                itemData.explorationRadius, itemData.pickupRadius);
        }
        public void SplitSuit(int itemIndex, float quantity)
        {
            var itemData = suitDataList[itemIndex];
            itemFactory.SplitSuit(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable,
                itemData.physicalProtection, itemData.fireProtection, itemData.coldProtection, itemData.poisonProtection, itemData.energyProtection, itemData.psiProtection, itemData.shieldPoints, itemData.armor,
                itemData.hitPoints, itemData.energyCapacity, itemData.durability, itemData.maxDurability, itemData.inventorySlots, itemData.strength, itemData.perception, itemData.intelligence, itemData.agility,
                itemData.charisma, itemData.willpower);
        }
        public void SplitItem(int itemIndex, float quantity)
        {
            var itemData = itemDataList[itemIndex];
            itemFactory.SplitItem(quantity, itemTemplate, itemData.itemProduct, itemData.itemType, itemData.itemClass, itemData.itemName, itemData.index, itemData.stackLimit, itemData.equipable);
        }
    }
}
