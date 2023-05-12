using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNewGame : MonoBehaviour
{
    public GameObject newGameUI;
    public GameObject legal;
    public GameObject title;
    public GameObject loadingBar;
    public GameObject coroutineManager;
    public static bool loadingNewGame;

    public void startNewGame() {
        newGameUI.SetActive(false);
        legal.SetActive(false);
        title.SetActive(false);
        coroutineManager.SetActive(false);
        
        loadingNewGame = true;
        loadingBar.SetActive(true);
        coroutineManager.SetActive(true);
    }
}
