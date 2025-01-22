using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//\warning On va avoir un probl�me sur ce script quand on va vouloir faire une animation dans le menu principal
//Il va falloir charger une sc�ne MAIS rester en PREGAME pour ne pas r�veiller le joueur.

/*\brief Main manager
 * 
 * This manager must be in a empty scene to work properly. It will:
 * 
 * - Instanciates/Destroys a list of prefabs (should be other managers)
 * - Manages the main State Machine of a game
 * - Loads/Unloads levels
 * - Manages Unity's timeScale
 * - Reacts to Application events
 * 
 */
public class GameManager : Singleton<GameManager>
{
    //  Events
    public Events.Game.GameStateChanged OnGameStateChanged = new Events.Game.GameStateChanged();
    public Events.Game.LevelLoadingStarted OnLoadingStarted = new Events.Game.LevelLoadingStarted();
    public Events.Game.LevelLoadingEnded OnLoadingEnded = new Events.Game.LevelLoadingEnded();
    public Events.Game.LevelUnloadingStarted OnUnloadingStarted = new Events.Game.LevelUnloadingStarted();
    public Events.Game.LevelUnloadingEnded OnUnloadingEnded = new Events.Game.LevelUnloadingEnded();

    public Events.Game.ErrorLogReceived OnErrorLogReceived = new Events.Game.ErrorLogReceived();

    /*\brief Enumeration of all game states
     * 
     * Contains all event that are related to the game, in general.
     * 
     * \var PREGAME
     * Should be when the game is on the "main menu", without any player.
     * \var RUNNING
     * Should be when the game is running, at least one level is loaded,
     * and the player is also loaded.
     * \var PAUSED
     * Should be when the game time is frozen.
     * \var DIALOG
     * Should be when the player is talking with an NPC.
     * This should also trigger an special dialog UI.
     * \var FROZEN
     * Should be when a cinematic is on.
     */
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        DIALOG, //peut �tre move dans un futur DialogueManager
        FROZEN
    }

    //The first GameState is always PreGame
    private static GameState _gameState = GameState.PREGAME;

    private static string _currentLevelName = string.Empty;
    List<AsyncOperation> _loadOperations;
    List<AsyncOperation> _unloadOperations;

    [SerializeField] private GameObject[] SystemPrefabs;
    private List<GameObject> _instanciedSystemPrefabs;

    [System.Serializable]
    public class PlayerPosition
    {
        public float x;
        public float y;
    }

    [System.Serializable]
    public class SaveData
    {
        public PlayerPosition playerPosition;
    }

    private void Start()
    {
        Application.logMessageReceived += HandleLogMessageReceived;

        _loadOperations = new List<AsyncOperation>();
        _unloadOperations = new List<AsyncOperation>();
        _instanciedSystemPrefabs = new List<GameObject>();
        InstantiateSystemPrefabs();
    }

    private void HandleLogMessageReceived(string logText, string stackTrace, LogType logType)
    {
        if (logType == LogType.Error || logType == LogType.Exception)
        {
            OnErrorLogReceived.Invoke(logText, stackTrace, logType);
            ExitGame();
        }
    }

    /*\brief Instanciates the list of prefabs.
     * 
     * Instanciates the list of prefabs and
     * save the references
     */
    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instanciedSystemPrefabs.Add(prefabInstance);
        }
    }

    //\brief Destroys the list of prefabs.
    protected override void OnDestroy()
    {
        for (int i = 0; i < _instanciedSystemPrefabs.Count; i++)
        {
            Destroy(_instanciedSystemPrefabs[i]);
        }
        _instanciedSystemPrefabs.Clear();
        base.OnDestroy();
    }

    /*\brief Loads a level (asyncrous)
     * 
     * Starts to load a Unity level using the SceneManager.LoadSceneAsync method.
     * Adds this operation to the loading operation list.
     * If there's already a loading level, returns.
     * Invokes the OnLoadingStarted event.
     * \warning You shouldn't try to load more than the managers scene(s) and the current level.
     * \param levelName The scene's name to load. Can log an error if given.
     * \param updateGameState If true, updates the GameState to RUNNING when the loading is finished.
     * string does not correspond to a level
     * 
     */
    public void LoadLevel(string levelName, bool updateGameState = true)
    {
        if (_loadOperations.Count != 0)
        {
            Debug.LogWarning("There's already a loading operation. Please wait for it to finish.");
            return;
        }
        else if (_currentLevelName != string.Empty)
        {
            Debug.LogWarning("There's already a loaded level. Please unload it before loading another one.");
            return;
        }
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("Unable to load level " + levelName + ". Did you check that you've made no mistakes ?");
            return;
        }
        ao.completed += OnLoadOperationComplete;
        //Adds a lambda function to the completed event of the AsyncOperation
        //This lambda function updates the GameState to RUNNING when the loading is finished:
        if (updateGameState) //sera exec dans l'ordre ???
        {
            ao.completed += (AsyncOperation ao) =>
            {
                if (_loadOperations.Count == 0)
                    UpdateGameState(GameState.RUNNING);
            };
        }
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
        OnLoadingStarted.Invoke(_currentLevelName);

        /*\todo: Donc le joueur, quand il sera dans un chargement, ne pourra pas:
         * - Mettre le jeu en pause (ce menu se ferme)
         * - Regarder son inventaire
         * - Bouger, attaquer...
         * 
         * > Si le GameState est PREGAME et qu'un chargement est lanc�, cela fait que:
         * - Une ic�ne de chargement appara�t en bas � droite
         * Quand ce chargement est termin�, le jeu passe en RUNNING. La cam�ra change alors
         * de celle de l'ui -> celle du joueur
         * 
         * > Si le GameState est RUNNING et qu'un changement de niveau est lanc�, cela fait que
         * - Une ic�ne de chargement appara�t en bas � droite
         * Quand ce chargement est termin�, rien ne se passe
         * */

    }

    /*\brief Should be called when an loading level AsyncOperation finished its job.
     * 
     * Deletes the previous operation from the loading operation list
     * If there is no more loading operations, updates the GameState to RUNNING.
     * Invokes the OnLoadingEnded event.
     * \param aso Completed operation, to remove from the list
     * 
     */
    private void OnLoadOperationComplete(AsyncOperation aso)
    {
        _loadOperations.Remove(aso);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLevelName));
        OnLoadingEnded.Invoke(_currentLevelName);
    }

    /*\brief Unloads a level (asyncrous)
     * 
     * Starts to unload a Unity level using the SceneManager.LoadSceneAsync method.
     * Adds this operation to the unloading operation list.
     * Invokes the OnUnloadingStarted event.
     * \param levelName The scene's name to unload. Can log an error if given
     * string does not correspond to a loaded level.
     * \param updateGameState If true, updates the GameState to PREGAME when the unloading is finished.
     */
    public void UnloadLevel(string levelName, bool updateGameState = true)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("Unable to unload level " + levelName + ". Did you check that you've made no mistakes ?");
            return;
        }
        ao.completed += OnUnloadOperationComplete;
        if (updateGameState)
        {
            ao.completed += (AsyncOperation ao) =>
            {
                if (_unloadOperations.Count == 0)
                    UpdateGameState(GameState.PREGAME);
            };
        }
        _unloadOperations.Add(ao);
        _currentLevelName = string.Empty;
        OnUnloadingStarted.Invoke(levelName);
    }

    public void UnloadAndSavePosition(string levelName, float x, float y, bool updateGameState = true)
    {
        // Créez une instance de SaveData avec les nouvelles coordonnées
        SaveData saveData = new SaveData
        {
            playerPosition = new PlayerPosition
            {
                x = x,
                y = y
            }
        };

        // Sérialisez cette instance en JSON
        string json = JsonUtility.ToJson(saveData);

        // Enregistrez le JSON dans le fichier
        string filePath = Path.Combine(Application.dataPath, "Data/save.json");
        File.WriteAllText(filePath, json);

        // Déchargez le niveau actuel
        if (!string.IsNullOrEmpty(levelName))
        {
            UnloadLevel(levelName);
        }
    }

    public void LoadLevelAndPositionPlayer(string levelName, bool updateGameState = true)
    {
        if (_loadOperations.Count != 0)
        {
            Debug.LogWarning("There's already a loading operation. Please wait for it to finish.");
            return;
        }
        else if (_currentLevelName != string.Empty)
        {
            Debug.LogWarning("There's already a loaded level. Please unload it before loading another one.");
            return;
        }
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("Unable to load level " + levelName + ". Did you check that you've made no mistakes ?");
            return;
        }
        ao.completed += OnLoadOperationComplete;
        // Adds a lambda function to the completed event of the AsyncOperation
        // This lambda function updates the GameState to RUNNING when the loading is finished:
        if (updateGameState) // sera exec dans l'ordre ???
        {
            ao.completed += (AsyncOperation ao) =>
            {
                if (_loadOperations.Count == 0)
                {
                    UpdateGameState(GameState.RUNNING);
                    PositionPlayerFromSave();
                }
            };
        }
        _loadOperations.Add(ao);
        _currentLevelName = levelName;
        OnLoadingStarted.Invoke(_currentLevelName);
    }

    private void PositionPlayerFromSave()
    {
        string filePath = Path.Combine(Application.dataPath, "Data/save.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            float x = saveData.playerPosition.x;
            float y = saveData.playerPosition.y;
            LoadLevelAndPositionPlayerCoroutine();
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(x, y, player.transform.position.z);
            }
            else
            {
                Debug.LogError("Player object not found.");
            }
        }
        else
        {
            Debug.LogError("Save file not found.");
        }
    }

    //Faire une coroutine pour attendre que le joueur soit bien positionné avant de continuer le chargement
    private IEnumerator LoadLevelAndPositionPlayerCoroutine()
    {
        yield return new WaitForSeconds(.5f);
    }

    /*\brief Should be called when an unloading level AsyncOperation finished its job.
     * 
     * Deletes the previous operation from the unloading operation list
     * Invokes the OnUnloadingEnded event.
     * \param aso Completed operation, to remove from the list
     */
    private void OnUnloadOperationComplete(AsyncOperation aso)
    {
        _unloadOperations.Remove(aso);
        OnUnloadingEnded.Invoke();
    }

    /*\brief Unloads the current level (except the one with managers) and loads the given one (asyncrous)
     * 
     * Starts the CoroutineChangeLevel coroutine.
     * \param levelName The scene's name to load. Can log an error if given string does not correspond to a level
     * 
     */
    public void ChangeLevel(string levelName, bool updateGameState = false)
    {
        StartCoroutine(CoroutineChangeLevel(levelName, updateGameState));
    }

    /*\brief Couroutine to unload the current level (except the one with managers) and loads the given one (asyncrous)
     * 
     * \param levelName The scene's name to load. Can log an error if given string does not correspond to a level
     * \param updateGameState If true, updates the GameState when the unloading is finished.
     * 
     */
    private IEnumerator CoroutineChangeLevel(string levelName, bool updateGameState = false)
    {
        UnloadLevel(_currentLevelName, false);
        while (_unloadOperations.Count != 0)
        {
            yield return null;
        }
        LoadLevel(levelName, updateGameState);
        while (_loadOperations.Count != 0)
        {
            yield return null;
        }
    }

    /*\brief Updates the GameState and invokes the OnGameStateChanged event.
     * 
     * Updates the GameState.
     * See the GameState enum for more informations.
     * Can log an error if given paramater is not valid.
     * \param gameState The new GameState
     */
    public void UpdateGameState(GameState gameState)
    {
        GameState previousGameState = new GameState();
        previousGameState = _gameState;

        switch (gameState)
        {
            case GameState.PREGAME:
                {
                    _gameState = GameState.PREGAME;
                    Time.timeScale = 1;
                    break;
                }
            case GameState.RUNNING:
                {
                    _gameState = GameState.RUNNING;
                    Time.timeScale = 1;
                    break;
                }
            case GameState.PAUSED:
                {
                    _gameState = GameState.PAUSED;
                    Time.timeScale = 0;
                    break;
                }
            case GameState.DIALOG:
                {
                    _gameState = GameState.DIALOG;
                    Time.timeScale = 1;
                    break;
                }
            case GameState.FROZEN:
                {
                    _gameState = GameState.FROZEN;
                    Time.timeScale = 1;
                    break;
                }
            default:
                {
                    Debug.LogError("Unvalid given paramater in function " + "UpdateGameState");
                    break;
                }
        }

        if (previousGameState != _gameState)
            OnGameStateChanged.Invoke(_gameState, previousGameState);

        //Debug.LogWarning("Game state change to " + _gameState);
    }

    /*\brief Exits the game.
     * 
     * Calls Unity's Application.Quit() method.
     */
    public void ExitGame()
    {
        Application.Quit();
    }

    /*\brief Updates the GameState to PREGAME.
     * 
     * Updates the GameState to PREGAME.
     * If there's no level loaded, updates to PREGAME and returns.
     */
    public void ReturnToMainMenu(bool updateGameState = true)
    {
        if (_currentLevelName == string.Empty)
        {
            UpdateGameState(GameState.PREGAME);
            return;
        }
        UnloadLevel(_currentLevelName, updateGameState);
    }

    /*\brief Toggles pause. Time is frozen when paused.
     * 
     * Toggle pause. Time is frozen when paused.
     * If the game is neither RUNNING nor PAUSED, returns.
     */
    public void TogglePause()
    {
        if (_gameState == GameState.RUNNING)
            UpdateGameState(GameState.PAUSED);
        else if (_gameState == GameState.PAUSED)
            UpdateGameState(GameState.RUNNING);
        else return;
    }

    public static GameState CurrentGameState
    {
        get { return _gameState; }
        private set { _gameState = value; }
    }

    public static string CurrentLevelName
    {
        get { return new string(_currentLevelName); }
    }
}
