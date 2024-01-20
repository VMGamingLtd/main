public static class Achievements
{
    public static int AchievementPoints;

    public static void AddPoints(ref int amount)
    {
        AchievementPoints += amount;
    }

    public static int GetPoints()
    {
        return AchievementPoints;
    }

    public static void ResetPoints()
    {
        AchievementPoints = 0;
    }
}
