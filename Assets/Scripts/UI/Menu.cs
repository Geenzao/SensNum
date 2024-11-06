using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : DisplayableUIElement
{
    protected virtual void Start()
    {
        //_animator = GetComponent<Animator>();
        //if (!_animator)
        //{
        //    Debug.LogError("No Animator attached. This will cause issues with the start menu.");
        //}
        //_animShouldAppear = _animator.GetBool("ShouldAppear");
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        UIManager.Instance.OnMenuStateChanged.AddListener(HandleMenuStateChanged);
    }

    protected virtual void HandleGameStateChanged(GameManager.GameState newGS, GameManager.GameState oldGS) { }
    protected virtual void HandleMenuStateChanged(UIManager.MenuState newMS, UIManager.MenuState oldMS)
    {
        if (newMS == UIManager.MenuState.None)
            TriggerVisibility(false);
    }

}
