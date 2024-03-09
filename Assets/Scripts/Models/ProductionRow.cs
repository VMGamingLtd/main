using UnityEngine;

public enum ProductionRowType
{
    Manual,
    Energy,
    NoConsume,
    Consume,
    Research
}

public class ProductionRow : MonoBehaviour
{
    public ProductionRow(string name, ProductionRowType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; set; }

    public ProductionRowType Type { get; set; }
}
