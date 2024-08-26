using Cysharp.Threading.Tasks;
using ItemManagement;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Enumerations;

public class BattleCharacter : MonoBehaviour
{
    [SerializeField] Image Timebar;
    [SerializeField] GameObject Selection1;
    [SerializeField] GameObject Selection2;
    [SerializeField] GameObject Selection3;
    [SerializeField] GameObject Selection4;
    private BattleFormation BattleFormation;
    private Faction Faction;
    private Race Race;
    private int Position;
    private string Name = string.Empty;
    private int Level;
    private float AttackSpeed;
    private int HitChance;
    private int CriticalChance;
    private int CriticalDamage;
    private int Penetration;
    private int CounterChance;
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
    private int ShieldPoints;
    private int Armor;
    private int MaxArmor;
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
    private GameObject LevelHUD;
    private CancellationTokenSource cts = null;
    private CancellationToken cancellationToken;

    public void CreateCombatant(BattleFormation battleFormation, Faction faction, Race race, int position, string name, int level, BestiaryDataJson creatureData = null)
    {
        LevelHUD = gameObject.transform.Find("Level").gameObject;
        BattleFormation = battleFormation;
        Faction = faction;
        Race = race;
        Position = position;
        Name = name;
        Level = level;

        if (faction == Faction.Player || faction == Faction.Ally)
        {
            if (faction == Faction.Player)
            {
                InjectMainData(level, name, Player.AttackSpeed, Player.HitPoints, Player.Armor, Player.ShieldPoints, Player.MeleeAttack, Player.MeleePhysicalDamage, Player.MeleeFireDamage, Player.MeleeColdDamage,
                    Player.MeleePoisonDamage, Player.MeleeEnergyDamage, Player.RangedAttack, Player.RangedPhysicalDamage, Player.RangedFireDamage, Player.RangedColdDamage, Player.RangedPoisonDamage, Player.RangedEnergyDamage,
                    Player.PsiDamage, Player.PhysicalProtection, Player.FireProtection, Player.ColdProtection, Player.PoisonProtection, Player.EnergyProtection, Player.PsiProtection, Player.HitChance, Player.CriticalDamage,
                    Player.CriticalDamage, Player.Dodge, Player.Resistance, Player.CounterChance, Player.Penetration);

                CombatAbilities = Player.CombatAbilities;
            }
            else
            {

            }
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

                gameObject.transform.Find("CharacterRow/FirstRow/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot("Skull");
                gameObject.transform.Find("Level/LevelValue").GetComponent<TextMeshProUGUI>().text = Level.ToString();
                gameObject.transform.Find("Level/Image").GetComponent<Image>().color = Color.red;
                gameObject.transform.Find("CharacterRow/Border").GetComponent<Image>().color = Color.red;
                gameObject.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.invisibleCol;
            }

        }
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

                if (MeleeAttack > 0)
                {
                    gameObject.transform.Find("CharacterRow/ThirdRow/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedMeleeAttack.ToString();
                    gameObject.transform.Find("CharacterRow/ThirdRow/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
                }

                if (RangedAttack > 0)
                {
                    gameObject.transform.Find("CharacterRow/ThirdRow/RangedAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedRangedAttack.ToString();
                    gameObject.transform.Find("CharacterRow/ThirdRow/RangedAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
                }
            }
            else if (statusEffect.statAffection == StatAffection.Armor)
            {
                int debuffedArmor = MaxArmor * ((statusEffect.portionValue / 100) + 1);
                Armor -= debuffedArmor;
            }
        }
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

                if (MeleeAttack > 0)
                {
                    gameObject.transform.Find("CharacterRow/ThirdRow/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedMeleeAttack.ToString();
                    gameObject.transform.Find("CharacterRow/ThirdRow/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                }

                if (RangedAttack > 0)
                {
                    gameObject.transform.Find("CharacterRow/ThirdRow/RangedAttack/Value").GetComponent<TextMeshProUGUI>().text = deductedRangedAttack.ToString();
                    gameObject.transform.Find("CharacterRow/ThirdRow/RangedAttack/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
                }
            }
            else if (statusEffect.statAffection == StatAffection.Armor)
            {
                float buffedArmor = MaxArmor * ((float)statusEffect.portionValue / (float)100);
                BuffArmor = (int)Math.Round(buffedArmor);
                Armor += BuffArmor;

                gameObject.transform.Find("CharacterRow/SecondRow/Armor/Value").GetComponent<TextMeshProUGUI>().text = Armor.ToString();
                gameObject.transform.Find("CharacterRow/SecondRow/Armor/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonGreenFull;
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

        gameObject.transform.Find("CharacterRow/SecondRow/Shield/Value").GetComponent<TextMeshProUGUI>().text = ShieldPoints.ToString();
        gameObject.transform.Find("CharacterRow/SecondRow/Armor/Value").GetComponent<TextMeshProUGUI>().text = Armor.ToString();
        gameObject.transform.Find("CharacterRow/SecondRow/Health/Value").GetComponent<TextMeshProUGUI>().text = HitPoints.ToString();

        if (ShieldPoints == 0)
        {
            gameObject.transform.Find("CharacterRow/SecondRow/Shield/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }

        if (Armor == 0)
        {
            gameObject.transform.Find("CharacterRow/SecondRow/Armor/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }

        if (HitPoints == 0)
        {
            IsDead = true;
            Timebar.fillAmount = 0f;
            gameObject.transform.Find("CharacterRow/SecondRow/Health/Value").GetComponent<TextMeshProUGUI>().color = UIColors.NeonRedFull;
        }
    }

    public void BlinkCombatant()
    {
        gameObject.transform.Find("CharacterRow/Blink").GetComponent<Animation>().Play("BlinkTarget");
    }

    public void StopBlinkingCombatant()
    {
        gameObject.transform.Find("CharacterRow/Blink").GetComponent<Animation>().Stop("BlinkTarget");
        gameObject.transform.Find("CharacterRow/Blink").GetComponent<Image>().color = UIColors.YellowInvisible;
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

    public void DisplayLevelHUD(bool state)
    {
        LevelHUD.SetActive(state);
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

                var randomAbility = CombatAbilities[abilityIndex];
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
                        gameObject.transform.Find("CharacterRow/SecondRow/Armor/Value").GetComponent<TextMeshProUGUI>().color = UIColors.White;
                    }
                }

                RemovePositiveEffect(effectsToRemove);
            }

            Armor += BuffArmor;
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

                if (statusEffect.currentDuration == 1)
                {
                    effectsToRemove.Add(statusEffect);

                    if (statusEffect.statAffection == StatAffection.Armor)
                    {
                        Armor -= BuffArmor;
                        BuffArmor = 0;
                        gameObject.transform.Find("CharacterRow/SecondRow/Armor/Value").GetComponent<TextMeshProUGUI>().color = UIColors.White;
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

                // if character has less than 5 effects, we have to enable the Level HUD

                if (CheckStatusEffectCount() < 5)
                {
                    DisplayLevelHUD(true);
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

        // Only blink a combatant that has a DoT applied with a real damage reduction
        if (shouldBlink)
        {
            BlinkCombatant();
            await UniTask.WaitForSeconds(1);
            StopBlinkingCombatant();
        }
    }

    private void RemovePositiveEffect(IList<StatusEffect> effectsToRemove)
    {
        if (effectsToRemove.Count > 0)
        {
            foreach (var effect in effectsToRemove)
            {
                PositiveAppliedEffects.Remove(effect);

                // if character has less than 5 effects, we have to enable the Level HUD
                if (CheckStatusEffectCount() < 5)
                {
                    DisplayLevelHUD(true);
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

        gameObject.transform.Find("Level/LevelValue").GetComponent<TextMeshProUGUI>().text = level.ToString();
        gameObject.transform.Find("CharacterRow/FirstRow/Character").GetComponent<TextMeshProUGUI>().text = name;
        gameObject.transform.Find("CharacterRow/FirstRow/AttackSpeedValue").GetComponent<TextMeshProUGUI>().text = attackSpeed.ToString("F1");
        gameObject.transform.Find("CharacterRow/SecondRow/Health/Value").GetComponent<TextMeshProUGUI>().text = hitPoints.ToString();

        if (armor > 0)
        {
            gameObject.transform.Find("CharacterRow/SecondRow/Armor/Value").GetComponent<TextMeshProUGUI>().text = armor.ToString();
        }
        else
        {
            gameObject.transform.Find("CharacterRow/SecondRow/Armor").gameObject.SetActive(false);
        }

        if (shieldPoints > 0)
        {
            gameObject.transform.Find("CharacterRow/SecondRow/Shield/Value").GetComponent<TextMeshProUGUI>().text = shieldPoints.ToString();
        }
        else
        {
            gameObject.transform.Find("CharacterRow/SecondRow/Shield").gameObject.SetActive(false);
        }

        if (meleeAttack > 0)
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/MeleeAttack/Value").GetComponent<TextMeshProUGUI>().text = meleeAttack.ToString();
        }
        else
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/MeleeAttack").gameObject.SetActive(false);
        }

        if (rangedAttack > 0)
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/RangedAttack/Value").GetComponent<TextMeshProUGUI>().text = rangedAttack.ToString();
        }
        else
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/RangedAttack").gameObject.SetActive(false);
        }

        if (psiDamage > 0)
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/PsiAttack/Value").GetComponent<TextMeshProUGUI>().text = psiDamage.ToString();
        }
        else
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/PsiAttack").gameObject.SetActive(false);
        }

        if (hitChance > 0)
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/HitChance/Value").GetComponent<TextMeshProUGUI>().text = hitChance.ToString() + "%";
        }
        else
        {
            gameObject.transform.Find("CharacterRow/ThirdRow/HitChance").gameObject.SetActive(false);
        }

        if (dodge > 0)
        {
            gameObject.transform.Find("CharacterRow/FourthRow/Dodge/Value").GetComponent<TextMeshProUGUI>().text = dodge.ToString() + "%";
        }
        else
        {
            gameObject.transform.Find("CharacterRow/FourthRow/Dodge").gameObject.SetActive(false);
        }

        if (resistance > 0)
        {
            gameObject.transform.Find("CharacterRow/FourthRow/Resistance/Value").GetComponent<TextMeshProUGUI>().text = resistance.ToString();
        }
        else
        {
            gameObject.transform.Find("CharacterRow/FourthRow/Resistance").gameObject.SetActive(false);
        }

        if (counterChance > 0)
        {
            gameObject.transform.Find("CharacterRow/FourthRow/CounterChance/Value").GetComponent<TextMeshProUGUI>().text = counterChance.ToString() + "%";
        }
        else
        {
            gameObject.transform.Find("CharacterRow/FourthRow/CounterChance").gameObject.SetActive(false);
        }

        if (penetration > 0)
        {
            gameObject.transform.Find("CharacterRow/FourthRow/Penetration/Value").GetComponent<TextMeshProUGUI>().text = penetration.ToString();
        }
        else
        {
            gameObject.transform.Find("CharacterRow/FourthRow/Penetration").gameObject.SetActive(false);
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
