using TMPro;
using UnityEngine;

public class AddToTextArrayAdaptive : MonoBehaviour
{
    private CoroutineManager manager;
    private TextMeshProUGUI myText;

    public void AssignTextToCoroutineManagerArray()
    {
        myText = GetComponent<TextMeshProUGUI>();
        string thisName = gameObject.name + "Texts";
        manager = GameObject.Find("CoroutineManager").GetComponent<CoroutineManager>();
        System.Reflection.FieldInfo targetVariable = manager.GetType().GetField(thisName);

        if (targetVariable != null)
        {
            TextMeshProUGUI[] array = (TextMeshProUGUI[])targetVariable.GetValue(manager);

            if (array != null)
            {
                TextMeshProUGUI[] newArray = AddTextToArray(array, myText);
                targetVariable.SetValue(manager, newArray);
            }
            else
            {
                Debug.LogError($"Array for {thisName} was found but is null");
            }
        }
        else
        {
            Debug.LogError($"Field {thisName} was not found in CoroutineManager");
        }
    }

    private TextMeshProUGUI[] AddTextToArray(TextMeshProUGUI[] array, TextMeshProUGUI text)
    {
        TextMeshProUGUI[] newArray = new TextMeshProUGUI[array.Length + 1];
        array.CopyTo(newArray, 0);
        newArray[^1] = text;
        return newArray;
    }
}