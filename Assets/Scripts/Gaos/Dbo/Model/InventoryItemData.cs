#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    [System.Serializable]
    public class InventoryItemData
    {
        public int Id;
        public int? UserSlotId;
        public UserSlot? UserSlot;
        public int? InventoryItemDataKindId;
        public InventoryItemDataKind? InventoryItemDataKind;

        public int? ID;
        public float? stackLimit;
        public string? ItemName;
        public string? ItemType;
        public string? ItemClass;
        public string? ItemProduct;
        public float? ItemQuantity;

    }
}
