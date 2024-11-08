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
        if (GameManager.CurrentGameState == GameState.RUNNING)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (UIManager.CurrentMenuState == UIManager.MenuState.None)
                    UIManager.Instance.UpdateMenuState(UIManager.MenuState.OptionMenu);
                else
                    UIManager.Instance.UpdateMenuState(UIManager.MenuState.None);
            }
        }
    }
}
