using UnityEngine;

public class DiscoveryManager
{
    private Planet Planet;
    public void CreateRandomCave()
    {
        Planet = GameObject.Find("PlanetParent/StartPlanet").GetComponent<Planet>();

        var sizeChance = Random.Range(0f, 100f);
        var eventSize = EventSize.Small;

        if (sizeChance < 10)
        {
            eventSize = EventSize.Large;
        }
        else if (sizeChance < 30)
        {
            eventSize = EventSize.Medium;
        }

        var typeChance = Random.Range(0f, 100f);
        var eventType = EventIconType.VolcanicCave;

        if (typeChance < 25)
        {
            eventType = EventIconType.IceCave;
        }
        else if (typeChance < 50)
        {
            eventType = EventIconType.HiveNest;
        }
        else if (typeChance < 75)
        {
            eventType = EventIconType.CyberHideout;
        }

        var dungeonLevel = Player.Level;
        var difficultyChance = Random.Range(0f, 100f);

        if (difficultyChance < 10)
        {
            dungeonLevel++;
        }
        else if (difficultyChance < 30)
        {
            if (dungeonLevel > 2)
            {
                dungeonLevel -= 2;
            }
            else if (dungeonLevel > 1)
            {
                dungeonLevel -= 1;
            }
        }
        else if (difficultyChance < 50)
        {
            if (dungeonLevel > 1)
            {
                dungeonLevel -= 1;
            }
        }

        float x = 0f;
        float y = 0f;
        float z = 0f;

        var randomLocation = Random.Range(0, 7);
        if (randomLocation == 0)
        {
            x = 0.1f;
            y = -0.1f;
        }
        else if (randomLocation == 1)
        {
            x = -0.1f;
            y = -0.1f;
        }
        else if (randomLocation == 2)
        {
            x = 0.1f;
            y = 0.1f;
        }
        else if (randomLocation == 3)
        {
            x = -0.1f;
            y = 0.1f;
        }
        else if (randomLocation == 4)
        {
            x = 0.2f;
            y = -0.2f;
        }
        else if (randomLocation == 5)
        {
            x = -0.2f;
            y = -0.2f;
        }
        else if (randomLocation == 6)
        {
            x = 0.2f;
            y = 0.2f;
        }
        else if (randomLocation == 7)
        {
            x = -0.2f;
            y = 0.2f;
        }

        Planet.CreateDungeonObject(eventSize, eventType, dungeonLevel, x, y, z);
    }
}
