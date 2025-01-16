using Events.GameProgress;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static GameManager;
using static GameProgressManager;

public class PathManager : Singleton<PathManager>
{
    public enum PathState
    {
        Village1,
        Mine,
        Village2,
        AssemblyFactory,
        Village3,
        RecycleFactory,
        Village4,
        End
    }

    private static PathState _currentPathState;

    public void UpdatePathState(PathState newPathState)
    {
        PathState oldGameProgressState = _currentPathState;
        _currentPathState = newPathState;

        //switch (newPathState)
        //{
        //    case PathState.Village1:
        //        Debug.Log("Village 1");
        //        break;
        //    case PathState.Mine:
        //        Debug.Log("Mine");
        //        break;
        //    case PathState.Village2:
        //        Debug.Log("Village 2");
        //        break;
        //    case PathState.AssemblyFactory:
        //        Debug.Log("Assembly Factory");
        //        break;
        //    case PathState.Village3:
        //        Debug.Log("Village 3");
        //        break;
        //    case PathState.RecycleFactory:
        //        Debug.Log("Recycle Factory");
        //        break;
        //    case PathState.Village4:
        //        Debug.Log("Village 4");
        //        break;
        //    case PathState.End:
        //        Debug.Log("End");
        //        break;
        //    default:
        //        Debug.Log("Path state not found");
        //        break;
        //}

        Debug.LogWarning("Path state changed to " + _currentPathState);
    }

    public static PathState CurrentPathState
    {
        get => _currentPathState;
    }
}
