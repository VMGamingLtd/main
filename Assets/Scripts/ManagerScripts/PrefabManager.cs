using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] List<GameObject> AbilityPrefabs;

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
