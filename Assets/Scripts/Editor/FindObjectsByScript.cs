using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindObjectsByScript : EditorWindow
{
    private string scriptName = "";

    [MenuItem("Tools/Find Objects By Script")]
    public static void ShowWindow()
    {
        GetWindow<FindObjectsByScript>("Find Objects By Script");
    }

    void OnGUI()
    {
        scriptName = EditorGUILayout.TextField("Script Name", scriptName);

        if (GUILayout.Button("Find"))
        {
            //FindObjectsWithScript(scriptName);
            FindObjectsWithScriptIncludingInactive(scriptName);
        }
    }

    private void FindObjectsWithScript(string scriptName)
    {
        List<GameObject> results = new List<GameObject>();
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();

        foreach (var script in allScripts)
        {
            //Debug.Log(script.GetType().Name);
            if (script.GetType().Name == scriptName)
            {
                results.Add(script.gameObject);
            }
        }

        Debug.Log($"Found {results.Count} GameObject(s) with script '{scriptName}'");
        foreach (var go in results)
        {
            Debug.Log(go.name, go);
        }
    }

    private void FindObjectsWithScriptIncludingInactive(string scriptName)
    {
        var allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> results = new List<GameObject>();

        foreach (var go in allGameObjects)
        {
            // Check if the GameObject is part of a scene (to exclude prefabs not currently in the scene)
            if (go.hideFlags == HideFlags.None)
            {
                var scripts = go.GetComponents<MonoBehaviour>();
                foreach (var script in scripts)
                {
                    if (script != null && script.GetType().Name == scriptName)
                    {
                        results.Add(go);
                        break;
                    }
                }
            }
        }

        Debug.Log($"Found {results.Count} GameObject(s) with script '{scriptName}' including inactive ones");
        foreach (var result in results)
        {
            Debug.Log(result.name, result);
        }
    }

}
