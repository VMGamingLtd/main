using UnityEngine;

public class Enumerations : MonoBehaviour
{
    public enum BattleFormation
    {
        Front,
        Back
    }

    public enum BattleGroupType
    {
        Player,
        Enemy
    }

    public enum Faction
    {
        Player,
        Ally,
        Enemy
    }

    public enum Race
    {
        Humanoid = 0,
        Beast = 1,
        Insect = 2,
        Reptilian = 3,
        Mechanical = 4,
        Ethereal = 5,
        Mutant = 6,
        Amorphous = 7
    }

    public enum ClassType
    {
        Warrior = 0,
        Rogue = 1,
        Ranger = 2,
        Mage = 3,
        Warlock = 4
    }

    public enum EnemyGroupSize
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six
    }

    public enum EventIconType
    {
        Player = 0,
        FriendlyPlayer = 1,
        EnemyPlayer = 2,
        Fish = 3,
        Plant = 4,
        Mineral = 5,
        FurAnimal = 6,
        MilkAnimal = 7,
        Animal = 8,
        VolcanicCave = 9,
        IceCave = 10,
        HiveNest = 11,
        CyberHideout = 12,
        AlienBase = 13,
        WormTunnels = 14,
        Shipwreck = 15,
        MysticTemple = 16,
        XenoSpider = 17,
        SporeBehemoth = 18,
        ElectroBeast = 19,
        VoidReaper = 20
    }

    public enum EventSize
    {
        Small,
        Medium,
        Large,
    }

    public enum StatusEffectType
    {
        Damage = 0,
        Stun = 1,
        Sleep = 2,
        Buff = 3,
        Debuff = 4,
        Passive = 5
    }

    public enum AbilityTrigger
    {
        None = 0,
        Health25 = 1,
        Health50 = 2,
        Health75 = 3
    }

    public enum StatAffection
    {
        None = 0,
        Attack = 1,
        AttackSpeed = 2,
        Protection = 3,
        Health = 4,
        Armor = 5,
        Shield = 6,
        HitChance = 7,
        Dodge = 8,
        CounterChance = 9
    }

    public enum PrefabSpawn
    {
        User,
        Target,
        BattlefieldCenter
    }

    public enum PrefabRotation
    {
        None = 0,
        ToEnemy = 1,
        Up = 2,
        Down = 3,
        Left = 4,
        Right = 5
    }

    public enum PrefabStart
    {
        MovePhase = 0,
        DamagePhase = 1,
        ReturnPhase = 2
    }

    public enum PrefabMovement
    {
        None = 0,
        ToEnemy = 1
    }
}
