
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using static Enumerations;

[Serializable]
public class StatusEffect
{
    public Guid guid;
    public string name;
    public int chance;
    public int portionValue;
    public int damageValue; // Main damage value created from the main ability from which the portion value derives
    public int duration;
    public int currentDuration;
    public bool isFrontLineAoe;
    public bool isBackLineAoe;
    [JsonConverter(typeof(StringEnumConverter))]
    public StatusEffectType type;
    [JsonConverter(typeof(StringEnumConverter))]
    public StatAffection statAffection;

    public StatusEffect(string Name, StatusEffectType Type, StatAffection StatAffection, int Chance, int PortionValue, int DamageValue,
        int Duration, bool IsFrontLineAoe, bool IsBackLineAoe)
    {
        guid = Guid.NewGuid();
        name = Name;
        type = Type;
        statAffection = StatAffection;
        chance = Chance;
        portionValue = PortionValue;
        damageValue = DamageValue;
        duration = Duration;
        currentDuration = Duration;
        isFrontLineAoe = IsFrontLineAoe;
        isBackLineAoe = IsBackLineAoe;
    }
}
