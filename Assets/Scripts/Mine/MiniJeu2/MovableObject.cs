using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 startPosition;
    private bool isDragging = false;
    private bool isStopped = false;
    private bool isUp = false;
    [SerializeField] private string objectTag;
    [SerializeField] private OreCounter oreCounter;

    void Start()
    {
        startPosition = transform.position;

        GameObject zonesRecoltes = GameObject.Find("ZonesRecoltes");
        if (zonesRecoltes != null)
        {
            oreCounter = zonesRecoltes.GetComponent<OreCounter>();
            if (oreCounter == null)
            {
                Debug.LogError("OreCounter component not found on ZonesRecoltes GameObject.");
            }
        }
        else
        {
            Debug.LogError("GameObject 'ZonesRecoltes' not found.");
        }
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

        if (objectTag == "Gold" && isUp)
        {
            oreCounter.RmAu();
        }
        else if (objectTag == "Copper" && isUp)
        {
            oreCounter.RmCu();
        }
        else if (objectTag == "Lithium" && isUp)
        {
            oreCounter.RmLi();
        }

        isUp = false;
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
                // ToDo : Si on clic sur l'objet alors on enlève 1 de l'oercounter 
                isUp = true;
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