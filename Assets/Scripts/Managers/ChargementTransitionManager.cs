using UnityEngine;
using System;
using System.Collections;
using static GameProgressManager;

public class ChargementTransitionManager : Singleton<ChargementTransitionManager>
{
    public static event Action OnLoadPage;
    public static event Action OnUnloadPage;
    public static event Action OnResetJoystick;


    public GameProgressState gameProgressState;

    public static void InvokeOnLoadPage()
    {
        OnLoadPage?.Invoke();
    }
    public void StopGame()
    {
        OnLoadPage?.Invoke();
    }

    public void StartGame()
    {
        OnUnloadPage?.Invoke();
        /*Debug.Log("Fin Transition");*/
        GameProgressManager.Instance.UpdateGameProgressState(gameProgressState);
    }

    public IEnumerator LoadScene(GameProgressState gameProgressState, string currentScene, string sceneNameToGo, bool playerInNextScene,float x = 0, float y = 0)
    {
        UIManager.Instance.UpdateMenuState(UIManager.MenuState.Loading);
        /*Debug.Log(gameProgressState);*/
        ChargementTransitionManager.Instance.gameProgressState = gameProgressState;

        //Si on est en portable, on reset le joystick
        if (PlatformManager.Instance.fctisMobile())
        {
            OnResetJoystick?.Invoke();
        }

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
