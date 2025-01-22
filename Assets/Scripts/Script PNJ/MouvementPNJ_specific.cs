using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Cette classe permet de gérer les mouvement des PNJ qui ont 3 action possible : idl, marche  et actionSpecifique
 * Elle gère leur état et leur déplacement
 */

public class MouvementPNJ_specific : MonoBehaviour
{
    public GameObject[] waypoints;  // Points de destination
    public Animator animator;

    [SerializeField] private float minIdleTime = 2f;   // Temps minimum en idle
    [SerializeField] private float maxIdleTime = 5f;   // Temps maximum en idle
    [SerializeField] private float minActionTime = 3f; // Temps minimum pour l'action
    [SerializeField] private float maxActionTime = 6f; // Temps maximum pour l'action

    private float timeIdle = 0f;
    private float timeAction = 0f;

    private enum PNJState { Idle, ActionSpecific, Moving }
    private PNJState currentState = PNJState.Idle;

    private int currentWaypointIndex = 0;
    private float stateTimer = 0f;
    private float speed = 2f;

    //pour stoker le scale du pnj au début 
    private Vector3 initialScale;

    void Start()
    {
        initialScale = gameObject.transform.localScale;
        timeIdle = GetRandomIdleTime();
        timeAction = GetRandomActionTime();
        SetState(PNJState.Idle);
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        switch (currentState)
        {
            case PNJState.Idle:
                if (stateTimer >= timeIdle)
                {
                    ChosseNextState();
                }
                break;

            case PNJState.ActionSpecific:
                if (stateTimer >= timeAction)
                {
                    ChosseNextState();
                }
                break;

            case PNJState.Moving:
                MoveToNextWaypoint();
                break;
        }
    }

    private void SetState(PNJState newState)
    {
        currentState = newState;
        stateTimer = 0f;
        if(currentState == PNJState.Idle)
        {
            animator.SetTrigger("idl");
        }
        else if (currentState == PNJState.ActionSpecific)
        {
            animator.SetTrigger("interaction");
        }
        else if (currentState == PNJState.Moving)
        {
            animator.SetTrigger("walk");
            UpdateSpriteDirection();
        }
    }

    //pour décider du prochain état du pnj
    private void ChosseNextState()
    {
        if (currentState == PNJState.Idle)
        {
            SetState(PNJState.ActionSpecific);
        }
        else if (currentState == PNJState.ActionSpecific)
        {
            SetState(PNJState.Moving);
        }
        else if (currentState == PNJState.Moving)
        {
            SetState(PNJState.Idle);
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex].transform;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            ChosseNextState();
        }
    }

    private void UpdateSpriteDirection()
    {
        Transform targetPoint = waypoints[currentWaypointIndex].transform;
        float direction = targetPoint.position.x > transform.position.x ? 1 : -1;
        transform.localScale = new Vector3(initialScale.x * direction, initialScale.y, initialScale.z);  // Applique la direction tout en gardant la taille initiale    }
    }

    private float GetRandomIdleTime() => Random.Range(minIdleTime, maxIdleTime);
    private float GetRandomActionTime() => Random.Range(minActionTime, maxActionTime);
}