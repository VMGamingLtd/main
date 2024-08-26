using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public List<GameObject> AbilityPrefabs;

    public GameObject GetAbilityPrefab(string name)
    {
        foreach (var abilityPrefab in AbilityPrefabs)
        {
            if (abilityPrefab.name == name)
            {
                return abilityPrefab;
            }
        }

        return null;
    }
}
