
using System;

[Serializable]
public class StatusEffect
{
    public string name;
    public int value;
    public int duration;
    public bool isFrontLineAoe;
    public bool isBackLineAoe;

    public StatusEffect(string Name, int Value, int Duration, bool IsFrontLineAoe, bool IsBackLineAoe)
    {
        name = Name;
        value = Value;
        duration = Duration;
        isFrontLineAoe = IsFrontLineAoe;
        isBackLineAoe = IsBackLineAoe;
    }
}
