using UnityEngine;

public class OreCounter : MonoBehaviour
{
    public int cptAu { get; set; }
    public int cptCu { get; set; }
    public int cptLi { get; set; }

    private void Start()
    {
        cptAu = 0;
        cptCu = 0;
        cptLi = 0;
    }

    public void AddAu()
    {
        cptAu++;
    }

    public void AddCu()
    {
        cptCu++;
    }

    public void AddLi()
    {
        cptLi++;
    }
}
