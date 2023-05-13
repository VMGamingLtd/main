using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using System.Text;

public class GuestLogin : MonoBehaviour
{
    public CoroutineManager coroutineManager;
    public GlobalCalculator globalCalculator;
    public GameObject myObject;

    void OnEnable()
    {
        bool gameStarted = globalCalculator.GameStarted;

        if (gameStarted == false)
        {
            myObject.SetActive(true);
        }
        else 
        { 
            myObject.SetActive(false);
        }

        
    }

    public void OnButtonClick()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] shuffledChars = chars.OrderBy(c => Random.value).ToArray();

        UserName.userName = new string(Enumerable.Range(0, 10)
          .Select(i => shuffledChars[Random.Range(0, shuffledChars.Length)]).ToArray());

        StartCoroutine(coroutineManager.LoadSaveSlots());

        /*Guid uuid = Guid.NewGuid();
        Debug.Log("Generated UUID: " + uuid.ToString());*/
    }
}