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
        MineEnd,
        Factory,
        Residence,
        ThirdGameMine,
        SecondGameMine,
        AssemblyGame,
        CandyCrush,
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

        switch(newGameProgressState)
        {
            case GameProgressState.ThirdGameMine:
                UIManager.Instance.UpdateMenuState(MenuState.ThirdGameMine);
                break;
            case GameProgressState.AssemblyGame:
                UIManager.Instance.UpdateMenuState(MenuState.AssemblyGame);
                break;
            case GameProgressState.SecondGameMine:
                UIManager.Instance.UpdateMenuState(MenuState.SecondGameMine);
                break;
            case GameProgressState.CandyCrush:
                UIManager.Instance.UpdateMenuState(MenuState.CandyCrush);
                break;
            default:
                UIManager.Instance.UpdateMenuState(MenuState.None);
                break;
        }

        Debug.LogWarning("Game progress state changed to " + _currentGameProgressState);
    }

    public static GameProgressState CurrentGameProgressState
    {
        get => _currentGameProgressState;
    }
}

