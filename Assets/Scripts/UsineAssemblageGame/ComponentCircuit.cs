using UnityEngine;

public enum ComponentType
{
    blue,
    red,
    pink,
    cyan
}

public class ComponentCircuit : MonoBehaviour
{
    public ComponentType type;

    public ComponentCircuit(ComponentType type)
    {
        this.type = type;
    }
}
