using UnityEngine;

public enum ComponentType
{
    red,
    gray, 
    blue,
    pink
}

public class Component : MonoBehaviour
{
    public ComponentType type;

    public Component(ComponentType type)
    {
        this.type = type;
    }
}
