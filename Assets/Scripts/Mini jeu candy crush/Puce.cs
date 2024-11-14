using UnityEngine;

public class Puce : MonoBehaviour
{
    public PotionType potionType;
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
}

public enum PotionType
{
    puce1,
    puce2, 
    puce3, 
    puce4,
    puce5,
}
