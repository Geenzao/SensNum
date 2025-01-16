using UnityEngine;
using System.Collections.Generic;

public class ChangeurDeScene : MonoBehaviour
{
    [SerializeField] private string currentScene;
    [SerializeField] private List<string> sceneNameToGo;
    [SerializeField] private List<PathManager.PathState> path;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (PathManager.CurrentPathState == path[i])
                {
                    GameManager.Instance.UnloadLevel(currentScene);
                    GameManager.Instance.LoadLevel(sceneNameToGo[i]);
                }
            }

        }
    }
}
