using TMPro.Examples;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GameManager;

public class InputManager : Singleton<InputManager>
{

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentGameState != GameState.PREGAME)
        { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.CurrentGameState == GameState.RUNNING)
                {
                    GameManager.Instance.UpdateGameState(GameState.PAUSED);
                }
                else if (GameManager.CurrentGameState == GameState.PAUSED)
                {
                    GameManager.Instance.UpdateGameState(GameState.RUNNING);
                }
            }
        }
    }
}
