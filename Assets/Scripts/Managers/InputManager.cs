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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed");
                GameManager.Instance.UnloadLevel("Village");
                GameManager.Instance.LoadLevel("MiniJeu3");
                UIManager.Instance.UpdateMenuState(UIManager.MenuState.ThirdGameMine);
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.ThirdGameMine);
            }
        }
        //Pour Le jeux Usine Assemblage 
        if (Input.GetMouseButtonDown(0))
        {
            UsineAssemblageGameManager.Instance.UserClicLeftDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            UsineAssemblageGameManager.Instance.UserClicLeftUp();
        }
    }
}
