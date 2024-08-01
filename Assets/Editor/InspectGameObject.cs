using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class InspectGameObject : EditorWindow
{
    private GameObject target;

    [MenuItem("Tools/Inspect GameObject")]
    public static void ShowWindow()
    {
        GetWindow<InspectGameObject>("Find References");
    }

    void OnGUI()
    {
        GUILayout.Label("Find References to GameObject", EditorStyles.boldLabel);
        target = (GameObject)EditorGUILayout.ObjectField("Target GameObject", target, typeof(GameObject), true);

        if (GUILayout.Button("Inspect Object Properties"))
        {
            InspectObjectProperties();
        }
    }

    void InspectObjectProperties()
    {
        if (target == null)
        {
            Debug.LogWarning("No target GameObject specified.");
            return;
        }

        Component[] components = target.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is MonoBehaviour)
            {
                Debug.Log("Component: " + component.ToString());
                Debug.Log($"------------- properties - start -------------------");
                var so = new SerializedObject(component);
                SerializedProperty sp = so.GetIterator();
                Debug.Log(sp.name);
                while (sp.Next(true))
                {
                    Debug.Log($"property_name: {sp.name}, property_type: {sp.propertyType}, {sp.objectReferenceInstanceIDValue}");
                }
                Debug.Log($"------------- properties - end -------------------");
            }
        }

    }
}
