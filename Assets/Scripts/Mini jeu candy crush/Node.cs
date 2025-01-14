using UnityEngine;

public class Node
{
    public bool isUsable;
    public GameObject puce;

    public Node(bool _isUsable, GameObject _puce)
    {
        isUsable = _isUsable;
        puce = _puce;
    }
}
