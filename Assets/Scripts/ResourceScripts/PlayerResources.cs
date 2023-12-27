public static class PlayerResources
{
    public static float PlayerOxygen = 0f;
    public static float PlayerWater = 0f;
    public static float PlayerEnergy = 0f;
    public static float PlayerHunger = 0f;
    public static float PlayerMovementSpeed = 20f;
    public static float PlayerTotalDistance;
    public static float PlayerRemainingDistance;
    public static float PlayerPixelDistance;
    public static float PlayerDistancePerLinePoint;
    public static float PlayerRemainingTravelTime;
    public static float PlayerCurrentTravelProgress;

    public static bool PlayerMovement;

    public static void InitializeStartingConsumation()
    {
        PlayerWater = 0.1f;
        PlayerHunger = 0.1f;
    }

}
