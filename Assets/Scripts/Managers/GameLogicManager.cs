using Settings.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameLogicManager : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

        GameManager.Instance.LoadLevel("Start", false);
        HandleGameStateChanged(GameManager.GameState.PREGAME, GameManager.GameState.PREGAME);

        //foreach (var evt in InputManager.Instance.PlayerInput.actionEvents)
        //{
        //    if (evt.actionName.Contains("UI/Pause"))
        //    {
        //        evt.AddListener(OnPauseInput);
        //        continue;
        //    }
        //}
    }

    private void OnPauseInput(InputAction.CallbackContext val)
    {
        GameManager.Instance.TogglePause();
    }

    private void HandleGameStateChanged(GameManager.GameState newGS, GameManager.GameState oldGS)
    {
        //should not be a switch in case we use oldGS
        if (newGS == GameManager.GameState.PREGAME)
        {
            UIManager.Instance.UpdateMenuState(UIManager.MenuState.MainMenu);
        }
        else if (newGS == GameManager.GameState.RUNNING)
        {
            UIManager.Instance.UpdateMenuState(UIManager.MenuState.None);
        }
        else if (newGS == GameManager.GameState.PAUSED)
        {
            UIManager.Instance.UpdateMenuState(UIManager.MenuState.OptionsMenu);
        }
        //else if (newGS == GameManager.GameState.FROZEN)
        //{
        //    UIManager.Instance.UpdateMenuState(UIManager.MenuState.Hidden);
        //    InputManager.Instance.DisableInput();
        //}
    }
}
