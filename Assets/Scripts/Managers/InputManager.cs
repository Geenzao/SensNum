using TMPro.Examples;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GameManager;

public class InputManager : Singleton<InputManager>
{
    private UsineAssemblageUI UsineAssemblageUI;

    void Start()
    {
        UsineAssemblageUI = GameObject.Find("AssemblyGameUI").GetComponent<UsineAssemblageUI>();
    }

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
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("G pressed");
                GameManager.Instance.UnloadAndSavePosition("Village", 20, 10);
                GameManager.Instance.LoadLevel("Mine3emeJeux");
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.ThirdGameMine);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("G pressed");
                GameManager.Instance.UnloadLevel("Mine3emeJeux");
                GameManager.Instance.LoadLevelAndPositionPlayer("Village");
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.Start);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F pressed");
                GameManager.Instance.UnloadLevel("Village");
                GameManager.Instance.LoadLevel("AssemblageJeux");
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.AssemblyGame);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("F pressed");
                GameManager.Instance.UnloadLevel("Village");
                GameManager.Instance.LoadLevel("Mine2emeJeux");
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.SecondGameMine);
            }
            if (Input.GetMouseButtonDown(0) && 
                UsineAssemblageUI.State == UsineAssemblageState.rule && 
                UIManager.CurrentMenuState == UIManager.MenuState.AssemblyGame && 
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.AssemblyGame)
            {
                UsineAssemblageUI.RunGame();
            }

            if (Input.GetMouseButtonDown(0) &&
                UsineAssemblageUI.State == UsineAssemblageState.game &&
                UIManager.CurrentMenuState == UIManager.MenuState.AssemblyGame &&
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.AssemblyGame)
            {
                //print("Clic Down");
                UsineAssemblageGameManager.Instance.UserClicLeftDown();
            }
            if (Input.GetMouseButtonUp(0) &&
                UsineAssemblageUI.State == UsineAssemblageState.game &&
                UIManager.CurrentMenuState == UIManager.MenuState.AssemblyGame &&
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.AssemblyGame)
            {
                //print("Clic Up");
                UsineAssemblageGameManager.Instance.UserClicLeftUp();
            }
        }
    }
}
