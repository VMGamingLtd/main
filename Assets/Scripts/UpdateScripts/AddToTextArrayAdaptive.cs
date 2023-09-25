using UnityEngine;
using TMPro;

public class AddToTextArrayAdaptive : MonoBehaviour
{
    private CoroutineManager manager;
    private TextMeshProUGUI myText;

    public void AssignTextToCoroutineManagerArray()
    {
        Debug.Log("starting function");
        myText = GetComponent<TextMeshProUGUI>();
        string thisName = gameObject.name + "Texts";
        Debug.Log(thisName);
        manager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        System.Reflection.FieldInfo targetVariable = manager.GetType().GetField(thisName);
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