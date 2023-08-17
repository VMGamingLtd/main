#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.GameDataJson
{
    [System.Serializable]
    public class UserGameDataGetResponse
    {
        public bool? IsError;
        public string? ErrorMessage;

        public Gaos.Dbo.Model.GameData? GameData;


        public Gaos.Dbo.Model.InventoryItemData[]? BasicInventoryObjects;
        public Gaos.Dbo.Model.InventoryItemData[]? ProcessedInventoryObjects;
        public Gaos.Dbo.Model.InventoryItemData[]? EnhancedInventoryObjects;
        public Gaos.Dbo.Model.InventoryItemData[]? AssembledInventoryObjects;

        public Gaos.Dbo.Model.RecipeData[]? BasicRecipeObjects;
        public Gaos.Dbo.Model.RecipeData[]? ProcessedRecipeObjects;
        public Gaos.Dbo.Model.RecipeData[]? EnhancedRecipeObjects;
        public Gaos.Dbo.Model.RecipeData[]? AssembledRecipeObjects;

        public string? GameDataJson;
    }
}
