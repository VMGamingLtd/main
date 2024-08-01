using System.Collections.Generic;
using static Enumerations;

public static class Player
{
    // BASE STATS
    public static int Id = 0; // player/host starts always on 0 Id. Visitors starts from 1.
    public static string Name;
    public static int Level = 1;
    public static int CurrentExp;
    public static int MaxExp = 40;
    public static int SkillPoints;
    public static int StatPoints;
    public static float ResearchPoints;

    // BATTLE GROUP STATS
    public static BattleFormation BattleFormation = BattleFormation.Front;
    public static int BattlePosition = 1;

    // SKILLS
    public static int Biology;
    public static int Engineering;
    public static int Chemistry;
    public static int Physics;
    public static int Computing;
    public static int Psychology;

    // ATTACK STATS
    public static float AttackSpeed = 1;
    public static int Penetration;
    public static int CounterChance;
    public static int HitChance;
    public static int MeleeAttack;
    public static int RangedAttack;
    public static int PsiDamage;
    public static int MeleePhysicalDamage;
    public static int MeleeFireDamage;
    public static int MeleeColdDamage;
    public static int MeleePoisonDamage;
    public static int MeleeEnergyDamage;
    public static int RangedPhysicalDamage;
    public static int RangedFireDamage;
    public static int RangedColdDamage;
    public static int RangedPoisonDamage;
    public static int RangedEnergyDamage;

    // DEFENSE STATS
    public static int ShieldPoints;
    public static int Armor;
    public static int HitPoints = 10;
    public static int PhysicalProtection;
    public static int FireProtection;
    public static int ColdProtection;
    public static int PoisonProtection;
    public static int EnergyProtection;
    public static int PsiProtection;
    public static int Resistance;
    public static int Dodge;

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
    public static int InventorySlots = 30;
    public static int ExplorationRadius;
    public static int VisibilityRadius;
    public static int PickupRadius;
    public static int UsedInventorySlots;
    public static int BattleGroupSize = 3;
    public static int ActiveCombatants = 0; // combatants that are already assigned in battle
    public static int PassiveCombatants = 0; // combatants not picked for battle, assigned in the available section
    public static float ProductionSpeed; // players start with 1 from the first item equipped.
    public static float MaterialCost; // players start with 1 from the first item equipped.
    public static float OutcomeRate; // players start with 1 from the first item equipped.

    // MODE VARIABLES
    public static bool CanProduce; // represents if player has Fabricator tool equipped
    public static bool InCombat; // represents if player is currently in battle (will pause all BuildingCycles)

    // DISCOVERY FILTER
    public static bool CavesSwitch;
    public static bool VolcanicCaveSwitch;
    public static bool IceCaveSwitch;
    public static bool HiveNestSwitch;
    public static bool CyberHideoutSwitch;
    public static bool MissionsSwitch;
    public static bool AlienBaseSwitch;
    public static bool WormTunnelsSwitch;
    public static bool ShipwreckSwitch;
    public static bool MysticTempleSwitch;
    public static bool MonstersSwitch;
    public static bool XenoSpiderSwitch;
    public static bool SporeBehemothSwitch;
    public static bool ElectroBeastSwitch;
    public static bool VoidReaperSwitch;
    public static bool ResourcesDiscoverySwitch;
    public static bool PlantsDiscoverySwitch;
    public static bool LiquidsDiscoverySwitch;
    public static bool MineralsDiscoverySwitch;
    public static bool GasDiscoverySwitch;
    public static bool FoamsDiscoverySwitch;
    public static bool MeatDiscoverySwitch;
    public static bool AnomalySwitch;
    public static bool MysteryDevicesSwitch;

    // COLLECTING FILTER
    public static bool ItemsCollectionSwitch;
    public static bool ResourcesCollectionSwitch;

    // PLAYER RESEARCH PROGRESS
    public static bool ScienceProjectsResearch;
    public static bool SteamPowerResearch;
    public static bool BasicAlchemyResearch;
    public static bool ElectricityFundamentalsResearch;
    public static bool BotanyResearch;
    public static bool DataAnalysisResearch;
    public static bool CognitiveEnhancementResearch;
    public static bool PoweredEngineeringResearch;
    public static bool PharmaceuticalsResearch;
    public static bool ExplosivesResearch;

    // Player abilities and status effects
    public static IList<CombatAbility> CombatAbilities = new List<CombatAbility>();
    public static IList<StatusEffect> CombatStatusEffects = new List<StatusEffect>();

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
