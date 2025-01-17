using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChangeurDeScene : MonoBehaviour
{
    [SerializeField] private string currentScene;
    [SerializeField] private List<string> sceneNameToGo;
    [SerializeField] private List<PathManager.PathState> path;
    //[SerializeField] private List<GameProgressManager.GameProgressState> gameProgress;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (PathManager.CurrentPathState == path[i])
                {
                    //Lancer l'ui ici
                    UIManager.Instance.UpdateMenuState(UIManager.MenuState.Loading);
                    //ChargementTransition.Instance.gameProgressState = gameProgress[i];
                    StartCoroutine(LoadScene(i));
                }
            }

        }
    }

    private IEnumerator LoadScene(int i)
    {
        yield return new WaitForSecondsRealtime(1.2f);
        GameManager.Instance.UnloadLevel(currentScene);
        GameManager.Instance.LoadLevel(sceneNameToGo[i]);
        //ChargementTransition.InvokeOnLoadPage();
    }
}
