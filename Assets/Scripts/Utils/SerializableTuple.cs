//\brief A tuple is a pair of two elements.
//\param T1 The type of the first element.
//\param T2 The type of the second element.
//Note: Serializable
[System.Serializable]
public class CTuple<T1, T2>
{
    public T1 Item1;
    public T2 Item2;

    public CTuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}
