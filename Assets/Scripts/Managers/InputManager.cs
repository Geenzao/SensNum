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
                if (GameManager.CurrentGameState == GameState.RUNNING && UIManager.CurrentMenuState == UIManager.MenuState.None)
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
                PathManager.Instance.UpdatePathState(PathManager.PathState.Mine);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("G pressed");
                PathManager.Instance.UpdatePathState(PathManager.PathState.Village2);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("H pressed");
                PathManager.Instance.UpdatePathState(PathManager.PathState.AssemblyFactory);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("J pressed");
                PathManager.Instance.UpdatePathState(PathManager.PathState.Village3);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("J pressed");
                PathManager.Instance.UpdatePathState(PathManager.PathState.Village3);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("K pressed");
                PathManager.Instance.UpdatePathState(PathManager.PathState.RecycleFactory);
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
