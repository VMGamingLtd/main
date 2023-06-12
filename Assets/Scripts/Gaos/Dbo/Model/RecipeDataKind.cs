#pragma warning disable 8632
namespace Gaos.Dbo.Model
{
    enum RecipeDataKindEnum
    {
        BasicRecipeObjects,
        ProcessedRecipeObjects,
        RefinedRecipeObjects,
        AssembledRecipeObjects,
    };

    [System.Serializable]
    public class RecipeDataKind
    {
        public int Id;
        public string? Name;
    }
}
