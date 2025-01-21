using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameEndSingletons : MonoBehaviour
{
    public static GameEndSingletons Instance { get; private set; }
    bool endGame = false;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    public void EndGame(bool PoliceWon)
    {
        endGame = true;
        Debug.Log("game end");
    }
}
