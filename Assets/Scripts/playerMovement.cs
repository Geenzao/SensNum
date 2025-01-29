using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : Singleton<playerMovement>
{
    public float f_moveSpeed;
    private Vector3 vector3_moveDirection;

    private Animator animator_anim;

    private bool isMobilePlatform = false;

    private void Awake()
    {
        base.Awake();
        ChargementTransitionManager.OnLoadPage += StopPlayerMouvement;
        ChargementTransitionManager.OnUnloadPage += ActivePlayerMouvement;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator_anim = GetComponent<Animator>();

        isMobilePlatform = PlatformManager.Instance.fctisMobile();
    }
    float moveX;
    float moveY;
    public void setMove(float x, float y)
    {
        moveX = x;
        moveY = y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMobilePlatform)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }

        //Si on veut bouger seulement verticalement et horizontalement
        /* 
        if (moveX != 0 && moveY != 0)
        {
            moveX = 0;
            moveY = 0;
        }
        else
        {
            if (moveY != 0 && moveX == 0)
            {
                moveX = 0;
                if (moveY < 0)
                {
                    animator_anim.SetInteger("whereLooking", 0); // Bas
                }
                else
                {
                    animator_anim.SetInteger("whereLooking", 2); // Haut
                }
            }
            else if (moveX != 0 && moveY == 0)
            {
                moveY = 0;
                animator_anim.SetInteger("whereLooking", 1); // Côté
                transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1); // Rotation côté
            }
        }
        */
        if(UIManager.CurrentMenuState != UIManager.MenuState.Loading)
        {
            if (f_moveSpeed!=0)
            {
                //Si on veut bouger dans toutes les directions
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
                    animator_anim.SetInteger("whereLooking", 1); // Côté
                    transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1); // Rotation côté
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
    }

    //Ces fonction servent à stoper le player au niveau de ses mouvements
    public void StopPlayerMouvement()
    {
        f_moveSpeed = 0;
    }

    public void ActivePlayerMouvement()
    {
        f_moveSpeed = 5.0f;
        if(UIManager.CurrentMenuState != UIManager.MenuState.None)
            UIManager.Instance.UpdateMenuState(UIManager.MenuState.None);
    }

}

        



