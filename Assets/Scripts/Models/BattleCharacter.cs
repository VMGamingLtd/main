using Cysharp.Threading.Tasks;
using ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enumerations;

public class BattleCharacter : MonoBehaviour
{
    [SerializeField] Image Timebar;
    [SerializeField] Image ShieldBar;
    [SerializeField] Image ArmorBar;
    [SerializeField] Image LifeBar;
    [SerializeField] GameObject Selection1;
    [SerializeField] GameObject Selection2;
    [SerializeField] GameObject Selection3;
    [SerializeField] GameObject Selection4;
    [SerializeField] GameObject InfoPanel;
    private BattleFormation BattleFormation;
    private Faction Faction;
    private Race Race;
    private int Position;
    private string Name = string.Empty;
    private int Level;
    private float BuffAttackSpeed;
    private float DebuffAttackSpeed;
    private float AttackSpeed;
    private float StoredAttackSpeed;
    private int HitChance;
    private int BuffHitChance;
    private int DebuffHitChance;
    private int CriticalChance;
    private int CriticalDamage;
    private int Penetration;
    private int CounterChance;
    private int BuffCounterChance;
    private int DebuffCounterChance;
    private int MeleeAttack;
    private int RangedAttack;
    private int PsiDamage;
    private int MeleePhysicalDamage;
    private int MeleeFireDamage;
    private int MeleeColdDamage;
    private int MeleePoisonDamage;
    private int MeleeEnergyDamage;
    private int RangedPhysicalDamage;
    private int RangedFireDamage;
    private int RangedColdDamage;
    private int RangedPoisonDamage;
    private int RangedEnergyDamage;
    private int MaxShieldPoints;
    private int ShieldPoints;
    private int StoredShieldPoints;
    private int BufShieldPoints;
    private int Armor;
    private int MaxArmor;
    private int StoredArmor;
    private int BuffArmor;
    private int HitPoints;
    private int MaxHitPoints;
    private int PhysicalProtection;
    private int FireProtection;
    private int ColdProtection;
    private int PoisonProtection;
    private int EnergyProtection;
    private int PsiProtection;
    private int Resistance;
    private int Dodge;
    private int Strength;
    private int Perception;
    private int Intelligence;
    private int Agility;
    private int Charisma;
    private int Willpower;
    private bool IsDead;
    private IList<CombatAbility> CombatAbilities = new List<CombatAbility>();
    private readonly IList<StatusEffect> NegativeAppliedEffects = new List<StatusEffect>();
    private readonly IList<StatusEffect> PositiveAppliedEffects = new List<StatusEffect>();
    private CancellationTokenSource cts = null;
    private CancellationToken cancellationToken;

    public void CreateCombatant(BattleFormation battleFormation, Faction faction, Race race, int position, string name, int level, BestiaryDataJson creatureData = null)
    {
        BattleFormation = battleFormation;
        Faction = faction;
        Race = race;
        Position = position;
        Name = name;
        Level = level;

        if (faction == Faction.Player)
        {
            InjectMainData(level, name, Player.AttackSpeed, Player.HitPoints, Player.Armor, Player.ShieldPoints, Player.MeleeAttack, Player.MeleePhysicalDamage, Player.MeleeFireDamage, Player.MeleeColdDamage,
                Player.MeleePoisonDamage, Player.MeleeEnergyDamage, Player.RangedAttack, Player.RangedPhysicalDamage, Player.RangedFireDamage, Player.RangedColdDamage, Player.RangedPoisonDamage, Player.RangedEnergyDamage,
                Player.PsiDamage, Player.PhysicalProtection, Player.FireProtection, Player.ColdProtection, Player.PoisonProtection, Player.EnergyProtection, Player.PsiProtection, Player.HitChance, Player.CriticalDamage,
                Player.CriticalDamage, Player.Dodge, Player.Resistance, Player.CounterChance, Player.Penetration);

            CombatAbilities = Player.CombatAbilities;
        }
        else if (faction == Faction.Ally)
        {

        }
        else
        {
            if (creatureData != null)
            {
                ItemCreator itemCreator = GameObject.Find(Constants.ItemCreatorList).GetComponent<ItemCreator>();

                foreach (var creatureAbility in creatureData.abilities)
                {
                    foreach (var ability in itemCreator.abilitiesDataList)
                    {
                        if (creatureAbility.index == ability.index)
                        {
                            CombatAbilities.Add(itemCreator.CreateCombatAbility(ability));
                        }
                    }
                }

                var creatureMeleeAttack = creatureData.meleePhysicalDamage + creatureData.meleeFireDamage + creatureData.meleeColdDamage
                    + creatureData.meleePoisonDamage + creatureData.meleeEnergyDamage;

                var creatureRangedAttack = creatureData.rangedPhysicalDamage + creatureData.rangedFireDamage + creatureData.rangedColdDamage
                    + creatureData.rangedPoisonDamage + creatureData.rangedEnergyDamage;

                InjectMainData(level, name, creatureData.attackSpeed, creatureData.hitPoints, creatureData.armor, creatureData.shieldPoints, creatureMeleeAttack, creatureData.meleePhysicalDamage,
                    creatureData.meleeFireDamage, creatureData.meleeColdDamage, creatureData.meleePoisonDamage, creatureData.meleeEnergyDamage, creatureRangedAttack, creatureData.rangedPhysicalDamage,
                    creatureData.rangedFireDamage, creatureData.rangedColdDamage, creatureData.rangedPoisonDamage, creatureData.rangedEnergyDamage, creatureData.psiDamage, creatureData.physicalProtection,
                    creatureData.fireProtection, creatureData.coldProtection, creatureData.poisonProtection, creatureData.energyProtection, creatureData.psiProtection, creatureData.hitChance, creatureData.criticalChance,
                    creatureData.criticalDamage, creatureData.dodge, creatureData.resistance, creatureData.counterChance, creatureData.penetration);

                gameObject.transform.Find("VisualPanel/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCharacterSpriteToSlot(name);
                gameObject.transform.Find("VisualPanel/Frame").GetComponent<Image>().color = Color.red;
            }

        }

        ApplyPassiveEffects();
    }

    public BattleFormation GetBattleFormation()
    {
        return BattleFormation;
    }

    public bool IsCombatantDead()
    {
        return IsDead;
    }

    public void AddStatusEffect(StatusEffect statusEffect, bool isPositive)
    {
        if (isPositive)
        {
            PositiveAppliedEffects.Add(statusEffect);
        }
        else
        {
            NegativeAppliedEffects.Add(statusEffect);
        }
    }

    /// <summary>
    /// Applies a specific debuff effect to the target by using its portion value to reduce the appropriate stat.
    /// </summary>
    /// <param name="statusEffect"></param>
    public void ApplyDebuffEffect(StatusEffect statusEffect)
    {
        if (statusEffect.type == StatusEffectType.Debuff)
        {
            if (statusEffect.statAffection == StatAffection.Attack)
            {
                MeleePhysicalDamage -= MeleePhysicalDamage * (statusEffect.portionValue / 100);
                MeleeFireDamage -= MeleeFireDamage * (statusEffect.portionValue / 100);
                MeleeColdDamage -= MeleeColdDamage * (statusEffect.portionValue / 100);
                MeleePoisonDamage -= MeleePoisonDamage * (statusEffect.portionValue / 100);
                MeleeEnergyDamage -= MeleeEnergyDamage * (statusEffect.portionValue / 100);

                RangedPhysicalDamage -= RangedPhysicalDamage * (statusEffect.portionValue / 100);
                RangedFireDamage -= RangedFireDamage * (statusEffect.portionValue / 100);
                RangedColdDamage -= RangedColdDamage * (statusEffect.portionValue / 100);
                RangedPoisonDamage -= RangedPoisonDamage * (statusEffect.portionValue / 100);
                RangedEnergyDamage -= RangedEnergyDamage * (statusEffect.portionValue / 100);

                var deductedMeleeAttack = MeleePhysicalDamage + MeleeFireDamage + MeleeColdDamage
                    + MeleePoisonDamage + MeleeEnergyDamage;

                var deductedRangedAttack = RangedPhysicalDamage + RangedFireDamage + RangedColdDamage
                    + RangedPoisonDamage + RangedEnergyDamage;

                if (InfoPanel.activeSelf)
                {
                    if (MeleeAttack > 0)
                    {
                        gameObject.transform.Find("InfoPanel/Row0/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedMeleeAttack.ToString();
                        gameObject.transform.Find("InfoPanel/Row0/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
                    }

                    if (RangedAttack > 0)
                    {
                        gameObject.transform.Find("InfoPanel/Row0/RangedAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedRangedAttack.ToString();
                        gameObject.transform.Find("InfoPanel/Row0/RangedAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
                    }
                }
            }
            else if (statusEffect.statAffection == StatAffection.Armor)
            {
                int debuffedArmor = MaxArmor * ((statusEffect.portionValue / 100) + 1);
                Armor -= debuffedArmor;
            }
            else if (statusEffect.statAffection == StatAffection.HitChance)
            {
                DebuffHitChance = statusEffect.portionValue;
                HitChance -= statusEffect.portionValue;

                if (InfoPanel.activeSelf)
                {
                    gameObject.transform.Find("InfoPanel/Row1/HitChance/Value").GetComponent<TextMeshProUGUI>().text = HitChance.ToString();
                    gameObject.transform.Find("InfoPanel/Row1/HitChance/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
                }
            }
            else if (statusEffect.statAffection == StatAffection.CounterChance)
            {
                DebuffCounterChance = statusEffect.portionValue;
                CounterChance -= statusEffect.portionValue;

                if (InfoPanel.activeSelf)
                {
                    gameObject.transform.Find("InfoPanel/Row3/CounterChance/Value").GetComponent<TextMeshProUGUI>().text = CounterChance.ToString();
                    gameObject.transform.Find("InfoPanel/Row3/CounterChance/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
                }
            }
            else if (statusEffect.statAffection == StatAffection.AttackSpeed)
            {
                StoredAttackSpeed = AttackSpeed;
                DebuffAttackSpeed = AttackSpeed * (statusEffect.portionValue / 100);
                AttackSpeed -= DebuffAttackSpeed;

                if (InfoPanel.activeSelf)
                {
                    gameObject.transform.Find("InfoPanel/Row1/AttackSpeed/Value").GetComponent<TextMeshProUGUI>().text = CounterChance.ToString();
                    gameObject.transform.Find("InfoPanel/Row1/AttackSpeed/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
                }
            }
        }
    }

    /// <summary>
    /// At the start of the battle check for all passive effects and apply them to the combatant.
    /// But only those that should proc every turn.
    /// </summary>
    public void ApplyPassiveEffects()
    {
        if (CombatAbilities.Count > 0)
        {
            foreach (var ability in CombatAbilities)
            {
                if (ability.Type == Constants.Passive || ability.Type == Constants.PassiveReflect)
                {
                    foreach (var statusEffect in ability.PositiveStatusEffects)
                    {
                        // this means that the passive effect should proc every start of the turn
                        if (statusEffect.duration == 0)
                        {
                            AddStatusEffect(statusEffect, true);
                            DisplayEffect(gameObject, statusEffect, false, true);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if combatant has any reflection effects that should be applied to attacker during the attack and puts them into a list.
    /// </summary>
    /// <returns></returns>
    public List<StatusEffect> GetReflectionStatusEffects()
    {
        List<StatusEffect> statusEffects = new();

        foreach (var ability in CombatAbilities)
        {
            if (ability.Type == Constants.PassiveReflect)
            {
                foreach (var statusEffect in ability.NegativeStatusEffects)
                {
                    statusEffects.Add(statusEffect);
                }
            }
        }

        return statusEffects;
    }

    /// <summary>
    /// Used to check if character doesn't have some passive abilities that can trigget e.g. when his HP is below 50%.
    /// </summary>
    public void CheckAbilityTriggers()
    {

    }

    /// <summary>
    /// Displays the buf or debuff effect visually in the StatusEffects part of the combatant object.
    /// </summary>
    /// <param name="targetCombatant"></param>
    /// <param name="statusEffect"></param>
    /// <param name="isDebuff"></param>
    public void DisplayEffect(GameObject targetCombatant, StatusEffect statusEffect, bool isDebuff, bool isPassive)
    {
        FightManager fightManager = GameObject.Find(Constants.FightManager).GetComponent<FightManager>();
        GameObject newItem;
        Transform targetTransform = targetCombatant.transform.Find("StatusEffects");
        newItem = Instantiate(fightManager.GetStatusEffectTemplate(), targetTransform);

        if (!isDebuff)
        {
            newItem.transform.Find("Image").GetComponent<Image>().color = UIColors.NeonGreenFull;
        }
        else
        {
            newItem.transform.Find("Image").GetComponent<Image>().color = UIColors.NeonRedFull;
        }

        if (!isPassive)
        {
            newItem.transform.Find("Duration").GetComponent<TextMeshProUGUI>().text = statusEffect.duration.ToString();
        }
        else
        {
            newItem.transform.Find("Duration").gameObject.SetActive(false);
        }
   
        newItem.transform.Find("Image/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot(statusEffect.name);
        newItem.transform.Find("GUID/guid").name = statusEffect.guid.ToString();
    }

    public void ApplyBuffEffect(StatusEffect statusEffect)
    {
        if (statusEffect.type == StatusEffectType.Buff)
        {
            if (statusEffect.statAffection == StatAffection.Attack)
            {
                MeleePhysicalDamage += MeleePhysicalDamage * (statusEffect.portionValue / 100);
                MeleeFireDamage += MeleeFireDamage * (statusEffect.portionValue / 100);
                MeleeColdDamage += MeleeColdDamage * (statusEffect.portionValue / 100);
                MeleePoisonDamage += MeleePoisonDamage * (statusEffect.portionValue / 100);
                MeleeEnergyDamage += MeleeEnergyDamage * (statusEffect.portionValue / 100);

                RangedPhysicalDamage += RangedPhysicalDamage * (statusEffect.portionValue / 100);
                RangedFireDamage += RangedFireDamage * (statusEffect.portionValue / 100);
                RangedColdDamage += RangedColdDamage * (statusEffect.portionValue / 100);
                RangedPoisonDamage += RangedPoisonDamage * (statusEffect.portionValue / 100);
                RangedEnergyDamage += RangedEnergyDamage * (statusEffect.portionValue / 100);

                var deductedMeleeAttack = MeleePhysicalDamage + MeleeFireDamage + MeleeColdDamage
                    + MeleePoisonDamage + MeleeEnergyDamage;

                var deductedRangedAttack = RangedPhysicalDamage + RangedFireDamage + RangedColdDamage
                    + RangedPoisonDamage + RangedEnergyDamage;

                if (InfoPanel.activeSelf)
                {
                    if (MeleeAttack > 0)
                    {
                        gameObject.transform.Find("InfoPanel/Row0/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedMeleeAttack.ToString();
                        gameObject.transform.Find("InfoPanel/Row0/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                    }

                    if (RangedAttack > 0)
                    {
                        gameObject.transform.Find("InfoPanel/Row0/RangedAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedRangedAttack.ToString();
                        gameObject.transform.Find("InfoPanel/Row0/RangedAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                    }
                }
            }
            else if (statusEffect.statAffection == StatAffection.Armor)
            {
                if (BuffArmor > 0)
                {
                    Armor -= BuffArmor;
                }

                float buffedArmor = MaxArmor * ((float)statusEffect.portionValue / (float)100);
                StoredArmor = Armor;
                BuffArmor = (int)Math.Round(buffedArmor);
                Armor += BuffArmor;

                if (InfoPanel.activeSelf)
                {
                    gameObject.transform.Find("InfoPanel/Armor/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = Armor.ToString();
                    gameObject.transform.Find("InfoPanel/Armor/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                }
            }
            else if (statusEffect.statAffection == StatAffection.HitChance)
            {
                if (BuffHitChance > 0)
                {
                    HitChance -= BuffHitChance;
                }

                BuffHitChance = statusEffect.portionValue;
                HitChance += statusEffect.portionValue;

                if (InfoPanel.activeSelf)
                {
                    gameObject.transform.Find("InfoPanel/Row1/HitChance/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = HitChance.ToString();
                    gameObject.transform.Find("InfoPanel/Row1/HitChance/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                }
            }
            else if (statusEffect.statAffection == StatAffection.CounterChance)
            {
                if (BuffCounterChance > 0)
                {
                    CounterChance -= BuffCounterChance;
                }

                BuffCounterChance = statusEffect.portionValue;
                CounterChance += statusEffect.portionValue;

                if (InfoPanel.activeSelf)
                {
                    gameObject.transform.Find("InfoPanel/Row3/CounterChance/Value").GetComponent<TextMeshProUGUI>().text = CounterChance.ToString();
                    gameObject.transform.Find("InfoPanel/Row3/CounterChance/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                }
            }
            else if (statusEffect.statAffection == StatAffection.AttackSpeed)
            {
                StoredAttackSpeed = AttackSpeed;
                BuffAttackSpeed = AttackSpeed * (statusEffect.portionValue / 100);
                AttackSpeed -= DebuffAttackSpeed;

                if (InfoPanel.activeSelf)
                {
                    gameObject.transform.Find("InfoPanel/Row1/AttackSpeed/Value").GetComponent<TextMeshProUGUI>().text = CounterChance.ToString();
                    gameObject.transform.Find("InfoPanel/Row1/AttackSpeed/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                }
            }
        }
    }

    public int ApplyDamageOverTimeReduction(StatusEffect statusEffect)
    {
        float rawDamage = statusEffect.damageValue * statusEffect.portionValue / 100;
        int damage = (int)Math.Round(rawDamage);
        ReceiveDamage(damage);
        return damage;
    }

    public int ApplyDamagedReductions(BattleCharacter attacker, CombatAbility attackerAbility)
    {
        int damage;
        float lowerDamage;
        float higherDamage;
        float rawDamage;

        if (attackerAbility.MeleeDamageScale > 0)
        {
            var meleePhysicalDamageDiff = Math.Max(0, attacker.MeleePhysicalDamage - PhysicalProtection);
            var firePhysicalDamageDiff = Math.Max(0, attacker.MeleeFireDamage - FireProtection);
            var coldPhysicalDamageDiff = Math.Max(0, attacker.MeleeColdDamage - ColdProtection);
            var poisonPhysicalDamageDiff = Math.Max(0, attacker.MeleePoisonDamage - PoisonProtection);
            var energyPhysicalDamageDiff = Math.Max(0, attacker.MeleeEnergyDamage - EnergyProtection);

            lowerDamage = meleePhysicalDamageDiff + firePhysicalDamageDiff + coldPhysicalDamageDiff + poisonPhysicalDamageDiff + energyPhysicalDamageDiff;
        }
        else if (attackerAbility.RangedDamageScale > 0)
        {
            var rangedPhysicalDamageDiff = Math.Max(0, attacker.RangedPhysicalDamage - PhysicalProtection);
            var rangedFireDamageDiff = Math.Max(0, attacker.RangedFireDamage - FireProtection);
            var rangedColdDamageDiff = Math.Max(0, attacker.RangedColdDamage - ColdProtection);
            var rangedPoisonDamageDiff = Math.Max(0, attacker.RangedPoisonDamage - PoisonProtection);
            var rangedEnergyDamageDiff = Math.Max(0, attacker.RangedEnergyDamage - EnergyProtection);

            lowerDamage = rangedPhysicalDamageDiff + rangedFireDamageDiff + rangedColdDamageDiff + rangedPoisonDamageDiff + rangedEnergyDamageDiff;
        }
        else
        {
            var psiDamageDiff = Math.Max(0, attacker.PsiDamage - PsiProtection);

            lowerDamage = psiDamageDiff;
        }

        higherDamage = lowerDamage * attackerAbility.ScaleMultiplication;
        rawDamage = UnityEngine.Random.Range(lowerDamage, higherDamage);
        damage = (int)Math.Round(rawDamage);

        return damage;
    }

    public void ReceiveDamage(int damage)
    {
        if (ShieldPoints > 0)
        {
            if (ShieldPoints >= damage)
            {
                ShieldPoints -= damage;
            }
            else
            {
                int remainingDamage = damage - ShieldPoints;
                ShieldPoints = 0;
                damage = remainingDamage;

                DeductArmor(damage);
            }
        }
        else if (Armor > 0)
        {
            DeductArmor(damage);
        }
        else
        {
            DeductHitPoints(damage);
        }

        if (ShieldPoints > 0)
        {
            ShieldBar.fillAmount = (float)ShieldPoints / (float)MaxShieldPoints;
        }
        else
        {
            ShieldBar.fillAmount = 0;
        }
        
        if (Armor > 0)
        {
            ArmorBar.fillAmount = (float)Armor / (float)MaxArmor;
        }
        else
        {
            ArmorBar.fillAmount = 0;
        }
        
        if (HitPoints > 0)
        {
            LifeBar.fillAmount = (float)HitPoints / (float)MaxHitPoints;
        }
        else
        {
            LifeBar.fillAmount = 0;
        }

        if (InfoPanel.activeSelf)
        {
            gameObject.transform.Find("InfoPanel/Shield/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = ShieldPoints.ToString();
            gameObject.transform.Find("InfoPanel/Armor/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = Armor.ToString();
            gameObject.transform.Find("InfoPanel/Health/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = HitPoints.ToString();
        }


        if (ShieldPoints == 0)
        {
            if (InfoPanel.activeSelf)
                gameObject.transform.Find("InfoPanel/Shield/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }

        if (Armor == 0)
        {
            if (InfoPanel.activeSelf)
                gameObject.transform.Find("InfoPanel/Armor/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }

        if (HitPoints == 0)
        {
            IsDead = true;
            Timebar.fillAmount = 0f;

            if (InfoPanel.activeSelf)
                gameObject.transform.Find("InfoPanel/Health/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }
    }

    public void BlinkCombatant()
    {
        if (InfoPanel.activeSelf)
        {
            gameObject.transform.Find("Blink").GetComponent<Animation>().Play("BlinkTarget");
        }
        else
        {
            gameObject.transform.Find("VisualPanel/Blink").GetComponent<Animation>().Play("BlinkTarget");
        }

    }

    public void StopBlinkingCombatant()
    {
        if (InfoPanel.activeSelf)
        {
            gameObject.transform.Find("Blink").GetComponent<Animation>().Stop("BlinkTarget");
            gameObject.transform.Find("Blink").GetComponent<Image>().color = UIColors.YellowInvisible;
        }
        else
        {
            gameObject.transform.Find("VisualPanel/Blink").GetComponent<Animation>().Stop("BlinkTarget");
            gameObject.transform.Find("VisualPanel/Blink").GetComponent<Image>().color = UIColors.YellowInvisible;
        }
    }

    public bool IsCombatantStunned()
    {
        if (NegativeAppliedEffects.Count > 0)
        {
            foreach (var effect in NegativeAppliedEffects)
            {
                if (effect.type == StatusEffectType.Stun ||
                    effect.type == StatusEffectType.Sleep)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public CombatAbility GetCombatantAbility(string abilityName)
    {
        foreach (var ability in CombatAbilities)
        {
            if (ability.Name == abilityName)
            {
                return ability;
            }
        }

        return null;
    }

    public int GetCombatantPosition()
    {
        return Position;
    }

    public string GetCombatantName()
    {
        return Name;
    }

    public int CheckStatusEffectCount()
    {
        return PositiveAppliedEffects.Count + NegativeAppliedEffects.Count;
    }

    // Returns any Integer based stat value
    public int GetCombatantIntStat(string statName)
    {
        switch (statName)
        {
            case Constants.HitChance: return HitChance;
            case Constants.CriticalChance: return CriticalChance;
            case Constants.CriticalDamage: return CriticalDamage;
            case Constants.Dodge: return Dodge;
            case Constants.Penetration: return Penetration;
            case Constants.Resistance: return Resistance;
            case Constants.CounterChance: return CounterChance;
            case Constants.HitPoints: return HitPoints;
            case Constants.Armor: return Armor;
            case Constants.ShieldPoints: return ShieldPoints;
            case Constants.Strength: return Strength;
            case Constants.Perception: return Perception;
            case Constants.Willpower: return Willpower;
            case Constants.Agility: return Agility;
            case Constants.Intelligence: return Intelligence;
            case Constants.Charisma: return Charisma;
            default:
                throw new ArgumentException("Invalid stat name");
        }
    }

    public async UniTask StartTimebar()
    {
        cts = new CancellationTokenSource();
        cancellationToken = cts.Token;
        await CalculateTimebar(cancellationToken);
        await UniTask.CompletedTask;
    }

    public void StopCurrentTask()
    {
        cts.Cancel();
    }

    public void ClearCombatantTimebar()
    {
        Timebar.fillAmount = 0f;
    }

    private async UniTask CalculateTimebar(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        while (Timebar.fillAmount < 1)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Timebar.fillAmount += AttackSpeed / 200;
            await UniTask.DelayFrame(1);
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        Timebar.fillAmount = 1f;

        if (GameObject.Find(Constants.FightManager).TryGetComponent<FightManager>(out var fightManager))
        {
            if (IsCombatantStunned())
            {
                Timebar.fillAmount = 0;
                CheckNegativeEffects(fightManager).Forget();
                CheckPositiveEffects(fightManager).Forget();
                ReduceAbilitiesCooldowns();
                StartTimebar().Forget();
                return;
            }

            CheckNegativeEffects(fightManager).Forget();
            CheckPositiveEffects(fightManager).Forget();
            ReduceAbilitiesCooldowns();

            fightManager.StopTimebars();
            fightManager.ActiveCombatant = gameObject;

            if (Faction == Faction.Player)
            {
                fightManager.ActionPanel.SetActive(true);
                fightManager.FillCombatAbilities(CombatAbilities);
            }
            else if (Faction == Faction.Enemy)
            {
                SetEnemyTarget(fightManager);
                SetCombatantAbilityAndAttack(fightManager, -1);
            }
        }
    }

    public void SetEnemyTarget(FightManager fightManager)
    {
        if (fightManager.EnemiesTarget == null)
        {
            foreach (var combatant in fightManager.Combatants)
            {
                if (combatant.TryGetComponent<BattleCharacter>(out var battleCharacter))
                {
                    if (battleCharacter != null && battleCharacter.Faction == Faction.Player)
                    {
                        fightManager.EnemiesTarget = combatant;
                        break;
                    }
                }
            }
        }
    }

    public void SetAttackerAsPlayerTarget(FightManager fightManager, GameObject enemyTarget)
    {
        fightManager.EnemyTarget = enemyTarget;
    }

    /// <summary>
    /// Sets a desired combatant ability for the upcoming attack. If index is -1 ability is randomly picked.
    /// This is used for enemy combatants attacking player.
    /// </summary>
    /// <param name="fightManager"></param>
    /// <param name="index"></param>
    public void SetCombatantAbilityAndAttack(FightManager fightManager, int index)
    {
        if (gameObject.TryGetComponent<CombatantFunctions>(out var combatantFunctions))
        {
            if (CombatAbilities.Count > 0)
            {
                int abilityIndex;

                if (index == -1)
                {
                    abilityIndex = UnityEngine.Random.Range(0, CombatAbilities.Count);
                }
                else
                {
                    abilityIndex = index;
                }

                var filteredAbilities = CombatAbilities.Where(ability => ability.Type != Constants.Passive).ToList();
                var randomAbility = filteredAbilities[abilityIndex];
                fightManager.ActiveAbility = randomAbility;
                combatantFunctions.AttackAllyTarget(randomAbility);
            }
        }
    }

    public void SetPlayerAbilityAndAttack(FightManager fightManager, int index)
    {
        if (gameObject.TryGetComponent<CombatantFunctions>(out var combatantFunctions))
        {
            if (CombatAbilities.Count > 0)
            {
                int abilityIndex;

                if (index == -1)
                {
                    abilityIndex = UnityEngine.Random.Range(0, CombatAbilities.Count);
                }
                else
                {
                    abilityIndex = index;
                }

                var randomAbility = CombatAbilities[abilityIndex];

                bool isAoe = false;

                if (randomAbility.IsBacklineAoe || randomAbility.IsFrontlineAoe)
                {
                    isAoe = true;
                }

                fightManager.ActiveAbility = randomAbility;
                combatantFunctions.AttackEnemyTarget(isAoe);
            }
        }
    }

    public void ReduceAbilitiesCooldowns()
    {
        if (CombatAbilities.Count > 0)
        {
            foreach (var ability in CombatAbilities)
            {
                if (ability.IsAbilityOnCooldown())
                {
                    ability.ReduceAbilityCooldown();
                }
            }
        }
    }

    public void TargetCombatant()
    {
        Selection1.SetActive(true);
        Selection2.SetActive(true);
        Selection3.SetActive(true);
        Selection4.SetActive(true);

        if (GameObject.Find("FIGHTMANAGER").TryGetComponent<FightManager>(out var fightManager))
        {
            fightManager.EnemyTarget = gameObject;
        }
    }

    public bool IsEnemy()
    {
        if (Faction == Faction.Enemy)
        {
            return true;
        }
        else if (Faction == Faction.Player)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This method supplements animation where InfoPanel is opened to keep it in its parameters.
    /// It is enclosed in CombatantTemplate/Button UI button.
    /// </summary>
    public void ShowInfoPanel()
    {
        InfoPanel.SetActive(true);
        FillInfoPanelStats(AttackSpeed, HitPoints, Armor, ShieldPoints, MeleeAttack, RangedAttack, PsiDamage, HitChance, Dodge, Resistance, CounterChance, Penetration);
        _ = ShowInfoPanelAsync();
    }

    /// <summary>
    /// This method supplements animation where InfoPanel is hidden.
    /// It is contained and used in CombatantTemplate/InfoPanel/Close UI button.
    /// </summary>
    public void HideInfoPanel()
    {
        _ = HideInfoPanelAsync();
    }

    public async UniTask ShowInfoPanelAsync()
    {
        float totalTime = 1f;
        float currentTime = 0f;
        float visualPosXend = 119.81f;
        float visualPosXstart = 0f;
        float infoPosXend = -78f;
        float infoPosXstart = 0f;
        float infoWidthEnd = 253.6927f;
        float infoWidthStart = 0f;
        GameObject visualPanel = gameObject.transform.Find("VisualPanel").gameObject;

        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            var t = currentTime / totalTime;
            float newVisualPanelX = Mathf.Lerp(visualPosXstart, visualPosXend, currentTime / totalTime);
            float newInfoPanelX = Mathf.Lerp(infoPosXstart, infoPosXend, currentTime / totalTime);
            visualPanel.transform.localPosition = new(newVisualPanelX, 0);
            InfoPanel.transform.localPosition = new(newInfoPanelX, 0);
            InfoPanel.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Lerp(infoWidthStart, infoWidthEnd, t), 170);
            await UniTask.Delay(1);
        }
    }

    public async UniTask HideInfoPanelAsync()
    {
        float totalTime = 1f;
        float currentTime = 0f;
        float visualPosXstart = 119.81f;
        float visualPosXend = 0f;
        float infoPosXstart = -78f;
        float infoPosXend = 0f;
        float infoWidthStart = 253.6927f;
        float infoWidthEnd = 0f;
        GameObject visualPanel = gameObject.transform.Find("VisualPanel").gameObject;

        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            var t = currentTime / totalTime;
            float newVisualPanelX = Mathf.Lerp(visualPosXstart, visualPosXend, currentTime / totalTime);
            float newInfoPanelX = Mathf.Lerp(infoPosXstart, infoPosXend, currentTime / totalTime);
            visualPanel.transform.localPosition = new(newVisualPanelX, 0);
            InfoPanel.transform.localPosition = new(newInfoPanelX, 0);
            InfoPanel.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Lerp(infoWidthStart, infoWidthEnd, t), 170);
            await UniTask.Delay(1);
        }

        InfoPanel.SetActive(false);
    }

    private void DeductArmor(int damage)
    {
        if (BuffArmor > 0)
        {
            if (BuffArmor >= damage)
            {
                BuffArmor -= damage;
            }
            else
            {
                int remainingDamage = damage - BuffArmor;
                BuffArmor = 0;
                damage = remainingDamage;
            }

            if (BuffArmor == 0)
            {
                IList<StatusEffect> effectsToRemove = new List<StatusEffect>();

                foreach (var positiveEffect in PositiveAppliedEffects)
                {
                    if (positiveEffect.statAffection == StatAffection.Armor)
                    {
                        effectsToRemove.Add(positiveEffect);

                        if (InfoPanel.activeSelf)
                            gameObject.transform.Find("InfoPanel/Armor/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.White;
                    }
                }

                RemovePositiveEffect(effectsToRemove);
            }

            Armor = StoredArmor + BuffArmor;
        }

        if (Armor >= damage)
        {
            Armor -= damage;
        }
        else
        {
            int remainingDamage = damage - Armor;
            Armor = 0;
            damage = remainingDamage;

            DeductHitPoints(damage);
        }
    }

    private void DeductHitPoints(int damage)
    {
        if (HitPoints >= damage)
        {
            HitPoints -= damage;
        }
        else
        {
            HitPoints = 0;
        }
    }

    private async UniTask CheckPositiveEffects(FightManager fightManager)
    {
        IList<StatusEffect> effectsToRemove = new List<StatusEffect>();

        bool shouldBlink = false;

        if (PositiveAppliedEffects.Count > 0)
        {
            foreach (var statusEffect in PositiveAppliedEffects)
            {
                if (statusEffect.portionValue > 0 && statusEffect.type == StatusEffectType.Buff)
                {
                    // If there will be ano HoTs triggering at the start of the turn then the code will be later applied here
                    if (statusEffect.statAffection == StatAffection.Health)
                    {
                        shouldBlink = true;
                    }
                }
                // it this is a passive effect that should proc at the start of every turn
                else if (statusEffect.portionValue > 0 && statusEffect.type == StatusEffectType.Passive && statusEffect.duration == 0)
                {
                    if (statusEffect.statAffection == StatAffection.Health)
                    {
                        if (HitPoints < MaxHitPoints)
                        {
                            HitPoints *= (statusEffect.portionValue / 100) + 1;

                            if (InfoPanel.activeSelf)
                                gameObject.transform.Find("InfoPanel/Health/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.White;
                        }
                    }
                }

                if (statusEffect.currentDuration == 1)
                {
                    effectsToRemove.Add(statusEffect);

                    if (statusEffect.statAffection == StatAffection.Armor)
                    {
                        Armor -= BuffArmor;
                        BuffArmor = 0;

                        if (InfoPanel.activeSelf)
                            gameObject.transform.Find("InfoPanel/Armor/Value/CurrentValue").GetComponent<TextMeshProUGUI>().color = UIColors.White;
                    }
                }
                else
                {
                    LowerStatusEffectDuration(statusEffect);
                }
            }
        }

        if (effectsToRemove.Count > 0)
        {
            RemovePositiveEffect(effectsToRemove);
        }

        // Only blink a combatant that has a HoT applied
        if (shouldBlink)
        {
            BlinkCombatant();
            await UniTask.WaitForSeconds(1);
            StopBlinkingCombatant();
        }
    }

    private async UniTask CheckNegativeEffects(FightManager fightManager)
    {
        IList<StatusEffect> effectsToRemove = new List<StatusEffect>();

        bool shouldBlink = false;

        if (NegativeAppliedEffects.Count > 0)
        {
            foreach (var statusEffect in NegativeAppliedEffects)
            {
                // apply DoT damage here
                if (statusEffect.portionValue > 0 && statusEffect.type == StatusEffectType.Damage)
                {
                    shouldBlink = true;
                    var DoTdamage = ApplyDamageOverTimeReduction(statusEffect);

                    TranslationManager translationManager = GameObject.Find(Constants.TranslationManager).GetComponent<TranslationManager>();
                    var jsonCombatMessage = translationManager.Translate("DoTMessage");
                    var updatedMessage = jsonCombatMessage
                            .Replace("{activeCharName}", GetCombatantName())
                            .Replace("{statusEffectName}", statusEffect.name)
                            .Replace("{damage}", DoTdamage.ToString());

                    fightManager.CreateCombatMessage(updatedMessage);
                }

                if (statusEffect.currentDuration == 1)
                {
                    effectsToRemove.Add(statusEffect);
                }
                else
                {
                    LowerStatusEffectDuration(statusEffect);
                }
            }
        }

        if (effectsToRemove.Count > 0)
        {
            foreach (var effect in effectsToRemove)
            {
                NegativeAppliedEffects.Remove(effect);

                if (effect.type == StatusEffectType.Debuff)
                {
                    RemoveDebuff(effect);
                }

                // if character has less than 5 effects, we have to enable the Level HUD

                // as we remove the effect from the combatant we also have to remove it visually from the object
                var statusEffectsLocation = gameObject.transform.Find("StatusEffects").transform;
                if (statusEffectsLocation != null)
                {
                    foreach (Transform child in statusEffectsLocation)
                    {
                        if (Guid.TryParse(child.gameObject.transform.Find("GUID").GetChild(0).name, out Guid guid))
                        {
                            if (guid == effect.guid)
                            {
                                Destroy(child.gameObject);
                            }
                        }
                    }
                }
            }
        }

        // Only blink a combatant that has a DoT applied with a real damage reduction
        if (shouldBlink)
        {
            BlinkCombatant();
            await UniTask.WaitForSeconds(1);
            StopBlinkingCombatant();
        }
    }

    /// <summary>
    /// Removes the particular debuff from the target combatant and returns the affected stat back to 
    /// its original portion. This only applies for debuff effects and those that expire only after
    /// desired turns or are dispelled.
    /// </summary>
    /// <param name="statusEffect"></param>
    private void RemoveDebuff(StatusEffect statusEffect)
    {
        if (statusEffect.statAffection == StatAffection.HitChance)
        {
            HitChance += DebuffHitChance;
            DebuffHitChance = 0;

            if (InfoPanel.activeSelf)
                ReturnStatToOriginal("InfoPanel/Row1/HitChance/Value", HitChance, BuffHitChance, DebuffHitChance);
        }
        else if (statusEffect.statAffection == StatAffection.CounterChance)
        {
            CounterChance += DebuffCounterChance;
            DebuffCounterChance = 0;

            if (InfoPanel.activeSelf)
                ReturnStatToOriginal("InfoPanel/Row3/CounterChance/Value", HitChance, BuffHitChance, DebuffHitChance);
        }
        else if (statusEffect.statAffection == StatAffection.AttackSpeed)
        {
            AttackSpeed += DebuffAttackSpeed;
            DebuffAttackSpeed = 0;

            if (InfoPanel.activeSelf)
                ReturnStatToOriginalFloat("InfoPanel/Row1/AttackSpeed/Value", AttackSpeed, BuffAttackSpeed, DebuffAttackSpeed);
        }
    }

    /// <summary>
    /// Removes the particular buff from the target combatant and returns the affected stat back
    /// to its original portion. This only applies to buff effects that add fixed unchanged value
    /// and expire after fixed amount of turns or are dispelled.
    /// </summary>
    /// <param name="statusEffect"></param>
    private void RemoveBuff(StatusEffect statusEffect)
    {
        if (statusEffect.statAffection == StatAffection.HitChance)
        {
            HitChance -= BuffHitChance;
            BuffHitChance = 0;

            if (InfoPanel.activeSelf)
                ReturnStatToOriginal("InfoPanel/Row1/HitChance/Value", HitChance, BuffHitChance, DebuffHitChance);
        }
        else if (statusEffect.statAffection == StatAffection.CounterChance)
        {
            CounterChance -= BuffCounterChance;
            BuffCounterChance = 0;

            if (InfoPanel.activeSelf)
                ReturnStatToOriginal("InfoPanel/Row3/CounterChance/Value", HitChance, BuffHitChance, DebuffHitChance);
        }
        else if (statusEffect.statAffection == StatAffection.AttackSpeed)
        {
            AttackSpeed -= BuffAttackSpeed;
            BuffAttackSpeed = 0;

            if (InfoPanel.activeSelf)
                ReturnStatToOriginalFloat("InfoPanel/Row1/AttackSpeed/Value", AttackSpeed, BuffAttackSpeed, DebuffAttackSpeed);
        }
    }

    private void ReturnStatToOriginal(string path, int stat, int buffStat, int debuffStat)
    {
        gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().text = HitChance.ToString();

        if (buffStat > 0)
        {
            gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
        }
        else if (debuffStat > 0)
        {
            gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }
        else
        {
            gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().color = UIColors.White;
        }
    }

    private void ReturnStatToOriginalFloat(string path, float stat, float buffStat, float debuffStat)
    {
        gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().text = HitChance.ToString();

        if (buffStat > 0)
        {
            gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
        }
        else if (debuffStat > 0)
        {
            gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }
        else
        {
            gameObject.transform.Find(path).GetComponent<TextMeshProUGUI>().color = UIColors.White;
        }
    }

    private void RemovePositiveEffect(IList<StatusEffect> effectsToRemove)
    {
        if (effectsToRemove.Count > 0)
        {
            foreach (var effect in effectsToRemove)
            {
                PositiveAppliedEffects.Remove(effect);

                if (effect.type == StatusEffectType.Buff)
                {
                    RemoveBuff(effect);
                }

                // as we remove the effect from the combatant we also have to remove it visually from the object
                var statusEffectsLocation = gameObject.transform.Find("StatusEffects").transform;
                if (statusEffectsLocation != null)
                {
                    foreach (Transform child in statusEffectsLocation)
                    {
                        if (Guid.TryParse(child.gameObject.transform.Find("GUID").GetChild(0).name, out Guid guid))
                        {
                            if (guid == effect.guid)
                            {
                                Destroy(child.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    private void InjectMainData(int level, string name, float attackSpeed, int hitPoints, int armor, int shieldPoints, int meleeAttack, int meleePhysicalDamage, int meleeFireDamage, int meleeColdDamage,
        int meleePoisonDamage, int meleeEnergyDamage, int rangedAttack, int rangedPhysicalDamage, int rangedFireDamage, int rangedColdDamage, int rangedPoisonDamage, int rangedEnergyDamage, int psiDamage,
        int physicalProtection, int fireProtection, int coldProtection, int poisonProtection, int energyProtection, int psiProtection, int hitChance, int criticalChance, int criticalDamage, int dodge,
        int resistance, int counterChance, int penetration)
    {
        Level = level;
        Name = name;
        AttackSpeed = attackSpeed;
        HitPoints = hitPoints;
        MaxHitPoints = hitPoints;
        Armor = armor;
        MaxArmor = armor;
        ShieldPoints = shieldPoints;
        MaxShieldPoints = shieldPoints;
        MeleeAttack = meleeAttack;
        RangedAttack = rangedAttack;
        Dodge = dodge;
        Resistance = resistance;
        CounterChance = counterChance;
        Penetration = penetration;
        HitChance = hitChance;
        CriticalChance = criticalChance;
        CriticalDamage = criticalDamage;
        PsiDamage = psiDamage;
        MeleePhysicalDamage = meleePhysicalDamage;
        MeleeFireDamage = meleeFireDamage;
        MeleeColdDamage = meleeColdDamage;
        MeleePoisonDamage = meleePoisonDamage;
        MeleeEnergyDamage = meleeEnergyDamage;
        RangedPhysicalDamage = rangedPhysicalDamage;
        RangedFireDamage = rangedFireDamage;
        RangedColdDamage = rangedColdDamage;
        RangedPoisonDamage = rangedPoisonDamage;
        RangedEnergyDamage = rangedEnergyDamage;
        PhysicalProtection = physicalProtection;
        FireProtection = fireProtection;
        ColdProtection = coldProtection;
        PoisonProtection = poisonProtection;
        EnergyProtection = energyProtection;
        PsiProtection = psiProtection;

        LifeBar.fillAmount = 1f;

        gameObject.transform.Find("VisualPanel/Level/LevelValue").GetComponent<TextMeshProUGUI>().text = level.ToString();
        gameObject.transform.Find("VisualPanel/Name").GetComponent<TextMeshProUGUI>().text = name;

        if (InfoPanel.activeSelf)
        {
            FillInfoPanelStats(attackSpeed, hitPoints, armor, shieldPoints, meleeAttack, rangedAttack, psiDamage, hitChance, dodge, resistance, counterChance, penetration);
        }
        else
        {
            if (armor > 0)
            {
                ArmorBar.fillAmount = 1f;
            }
            else
            {
                ArmorBar.fillAmount = 0f;
            }

            if (shieldPoints > 0)
            {
                ShieldBar.fillAmount = 1f;
            }
            else
            {
                ShieldBar.fillAmount = 0f;
            }
        }
    }

    public void FillInfoPanelStats(float attackSpeed, int hitPoints, int armor, int shieldPoints, int meleeAttack, int rangedAttack, int psiDamage, int hitChance,
        int dodge, int resistance, int counterChance, int penetration)
    {
        if (InfoPanel.activeSelf)
        {
            gameObject.transform.Find("InfoPanel/Row1/AttackSpeed/Value").GetComponent<TextMeshProUGUI>().text = attackSpeed.ToString("F1");
            gameObject.transform.Find("InfoPanel/Health/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = hitPoints.ToString();
            gameObject.transform.Find("InfoPanel/Health/Value/MaxValue").GetComponent<TextMeshProUGUI>().text = hitPoints.ToString();

            if (armor > 0)
            {
                gameObject.transform.Find("InfoPanel/Armor/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = armor.ToString();
                gameObject.transform.Find("InfoPanel/Armor/Value/MaxValue").GetComponent<TextMeshProUGUI>().text = armor.ToString();
                ArmorBar.fillAmount = 1f;
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Armor").gameObject.SetActive(false);
                ArmorBar.fillAmount = 0f;
            }

            if (shieldPoints > 0)
            {
                gameObject.transform.Find("InfoPanel/Shield/Value/CurrentValue").GetComponent<TextMeshProUGUI>().text = shieldPoints.ToString();
                gameObject.transform.Find("InfoPanel/Shield/Value/MaxValue").GetComponent<TextMeshProUGUI>().text = shieldPoints.ToString();
                ShieldBar.fillAmount = 1f;
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Shield").gameObject.SetActive(false);
                ShieldBar.fillAmount = 0f;
            }

            if (meleeAttack > 0)
            {
                gameObject.transform.Find("InfoPanel/Row0/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().text = meleeAttack.ToString();
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Row0/MeleeAttack").gameObject.SetActive(false);
            }

            if (rangedAttack > 0)
            {
                gameObject.transform.Find("InfoPanel/Row0/RangedAttack/Value").GetComponent<TextMeshProUGUI>().text = rangedAttack.ToString();
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Row0/RangedAttack").gameObject.SetActive(false);
            }

            if (psiDamage > 0)
            {
                gameObject.transform.Find("InfoPanel/PsiAttack/Value").GetComponent<TextMeshProUGUI>().text = psiDamage.ToString();
            }
            else
            {
                gameObject.transform.Find("InfoPanel/PsiAttack").gameObject.SetActive(false);
            }

            if (hitChance > 0)
            {
                gameObject.transform.Find("InfoPanel/Row1/HitChance/Value").GetComponent<TextMeshProUGUI>().text = hitChance.ToString() + "%";
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Row1/HitChance").gameObject.SetActive(false);
            }

            if (dodge > 0)
            {
                gameObject.transform.Find("InfoPanel/Row2/Dodge/Value").GetComponent<TextMeshProUGUI>().text = dodge.ToString() + "%";
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Row2/Dodge").gameObject.SetActive(false);
            }

            if (resistance > 0)
            {
                gameObject.transform.Find("InfoPanel/Row2/Resistance/Value").GetComponent<TextMeshProUGUI>().text = resistance.ToString();
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Row2/Resistance").gameObject.SetActive(false);
            }

            if (counterChance > 0)
            {
                gameObject.transform.Find("InfoPanel/Row3/CounterChance/Value").GetComponent<TextMeshProUGUI>().text = counterChance.ToString() + "%";
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Row3/CounterChance").gameObject.SetActive(false);
            }

            if (penetration > 0)
            {
                gameObject.transform.Find("InfoPanel/Row3/Penetration/Value").GetComponent<TextMeshProUGUI>().text = penetration.ToString();
            }
            else
            {
                gameObject.transform.Find("InfoPanel/Row3/Penetration").gameObject.SetActive(false);
            }
        }
    }

    private void LowerStatusEffectDuration(StatusEffect statusEffect)
    {
        statusEffect.currentDuration--;

        // as we change the duration we also have to projects the changes in the visualized effects in hierarchy
        var statusEffectsLocation = gameObject.transform.Find("StatusEffects").transform;
        if (statusEffectsLocation != null)
        {
            foreach (Transform child in statusEffectsLocation)
            {
                if (Guid.TryParse(child.gameObject.transform.Find("GUID").GetChild(0).name, out Guid guid))
                {
                    if (guid == statusEffect.guid)
                    {
                        child.transform.Find("Duration").GetComponent<TextMeshProUGUI>().text = statusEffect.currentDuration.ToString();
                    }
                }
            }
        }
    }
}
