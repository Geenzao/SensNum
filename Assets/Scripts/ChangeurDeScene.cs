using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChangeurDeScene : MonoBehaviour
{
    [SerializeField] private string currentScene;
    [SerializeField] private List<string> sceneNameToGo;
    [SerializeField] private List<PathManager.PathState> path;
    [SerializeField] private List<GameProgressManager.GameProgressState> gameProgress;
    [SerializeField] private float x,y;
    [SerializeField] private bool positionPlayerInNextScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (PathManager.CurrentPathState == path[i])
                {
                    //Lancer l'ui ici
                    StartCoroutine(ChargementTransitionManager.Instance.LoadScene(gameProgress[i], currentScene, sceneNameToGo[i], positionPlayerInNextScene, x, y));
                }
            }

        }
    }
}
