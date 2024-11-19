using UnityEngine;
using static GameManager;
using static UIManager;

public class GameProgressManager : Singleton<GameProgressManager>
{
    public Events.GameProgress.GameProgressStateChange OnGameProgressStateChange = new Events.GameProgress.GameProgressStateChange();

    public enum GameProgressState
    {
        None,
        Menu,
        Start,
        Mine,
        Factory,
        Residence,
        ThirdGameMine,
        AssemblyGame,
        End
    }
    
    private static GameProgressState _currentGameProgressState;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameState currentState, GameState previousState)
    {
        //if (currentState == GameState.PREGAME)
        //{
        //    UpdateGameProgressState(GameProgressState.None);
        //}
        //else if (previousState == GameState.PREGAME && currentState == GameState.RUNNING)
        //{
        //    UpdateGameProgressState(GameProgressState.Menu);
        //}
    }

    public void UpdateGameProgressState(GameProgressState newGameProgressState)
    {
        GameProgressState oldGameProgressState = _currentGameProgressState;
        _currentGameProgressState = newGameProgressState;
        OnGameProgressStateChange.Invoke(newGameProgressState, oldGameProgressState);

        if (newGameProgressState == GameProgressState.ThirdGameMine)
        {
            UIManager.Instance.UpdateMenuState(MenuState.ThirdGameMine);
        }
        else if (newGameProgressState == GameProgressState.AssemblyGame)
        {
            UIManager.Instance.UpdateMenuState(MenuState.AssemblyGame);
        }
        else if (newGameProgressState == GameProgressState.Start)
        {
            UIManager.Instance.UpdateMenuState(MenuState.None);
        }

        Debug.LogWarning("Game progress state changed to " + _currentGameProgressState);
    }

    public static GameProgressState CurrentGameProgressState
    {
        get => _currentGameProgressState;
    }
}

