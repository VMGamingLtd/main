using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using static Enumerations;

[Serializable]
public class AbilityPrefab
{
    public string prefabName;
    [JsonConverter(typeof(StringEnumConverter))]
    public PrefabRotation prefabRotation;
    [JsonConverter(typeof(StringEnumConverter))]
    public PrefabStart prefabStart;
    [JsonConverter(typeof(StringEnumConverter))]
    public PrefabMovement prefabMovement;
}
public class CombatAbility
{
    public int Index { get; private set; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public string Weapon { get; private set; }
    public bool IsMovingAbility { get; private set; }
    public bool IsFrontlineAoe { get; private set; }
    public bool IsBacklineAoe { get; private set; }
    public float MeleeDamageScale { get; private set; }
    public float RangedDamageScale { get; private set; }
    public float PsiDamageScale { get; private set; }
    public float ScaleMultiplication { get; private set; }
    public float AbilityLowerDamage { get; set; }
    public float AbilityHigherDamage { get; set; }
    public int Cooldown { get; private set; }
    public int RemainingCooldown { get; private set; }
    public List<StatusEffect> PositiveStatusEffects { get; private set; } = new List<StatusEffect>();
    public List<StatusEffect> NegativeStatusEffects { get; private set; } = new List<StatusEffect>();
    public List<AbilityPrefab> AbilityPrefabs { get; private set; } = new List<AbilityPrefab>();

    public CombatAbility(
        int index, string name, string type, string weapon, bool isFrontlineAoe, bool isBacklineAoe, float meleeDamageScale,
        float rangedDamageScale, float psiDamageScale, float scaleMultiplication, int cooldown, bool isMovingAbility,
        List<StatusEffect> positiveStatusEffects, List<StatusEffect> negativeStatusEffects, List<AbilityPrefab> abilityPrefabs)
    {
        Index = index;
        Name = name;
        Type = type;
        Weapon = weapon;

        IsMovingAbility = isMovingAbility;
        IsFrontlineAoe = isFrontlineAoe;
        IsBacklineAoe = isBacklineAoe;
        MeleeDamageScale = meleeDamageScale;
        RangedDamageScale = rangedDamageScale;
        PsiDamageScale = psiDamageScale;
        ScaleMultiplication = scaleMultiplication;
        Cooldown = cooldown;
        PositiveStatusEffects = positiveStatusEffects;
        NegativeStatusEffects = negativeStatusEffects;
        AbilityPrefabs = abilityPrefabs;

        if (meleeDamageScale > 0)
        {
            AbilityLowerDamage = Player.MeleeAttack * meleeDamageScale;
            AbilityHigherDamage = Player.MeleeAttack * scaleMultiplication;
        }
        else if (rangedDamageScale > 0)
        {
            AbilityLowerDamage = Player.RangedAttack * rangedDamageScale;
            AbilityHigherDamage = Player.RangedAttack * scaleMultiplication;
        }
        else if (psiDamageScale > 0)
        {
            AbilityLowerDamage = Player.PsiDamage * psiDamageScale;
            AbilityHigherDamage = Player.PsiDamage * scaleMultiplication;
        }
    }

    public bool IsAbilityOnCooldown()
    {
        if (RemainingCooldown > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAbilityOnCooldown()
    {
        RemainingCooldown = Cooldown;
    }

    public void ReduceAbilityCooldown()
    {
        if (RemainingCooldown > 0)
        {
            RemainingCooldown--;
        }
    }
}
