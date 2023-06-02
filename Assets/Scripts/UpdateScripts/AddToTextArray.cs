using UnityEngine;
using TMPro;

public class AddToTextArray : MonoBehaviour
{
    public GameObject coroutineManager;
    public string targetVariableName;
    private TextMeshProUGUI myText;

    private void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
        CoroutineManager manager = coroutineManager.GetComponent<CoroutineManager>();
        System.Reflection.FieldInfo targetVariable = manager.GetType().GetField(targetVariableName);
        if (GlobalCalculator.GameStarted == true)
        {
            TextMeshProUGUI[] array = (TextMeshProUGUI[])targetVariable.GetValue(manager);
            TextMeshProUGUI[] newArray = AddTextToArray(array, myText);
            targetVariable.SetValue(manager, newArray);
        }
    }

    private TextMeshProUGUI[] AddTextToArray(TextMeshProUGUI[] array, TextMeshProUGUI text)
    {
        TextMeshProUGUI[] newArray = new TextMeshProUGUI[array.Length + 1];
        array.CopyTo(newArray, 0);
        newArray[newArray.Length - 1] = text;
        return newArray;
    }
}
