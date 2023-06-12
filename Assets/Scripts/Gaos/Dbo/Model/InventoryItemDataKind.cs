#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    enum InventoryItemDataKindEnum
    {
        BasicInventoryObjects,
        ProcessedInventoryObjects,
        RefinedInventoryObjects,
        AssembledInventoryObjects,
    };

    [System.Serializable]
    public class InventoryItemDataKind
    {
        public int Id;
        public string? Name;
    }
}
