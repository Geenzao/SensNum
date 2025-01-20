using UnityEngine;
using System;
using static GameProgressManager;

public class ChargementTransitionManager : Singleton<ChargementTransitionManager>
{
    public static event Action OnLoadPage;
    public static event Action OnUnloadPage;

    public GameProgressState gameProgressState;

    public static void InvokeOnLoadPage()
    {
        OnLoadPage?.Invoke();
    }
    public void StopGame()
    {
        Debug.Log("StopGame");
        //Time.timeScale = 0;
        OnLoadPage?.Invoke();
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        //Time.timeScale = 1;
        OnUnloadPage?.Invoke();
        GameProgressManager.Instance.UpdateGameProgressState(gameProgressState);
    }
}
