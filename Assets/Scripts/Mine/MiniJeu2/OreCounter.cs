using UnityEngine;

public class OreCounter : MonoBehaviour
{
    public int cptAu = 0;
    public int cptCu = 0;
    public int cptLi = 0;

    public void AddAu()
    {
        cptAu++;
        Debug.Log("Gold count: " + cptAu);
    }

    public void RmAu()
    {
        cptAu--;
        Debug.Log("Gold count: " + cptAu);
    }

    public void AddCu()
    {
        cptCu++;
        Debug.Log("Copper count: " + cptCu);
    }

    public void RmCu()
    {
        cptCu--;
        Debug.Log("Copper count: " + cptCu);
    }

    public void AddLi()
    {
        cptLi++;
        Debug.Log("Lithium count: " + cptLi);
    }

    public void RmLi()
    {
        cptLi--;
        Debug.Log("Lithium count: " + cptLi);
    }
}
