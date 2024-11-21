using UnityEngine;

public class DropZone : MonoBehaviour
{
    public string requiredTag;
    private int cptOr = 0;
    private int cptCu = 0;
    private int cptLi = 0;

    public int CptOr => cptOr;
    public int CptCu => cptCu;
    public int CptLi => cptLi;

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
        cptOr = 0;
        cptCu = 0;
        cptLi = 0;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Gold"))
            {
                cptOr++;
            }
            else if (collider.CompareTag("Copper"))
            {
                cptCu++;
            }
            else if (collider.CompareTag("Lithium"))
            {
                cptLi++;
            }
        }

        Debug.Log($"Or: {cptOr}, Cuivre: {cptCu}, Lithium: {cptLi}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gold") || collision.gameObject.CompareTag("Copper") || collision.gameObject.CompareTag("Lithium"))
        {
            UpdateCounters();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Gold") || collision.gameObject.CompareTag("Copper") || collision.gameObject.CompareTag("Lithium"))
        {
            UpdateCounters();
        }
    }
}