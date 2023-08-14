using UnityEngine;
public static class PlayerResources
{
    public static float PlayerOxygen = 0f;
    public static float PlayerWater = 0f;
    public static float PlayerEnergy = 0f;
    public static float PlayerHunger = 0f;

    public static void InitializeStartingConsumation()
    {
        PlayerWater = 0.1f;
        PlayerHunger = 0.1f;
    }

}
