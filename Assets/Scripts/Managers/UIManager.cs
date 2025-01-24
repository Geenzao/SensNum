using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static GameManager;


/*! \brief Class managing the UI
 *
 * This manager manages the UI. It will:
 *
 * - Contain the UI related events
 * - Manage the UI Camera (only active when no other camera are enabled, in PREGAME)
 *  
 */
public class UIManager : Singleton<UIManager>
{
    ////Events
    //public Events.UI.EventFadeMMenuComplete OnMainMenuFadeComplete = new Events.UI.EventFadeMMenuComplete();
    public Events.UI.MenuStateChanged OnMenuStateChanged = new Events.UI.MenuStateChanged();

    public enum MenuState
    {
        None,
        MainMenu,
        PauseMenu,
        OptionMenu,
        ThirdGameMine,
        SecondGameMine,
        AssemblyGame,
        CandyCrush,
        Loading
    }

    //\brief Currently displayed menu
    private static MenuState _currentMenuState;
    //private static VirtualKeyboard _virtualKeyboard;

    [SerializeField] private Camera _UICamera;

    //Le game manager commence � charger un level
    //Logo de chargement s'affiche

    //A la fin du chargement, le logo passe dans un �tat "fin de chargement"
    //Un son de fin de chargement est jou�
    //Le logo disparait

    //En m�me temps, � la fin du chargement, le menu principal fade out
    //A la fin du fade out, le GameState doit passer a RUNNING.
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        GameManager.Instance.OnLoadingEnded.AddListener(HandleLoadingEnded);
    }
    private void HandleGameStateChanged(GameState currentState, GameState previousState)
    {
        if (currentState == GameState.PREGAME && previousState == GameState.RUNNING)
        {
            _UICamera.gameObject.SetActive(false);
            //UpdateMenuState(MenuState.MainMenu);
        }
        else if (currentState == GameState.PAUSED)
        {
            UpdateMenuState(MenuState.PauseMenu);
        }
        else
        {

            if (CurrentMenuState != MenuState.Loading && 
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.Start ||
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.Mine ||
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.Village2 ||
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.AssemblyZone ||
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.Village3 ||
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.Recycling ||
                GameProgressManager.CurrentGameProgressState == GameProgressManager.GameProgressState.Village4)
                UpdateMenuState(MenuState.None);
        }
    }

    private void HandleLoadingEnded(string lvlName)
    {
        if (CurrentGameState == GameState.PREGAME)
        {
            _UICamera.gameObject.SetActive(false);
        }
    }

    public void UpdateMenuState(MenuState newMS)
    {
        MenuState previousMenuState = new MenuState();
        previousMenuState = _currentMenuState;

        //switch (newMS)
        //{
        //    case MenuState.None:
        //        break;
        //    case MenuState.MainMenu:
        //        break;
        //    case MenuState.Options:
        //        break;
        //}

        _currentMenuState = newMS;

        OnMenuStateChanged.Invoke(_currentMenuState, previousMenuState);
        Debug.LogWarning("Menu state changed to " + _currentMenuState);
    }

    //public static VirtualKeyboard VirtualKeyboard
    //{
    //    get
    //    {
    //        if (_virtualKeyboard == null)
    //            return _virtualKeyboard = new VirtualKeyboard();
    //        else
    //            return _virtualKeyboard;
    //    }
    //}

    private List<GameObject> _listPopUps = new List<GameObject>();

    //!\brief Adds a popup to the UI
    //The popup prefab should have a PopUp component,
    //a rect transform and a canvas renderer.
    //public GameObject AddPopup(GameObject popUpPrefab)
    //{
    //    //a tester
    //    GameObject instance = Instantiate(popUpPrefab, canvas.transform);

    //    if (!instance.GetComponent<PopUp>()
    //        || !instance.GetComponent<RectTransform>()
    //        || !instance.GetComponent<CanvasRenderer>())
    //    {
    //        DestroyImmediate(popUpPrefab);
    //        return null;
    //    }

    //    _listPopUps.Add(popUpPrefab);
    //    return instance;
    //}

    //public void RemovePopup(GameObject popUp)
    //{
    //    _listPopUps.Remove(popUp);
    //    Destroy(popUp);
    //}

    public static MenuState CurrentMenuState
    {
        get => _currentMenuState;
    }

    public Canvas canvas
    {
        get => GetComponentInChildren<Canvas>() == null ? null : GetComponentInChildren<Canvas>();
    }
}
