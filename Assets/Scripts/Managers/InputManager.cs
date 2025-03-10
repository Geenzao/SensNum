using TMPro.Examples;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GameManager;
using UnityEngine.InputSystem;
using System.Collections;
using System;


public class InputManager : Singleton<InputManager>
{
    public event Action OnUserActionDialogue;
    public static event Action OnPathStateChanged;
    private UsineAssemblageUI UsineAssemblageUI;

    private bool isMobilePlatform = false;

    private float moveX;
    private float moveY;

    void Start()
    {
        UsineAssemblageUI = GameObject.Find("AssemblyGameUI").GetComponent<UsineAssemblageUI>();

        if (PlatformManager.Instance != null)
            isMobilePlatform = PlatformManager.Instance.fctisMobile();
        else
            Debug.LogWarning("Le PlatformManager n'est pas instancié");
    }

    public static void invokeOnPathStateChanged()
    {
        OnPathStateChanged?.Invoke();
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
            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    Debug.Log("F pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.Mine);
            //    OnPathStateChanged?.Invoke();
            //}
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    Debug.Log("G pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.Village2);
            //}
            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    Debug.Log("H pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.AssemblyFactory);
            //}
            //if (Input.GetKeyDown(KeyCode.J))
            //{
            //    Debug.Log("J pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.Village3);
            //}
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    Debug.Log("K pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.RecycleFactory);
            //}
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    Debug.Log("L pressed");
            //    PathManager.Instance.UpdatePathState(PathManager.PathState.Village3bis);
            //}

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

            //pour le candy crush
            if(Input.GetMouseButtonDown(0) &&
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.CandyCrush &&
                UIManager.CurrentMenuState == UIManager.MenuState.CandyCrush)
            {
                PuceBoard.Instance.UserClicLeftDown();
            }
            if (Input.GetMouseButtonUp(0) &&
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.CandyCrush &&
                UIManager.CurrentMenuState == UIManager.MenuState.CandyCrush)
            {
                PuceBoard.Instance.UserClicLeftUp();
            }
            /*********************************/
            //Ajout version Portable

            if (isMobilePlatform == false && UIManager.CurrentMenuState == UIManager.MenuState.None)
            {
                //pour les mouvement du player
                moveX = Input.GetAxisRaw("Horizontal");
                moveY = Input.GetAxisRaw("Vertical");
                playerMovement.Instance.setMove(moveX, moveY);

                //pour les dialogue
                if (dialogueManager.Instance.fctisDialogueActive() == false && Input.GetKeyDown(KeyCode.E))
                    OnUserActionDialogue?.Invoke();
            }
            if(isMobilePlatform == false && UIManager.CurrentMenuState == UIManager.MenuState.Dialogue)
            {
                //pour passer au dialogue suivant
                if (dialogueManager.Instance.fctisDialogueActive()
                    && Input.GetKeyDown(KeyCode.Space)
                    && dialogueManager.Instance.isAbleToNextSentence == true)
                {
                    dialogueManager.Instance.DisplayNextSentence();
                }

                
            }
            else
            {
            }


        }
    }
}
