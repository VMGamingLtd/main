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
            FindObjectsWithScript(scriptName);
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
}
