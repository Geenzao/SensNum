using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float f_moveSpeed;
    private Vector3 vector3_moveDirection;

    private Animator animator_anim;

    // Start is called before the first frame update
    void Start()
    {
        animator_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveY < 0)
        {
            animator_anim.SetInteger("whereLooking", 0); // Bas
        }
        else if (moveY > 0)
        {
            animator_anim.SetInteger("whereLooking", 2); // Haut
        }
        else if (moveX != 0)
        {
            animator_anim.SetInteger("whereLooking", 1); // C�t�
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1); // Rotation c�t�
        }

        vector3_moveDirection = new Vector3(moveX, moveY, 0).normalized;

        if (vector3_moveDirection != Vector3.zero)
        {
            animator_anim.SetBool("isMooving", true);
            transform.position += vector3_moveDirection * f_moveSpeed * Time.deltaTime;
        }
        else
        {
            animator_anim.SetBool("isMooving", false);
        }
    }
}

