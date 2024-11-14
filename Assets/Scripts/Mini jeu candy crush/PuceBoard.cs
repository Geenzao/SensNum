using UnityEngine;
using UnityEngine.Rendering;

public class PuceBoard : MonoBehaviour
{
    public int width = 6;
    public int height = 8;

    public float spacingX;
    public float spacingY;

    public GameObject[] pucePrefabs;
    private Node[,] puceBoard;
    public GameObject puceBoardGO;

    //public ArrayLayout arrayLayout;
    public static PuceBoard Instance;

    private void Awake()
    {
        Instance = this;

    }
    void Start()
    {
        InitializeBorad()
    }

    void InitializeBorad()
    {
        puceBoard = new Node[width, height];

        spacingX = (float)(width - 1) / 2;
        spacingY = (float)(height - 1) / 2;

        for (int y  = 0; i < height; y++) 
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new Vector2(x - spacingX, y - spacingY);

                int randomIndex = Random.Range(0, pucePrefabs.Length);

                GameObject puce = Instantiate(pucePrefabs[randomIndex], position , Quaternion.identity );
                puce.GetComponent<Puce>().SetIndicies(x, y);
                puceBoard[x, y] = new Node(true, puce);
            }
        }
    }
}
