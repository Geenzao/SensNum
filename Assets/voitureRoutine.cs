using UnityEngine;
using System.Collections;

public class voitureRoutine : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public GameObject pointC;
    public GameObject pointD;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform[] points;
    private int currentPointIndex;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        points = new Transform[] { pointB.transform, pointC.transform, pointD.transform, pointA.transform };
        currentPointIndex = 0;
        SetAnimationTrigger();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPoint(points[currentPointIndex]);
    }

    // Method to move the car to a specific point
    private void MoveToPoint(Transform targetPoint)
    {
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, points[currentPointIndex].position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length;
            SetAnimationTrigger();
        }
    }

    // Method to set the appropriate animation trigger
    private void SetAnimationTrigger()
    {
        if (points[currentPointIndex] == pointB.transform)
        {
            anim.SetTrigger("droite");
        }
        else if (points[currentPointIndex] == pointC.transform)
        {
            anim.SetTrigger("bas");
        }
        else if (points[currentPointIndex] == pointD.transform)
        {
            anim.SetTrigger("gauche");
        }
        else if (points[currentPointIndex] == pointA.transform)
        {
            anim.SetTrigger("haut");
        }
    }
}