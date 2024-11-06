//\brief A truple is a kind of tuple with three elements.
//\param T1 The type of the first element.
//\param T2 The type of the second element.
//\param T3 The type of the third element.
//Note: Serializable
[System.Serializable]
public class CTruple<T1, T2, T3>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;

    public CTruple(T1 item1, T2 item2, T3 item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }
}
