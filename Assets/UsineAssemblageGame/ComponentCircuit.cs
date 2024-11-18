using UnityEngine;

public enum ComponentType
{
    red,
    gray, 
    blue,
    pink
}

public class ComponentCircuit : MonoBehaviour
{
    public ComponentType type;

    public ComponentCircuit(ComponentType type)
    {
        this.type = type;
    }
}
