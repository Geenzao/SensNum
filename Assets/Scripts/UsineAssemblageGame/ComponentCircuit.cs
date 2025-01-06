using UnityEngine;

public enum ComponentType
{
    celluleBatterie,
    circuitIntegre,
    resistance,
    processeur
}

public class ComponentCircuit : MonoBehaviour
{
    public ComponentType type;

    public ComponentCircuit(ComponentType type)
    {
        this.type = type;
    }
}
