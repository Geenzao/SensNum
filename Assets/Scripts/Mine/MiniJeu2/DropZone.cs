using UnityEngine;

public class DropZone : MonoBehaviour
{
    public string requiredTag;

    [SerializeField] private OreCounter oreCounter;

    public bool CheckDroppedObject(GameObject droppedObject)
    {
        if (droppedObject.CompareTag(requiredTag))
        {
            UpdateCounters();
            return true;
        }
        return false;
    }

    public void UpdateCounters()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Gold"))
            {
                oreCounter.AddAu();
                break;
            }
            else if (collider.CompareTag("Copper"))
            {
                oreCounter.AddCu();
                break;
            }
            else if (collider.CompareTag("Lithium"))
            {
                oreCounter.AddLi();
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gold") || collision.gameObject.CompareTag("Copper") || collision.gameObject.CompareTag("Lithium"))
        {
            UpdateCounters();
        }
    }
}