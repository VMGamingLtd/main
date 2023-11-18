public static class Level
{
    public static int PlayerLevel = 1;
    public static int PlayerCurrentExp = 0;
    public static int PlayerMaxExp = 40;
    public static int SkillPoints = 0;
    public static int StatPoints = 0;
    public static float PlayerMovementSpeed = 0.2f;

    public static int AddCurrentResource(ref int currentResourceSet, int amount)
    {
        currentResourceSet += amount;
        return currentResourceSet;
    }

    public static int ReduceCurrentResource(ref int currentResourceSet, int amount)
    {
        currentResourceSet -= amount;
        if (currentResourceSet < 0)
        {
            currentResourceSet = 0;
        }
        return currentResourceSet;
    }

    public static int GetCurrentResource(ref int currentResourceSet)
    {
        return currentResourceSet;
    }

    public static int ResetCurrentResource(ref int currentResourceSet)
    {
        currentResourceSet = 0;
        return currentResourceSet;
    }
}
