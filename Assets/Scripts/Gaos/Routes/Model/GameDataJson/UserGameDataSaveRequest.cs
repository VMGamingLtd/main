#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataSaveRequest
    {
        public int UserId;
        public int SlotId;
        public Gaos.Dbo.Model.GameData? GameData;

        public Gaos.Dbo.Model.InventoryItemData[]? BasicInventoryObjects;
        public Gaos.Dbo.Model.InventoryItemData[]? ProcessedInventoryObjects;
        public Gaos.Dbo.Model.InventoryItemData[]? RefinedInventoryObjects;
        public Gaos.Dbo.Model.InventoryItemData[]? AssembledInventoryObjects;

        public Gaos.Dbo.Model.RecipeData[]? BasicRecipeObjects;
        public Gaos.Dbo.Model.RecipeData[]? ProcessedRecipeObjects;
        public Gaos.Dbo.Model.RecipeData[]? RefinedRecipeObjects;
        public Gaos.Dbo.Model.RecipeData[]? AssembledRecipeObjects;



    }
}
