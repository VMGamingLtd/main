using TMPro;
using UnityEngine;

public class AddToTextArray : MonoBehaviour
{
    private CoroutineManager manager;
    public string targetVariableName;
    private TextMeshProUGUI myText;

    private void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        manager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        System.Reflection.FieldInfo targetVariable = manager.GetType().GetField(targetVariableName);
        TextMeshProUGUI[] array = (TextMeshProUGUI[])targetVariable.GetValue(manager);
        TextMeshProUGUI[] newArray = AddTextToArray(array, myText);
        targetVariable.SetValue(manager, newArray);
    }

    private TextMeshProUGUI[] AddTextToArray(TextMeshProUGUI[] array, TextMeshProUGUI text)
    {
        TextMeshProUGUI[] newArray = new TextMeshProUGUI[array.Length + 1];
        array.CopyTo(newArray, 0);
        newArray[newArray.Length - 1] = text;
        return newArray;
    }
}