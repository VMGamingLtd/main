using UnityEngine;


public class GuestLogin : MonoBehaviour
{
    public CoroutineManager coroutineManager;
    public GlobalCalculator globalCalculator;
    public GameObject myObject;

    void OnEnable()
    {
        bool gameStarted = GlobalCalculator.GameStarted;

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
        StartCoroutine(coroutineManager.LoadSaveSlots());
    }

}