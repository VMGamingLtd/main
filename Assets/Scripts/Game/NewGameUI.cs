using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameUI : MonoBehaviour
{
    public GameObject newGameUI;
    public GameObject saveSlots;

    public void startNewGameUI()
    {
        newGameUI.SetActive(true);
        saveSlots.SetActive(false);
    }
}
