using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// \brief Contains all the event classes
namespace Events
{
    namespace Assignation
    {
        //public class EventDisplayPopulationResidenceAssignation : UnityEvent <ToDisplayAboutTheHabitation> { }
        //public class EventDisplayPopulationFactoryAssignation : UnityEvent <ToDisplayAboutTheFactory>{ }
    }
    
    // \brief Game related events
    namespace Game
    {
        /*! \brief Game State Changed Event
         * 
         * This event should be called when the game state changes.
         * 
         * \warning Please respect this : OnGameStateChanged(newGameState, oldGameState)
         * 
         * \tparam NEW Gamestate
         * \tparam OLD Gamestate
         */
        public class GameStateChanged : UnityEvent<GameManager.GameState,
        GameManager.GameState>
        { }

        /*! \brief Should be called when a level starts loading.
         *
         *  \tparam string Level Name
         */
        public class LevelLoadingStarted : UnityEvent<string> { }

        /*! \brief Should be called when a level is fully loaded.
         *
         *  \tparam string Level Name
         */
        public class LevelLoadingEnded : UnityEvent<string> { }

        /*! \brief Should be called when a level starts unloading.
         *
         *  \tparam string Level Name
         */
        public class LevelUnloadingStarted : UnityEvent<string> { }

        /*! \brief Should be called when a level is fully unloaded.
         *
         *  \tparam string Level Name
         */
        public class LevelUnloadingEnded : UnityEvent { }

        /*! \brief Should be called when an error log is received
         *
         *  \tparam string Log Text
         *  \tparam string Stack Trace
         *  \tparam LogType Unity Log Type
         */
        public class ErrorLogReceived : UnityEvent<string, string, LogType> { }

        public class FinInitialisationPlateau : UnityEvent { }

        public class PlateauPlusInit : UnityEvent { }
    }

    // \brief UI Related events
    namespace UI
    {
        /*! \brief Menu State Changed Event
         * 
         * This event should be called when the menu state changes.
         * 
         * \warning Please respect this : OnMenuStateChanged(newMenuState, oldMenuState)
         * 
         * \tparam NEW MenuState
         * \tparam OLD MenuState
         */
        public class MenuStateChanged : UnityEvent<UIManager.MenuState, UIManager.MenuState> { }

        /*! \brief Should be called when the main menu fade in/out animation is complete.
         * 
         * \tparam bool Is Fade Out
         */
        public class EventFadeMMenuComplete : UnityEvent<bool> { }
    }

    namespace GameProgress
    {
        /*! \brief Game Progress Event
         * 
         * This event should be called when the game progress changes.
         * 
         * \warning Please respect this : OnGameProgress(newProgress, oldProgress)
         * 
         * \tparam NEW Progress
         * \tparam OLD Progress
         */
        public class EventGameProgress : UnityEvent<GameProgressManager.GameProgressState, GameProgressManager.GameProgressState> { }
    }

    namespace Settings
    {
        public class OnSettingIntUpdate : UnityEvent<int> { }
        public class OnSettingFloatUpdate : UnityEvent<float> { }
        public class OnSettingStringUpdate : UnityEvent<string> { }
    }

    namespace calculators
    {
        //public class EventConstanteCO2Changed : UnityEvent { }
        //public class EventLevelChanged : UnityEvent { }
        //public class EventDistanceMaxPietonChanged : UnityEvent { }
        //public class EventRegimeAlimentaireChanged : UnityEvent { }
        //public class EventLoyerDeBaseChanged : UnityEvent { }
        //public class EventMalusCanicule : UnityEvent { }
        //public class EventPollutionHabsChanged : UnityEvent { }
        //public class EventPeopleHabsChanged : UnityEvent { }
        //public class EventHapinessHabsChanged : UnityEvent { }
        //public class EventMoneyPerSecondHabsChanged : UnityEvent { }
        //public class EventHappinessCellChanged : UnityEvent { }
        //public class EventMalheurCO2Changed : UnityEvent { }
    }

    namespace CurrentGame
    {
        //public class EventNiveauHDVChanged : UnityEvent { }
    }

    namespace Camera
    {
        //!\brief Should be listened by all cameras to determine if they are the prioritary camera.
        //\tparam int The Instance ID of the VCameraPriorityObserver component
        public class WantedLiveCameraUpdate : UnityEvent<int> { }
    }
}
