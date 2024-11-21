using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 startPosition;
    private bool isDragging = false;
    private bool isStopped = false;
    [SerializeField] private string objectTag;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!isDragging && !isStopped)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        startPosition = transform.position;
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = mousePosition;
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckDropZone();
    }

    void CheckDropZone()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        bool validDrop = false;

        foreach (Collider2D collider in colliders)
        {
            DropZone dropZone = collider.GetComponent<DropZone>();
            if (dropZone != null && dropZone.CheckDroppedObject(gameObject))
            {
                StopMovement();
                validDrop = true;
                break;
            }
        }

        if (!validDrop)
        {
            ReturnToStartPosition();
        }
    }

    public void ReturnToStartPosition()
    {
        transform.position = startPosition;
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D collider in colliders)
        {
            DropZone dropZone = collider.GetComponent<DropZone>();
        }
    }

    public void StopMovement()
    {
        isStopped = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LimitZone"))
        {
            Destroy(gameObject);
        }
    }
}