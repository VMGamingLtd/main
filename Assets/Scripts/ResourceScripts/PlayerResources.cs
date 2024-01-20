public static class PlayerResources
{
    public static float PlayerOxygen;
    public static float PlayerWater;
    public static float PlayerEnergy;
    public static float PlayerHunger;
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
