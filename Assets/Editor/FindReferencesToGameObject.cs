using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindReferencesToGameObject: EditorWindow
{
    private GameObject target;

    [MenuItem("Tools/Find References to GameObject")]
    public static void ShowWindow()
    {
        GetWindow<InspectGameObject>("Find References");
    }

    void OnGUI()
    {
        GUILayout.Label("Find References to GameObject", EditorStyles.boldLabel);
        target = (GameObject)EditorGUILayout.ObjectField("Target GameObject", target, typeof(GameObject), true);

        if (GUILayout.Button("Find References"))
        {
            FindReferences();
        }
    }

    void FindReferences()
    {
        if (target == null)
        {
            Debug.LogWarning("No target GameObject specified.");
            return;
        }

        List<string> references = new List<string>();

        // Find all MonoBehaviour scripts in the scene
        MonoBehaviour[] allScripts = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            SerializedObject so = new SerializedObject(script);
            SerializedProperty sp = so.GetIterator();

            //while (sp.NextVisible(true))
            while (sp.Next(true))
            {
                if (sp.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (sp.objectReferenceValue == target)
                    {
                        references.Add(script.GetType().Name + " on " + script.gameObject.name);
                    }
                }
            }
        }

        // Print results
        if (references.Count > 0)
        {
            Debug.Log("References to " + target.name + " found in:");
            foreach (string reference in references)
            {
                Debug.Log(reference);
            }
        }
        else
        {
            Debug.Log("No references to " + target.name + " found.");
        }
    }
}
