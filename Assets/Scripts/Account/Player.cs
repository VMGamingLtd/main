public static class Player
{
    // BASE STATS
    public static int PlayerLevel = 1;
    public static int PlayerCurrentExp = 0;
    public static int PlayerMaxExp = 40;
    public static int SkillPoints = 0;
    public static int StatPoints = 0;

    // SKILLS
    public static int Biology = 0;
    public static int Engineering = 0;
    public static int Chemistry = 0;
    public static int Physics = 0;
    public static int Computing = 0;
    public static int Psychology = 0;

    // ATTACK STATS
    public static int PhysicalDamage = 0;
    public static int FireDamage = 0;
    public static int ColdDamage = 0;
    public static int GasDamage = 0;
    public static int ExplosionDamage = 0;

    // PROTECTION STATS
    public static int ShieldPoints = 0;
    public static int HitPoints = 0;
    public static int PhysicalProtection = 0;
    public static int FireProtection = 0;
    public static int ColdProtection = 0;
    public static int GasProtection = 0;
    public static int ExplosionProtection = 0;

    // GENERAL STATS
    public static int Strength = 0;
    public static int Perception = 0;
    public static int Intelligence = 0;
    public static int Agility = 0;
    public static int Charisma = 0;
    public static int Willpower = 0;

    public static int StatPointsInStrength = 0;
    public static int StatPointsInPerception = 0;
    public static int StatPointsInIntelligence = 0;
    public static int StatPointsInAgility = 0;
    public static int StatPointsInCharisma = 0;
    public static int StatPointsInWillpower = 0;


    // UTILITY STATS
    public static int EnergyCapacity = 0;
    public static int InventorySlots = 10;
    public static int ExplorationRadius = 0;
    public static int VisibilityRadius = 0;
    public static int PickupRadius = 0;
    public static float ProductionSpeed = 0; // players starts with 1 from the first item equipped.
    public static float MaterialCost = 0; // players starts with 1 from the first item equipped.
    public static float OutcomeRate = 0; // players starts with 1 from the first item equipped.
    public static int UsedInventorySlots;

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
