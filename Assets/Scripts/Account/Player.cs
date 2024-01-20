public static class Player
{
    // BASE STATS
    public static int PlayerLevel = 1;
    public static int PlayerCurrentExp;
    public static int PlayerMaxExp = 40;
    public static int SkillPoints;
    public static int StatPoints;

    // SKILLS
    public static int Biology;
    public static int Engineering;
    public static int Chemistry;
    public static int Physics;
    public static int Computing;
    public static int Psychology;

    // ATTACK STATS
    public static int PhysicalDamage;
    public static int FireDamage;
    public static int ColdDamage;
    public static int GasDamage;
    public static int ExplosionDamage;

    // PROTECTION STATS
    public static int ShieldPoints;
    public static int HitPoints;
    public static int PhysicalProtection;
    public static int FireProtection;
    public static int ColdProtection;
    public static int GasProtection;
    public static int ExplosionProtection;

    // GENERAL STATS
    public static int Strength;
    public static int Perception;
    public static int Intelligence;
    public static int Agility;
    public static int Charisma;
    public static int Willpower;

    public static int StatPointsInStrength;
    public static int StatPointsInPerception;
    public static int StatPointsInIntelligence;
    public static int StatPointsInAgility;
    public static int StatPointsInCharisma;
    public static int StatPointsInWillpower;


    // UTILITY STATS
    public static int EnergyCapacity;
    public static int InventorySlots = 10;
    public static int ExplorationRadius;
    public static int VisibilityRadius;
    public static int PickupRadius;
    public static float ProductionSpeed;// players starts with 1 from the first item equipped.
    public static float MaterialCost; // players starts with 1 from the first item equipped.
    public static float OutcomeRate; // players starts with 1 from the first item equipped.
    public static int UsedInventorySlots;

    // MODE VARIABLES
    public static bool CanProduce; // represents if player has Fabricator tool equipped

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
