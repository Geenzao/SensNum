using UnityEngine;
using System;
using System.Collections;
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

    public IEnumerator LoadScene(GameProgressState gameProgressState, string currentScene, string sceneNameToGo, bool playerInNextScene,float x = 0, float y = 0)
    {
        UIManager.Instance.UpdateMenuState(UIManager.MenuState.Loading);
        ChargementTransitionManager.Instance.gameProgressState = gameProgressState;
        yield return new WaitForSecondsRealtime(1.2f);
        if(GameObject.FindWithTag("Player") != null)
            GameManager.Instance.UnloadAndSavePosition(currentScene, x, y);
        else
            GameManager.Instance.UnloadLevel(currentScene);
        if(playerInNextScene)
            GameManager.Instance.LoadLevelAndPositionPlayer(sceneNameToGo);
        else
            GameManager.Instance.LoadLevel(sceneNameToGo);
        InvokeOnLoadPage();
    }
}
