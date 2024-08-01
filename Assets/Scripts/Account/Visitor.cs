using static Enumerations;

public class Visitor
{
    // BASE STATS
    public int Id; // Id for checks of who started the event. Always starts from 1 as 0 is reserved for host.
    public int SaveSlotId; // slot ID of the visitor's game from which he entered the game of the host
    public string Name;
    public int Level;
    public int CurrentExp;
    public int MaxExp;
    public int SkillPoints;
    public int StatPoints;
    public float ResearchPoints;

    // BATTLE GROUP STATS
    public BattleFormation BattleFormation;
    public int BattlePosition;

    // SKILLS
    public int Biology;
    public int Engineering;
    public int Chemistry;
    public int Physics;
    public int Computing;
    public int Psychology;

    // ATTACK STATS
    public float AttackSpeed;
    public int Penetration;
    public int CounterChance;
    public int HitChance;
    public int MeleeAttack;
    public int RangedAttack;
    public int PsiDamage;
    public int MeleePhysicalDamage;
    public int MeleeFireDamage;
    public int MeleeColdDamage;
    public int MeleePoisonDamage;
    public int MeleeEnergyDamage;
    public int RangedPhysicalDamage;
    public int RangedFireDamage;
    public int RangedColdDamage;
    public int RangedPoisonDamage;
    public int RangedEnergyDamage;

    // DEFENSE STATS
    public int ShieldPoints;
    public int Armor;
    public int HitPoints;
    public int PhysicalProtection;
    public int FireProtection;
    public int ColdProtection;
    public int PoisonProtection;
    public int EnergyProtection;
    public int PsiProtection;
    public int Resistance;
    public int Dodge;

    // GENERAL STATS
    public int Strength;
    public int Perception;
    public int Intelligence;
    public int Agility;
    public int Charisma;
    public int Willpower;

    public int StatPointsInStrength;
    public int StatPointsInPerception;
    public int StatPointsInIntelligence;
    public int StatPointsInAgility;
    public int StatPointsInCharisma;
    public int StatPointsInWillpower;

    // UTILITY STATS
    public int EnergyCapacity;
    public int ExplorationRadius;
    public int VisibilityRadius;
    public int PickupRadius;
    public float ProductionSpeed;
    public float MaterialCost;
    public float OutcomeRate;

    // MODE VARIABLES
    public bool CanProduce; // represents if player has Fabricator tool equipped
    public bool InCombat; // represents if player is currently in battle (will pause all BuildingCycles)

    public static int AddCurrentResource(ref int currentResourceSet, int amount)
    {
        currentResourceSet += amount;
        return currentResourceSet;
    }

    public static int ReduceCurrentResource(ref int currentResourceSet, int amount)
    {
        currentResourceSet -= amount;
        if (currentResourceSet < 0)
        {
            currentResourceSet = 0;
        }
        return currentResourceSet;
    }

    public static int GetCurrentResource(ref int currentResourceSet)
    {
        return currentResourceSet;
    }

    public static int ResetCurrentResource(ref int currentResourceSet)
    {
        currentResourceSet = 0;
        return currentResourceSet;
    }
}
