using UnityEngine;

public class ChargementTransition : Singleton<ChargementTransition>
{
    public Animator animator;
    public GameObject LoadGreenRoue;

    //pour des test 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadChargement();
        }
    }

    public void LoadChargement()
    {
        animator.SetTrigger("LoadPage");
    }

    public void StopGame()
    {
        Debug.Log("StopGame");
        //Time.timeScale = 0;
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        //Time.timeScale = 1;
    }
}
