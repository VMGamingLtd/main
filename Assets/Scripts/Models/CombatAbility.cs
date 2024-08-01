using System.Collections.Generic;

public class CombatAbility
{
    public int Index { get; private set; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public string Weapon { get; private set; }
    public bool IsEnemyAbility { get; private set; }
    public bool IsFrontlineAoe { get; private set; }
    public bool IsBacklineAoe { get; private set; }
    public float MeleeDamageScale { get; private set; }
    public float RangedDamageScale { get; private set; }
    public float PsiDamageScale { get; private set; }
    public float ScaleMultiplication { get; private set; }
    public float AbilityLowerDamage { get; set; }
    public float AbilityHigherDamage { get; set; }
    public int Cooldown { get; private set; }
    public List<StatusEffect> PositiveStatusEffects { get; private set; } = new List<StatusEffect>();
    public List<StatusEffect> NegativeStatusEffects { get; private set; } = new List<StatusEffect>();

    public CombatAbility(
        int index, string name, string type, string weapon, bool isFrontlineAoe, bool isBacklineAoe, float meleeDamageScale,
        float rangedDamageScale, float psiDamageScale, float scaleMultiplication, int cooldown, bool isEnemyAbility,
        List<StatusEffect> positiveStatusEffects, List<StatusEffect> negativeStatusEffects)
    {
        Index = index;
        Name = name;
        Type = type;
        Weapon = weapon;

        IsEnemyAbility = isEnemyAbility;
        IsFrontlineAoe = isFrontlineAoe;
        IsBacklineAoe = isBacklineAoe;
        MeleeDamageScale = meleeDamageScale;
        RangedDamageScale = rangedDamageScale;
        PsiDamageScale = psiDamageScale;
        ScaleMultiplication = scaleMultiplication;
        Cooldown = cooldown;
        PositiveStatusEffects = positiveStatusEffects;
        NegativeStatusEffects = negativeStatusEffects;

        if (meleeDamageScale > 0)
        {
            AbilityLowerDamage = Player.MeleeAttack * meleeDamageScale;
            AbilityHigherDamage = Player.MeleeAttack * scaleMultiplication;
        }
        else if (rangedDamageScale > 0)
        {
            AbilityLowerDamage = Player.RangedAttack * meleeDamageScale;
            AbilityHigherDamage = Player.RangedAttack * scaleMultiplication;
        }
    }
}
