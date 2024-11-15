using System.Collections;
using UnityEngine;

public class Puce : MonoBehaviour
{
    public PuceType puceType;
    public int xIndex;
    public int yIndex;

    public bool isMatched;
    private Vector2 currentPos;
    private Vector2 targetPos;

    public bool isMoving;

    public Puce(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
    }

    public void SetIndicies(int _x, int _y)
    {
        xIndex = _x;
        yIndex = _y;
    }

    public void MoveToTarget(Vector2 targetPos)
    {
        StartCoroutine(MoveCoroutine(targetPos));
    }

    private IEnumerator MoveCoroutine(Vector2 targetPos)
    {
        isMoving = true;
        float duration = 0.2f;

        Vector2 startPosition = transform.position;
        float elaspedTime = 0f;
        while (elaspedTime < duration)
        {
            float t = elaspedTime/duration;

            transform.position = Vector2.Lerp(startPosition,targetPos, t) ;
            elaspedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
}

public enum PuceType
{
    puce1,
    puce2, 
    puce3, 
    puce4,
    puce5,
}
