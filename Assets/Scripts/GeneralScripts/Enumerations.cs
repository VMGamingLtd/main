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
        Humanoid,
        Beast,
        Insect,
        Reptilian,
        Mechanical,
        Ethereal,
        Mutant
    }
}
