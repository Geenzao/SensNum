using UnityEngine;
using static GameManager;
using static UIManager;

public class GameProgressManager : Singleton<GameProgressManager>
{
    public Events.GameProgress.EventGameProgress OnGameProgress = new Events.GameProgress.EventGameProgress();

    public enum GameProgressState
    {
        None,
        Start,
        Mine,
        Factory,
        Residence,
        End
    }
    
    private static GameProgressState _currentGameProgressState;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameState currentState, GameState previousState)
    {
        if (currentState == GameState.PREGAME && previousState == GameState.RUNNING)
        {
            UpdateGameProgressState(GameProgressState.Start);
        }
        else
        {
            UpdateGameProgressState(GameProgressState.None);
        }
    }

    public void UpdateGameProgressState(GameProgressState newGameProgressState)
    {
        GameProgressState oldGameProgressState = _currentGameProgressState;
        _currentGameProgressState = newGameProgressState;
        OnGameProgress.Invoke(newGameProgressState, oldGameProgressState);

        Debug.LogWarning("Game progress state changed to " + _currentGameProgressState);
    }

    public static GameProgressState CurrentGameProgressState
    {
        get => _currentGameProgressState;
    }
}

