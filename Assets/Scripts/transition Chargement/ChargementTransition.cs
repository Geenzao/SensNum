using System;
using UnityEngine;

public class ChargementTransition : Singleton<ChargementTransition>
{
    public Animator animator;
    public GameObject LoadGreenRoue;

    public static event Action OnLoadPage;
    public static event Action OnUnloadPage;

    public GameProgressManager.GameProgressState gameProgressState;

    public static void InvokeOnLoadPage()
    {
        OnLoadPage?.Invoke();
    }

    //pour des test 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadChargement();
        }
    }

    public void LoadChargement()
    {
        animator.SetTrigger("LoadPage");
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
