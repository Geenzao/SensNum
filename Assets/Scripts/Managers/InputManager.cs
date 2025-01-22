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
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("F pressed");
                GameManager.Instance.UnloadAndSavePosition("Mine", -39, -10);
                GameManager.Instance.LoadLevel("CinematiqueMine");
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.None);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("G pressed");
                PathManager.Instance.UpdatePathState(PathManager.PathState.Village2);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("H pressed");
                GameManager.Instance.UnloadAndSavePosition("Village", -1.5f, 5.5f);
                GameManager.Instance.LoadLevel("AssemblageJeux");
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.AssemblyGame);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("K pressed");
                GameManager.Instance.UnloadLevel("Village");
                GameManager.Instance.LoadLevel("MiniJeuCandycrush");
                GameProgressManager.Instance.UpdateGameProgressState(GameProgressManager.GameProgressState.CandyCrush);
            }

            //if (Input.GetKeyDown(KeyCode.J))
            //{
            //    Debug.Log("J pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.Mine);
            //}
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    Debug.Log("K pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.AssemblyFactory);
            //}
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    Debug.Log("L pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.RecycleFactory);
            //}

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
