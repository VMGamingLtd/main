using Cysharp.Threading.Tasks;
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
    private int HitPoints;
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
    private CancellationTokenSource cts = null;
    private CancellationToken cancellationToken;

    public void CreateCombatant(BattleFormation battleFormation, Faction faction, Race race, int position, string name, int level)
    {
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
                InjectMainData(level, name, Player.AttackSpeed, Player.HitPoints, Player.Armor, Player.ShieldPoints, Player.MeleeAttack, Player.RangedAttack,
                               Player.PsiDamage, Player.HitChance, Player.Dodge, Player.Resistance, Player.CounterChance, Player.Penetration);

                CombatAbilities = Player.CombatAbilities;
            }
            else
            {

            }
        }
        else
        {
            InjectMainData(level, name, 1f, 16, 16, 0, 8, 0, 0, 75, 2, 0, 2, 0);

            gameObject.transform.Find("CharacterRow/FirstRow/Icon").GetComponent<Image>().sprite = AssetBundleManager.AssignCombatSpriteToSlot("Skull");
            gameObject.transform.Find("Level/LevelValue").GetComponent<TextMeshProUGUI>().text = Level.ToString();
            gameObject.transform.Find("Level/Image").GetComponent<Image>().color = Color.red;
            gameObject.transform.Find("CharacterRow/Border").GetComponent<Image>().color = Color.red;
            gameObject.transform.Find("CharacterRow/Glow").GetComponent<Image>().color = UIColors.invisibleCol;
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

        if (GameObject.Find("FIGHTMANAGER").TryGetComponent<FightManager>(out var fightManager))
        {
            fightManager.StopTimebars();
            fightManager.ActiveCombatant = gameObject;

            if (Faction == Faction.Player)
            {
                fightManager.ActionPanel.SetActive(true);
                fightManager.FillCombatAbilities(CombatAbilities);
            }
            else if (Faction == Faction.Enemy)
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

                if (gameObject.TryGetComponent<CombatantFunctions>(out var combatantFunctions))
                {
                    combatantFunctions.AttackAllyTarget(false);
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

    private void InjectMainData(int level, string name, float attackSpeed, int hitPoints, int armor, int shieldPoints, int meleeAttack, int rangedAttack,
                                int psiDamage, int hitChance, int dodge, int resistance, int counterChance, int penetration)
    {
        Level = level;
        Name = name;
        AttackSpeed = attackSpeed;
        HitPoints = hitPoints;
        Armor = armor;
        ShieldPoints = shieldPoints;
        MeleeAttack = meleeAttack;
        RangedAttack = rangedAttack;
        Dodge = dodge;
        Resistance = resistance;
        CounterChance = counterChance;
        Penetration = penetration;

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
}
